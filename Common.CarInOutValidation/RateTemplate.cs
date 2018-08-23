using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Common.Entities;
using Common.Entities.Parking;
using PKFee;
using Common.Services.Park;
using Common.Services;
using Common.Entities.Validation;
using Common.Utilities.Helpers;

namespace Common.CarInOutValidation
{
    public class RateTemplate
    {
        public void Process(InputAgs args, ResultAgs rst)
        {
            //仅主道口出才会计费
            if (args.GateInfo.IoState != IoState.GoOut)
            {
                return;
            }
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                if (args.AreadInfo.Parent != null && args.Timeseries == null)//有上级区域 内部车场
                {
                    return;
                }
                if ((args.CarTypeInfo.BaseTypeID != BaseCarType.StoredValueCar && args.CarTypeInfo.BaseTypeID != BaseCarType.TempCar)
                    || (rst.ResCode != ResultCode.OutOK && rst.ResCode != ResultCode.RateNoSet && rst.ResCode != ResultCode.CarModeNoSelected)) //非系统出场
                {
                    return;
                }
                rst.Rate = new RateInfo();
                if (args.AreadInfo.NeedToll == YesOrNo.No)//需要收费
                {
                    return;
                }
                if (args.AreadInfo.IsNestArea)//内圈不收费 
                {
                    return;
                }

                if (args.CarModel == null)
                {
                    rst.ResCode = ResultCode.CarModeNoSelected;
                    return;
                }
                if (args.GateInfo.IsWeight)
                {
                    rst.ValidMsg = "称重车辆";
                    return;
                }
                if (args.IORecord == null && args.GateInfo.Box.IsCenterPayment == YesOrNo.Yes)
                {
                    return;
                }
                //获取车辆优免信息 计费的时候才判断优免
                GetDerate(args, rst);

                Calculate(args, rst);

                #region 线上缴费计算费 

                if ((args.CarTypeInfo.BaseTypeID == BaseCarType.TempCar
                    || args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar) && rst.ResCode == ResultCode.OutOK) //临时卡判断线上缴费 
                {
                    string errorMsg = "";
                    List<ParkOrder> reforders = null;
                    if (args.AreadInfo.Parent != null)
                    {
                        if (args.Timeseries != null)
                        {
                            reforders = ParkOrderServices.GetOrderByTimeseriesID(args.Timeseries.TimeseriesID, out errorMsg);
                        }
                    }
                    else
                    {
                        if (args.IORecord != null)
                        {
                            reforders = ParkOrderServices.GetOrderByIORecordID(args.IORecord.RecordID, out errorMsg);
                        }
                    }
                    if (reforders == null || reforders.Count <= 0)//订单存在
                    {
                        return;
                    }
                    //线上缴费后 重新计算金额
                    DateTime lastPayTime = DateTime.MinValue;
                    decimal payAmount = 0;
                    foreach (var item in reforders)
                    {
                        if (item.OrderTime > lastPayTime && item.Status == 1)
                        {
                            if (item.PayWay != OrderPayWay.PreferentialTicket)
                            {
                                lastPayTime = item.OrderTime;
                            }
                            payAmount += item.PayAmount;
                            if (item.PayWay == OrderPayWay.Alipay)
                            {
                                rst.ValidMsg = "支付宝支付";
                            }
                            else if (item.PayWay == OrderPayWay.WeiXin)
                            {
                                rst.ValidMsg = "微信支付";
                            }
                        }
                    }
                    if (rst.Rate.Amount <= payAmount || (args.Plateinfo.TriggerTime - lastPayTime).TotalMinutes <= args.AreadInfo.Parkinfo.CenterTime)//缴费金额大于需缴费金额 或者出场时间在中央缴费时间内 出场
                    {
                        rst.Rate.DiscountAmount = 0;//折扣金额也不现实
                        rst.Rate.OnlinePayAmount = payAmount;
                        rst.Rate.UnPayAmount = 0;
                        rst.Rate.Amount = 0;
                        rst.Rate.CardTransactionsAmount = 0;
                    }
                    else
                    {
                        if (rst.Rate.DiscountAmount > 0)
                        {
                            rst.Rate.OnlinePayAmount = payAmount;
                            if ((rst.Rate.OnlinePayAmount + rst.Rate.DiscountAmount) > rst.Rate.Amount)//线上+折扣大于订单金额
                            {
                                rst.Rate.DiscountAmount = rst.Rate.Amount - rst.Rate.OnlinePayAmount;
                                rst.Rate.UnPayAmount = 0;
                                rst.Rate.Amount = rst.Rate.Amount - rst.Rate.OnlinePayAmount;
                            }
                            else
                            {
                                rst.Rate.UnPayAmount = rst.Rate.Amount - payAmount - rst.Rate.DiscountAmount;
                                rst.Rate.Amount = rst.Rate.Amount - payAmount;
                            }
                        }
                        else
                        {
                            rst.Rate.OnlinePayAmount = payAmount;
                            rst.Rate.UnPayAmount = rst.Rate.Amount - rst.Rate.OnlinePayAmount;
                            rst.Rate.Amount = rst.Rate.Amount - payAmount;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                rst.ResCode = ResultCode.LocalError;
                LogerHelper.Loger.Error(ex);
            }
            finally
            {
                watch.Stop();
            }
        }
        private void Calculate(InputAgs args, ResultAgs rst)
        {

            string carTypeID = args.CarTypeInfo.CarTypeID;//算费的卡类型

            //if (args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar
            //    && args.CardInfo.EndDate != DateTime.MinValue && args.CardInfo.EndDate < DateTime.Now.Date) //过期储值卡按临时卡算费
            //{
            //    rst.ValidMsg = "过期储值卡转临停算费";
            //    var tempcardtype = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar);
            //    if (tempcardtype != null && tempcardtype.Count > 0)
            //    {
            //        carTypeID = tempcardtype.First().CarTypeID;
            //    }
            //} 

            //内场时间判断
            List<Tuple<ParkArea, ParkTimeseries>> nestareaTimes = new List<Tuple<ParkArea, ParkTimeseries>>();
            DateTime enterTime;
            DateTime exitTime;
            string ErrorMessage = "";
            //bool SeparateChangeFee = false;//是否分开收费，当内圈开启需要收费的时候，分开收费
            if (args.AreadInfo.IsNestArea)
            {
                enterTime = args.Timeseries.EnterTime;
                exitTime = args.Plateinfo.TriggerTime;
            }
            else
            {
                if (args.IORecord == null)
                {
                    enterTime = args.Plateinfo.TriggerTime.AddSeconds(-1);
                    LogerHelper.Loger.Info(args.Plateinfo.LicenseNum + ":进入重复出场算费逻辑");
                }
                else
                {
                    enterTime = args.IORecord.EntranceTime;
                    LogerHelper.Loger.Info(args.Plateinfo.LicenseNum + ":进入正常算费逻辑");
                }
                #region 出场时间判断
                exitTime = args.Plateinfo.TriggerTime;

                List<ParkTimeseries> timeserieses = null;
                if (args.IORecord != null)
                {
                    timeserieses = ParkTimeseriesServices.GetAllExitsTimeseriesesByIORecordID(args.AreadInfo.PKID, args.IORecord.RecordID, out ErrorMessage);
                }
                if (timeserieses != null)
                {
                    timeserieses.RemoveAll(o => o.IsExit == false);
                }
                if (timeserieses != null && timeserieses.Count > 0)//未出场的时序 忽略
                {
                    List<ParkTimeseries> sumTimeser = new List<ParkTimeseries>();
                    foreach (var item in timeserieses)
                    {
                        var area = ParkAreaServices.GetParkAreaByParkGateRecordId(item.EnterGateID);
                        if (area == null)
                        {
                            continue;
                        }
                        //if (area.NeedToll == YesOrNo.Yes)//内场收费的话 则内外圈分开收费
                        //{
                        //    SeparateChangeFee = true;
                        //    //continue;
                        //}
                        List<ParkFeeRule> list = ParkFeeRuleServices.QueryFeeRuleByCarModelAndCarType(area.AreaID, args.CarModel.CarModelID, carTypeID);
                        if (list == null || list.Count <= 0)//没有配收费规则 时间算大车场的 一次性算费即可，无需二次算费
                        {
                            continue;
                        }
                        nestareaTimes.Add(new Tuple<ParkArea, ParkTimeseries>(area, item));
                        sumTimeser.Add(item);
                    }
                    //总时长  减去内部车场 已经计费 或者内部车场 不需要算费的时长 等于大车场停车时长
                    TimeSpan span = new TimeSpan();
                    foreach (var item in sumTimeser)
                    {
                        span += item.ExitTime - item.EnterTime;
                    }
                    span = (exitTime - enterTime) - span;
                    exitTime = enterTime + span;
                }
                #endregion
            }

            bool isOverdueToTemp = false;
            if (rst.OverdueToTempSpan.TotalMinutes > 0)//如果车辆是临停转固定车辆 时段重新计算 
            {
                enterTime = args.Plateinfo.TriggerTime.Add(-rst.OverdueToTempSpan);
                exitTime = args.Plateinfo.TriggerTime;
                isOverdueToTemp = true;
            }
            List<ParkFeeRule> feerules = ParkFeeRuleServices.QueryFeeRuleByCarModelAndCarType(args.AreadInfo.AreaID, args.CarModel.CarModelID, carTypeID);
            if ((feerules == null || feerules.Count <= 0) && !args.AreadInfo.MasterID.IsEmpty())
            {
                feerules = ParkFeeRuleServices.QueryFeeRuleByCarModelAndCarType(args.AreadInfo.MasterID, args.CarModel.CarModelID, carTypeID);
            }
            if (feerules == null || feerules.Count <= 0)
            {
                rst.ResCode = ResultCode.RateNoSet;
                return;
            }
            rst.FeeRule = feerules.First();
            DateTime oldstartTime = enterTime;
            DateTime oldendTime = exitTime;
            RateInfo rateInfo = Calculate(args, enterTime, exitTime, rst.FeeRule);
            rst.Rate = rateInfo;

            //统计内圈收费金额
            if (nestareaTimes.Count > 0 && !isOverdueToTemp)
            {
                var mainAraeTimeSpan = exitTime - enterTime;
                bool continuesFreeTime = false;
                //没有费用 或者分开计费   都算免费时间  
                if (rst.Rate.Amount <= 0 || args.AreadInfo.Parkinfo.OuterringCharge)
                {

                    continuesFreeTime = true;
                }

                DateTime nestEnterTime = DateTime.Now;
                DateTime nestExitTime;
                TimeSpan nestTimeSpan = new TimeSpan();
                decimal nestAmount = 0;
                List<ParkFeeRule> nestfeerules = ParkFeeRuleServices.QueryFeeRuleByCarModelAndCarType(nestareaTimes[0].Item1.AreaID, args.CarModel.CarModelID, carTypeID);

                foreach (var item in nestareaTimes)//内圈停放时间
                {
                    if (item.Item1.NeedToll == YesOrNo.No)//内圈部收费
                    {
                        continue;
                    }
                    if (item.Item2.EnterTime < nestEnterTime)
                    {
                        nestEnterTime = item.Item2.EnterTime;
                    }
                    nestTimeSpan += (item.Item2.ExitTime - item.Item2.EnterTime);
                }

                nestExitTime = nestEnterTime.Add(nestTimeSpan);
                //包含免费时间，且不是分开计费，外圈的免费时间算到内圈
                if (continuesFreeTime && !args.AreadInfo.Parkinfo.OuterringCharge)
                {
                    nestEnterTime = nestEnterTime.AddMinutes(-mainAraeTimeSpan.TotalMinutes);
                }
                nestAmount = Calculate(args, nestEnterTime, nestExitTime, nestfeerules.First(), continuesFreeTime).Amount;

                if (nestAmount > 0)
                {
                    rst.Rate.Amount = rst.Rate.Amount + nestAmount;

                    CalMaxUseMoney(rst.Rate, args, GetFeeRele(nestfeerules.First()), oldstartTime, args.Plateinfo.TriggerTime);
                    if (args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar)
                    {
                        rst.Rate.AccountSurplus = args.CardInfo == null ? 0 : args.CardInfo.Usercard.Balance;
                        rst.Rate.CardTransactionsAmount = rst.Rate.AccountSurplus > rst.Rate.Amount ? rst.Rate.Amount : rst.Rate.AccountSurplus;
                        rst.Rate.CardTransationsBalance = args.CardInfo == null ? 0 : args.CardInfo.Usercard.Balance;
                        rst.Rate.UnPayAmount = rst.Rate.AccountSurplus > rst.Rate.Amount ? 0 : rst.Rate.Amount - rst.Rate.AccountSurplus;
                    }
                    else
                    {
                        rst.Rate.AccountSurplus = 0;
                        rst.Rate.UnPayAmount = rst.Rate.Amount;
                    }
                }
            }

            if (rst.Carderates != null && rst.Carderates.Count > 0)
            {
                var carderate = rst.Carderates.Find(c => c.Derate.DerateType == DerateType.NoPayment);
                if (carderate != null)
                {
                    rst.Carderates.Remove(carderate);
                    rst.Carderates.Add(carderate);//不收取费用比较特殊  会改订单金额，影响其他规则算费，把它放到最后计算
                }

                TimeSpan totalstartpan = new TimeSpan();
                TimeSpan totalendpan = new TimeSpan();
                decimal discountAmount = 0;
                foreach (var item in rst.Carderates)
                {
                    //计算优免   主要计算折扣金额 时间的商家扣除金额里面计算 
                    //优免折扣金额 到外面一起计算
                    TimeSpan startpan = new TimeSpan();
                    TimeSpan endpan = new TimeSpan();
                    discountAmount += CalDerate(enterTime, exitTime, args, rst, item, rst.FeeRule, out startpan, out endpan);
                    totalstartpan += startpan;
                    totalendpan += endpan;
                }

                if (totalstartpan.TotalMinutes > 0)//免前面时间
                {
                    var fnewrate = Calculate(args, oldstartTime, oldstartTime + totalstartpan, rst.FeeRule);
                    discountAmount += fnewrate.Amount;
                }
                if (totalendpan.TotalMinutes > 0)//免后面时间
                {
                    var fnewrate = Calculate(args, oldstartTime, oldendTime - totalendpan, rst.FeeRule);
                    discountAmount += (rateInfo.Amount - fnewrate.Amount);
                }
                if (discountAmount > rateInfo.Amount)
                {
                    discountAmount = rateInfo.Amount;
                }
                rateInfo.DiscountAmount = discountAmount;
                rateInfo.UnPayAmount = rateInfo.Amount - rateInfo.DiscountAmount;
                //超过打折时间加4块钱
                //var stoptimespan = exitTime - enterTime;
                //if ((stoptimespan > totalendpan && totalendpan.TotalSeconds > 0) ||
                //    (stoptimespan > totalstartpan && totalstartpan.TotalSeconds > 0))
                //{
                //    rateInfo.UnPayAmount += 4;
                //    rateInfo.Amount += 4;
                //}
            }
        }

        private IFeeRule GetFeeRele(ParkFeeRule feerule, bool containsfreeTime = true, bool containsfirstSpan = true)
        {
            IFeeRule FeeRule = null;
            if (feerule.FeeType == FeeType.Hour24)
            {
                FeeRule = new Hours24(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.Hour12)
            {
                FeeRule = new Hours12(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.DayAndNight)
            {
                FeeRule = new DayNight(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.NaturalDay)
            {
                FeeRule = new NaturalDay(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.Custom)
            {
                FeeRule = new Userdefined();
                FeeRule.FeeText = feerule.RuleText;
                LogerHelper.Loger.Info("算费文件内容：" + FeeRule.FeeText);
            }
            FeeRule.listRuleDetail = ParkFeeRuleServices.QueryFeeRuleDetailByFeeRuleId(feerule.FeeRuleID);
            return FeeRule;
        }
        //统计优免  减免金额 以及应扣商家的金额
        private decimal CalDerate(DateTime enterTime, DateTime exitTime, InputAgs args, ResultAgs rst, ParkCarDerate carderate, ParkFeeRule defaultFeerule, out TimeSpan startSpan, out TimeSpan endSpan)
        {
            startSpan = new TimeSpan();
            endSpan = new TimeSpan();
            //应扣商家的金额
            decimal sellerAmount = 0;
            decimal discountAmount = 0;
            switch (carderate.Derate.DerateType)
            {
                case DerateType.PaymentOnTime:
                    #region 按时   
                    discountAmount = rst.Rate.Amount;
                    sellerAmount = rst.Rate.Amount;
                    #endregion
                    break;
                case DerateType.NoPayment:
                    #region 不收取费用   
                    discountAmount = rst.Rate.Amount;
                    sellerAmount = 0;
                    #endregion
                    break;
                case DerateType.TimesPayment:
                    #region 按次收费
                    if (carderate.Derate.DerateSwparate == DerateSwparate.BeforeTime)//免前面时间
                    {
                        enterTime = enterTime.AddMinutes(carderate.Derate.FreeTime);
                        sellerAmount = carderate.Derate.DerateMoney;
                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            startSpan = new TimeSpan(0, carderate.Derate.FreeTime, 0);
                        }
                    }
                    if (carderate.Derate.DerateSwparate == DerateSwparate.AfterTime)//免后面时间
                    {
                        exitTime = exitTime.AddMinutes(-carderate.Derate.FreeTime);
                        sellerAmount = carderate.Derate.DerateMoney;
                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            exitTime = exitTime.AddMinutes(carderate.Derate.FreeTime);//时间还原
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            endSpan = new TimeSpan(0, carderate.Derate.FreeTime, 0);
                        }
                    }
                    #endregion
                    break;
                case DerateType.VotePayment:
                    #region 按票收费
                    if (carderate.Derate.DerateSwparate == DerateSwparate.BeforeTime)//免前面时间
                    {
                        var trate = Calculate(args, enterTime, enterTime.AddMinutes(carderate.FreeTime), defaultFeerule);
                        enterTime = enterTime.AddMinutes(carderate.FreeTime);
                        var tnewrate = Calculate(args, enterTime, exitTime, defaultFeerule);//减免时间后 重新算费

                        sellerAmount = trate.Amount;
                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            enterTime = enterTime.AddMinutes(-carderate.FreeTime);//时间还原
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            startSpan = new TimeSpan(0, carderate.FreeTime, 0);
                        }
                    }
                    if (carderate.Derate.DerateSwparate == DerateSwparate.AfterTime)//免后面时间 商家减免金额等于折扣金额
                    {
                        exitTime = exitTime.AddMinutes(-carderate.FreeTime);
                        var tnewrate = Calculate(args, enterTime, exitTime, defaultFeerule, false);//减免时间后 重新算费 不包含免费时段

                        sellerAmount = (rst.Rate.Amount - tnewrate.Amount);
                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            exitTime = exitTime.AddMinutes(carderate.FreeTime);//时间还原
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            endSpan = new TimeSpan(0, carderate.FreeTime, 0);
                        }
                    }
                    #endregion
                    break;
                case DerateType.TimePeriodPayment:
                    //待实现
                    break;
                case DerateType.StandardPayment:
                    #region 按收费规则
                    ParkFeeRule tfeerule = ParkFeeRuleServices.QueryParkFeeRuleByFeeRuleId(carderate.Derate.FeeRuleID);
                    if (tfeerule == null)
                    {
                        rst.DerateValidMsg = "优免计算失败，未找到指定费率.";
                        LogerHelper.Loger.Warn(rst.ValidMsg + "费率ID:" + carderate.Derate.FeeRuleID);
                        return 0;
                    }
                    var fnewrate = Calculate(args, enterTime, exitTime, tfeerule);
                    //如果有优免算费 需要计算差价
                    //订单金额的差价需要商家出  
                    sellerAmount = (rst.Rate.Amount - fnewrate.Amount);
                    discountAmount = (rst.Rate.Amount - fnewrate.Amount);
                    #endregion
                    break;
                case DerateType.TimePeriodAndTimesPayment:
                    #region 按次特殊规则
                    sellerAmount = carderate.Derate.DerateMoney;

                    if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                    {
                        discountAmount = 0;
                        sellerAmount = 0;
                        rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        return 0;
                    }
                    TimeSpan span = new TimeSpan();
                    //先算出在时段内的时间总和 
                    TimeSpan Startpan = carderate.Derate.StartTime - carderate.Derate.StartTime.Date;
                    TimeSpan Endpan = carderate.Derate.EndTime - carderate.Derate.EndTime.Date;
                    if ((enterTime.Date + Startpan) <= enterTime &&
                        (exitTime.Date + Endpan) >= exitTime)
                    {
                        span = exitTime - enterTime;
                    }
                    if ((enterTime.Date + Startpan) <= enterTime &&
                       (exitTime.Date + Endpan) < exitTime)
                    {
                        span = (exitTime.Date + Endpan) - enterTime;
                    }
                    if ((enterTime.Date + Startpan) > enterTime &&
                        (exitTime.Date + Endpan) >= exitTime)
                    {
                        span = exitTime - (enterTime.Date + Startpan);
                    }
                    if ((enterTime.Date + Startpan) > enterTime &&
                        (exitTime.Date + Endpan) < exitTime)
                    {
                        span = (exitTime.Date + Endpan) - (enterTime.Date + Startpan);
                    }
                    if (span.TotalMinutes > 0)
                    {
                        if (carderate.Derate.DerateSwparate == 0)//免前面时间
                        {
                            if (span.TotalMinutes > carderate.Derate.FreeTime)
                            {
                                enterTime = enterTime.AddMinutes(carderate.Derate.FreeTime);
                                startSpan = new TimeSpan(0, carderate.Derate.FreeTime, 0);
                            }
                            else
                            {
                                enterTime = enterTime + span;
                                startSpan = span;
                            }
                        }
                        else if (carderate.Derate.DerateSwparate == DerateSwparate.AfterTime)//免后面时间
                        {
                            if (span.TotalMinutes > carderate.Derate.FreeTime)
                            {
                                exitTime = exitTime.AddMinutes(-carderate.Derate.FreeTime);
                                endSpan = new TimeSpan(0, carderate.Derate.FreeTime, 0);
                            }
                            else
                            {
                                exitTime = exitTime + span;
                                endSpan = span;
                            }
                        }
                    }
                    #endregion
                    break;
                case DerateType.VoteSpecialPayment:
                    //待实现
                    break;
                case DerateType.SpecialTimePeriodPayment:
                    #region 减免费用
                    discountAmount = carderate.FreeMoney;
                    sellerAmount = carderate.FreeMoney;
                    #endregion
                    break;
                case DerateType.ReliefTime:
                    #region 减免时间
                    if (carderate.Derate.DerateSwparate == 0)//免前面时间
                    {
                        var drate = Calculate(args, enterTime, enterTime.AddMinutes(carderate.FreeTime), defaultFeerule);
                        enterTime = enterTime.AddMinutes(carderate.FreeTime);
                        sellerAmount = drate.Amount;

                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            enterTime = enterTime.AddMinutes(-carderate.FreeTime);//时间还原
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            startSpan = new TimeSpan(0, carderate.FreeTime, 0);
                        }
                    }
                    if (carderate.Derate.DerateSwparate == DerateSwparate.AfterTime)//免后面时间 商家减免金额等于折扣金额
                    {
                        exitTime = exitTime.AddMinutes(-carderate.FreeTime);
                        var newdrate = Calculate(args, enterTime, exitTime, defaultFeerule, false); //不包含免费时段
                        sellerAmount = (rst.Rate.Amount - newdrate.Amount);

                        if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
                        {
                            discountAmount = 0;
                            sellerAmount = 0;
                            exitTime = exitTime.AddMinutes(carderate.FreeTime);//时间还原
                            rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
                        }
                        else
                        {
                            endSpan = new TimeSpan(0, carderate.FreeTime, 0);
                        }
                    }
                    #endregion
                    break;
                case DerateType.DayFree:
                    #region 按天减免
                    if (exitTime > carderate.ExpiryTime)
                    {
                        var drate = Calculate(args, enterTime, carderate.ExpiryTime, defaultFeerule);
                        enterTime = carderate.ExpiryTime;
                        discountAmount = drate.Amount;
                        sellerAmount = 0;
                        carderate.FreeMoney += discountAmount; 
                    }
                    else
                    {
                        discountAmount = rst.Rate.Amount;
                        sellerAmount = 0;
                        carderate.FreeMoney += discountAmount;
                    }
                    #endregion
                    break;
            }

            if (!CanSellerDerate(carderate.Derate.Seller, sellerAmount))
            {
                //余额加透支额度小于优免金额时 无法优免。
                discountAmount = 0;
                sellerAmount = 0;
                rst.DerateValidMsg += "商家余额不足：" + carderate.Derate.Name + "无法优免";
            }
            if (carderate.FreeMoney <= 0)
            {
                carderate.FreeMoney = sellerAmount;
            }
            if (carderate.FreeTime <= 0)
            {
                carderate.FreeTime = carderate.Derate.FreeTime;
            }
            return discountAmount;
        }

        /// <summary>
        /// 商家能否优免
        /// </summary>
        /// <param name="seller">商家</param>
        /// <param name="sellerAmount">优免金额</param>
        /// <returns></returns>
        private bool CanSellerDerate(ParkSeller seller, decimal sellerAmount)
        {
            if (sellerAmount <= 0)//不扣钱的 不判断
            {
                return true;
            }
            if ((seller.Balance + seller.Creditline) < sellerAmount)
            {
                return false;
            }
            return true;
        }

        private RateInfo Calculate(InputAgs args, DateTime enterTime, DateTime exitTime, ParkFeeRule feerule, bool containsfreeTime = true)
        {
            RateInfo rateinfo = new RateInfo();
            IFeeRule FeeRule = null;

            //if (RateProcesser._XDocument != null)
            //{
            //    FeeRule = new Userdefined(containsfreeTime ? 1 : 0, RateProcesser._XDocument);
            //}
            if (feerule.FeeType == FeeType.Hour24)
            {
                FeeRule = new Hours24(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.Hour12)
            {
                FeeRule = new Hours12(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.DayAndNight)
            {
                FeeRule = new DayNight(containsfreeTime ? 1 : 0);
            }
            else if (feerule.FeeType == FeeType.NaturalDay)
            {
                FeeRule = new NaturalDay(containsfreeTime ? 1 : 0);
            }

            else if (feerule.FeeType == FeeType.Custom)
            {
                //RateProcesser.LoadCustomFeerule(feerule.RuleText);
                FeeRule = new Userdefined2();
                FeeRule.FeeText = feerule.RuleText;
            }
            FeeRule.ParkingBeginTime = enterTime;
            FeeRule.ParkingEndTime = exitTime;
            FeeRule.listRuleDetail = ParkFeeRuleServices.QueryFeeRuleDetailByFeeRuleId(feerule.FeeRuleID);
            rateinfo.Amount = FeeRule.CalcFee();
            LogerHelper.Loger.Info(args.Plateinfo.LicenseNum + "计算费用:" + rateinfo.Amount);
            rateinfo = CalMaxUseMoney(rateinfo, args, FeeRule, enterTime, exitTime);
            if (args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar)
            {
                rateinfo.AccountSurplus = args.CardInfo == null ? 0 : args.CardInfo.Usercard.Balance;
                rateinfo.CardTransactionsAmount = rateinfo.AccountSurplus > rateinfo.Amount ? rateinfo.Amount : rateinfo.AccountSurplus;
                rateinfo.CardTransationsBalance = args.CardInfo == null ? 0 : args.CardInfo.Usercard.Balance;
                rateinfo.UnPayAmount = rateinfo.AccountSurplus > rateinfo.Amount ? 0 : rateinfo.Amount - rateinfo.AccountSurplus;
            }
            else
            {
                rateinfo.AccountSurplus = 0;
                rateinfo.UnPayAmount = rateinfo.Amount;
            }
            return rateinfo;
        }
        /// <summary>
        /// 计算24小时最大收费
        /// </summary>
        /// <param name="rateinfo"></param>
        /// <param name="args"></param>
        /// <param name="FeeRule"></param>
        /// <param name="enterTime"></param>
        /// <param name="exitTime"></param>
        /// <returns></returns>
        private RateInfo CalMaxUseMoney(RateInfo rateinfo, InputAgs args, IFeeRule FeeRule, DateTime enterTime, DateTime exitTime)
        {
            if (rateinfo.Amount <= 0)
            {
                return rateinfo;
            }
            if (args.CarModel.MaxUseMoney <= 0)
            {
                CalDayAndMaxUseMoney(rateinfo, args, FeeRule, enterTime, exitTime);
                return rateinfo;
            }
            string ErrorMessage = "";
            //默认临时卡
            OrderType orderType = OrderType.TempCardPayment;
            if (args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar)
            {
                orderType = OrderType.ValueCardPayment;
            }
            double days = (exitTime - enterTime).TotalDays;//停车时长不足一天

            decimal Maxmoney = 0;
            //结余到24小时内的金额6
            ParkOrder order;
            #region 每天最大收费
            if (args.CarModel.IsNaturalDay)
            {
                //结余到24小时内的金额 
                if (days < 1)//计算每天最大限额
                {
                    TimeSpan span = new TimeSpan();
                    if (args.CarModel.NaturalTime != 0)
                    {
                        span = new TimeSpan(args.CarModel.NaturalTime, 0, 0);
                    }
                    Maxmoney = args.CarModel.MaxUseMoney;
                    #region  自然天 
                    if (enterTime < args.Plateinfo.TriggerTime.Date.Add(span) && args.Plateinfo.TriggerTime > args.Plateinfo.TriggerTime.Date.Add(span))//跨天了
                    {
                        order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, args.Plateinfo.TriggerTime.Date.AddDays(-1).Add(span), args.Plateinfo.TriggerTime.Date.Add(span), out ErrorMessage);

                        if (order == null)
                        {
                            FeeRule.ParkingBeginTime = args.Plateinfo.TriggerTime.Date.Add(span);
                            FeeRule.ParkingEndTime = exitTime;
                            var tamount = FeeRule.CalcFee();//今天的费用  
                            if (tamount > Maxmoney)
                            {
                                rateinfo.CashMoney = Maxmoney;
                            }
                            else
                            {
                                rateinfo.CashMoney = tamount;
                            }
                            rateinfo.CashTime = args.Plateinfo.TriggerTime;
                        }
                        else if (order.CashMoney >= args.CarModel.MaxUseMoney)
                        {
                            FeeRule.ParkingBeginTime = args.Plateinfo.TriggerTime.Date.Add(span);
                            FeeRule.ParkingEndTime = exitTime;
                            var tamount = FeeRule.CalcFee();//今天的费用  
                            rateinfo.Amount = tamount;
                            rateinfo.UnPayAmount = 0;
                            if (tamount > Maxmoney)
                            {
                                rateinfo.CashMoney = Maxmoney;
                            }
                            else
                            {
                                rateinfo.CashMoney = tamount;
                            }
                            rateinfo.CashTime = args.Plateinfo.TriggerTime;
                        }
                        else
                        {
                            FeeRule.ParkingBeginTime = enterTime;
                            FeeRule.ParkingEndTime = args.Plateinfo.TriggerTime.Date.Add(span);
                            var famount = FeeRule.CalcFee();//前一天的费用
                            if (famount > Maxmoney - order.CashMoney)
                            {
                                famount = Maxmoney - order.CashMoney;
                            }


                            FeeRule.ParkingBeginTime = args.Plateinfo.TriggerTime.Date.Add(span);
                            FeeRule.ParkingEndTime = exitTime;
                            var tamount = FeeRule.CalcFee();//今天的费用 
                            if (tamount > Maxmoney)
                            {
                                tamount = Maxmoney;
                            }
                            rateinfo.Amount = famount + tamount;
                            rateinfo.UnPayAmount = 0;
                            if (tamount > Maxmoney)
                            {
                                rateinfo.CashMoney = Maxmoney;
                            }
                            else
                            {
                                rateinfo.CashMoney = tamount;
                            }
                            rateinfo.CashTime = args.Plateinfo.TriggerTime;
                        }

                    }
                    else//当天的费用
                    {
                        if (args.Plateinfo.TriggerTime > args.Plateinfo.TriggerTime.Date.Add(span))
                        {
                            order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, args.Plateinfo.TriggerTime.Date.Add(span), args.Plateinfo.TriggerTime, out ErrorMessage);
                        }
                        else
                        {
                            order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, args.Plateinfo.TriggerTime.Date.AddDays(-1).Add(span), args.Plateinfo.TriggerTime, out ErrorMessage);
                        }


                        if (order != null && order.CashMoney > 0)
                        {
                            if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)
                            {
                                rateinfo.Amount = Maxmoney - order.CashMoney;
                                if (rateinfo.Amount < 0)
                                {
                                    rateinfo.Amount = 0;
                                }
                                rateinfo.UnPayAmount = 0;
                                rateinfo.CashMoney = Maxmoney;
                                rateinfo.CashTime = args.Plateinfo.TriggerTime;
                            }
                            else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                            {
                                rateinfo.CashMoney = rateinfo.Amount + order.CashMoney;
                                rateinfo.CashTime = args.Plateinfo.TriggerTime;
                            }
                        }
                        else
                        {
                            if (rateinfo.Amount > Maxmoney)
                            {
                                rateinfo.CashMoney = Maxmoney;
                            }
                            else
                            {
                                rateinfo.CashMoney = rateinfo.Amount;
                            }
                            rateinfo.CashTime = args.Plateinfo.TriggerTime;
                        }
                    }
                    #endregion
                }
                else
                {
                    TimeSpan span = new TimeSpan();

                    if (args.CarModel.NaturalTime != 0)
                    {
                        span = new TimeSpan(args.CarModel.NaturalTime, 0, 0);
                    }
                    Maxmoney = (int)days * args.CarModel.MaxUseMoney;
                    #region 自然天
                    if (args.Plateinfo.TriggerTime > args.Plateinfo.TriggerTime.Date.Add(span))
                    {
                        order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, enterTime.Date.Add(span), enterTime, out ErrorMessage);
                    }
                    else
                    {
                        order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, enterTime.Date.AddDays(-1).Add(span), enterTime, out ErrorMessage);
                    }
                    if (order != null && order.CashMoney > 0)//有结余金额重新算费
                    {
                        FeeRule.ParkingBeginTime = enterTime;
                        FeeRule.ParkingEndTime = enterTime.Date.Add(span).AddDays(1);//算出进场到结余时间的金额
                        var tempamount = FeeRule.CalcFee();
                        if (tempamount + order.CashMoney > args.CarModel.MaxUseMoney)
                        {
                            tempamount = args.CarModel.MaxUseMoney - order.CashMoney;
                        }
                        FeeRule.ParkingBeginTime = order.CashTime.Date.Add(span).AddDays(1);//再算结余时间后的金额
                        FeeRule.ParkingEndTime = exitTime;
                        var sumamount = FeeRule.CalcFee() + tempamount;
                        FeeRule.ParkingBeginTime = enterTime;
                        FeeRule.ParkingEndTime = exitTime;
                        var sumamount2 = FeeRule.CalcFee();
                        if (sumamount > sumamount2)
                        {
                            rateinfo.Amount = sumamount2;
                        }
                        else
                        {
                            rateinfo.Amount = sumamount;
                        }
                        rateinfo.CashTime = order.CashTime.AddDays((int)days);
                        rateinfo.CashMoney = rateinfo.Amount - Maxmoney;
                    }
                    else
                    {
                        if (rateinfo.Amount - Maxmoney > args.CarModel.MaxUseMoney)
                        {
                            rateinfo.CashMoney = args.CarModel.MaxUseMoney;
                        }
                        else
                        {
                            rateinfo.CashMoney = rateinfo.Amount - Maxmoney;
                        }
                        rateinfo.CashTime = args.Plateinfo.TriggerTime;
                        if (rateinfo.CashMoney < 0)
                        {
                            rateinfo.CashMoney = 0;
                        }
                    }
                    #endregion 
                }
            }
            else
            {
                if (days < 1)//计算每天最大限额
                {
                    Maxmoney = args.CarModel.MaxUseMoney;
                    #region 24小时
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, enterTime.AddHours(-24), enterTime, out ErrorMessage);
                    if (order != null && order.CashMoney > 0)
                    {
                        if ((exitTime - order.CashTime).Days >= 1)//说明跨了一天了
                        {
                            //重新计算跨段收费，分两种情况计算
                            //1、前一天结余满了
                            //2、前一天结余未满
                            if (order.CashMoney >= Maxmoney)//结余满了
                            {
                                FeeRule.ParkingBeginTime = order.CashTime.AddDays(1);//只需要算今天应该缴的费用
                                FeeRule.ParkingEndTime = exitTime;
                                if (rateinfo.Amount > 0)
                                {
                                    FeeRule.IsFirstFree = 0;
                                }
                                rateinfo.Amount = FeeRule.CalcFee();
                                if (rateinfo.Amount > Maxmoney)
                                {
                                    rateinfo.Amount = Maxmoney;
                                }
                                rateinfo.UnPayAmount = 0;
                                rateinfo.CashMoney = rateinfo.Amount;
                                rateinfo.CashTime = order.CashTime.AddDays(1);
                            }
                            else
                            {
                                if (rateinfo.Amount > Maxmoney)
                                {
                                    rateinfo.Amount = Maxmoney;
                                }

                                FeeRule.ParkingBeginTime = enterTime;
                                FeeRule.ParkingEndTime = order.CashTime.AddDays(1);
                                var amount = FeeRule.CalcFee();//前一天的应该缴的金额 

                                FeeRule.ParkingBeginTime = order.CashTime.AddDays(1);
                                FeeRule.ParkingEndTime = exitTime;
                                FeeRule.IsFirstFree = 0;
                                var todayamount = FeeRule.CalcFee();//今天应该缴的金额

                                if (Maxmoney < (order.CashMoney + amount))
                                {
                                    amount = Maxmoney - order.CashMoney;
                                }

                                if (todayamount + amount < rateinfo.Amount)
                                {
                                    rateinfo.Amount = todayamount + amount;
                                }
                                rateinfo.CashMoney = rateinfo.Amount - amount;
                                rateinfo.CashTime = order.CashTime.AddDays(1);
                            }
                        }
                        else if (order.CashMoney >= Maxmoney)//如果历史(一天内)缴费大于 每天最大 则不需要缴费
                        {
                            rateinfo.Amount = 0;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = Maxmoney;
                            rateinfo.CashTime = order.CashTime;
                        }
                        else if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)
                        {
                            rateinfo.Amount = Maxmoney - order.CashMoney;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = Maxmoney;
                            rateinfo.CashTime = order.CashTime;
                        }
                        else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                        {
                            rateinfo.CashMoney = rateinfo.Amount + order.CashMoney;
                            rateinfo.CashTime = order.CashTime;
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount;
                        rateinfo.CashTime = enterTime;
                    }
                    #endregion 
                }
                else
                {
                    Maxmoney = (int)days * args.CarModel.MaxUseMoney;
                    #region 24小时
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType, args.Plateinfo.LicenseNum, enterTime.AddHours(-24), enterTime, out ErrorMessage);

                    if (order != null && order.CashMoney > 0)//有结余金额重新算费
                    {
                        FeeRule.ParkingBeginTime = enterTime;
                        FeeRule.ParkingEndTime = order.CashTime.AddDays(1);//算出进场到结余时间的金额
                        var tempamount = FeeRule.CalcFee();
                        if (tempamount + order.CashMoney > args.CarModel.MaxUseMoney)
                        {
                            tempamount = args.CarModel.MaxUseMoney - order.CashMoney;
                        }

                        FeeRule.ParkingBeginTime = order.CashTime.AddDays(1);//再算结余时间后的金额
                        FeeRule.ParkingEndTime = exitTime;
                        var sumamount = FeeRule.CalcFee() + tempamount;
                        FeeRule.ParkingBeginTime = enterTime;
                        FeeRule.ParkingEndTime = exitTime;
                        var sumamount2 = FeeRule.CalcFee();
                        if (sumamount > sumamount2)
                        {
                            rateinfo.Amount = sumamount2;
                        }
                        else
                        {
                            rateinfo.Amount = sumamount;
                        }
                        if (rateinfo.Amount <= Maxmoney)
                        {
                            rateinfo.CashTime = order.CashTime.AddDays((int)days);
                            rateinfo.CashMoney = rateinfo.Amount;
                        }
                        else
                        {
                            rateinfo.CashTime = order.CashTime.AddDays((int)days + 1);
                            rateinfo.CashMoney = rateinfo.Amount - Maxmoney;
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount - Maxmoney;
                        rateinfo.CashTime = enterTime.AddDays((int)days);
                        if (rateinfo.CashMoney < 0)
                        {
                            rateinfo.CashMoney = 0;
                        }
                    }
                    #endregion
                }
            }
            #endregion
            return rateinfo;
        }
        /// <summary>
        /// 根据自定义配置的白天黑夜最大算费
        /// </summary>
        /// <param name="rateinfo"></param>
        /// <param name="args"></param>
        /// <param name="FeeRule"></param>
        /// <param name="enterTime"></param>
        /// <param name="exitTime"></param>
        /// <returns></returns>
        private RateInfo CalDayAndMaxUseMoney(RateInfo rateinfo, InputAgs args, IFeeRule FeeRule, DateTime enterTime, DateTime exitTime)
        {
            if (args.CarModel.DayMaxMoney <= 0 || args.CarModel.NightMaxMoney <= 0)
            {
                return rateinfo;
            }

            string ErrorMessage = "";
            //默认临时卡
            OrderType orderType = OrderType.TempCardPayment;
            if (args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar)
            {
                orderType = OrderType.ValueCardPayment;
            }
            double days = (exitTime - enterTime).TotalDays;//只考虑停车时间不足一天的情况
            if (days >= 1)
            {
                return rateinfo;
            }

            //先判断出场处于哪个时段，再判断是否跨端
            //跨段：如果是夜间时段，则判断白天是否收够最大金额
            //跨段：如果是白天时段，则判断夜间是否收够最大金额
            //不跨段：如果是夜间时段，则判断夜间是否收够最大金额
            //不跨段：如果是白天时段，则判断白天是否收够最大金额
            decimal Maxmoney = 0;
            //结余到24小时内的金额
            ParkOrder order;
            if (args.CarModel.DayStartTime.TimeOfDay < exitTime.TimeOfDay
                && exitTime.TimeOfDay < args.CarModel.DayEndTime.TimeOfDay)//白天时段
            {
                #region 白天
                if (enterTime.TimeOfDay < args.CarModel.DayStartTime.TimeOfDay)//跨段
                {
                    #region 跨段
                    Maxmoney = args.CarModel.DayMaxMoney + args.CarModel.NightMaxMoney;
                    //上个时段结余
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType,
                        args.Plateinfo.LicenseNum,
                        enterTime.Date.Date.AddDays(-1).Add(args.CarModel.DayEndTime.TimeOfDay), enterTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay), out ErrorMessage);
                    if (order != null && order.CashMoney > 0)//上个时段有结余
                    {
                        if (order.CashMoney >= args.CarModel.NightMaxMoney)//前一个时段结余满了，只算当前时段的
                        {
                            FeeRule.ParkingBeginTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                            FeeRule.ParkingEndTime = exitTime;
                            var sumamount2 = FeeRule.CalcFee();
                            rateinfo.Amount = sumamount2;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = rateinfo.Amount;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)//上个时段的结余已经满了
                        {
                            if (Maxmoney - order.CashMoney > rateinfo.Amount)
                            {
                                rateinfo.Amount = rateinfo.Amount;
                            }
                            else
                            {
                                rateinfo.Amount = Maxmoney - order.CashMoney;
                            }
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = rateinfo.Amount > args.CarModel.DayMaxMoney ? args.CarModel.DayMaxMoney : rateinfo.Amount;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                        {
                            rateinfo.CashMoney = rateinfo.Amount > args.CarModel.DayMaxMoney ? args.CarModel.DayMaxMoney : rateinfo.Amount;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount > args.CarModel.DayMaxMoney ? args.CarModel.DayMaxMoney : rateinfo.Amount;
                        rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                    }
                    #endregion 
                }
                else//不跨段
                {
                    #region 不跨段
                    Maxmoney = args.CarModel.DayMaxMoney;
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType,
                        args.Plateinfo.LicenseNum,
                       exitTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay), exitTime, out ErrorMessage);
                    if (order != null && order.CashMoney > 0)
                    {
                        if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)
                        {
                            rateinfo.Amount = Maxmoney - order.CashMoney;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = args.CarModel.DayMaxMoney;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                        {
                            rateinfo.CashMoney = rateinfo.Amount + order.CashMoney;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount > args.CarModel.DayMaxMoney ? args.CarModel.DayMaxMoney : rateinfo.Amount;
                        rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay);
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 夜间
                if (enterTime.TimeOfDay > args.CarModel.DayStartTime.TimeOfDay)//跨段
                {
                    #region 跨段
                    Maxmoney = args.CarModel.DayMaxMoney + args.CarModel.NightMaxMoney;
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType,
                        args.Plateinfo.LicenseNum,
                        enterTime.Date.Add(args.CarModel.DayStartTime.TimeOfDay), enterTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay), out ErrorMessage);//统计白天时段收费
                    if (order != null && order.CashMoney > 0)
                    {
                        if (order.CashMoney >= args.CarModel.DayMaxMoney)//前一个时段结余满了，只算当前时段的
                        {
                            FeeRule.ParkingBeginTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay);
                            FeeRule.ParkingEndTime = exitTime;
                            var sumamount2 = FeeRule.CalcFee();
                            rateinfo.Amount = sumamount2;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = rateinfo.Amount;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)
                        {
                            rateinfo.Amount = Maxmoney - order.CashMoney;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = args.CarModel.NightMaxMoney;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                        {
                            rateinfo.CashMoney = rateinfo.Amount > args.CarModel.NightMaxMoney ? args.CarModel.NightMaxMoney : rateinfo.Amount;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay);
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount > args.CarModel.NightMaxMoney ? args.CarModel.NightMaxMoney : rateinfo.Amount;
                        rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.Add(args.CarModel.DayEndTime.TimeOfDay);
                    }

                    #endregion
                }
                else//不跨段
                {
                    #region 不跨段
                    Maxmoney = args.CarModel.NightMaxMoney;
                    order = ParkOrderServices.GetCashMoneyCountByPlateNumber(args.AreadInfo.Parkinfo.PKID, orderType,
                        args.Plateinfo.LicenseNum,
                       exitTime.Date.AddDays(-1).Add(args.CarModel.DayEndTime.TimeOfDay), exitTime, out ErrorMessage);
                    if (order != null && order.CashMoney > 0)
                    {
                        if ((rateinfo.Amount + order.CashMoney) >= Maxmoney)
                        {
                            rateinfo.Amount = Maxmoney - order.CashMoney;
                            rateinfo.UnPayAmount = 0;
                            rateinfo.CashMoney = args.CarModel.NightMaxMoney;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.AddDays(-1).Add(args.CarModel.DayEndTime.TimeOfDay);
                        }
                        else if ((rateinfo.Amount + order.CashMoney) < Maxmoney)
                        {
                            rateinfo.CashMoney = rateinfo.Amount + order.CashMoney;
                            rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.AddDays(-1).Add(args.CarModel.DayEndTime.TimeOfDay);
                        }
                    }
                    else
                    {
                        rateinfo.CashMoney = rateinfo.Amount > args.CarModel.NightMaxMoney ? args.CarModel.NightMaxMoney : rateinfo.Amount;
                        rateinfo.CashTime = args.Plateinfo.TriggerTime.Date.AddDays(-1).Add(args.CarModel.DayEndTime.TimeOfDay);
                    }
                    #endregion
                }
                #endregion
            }
            return rateinfo;
        }

        private void GetDerate(InputAgs args, ResultAgs rst)
        {
            //仅出道口才获取优免
            if (args.GateInfo.IoState != IoState.GoOut)
            {
                return;
            }
            rst.Carderates = null;
            string errorMsg;
            if (args.IORecord == null)
            {
                return;
            }
            #region 查询对应的优免卷 根据车牌 卡号查 通行记录待定  

            var carderates = ParkSellerServices.GetCanUseCarderatesByPlatenumber(args.Plateinfo.LicenseNum, out errorMsg);
            if (carderates == null)
            {
                carderates = ParkSellerServices.GetCanUseCarderatesByIORecordid(args.IORecord.RecordID, out errorMsg);
            }
            if (carderates == null || carderates.Count <= 0)
            {
                return;
            }
            if (carderates != null && carderates.Count > 0)
            {
                foreach (var item in carderates)
                {
                    //如果有指定车场的话 则指定车场
                    if (!item.PKID.IsEmpty() && args.AreadInfo != null && item.PKID != args.AreadInfo.Parkinfo.PKID)
                    {
                        continue;
                    }
                    //如果有指定区域的话 则指定区域
                    if (!item.AreaID.IsEmpty() && args.AreadInfo != null && item.AreaID != args.AreadInfo.AreaID)
                    {
                        continue;
                    }
                    if (item.ExpiryTime < DateTime.Now)
                    {
                        continue;
                    }
                    var derate = ParkSellerServices.GetDerate(item.DerateID, out errorMsg);
                    if (derate == null)
                    {
                        continue;
                    }
                    var seller = ParkSellerServices.GetSeller(derate.SellerID, out errorMsg);
                    derate.Seller = seller;
                    item.Derate = derate;
                    if (rst.Carderates == null)
                    {
                        rst.Carderates = new List<ParkCarDerate>();
                    }
                    rst.Carderates.Add(item);
                }

            }
            #endregion

        }
    }
}