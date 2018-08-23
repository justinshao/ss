using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Factory;
using Common.Entities;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Factory.Statistics;
using Common.Entities.Parking;
using Common.DataAccess;
namespace Common.Services.Statistics
{
    /// <summary>
    /// 日统计( 包含按小时统计  按通道统计)
    /// </summary>
    internal class DailyGatherServices
    {
        public void Statistics_DailyByHour(BaseParkinfo park,DateTime dt,int loop)
        {
            DateTime startdate = DateTime.Parse(dt.AddDays(-loop).ToString("yyyy-MM-dd 00:00:00"));
            DateTime enddate = DateTime.Parse(dt.AddDays(-loop).ToString("yyyy-MM-dd 00:59:59"));
            int hours = 0;
            int maxhours = 23;
            List<Statistics_Gather> gathers = new List<Statistics_Gather>();
            List<DateTime> DeleteGather = new List<DateTime>();

            if (startdate.ToString("yyyyMMdd") == dt.ToString("yyyyMMdd"))
            {
                maxhours = dt.Hour;
            }
            IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
            IStatistics_Gather gatherfactory = Statistics_GatherFactory.GetFactory();
            IParkOrder iorder = ParkOrderFactory.GetFactory();

            while (hours <= maxhours)
            {
                Statistics_Gather gather = new Statistics_Gather();
                DateTime tempstartdate = startdate.AddHours(hours);
                DateTime tempenddate = enddate.AddHours(hours);
                gather.StatisticsGatherID = System.Guid.NewGuid().ToString();
                gather.ParkingID = park.PKID;
                gather.ParkingName = park.PKName;
                gather.GatherTime = tempstartdate;

                if (startdate.ToString("yyyyMMdd") == dt.ToString("yyyyMMdd"))
                {
                    if (hours == maxhours || hours == maxhours - 1)
                    {
                        DeleteGather.Add(tempstartdate);
                    }
                    else
                    {
                        if (gatherfactory.IsExistsGather(park.PKID, tempstartdate))
                        {
                            hours++;
                            continue;
                        }
                    }
                }
                else
                {
                    if (gatherfactory.IsExistsGather(park.PKID, tempstartdate))
                    {
                        hours++;
                        continue;
                    }
                }
                //查询进场数
                gather.Entrance_Count = iorecordfactory.EntranceCountByParkingID(park.PKID, tempstartdate, tempenddate);
                //查询出场数
                gather.Exit_Count = iorecordfactory.ExitCountByParkingID(park.PKID, tempstartdate, tempenddate);

                #region 进场卡片类型
                List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByParkingID(park.PKID, tempstartdate, tempenddate);
                if (_InCardType != null && _InCardType.Count > 0)
                {
                    foreach (KeyValue k in _InCardType)
                    {
                        switch (k.KeyName)
                        {
                            case "0":
                                gather.VIPCard = k.Key_Value;
                                break;
                            case "1":
                                gather.StordCard = k.Key_Value;
                                break;
                            case "2":
                                gather.MonthCard = k.Key_Value;
                                break;
                            case "3":
                                gather.TempCard = k.Key_Value;
                                break;
                            case "4":
                                gather.JobCard = k.Key_Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion

                #region 统计放行类型
                List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByParkingID(park.PKID, tempstartdate, tempenddate);
                if (_ReleaseType != null && _ReleaseType.Count > 0)
                {
                    foreach (var s in _ReleaseType)
                    {
                        switch (s.KeyName)
                        {
                            case "0":
                                gather.ReleaseType_Normal = s.Key_Value;
                                break;
                            case "1":
                                gather.ReleaseType_Charge = s.Key_Value;
                                break;
                            case "2":
                                gather.ReleaseType_Free = s.Key_Value;
                                break;
                            case "3":
                                gather.ReleaseType_Catch = s.Key_Value;
                                break;
                        }
                    }
                }
                #endregion

                #region 统计费用相关
                List<ParkOrder> _orderfee = iorder.GetOrdersByParkingID(park.PKID,tempstartdate, tempenddate);
                if (_orderfee != null && _orderfee.Count > 0)
                {
                    foreach (ParkOrder o in _orderfee)
                    {
                        gather.Receivable_Amount +=  o.Amount;
                        gather.Real_Amount += o.PayAmount;
                        gather.Diff_Amount +=  o.UnPayAmount;
                        //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                        switch (o.PayWay)
                        {
                            case OrderPayWay.Cash:
                                gather.Cash_Amount +=o.PayAmount;
                                gather.Cash_Count++;

                                if (o.DiscountAmount > 0)
                                {
                                    gather.CashDiscount_Amount += o.DiscountAmount;
                                    gather.CashDiscount_Count++;

                                    gather.Discount_Amount +=  o.DiscountAmount;
                                    gather.Discount_Count++;
                                }

                                break;
                            case OrderPayWay.WeiXin:
                            case OrderPayWay.Alipay:
                            case OrderPayWay.OnlineBanking:
                            case OrderPayWay.Wallet:
                                gather.OnLine_Amount += o.PayAmount;
                                gather.OnLine_Count++;

                                if (o.DiscountAmount > 0)
                                {
                                    gather.OnLineDiscount_Amount +=o.DiscountAmount;
                                    gather.OnLineDiscount_Count++;

                                    gather.Discount_Amount += o.DiscountAmount;
                                    gather.Discount_Count++;
                                }
                                break;
                            case OrderPayWay.PreferentialTicket:
                                gather.Discount_Amount += o.DiscountAmount;
                                gather.Discount_Count++;
                                break;
                            case OrderPayWay.ValueCard:
                                gather.StordCard_Amount += o.PayAmount;
                                gather.StordCard_Count++;
                                break;
                        }
                        switch (o.OrderType)
                        {
                            //临时卡缴费
                            case OrderType.TempCardPayment:
                            case OrderType.AreaTempCardPayment:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineTemp_Amount +=  o.PayAmount;
                                        gather.OnLineTemp_Count++;
                                        break;
                                    default:
                                        gather.Temp_Amount += o.PayAmount;
                                        gather.Temp_Count++;
                                        break;
                                }
                                break;
                            //月卡续期
                            case OrderType.MonthCardPayment:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineMonthCardExtend_Amount += o.PayAmount;
                                        gather.OnLineMonthCardExtend_Count++;
                                        break;
                                    default:
                                        gather.MonthCardExtend_Amount +=  o.PayAmount;
                                        gather.MonthCardExtend_Count++;
                                        break;
                                }
                                break;
                            //VIP卡续期
                            case OrderType.VIPCardRenewal:
                                gather.VIPExtend_Count++;
                                break;
                            //储值卡充值
                            case OrderType.ValueCardRecharge:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineStordCard_Amount += o.PayAmount;
                                        gather.OnLineStordCard_Count++;
                                        break;
                                    default:
                                        gather.StordCardRecharge_Amount += o.PayAmount;
                                        gather.StordCardRecharge_Count++;
                                        break;
                                }
                                break;
                        }
                    }
                }
                #endregion
                gathers.Add(gather);
                hours++;
            }
            if (gathers != null)
            {
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    try
                    {
                        dboperator.BeginTransaction();
                        foreach (DateTime d in DeleteGather)
                        {
                            gatherfactory.DeleteGather(park.PKID, d, dboperator);
                        }
                        foreach (Statistics_Gather g in gathers)
                        {
                            if (!gatherfactory.Insert(g, dboperator))
                                throw new Exception("插入日统计数据失败");
                        }
                        dboperator.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Common.Services.TxtLogServices.WriteTxtLog("添加日统计(按小时统计)异常 异常信息:{0}", ex.Message);
                        dboperator.RollbackTransaction();
                    }
                }
            }

        }

