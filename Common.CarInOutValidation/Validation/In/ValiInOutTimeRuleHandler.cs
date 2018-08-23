using System;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    /// 验证进出最小间隔
    /// </summary>
    class ValiInOutTimeRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (args.AreadInfo.IsNestArea && args.NestAreaIORecord == null)
            {
                return;
            }
            string errorMsg = "";
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            DateTime? datetime;

            if (args.GateInfo.IoState == IoState.GoIn)//入场时间
            {
                if (args.CardInfo == null)
                {
                    if (args.AreadInfo.IsNestArea)
                    {
                        datetime = ParkTimeseriesServices.GetLastRecordExitDate(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
                        var tempdatetime = ParkTimeseriesServices.GetLastRecordEnterTime(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
                        if (tempdatetime > datetime)
                        {
                            datetime = tempdatetime;
                        }
                    }
                    else
                    {
                        datetime = ParkIORecordServices.GetLastRecordExitDateByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
                        var tempdatetime = ParkIORecordServices.GetLastRecordEnterTimeByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
                        if (tempdatetime > datetime)
                        {
                            datetime = tempdatetime;
                        }
                    }
                }
                else
                {
                    if (args.AreadInfo.IsNestArea)
                    {
                        datetime = ParkTimeseriesServices.GetLastRecordExitDate(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
                    }
                    else
                    {
                        datetime = ParkIORecordServices.GetLastRecordExitDateByCarNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
                    }
                }
            }
            else //出场时间
            {
                if (args.CardInfo == null)
                {
                    if (args.AreadInfo.IsNestArea)
                    {
                        datetime = ParkTimeseriesServices.GetLastRecordEnterTime(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
                    }
                    else
                    {
                        datetime = ParkIORecordServices.GetLastRecordEnterTimeByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
                    }
                }
                else
                {
                    if (args.AreadInfo.IsNestArea)
                    {
                        datetime = ParkTimeseriesServices.GetLastRecordEnterTime(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
                    }
                    else
                    {
                        datetime = ParkIORecordServices.GetLastRecordEnterTimeByCarNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
                    }
                }
            }
            if (datetime != null)
            {
                TimeSpan tstem = args.Plateinfo.TriggerTime - datetime.Value;
                double s = tstem.TotalSeconds;// tstem.Days * 24 * 60 * 60 + tstem.Hours * 60 * 60 + tstem.Minutes * 60 + tstem.Seconds;
                if (args.CarTypeInfo.InOutTime > 0 && s < args.CarTypeInfo.InOutTime)
                {
                    rst.ResCode = ResultCode.InOutTime;
                }
                else
                {
                    if (args.GateInfo.IoState == IoState.GoIn)
                    {
                        rst.ResCode = ResultCode.InOK;
                    }
                }
            }
            else
            {
                if (args.GateInfo.IoState == IoState.GoIn)
                {
                    rst.ResCode = ResultCode.InOK;
                }
            }
        }
    }
}
