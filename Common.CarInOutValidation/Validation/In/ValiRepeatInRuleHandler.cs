
using Common.Entities.Parking;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    /// 重复入场验证
    /// </summary>
    class ValiRepeatInRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            if (args.GateInfo.Box.IsCenterPayment == YesOrNo.No)//中心收费不判断重复出入场
            {
                CheckReaptIn(args, rst);
            }
        }

        private void CheckReaptIn(InputAgs args, ResultAgs rst)
        {
            if (args.AreadInfo.IsNestArea)
            {
                CheckNestReaptIn(args, rst);
            }
            else
            {
                CheckMainReaptIn(args, rst);
            }
        }

        private void CheckMainReaptIn(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            ParkIORecord ioRecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);

            if (args.CarTypeInfo.RepeatIn == YesOrNo.Yes && ioRecord != null && !ioRecord.IsExit)
            {
                //删除入场记录
                RemoveReaptInIorecord(args);
                ioRecord.DataStatus = 2; 
            }
            else
            {
                if (ioRecord != null && !ioRecord.IsExit)
                {
                    rst.ResCode = ResultCode.RepeatIn;
                }
            }
        }

        private void CheckNestReaptIn(InputAgs args, ResultAgs rst)
        {
            ParkTimeseries timeseries = null;
            string errorMsg = "";
            if (args.NestAreaIORecord != null)//月卡 VIP卡需要
            {
                timeseries = ParkTimeseriesServices.GetTimeseriesesByIORecordID(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
            }
            if (args.CarTypeInfo.RepeatIn == YesOrNo.Yes && timeseries != null)
            {
                //删除入场记录
                RemoveReaptInIorecord(args);
                timeseries.DataStatus = DataStatus.Delete;
                //SynchroService.UpLoadModle(timeseries);
            }
            else if (args.CarTypeInfo.RepeatIn != YesOrNo.Yes && timeseries != null)
            {
                rst.ResCode = ResultCode.RepeatIn;
            }
        }

        private void RemoveReaptInTimeseries(ParkTimeseries args)
        {
            ParkTimeseriesServices.RemoveTimeseries(args.TimeseriesID);
        }

        private void RemoveReaptInIorecord(InputAgs args)
        {
            string errorMsg = "";
            ParkIORecordServices.RemoveRepeatInIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
        }
    }
}