        public void Statistics_DailyByHour(BaseParkinfo park, DateTime startdate, DateTime enddate)
        {
            int hours = 0;
            int maxhours = 23;
            List<Statistics_Gather> gathers = new List<Statistics_Gather>();
            List<DateTime> DeleteGather = new List<DateTime>();

            if (startdate.ToString("yyyyMMdd") == startdate.ToString("yyyyMMdd"))
            {
                maxhours = startdate.Hour;
            }
            IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
            IStatistics_Gather gatherfactory = Statistics_GatherFactory.GetFactory();
            IParkOrder iorder = ParkOrderFactory.GetFactory();

            while (hours <= maxhours)
            {
                Statistics_Gather gather = new Statistics_Gather();
                DateTime tempstartdate = startdate.AddHours(hours);
                DateTime tempenddate = enddate.AddHours(hours);
                gather.StatisticsGatherID = System.Guid.NewGuid().ToString();
                gather.ParkingID = park.PKID;
                gather.ParkingName = park.PKName;
                gather.GatherTime = tempstartdate;

                if (startdate.ToString("yyyyMMdd") == startdate.ToString("yyyyMMdd"))
                {
                    if (hours == maxhours || hours == maxhours - 1)
                    {
                        DeleteGather.Add(tempstartdate);
                    }
                    else
                    {
                        if (gatherfactory.IsExistsGather(park.PKID, tempstartdate))
                        {
                            hours++;
                            continue;
                        }
                    }
                }
                else
                {
                    if (gatherfactory.IsExistsGather(park.PKID, tempstartdate))
                    {
                        hours++;
                        continue;
                    }
                }
                //查询进场数
                gather.Entrance_Count = iorecordfactory.EntranceCountByParkingID(park.PKID, tempstartdate, tempenddate);
                //查询出场数
                gather.Exit_Count = iorecordfactory.ExitCountByParkingID(park.PKID, tempstartdate, tempenddate);

                #region 进场卡片类型
                List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByParkingID(park.PKID, tempstartdate, tempenddate);
                if (_InCardType != null && _InCardType.Count > 0)
                {
                    foreach (KeyValue k in _InCardType)
                    {
                        switch (k.KeyName)
                        {
                            case "0":
                                gather.VIPCard = k.Key_Value;
                                break;
                            case "1":
                                gather.StordCard = k.Key_Value;
                                break;
                            case "2":
                                gather.MonthCard = k.Key_Value;
                                break;
                            case "3":
                                gather.TempCard = k.Key_Value;
                                break;
                            case "4":
                                gather.JobCard = k.Key_Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion

                #region 统计放行类型
                List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByParkingID(park.PKID, tempstartdate, tempenddate);
                if (_ReleaseType != null && _ReleaseType.Count > 0)
                {
                    foreach (var s in _ReleaseType)
                    {
                        switch (s.KeyName)
                        {
                            case "0":
                                gather.ReleaseType_Normal = s.Key_Value;
                                break;
                            case "1":
                                gather.ReleaseType_Charge = s.Key_Value;
                                break;
                            case "2":
                                gather.ReleaseType_Free = s.Key_Value;
                                break;
                            case "3":
                                gather.ReleaseType_Catch = s.Key_Value;
                                break;
                        }
                    }
                }
                #endregion

                #region 统计费用相关
                List<ParkOrder> _orderfee = iorder.GetOrdersByParkingID(park.PKID, tempstartdate, tempenddate);
                if (_orderfee != null && _orderfee.Count > 0)
                {
                    foreach (ParkOrder o in _orderfee)
                    {
                        gather.Receivable_Amount += o.Amount;
                        gather.Real_Amount += o.PayAmount;
                        gather.Diff_Amount += o.UnPayAmount;
                        //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                        switch (o.PayWay)
                        {
                            case OrderPayWay.Cash:
                                gather.Cash_Amount += o.PayAmount;
                                gather.Cash_Count++;

                                if (o.DiscountAmount > 0)
                                {
                                    gather.CashDiscount_Amount += o.DiscountAmount;
                                    gather.CashDiscount_Count++;

                                    gather.Discount_Amount += o.DiscountAmount;
                                    gather.Discount_Count++;
                                }

                                break;
                            case OrderPayWay.WeiXin:
                            case OrderPayWay.Alipay:
                            case OrderPayWay.OnlineBanking:
                            case OrderPayWay.Wallet:
                                gather.OnLine_Amount += o.PayAmount;
                                gather.OnLine_Count++;

                                if (o.DiscountAmount > 0)
                                {
                                    gather.OnLineDiscount_Amount += o.DiscountAmount;
                                    gather.OnLineDiscount_Count++;

                                    gather.Discount_Amount += o.DiscountAmount;
                                    gather.Discount_Count++;
                                }
                                break;
                            case OrderPayWay.PreferentialTicket:
                                gather.Discount_Amount += o.DiscountAmount;
                                gather.Discount_Count++;
                                break;
                            case OrderPayWay.ValueCard:
                                gather.StordCard_Amount += o.PayAmount;
                                gather.StordCard_Count++;
                                break;
                        }
                        switch (o.OrderType)
                        {
                            //临时卡缴费
                            case OrderType.TempCardPayment:
                            case OrderType.AreaTempCardPayment:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineTemp_Amount += o.PayAmount;
                                        gather.OnLineTemp_Count++;
                                        break;
                                    default:
                                        gather.Temp_Amount += o.PayAmount;
                                        gather.Temp_Count++;
                                        break;
                                }
                                break;
                            //月卡续期
                            case OrderType.MonthCardPayment:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineMonthCardExtend_Amount += o.PayAmount;
                                        gather.OnLineMonthCardExtend_Count++;
                                        break;
                                    default:
                                        gather.MonthCardExtend_Amount += o.PayAmount;
                                        gather.MonthCardExtend_Count++;
                                        break;
                                }
                                break;
                            //VIP卡续期
                            case OrderType.VIPCardRenewal:
                                gather.VIPExtend_Count++;
                                break;
                            //储值卡充值
                            case OrderType.ValueCardRecharge:
                                switch (o.PayWay)
                                {
                                    case OrderPayWay.WeiXin:
                                    case OrderPayWay.Alipay:
                                    case OrderPayWay.OnlineBanking:
                                    case OrderPayWay.Wallet:
                                        gather.OnLineStordCard_Amount += o.PayAmount;
                                        gather.OnLineStordCard_Count++;
                                        break;
                                    default:
                                        gather.StordCardRecharge_Amount += o.PayAmount;
                                        gather.StordCardRecharge_Count++;
                                        break;
                                }
                                break;
                        }
                    }
                }
                #endregion
                gathers.Add(gather);
                hours++;
            }
            if (gathers != null)
            {
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    try
                    {
                        dboperator.BeginTransaction();
                        foreach (DateTime d in DeleteGather)
                        {
                            gatherfactory.DeleteGather(park.PKID, d, dboperator);
                        }
                        foreach (Statistics_Gather g in gathers)
                        {
                            if (!gatherfactory.Insert(g, dboperator))
                                throw new Exception("插入日统计数据失败");
                        }
                        dboperator.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Common.Services.TxtLogServices.WriteTxtLog("添加日统计(按小时统计)异常 异常信息:{0}", ex.Message);
                        dboperator.RollbackTransaction();
                    }
                }
            }

        }

        
    }
}
