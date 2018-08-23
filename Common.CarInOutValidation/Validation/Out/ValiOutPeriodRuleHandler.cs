using System.Collections.Generic; 
using System.Linq; 
using System;
using Common.Entities; 
using Common.Services.Park;
using Common.Core.Expands;
using Common.Services;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.Out
{
    /// <summary>
    ///  验证有效期
    /// </summary>
    class ValiOutPeriodRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;

            if (args.CardInfo == null)
            {
                return;
            }
            if (args.CardInfo.State == ParkGrantState.Pause || args.CardInfo.State == ParkGrantState.Stop)
            {
                //rst.ResCode = ResultCode.CarLocked;
                //rst.ValidMsg = args.CardInfo.State == 1 ? "月卡 停用" : "月卡 暂停";
                rst.ResCode = args.CardInfo.State == ParkGrantState.Pause ? ResultCode.MonthCarStop : ResultCode.MonthCarPause;
                return;
            }
            string error;
            var iorecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out error);

            if (args.GateInfo.IoState == IoState.GoOut)//过期用户  或者入场时间在有效时间之外
            {
                if (args.CardInfo.EndDate < args.Plateinfo.TriggerTime || args.CardInfo.EndDate == null)
                {
                    if (args.CarTypeInfo.OverdueToTemp == OverdueToTemp.ProhibitedInAndOut)//不允许入场
                    {
                        rst.ResCode = ResultCode.UserExpired;
                        return;
                    }
                    else//临停转固定
                    {

                        if (iorecord != null)
                        {
                            if (args.CardInfo.EndDate > iorecord.EntranceTime)
                            {
                                rst.OverdueToTempSpan = args.Plateinfo.TriggerTime - args.CardInfo.EndDate;
                            }
                            else
                            {
                                rst.OverdueToTempSpan = args.Plateinfo.TriggerTime - iorecord.EntranceTime;
                            }
                        }
                        rst.InOutBaseCardType = BaseCarType.TempCar;
                        args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First();// ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                        args.CardInfo = null;
                        rst.ResCode = ResultCode.OverdueToTemp;
                        rst.ValidMsg = "月卡车不在有效期内";
                        rst.EnterType = 1;
                        return;
                    }
                }

                if (iorecord != null)
                {
                    //如果是出场 还有入场记录的话 还需要判断入场时间在有效时间之外
                    if (args.CardInfo.BeginDate > iorecord.EntranceTime)
                    {
                        if (args.CarTypeInfo.OverdueToTemp == OverdueToTemp.ProhibitedInAndOut)//不允许入场
                        {
                            rst.ResCode = ResultCode.UserExpired;
                            return;
                        }
                        else//临停转固定
                        {
                            if (args.CardInfo.BeginDate != null)
                            {
                                if (args.CardInfo.BeginDate > args.Plateinfo.TriggerTime)
                                {
                                    rst.OverdueToTempSpan = args.Plateinfo.TriggerTime - iorecord.EntranceTime;
                                }
                                else
                                {
                                    rst.OverdueToTempSpan = args.CardInfo.BeginDate - iorecord.EntranceTime;
                                }
                            }
                            rst.InOutBaseCardType = BaseCarType.TempCar;
                            args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First();// ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                            args.CardInfo = null;
                            rst.ResCode = ResultCode.OverdueToTemp;
                            rst.ValidMsg = "月卡车不在有效期内";
                            rst.EnterType = 1;
                            return;
                        }
                    }
                }
            }

            if (args.AreadInfo.IsNestArea)//只在大车场判断车位
            {
                return;
            }
            if (args.GateInfo.IoState == IoState.GoOut)
            { // 出场时根据PkInterim表判断是否临停转固定  

                if (iorecord != null)
                {
                    var interimes = ParkInterimServices.GetInterimByIOrecord(iorecord.RecordID, out error);
                    if (interimes != null && interimes.Count > 0)//计算临停的时间
                    {
                        interimes = interimes.OrderBy(i => i.StartInterimTime).ToList();
                        DateTime date = DateTime.MinValue;
                        TimeSpan span = new TimeSpan();
                        foreach (var item in interimes)
                        {
                            if (item.EndInterimTime != null)
                            {
                                span += item.EndInterimTime - item.StartInterimTime;
                            }
                            else//如果存在 没有结束时间的 转固定记录  一般是最后一条，如果有多条  也只取最后一条
                            {
                                date = item.StartInterimTime;
                            }
                        }
                        if (date != DateTime.MinValue)
                        {
                            span += args.Plateinfo.TriggerTime - date;
                        }
                        rst.OverdueToTempSpan = span;
                        rst.InOutBaseCardType = BaseCarType.TempCar;
                        args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First(); // ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                        args.CardInfo = null;
                        rst.ResCode = ResultCode.OverdueToTemp;
                        rst.IsOverdueToTemp = true;
                        rst.EnterType = 2;
                        rst.ValidMsg = "月卡 临停：" + (span.Days > 0 ? span.ToString(@"dd\天hh\小\时mm\分\钟") : span.ToString(@"hh\小\时mm\分\钟"));
                    }
                }
            }

            if (iorecord != null && rst.OverdueToTempSpan.TotalSeconds <= 0)//如果以上都不是 判断是否有停入未授权区域
            {
                var list = ParkTimeseriesServices.GetAllExitsTimeseriesesByIORecordID(args.AreadInfo.PKID, iorecord.RecordID, out error);

                TimeSpan timespan = new TimeSpan();
                foreach (var item in list)
                {
                    if (!args.CardInfo.GateID.IsEmpty()
                          && !args.CardInfo.GateID.Contains(item.ExitGateID))//区域包含 且通道未控制授权 或 通道已经授权
                    {
                        //进入过未授权的区域 
                        timespan += args.Plateinfo.TriggerTime - item.EnterTime;
                    }
                }
                if (timespan.TotalSeconds > 0)//进入过未授权区域
                {
                    rst.OverdueToTempSpan = timespan;
                    rst.InOutBaseCardType = BaseCarType.TempCar;
                    args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First();// ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                    args.CardInfo = null;
                    rst.ResCode = ResultCode.OverdueToTemp;
                    rst.ValidMsg = "月卡在未授权区域停放";
                    rst.EnterType = 1;
                    return;
                }
            }
        }
    }
}
