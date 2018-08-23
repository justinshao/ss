 
using Common.Services.Park;
using Common.Entities;
using Common.Entities.Parking;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.Out
{
    class ValiRepeatOutRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (args.GateInfo.Box.IsCenterPayment==YesOrNo.No)//中心收费不判断重复出入场
            {
                if (args.AreadInfo.IsNestArea)
                {
                    CheckNestAreaRepeatOut(args, rst);
                }
                else
                {
                    CheckMainAreaRepeatOut(args, rst);
                }
            }
        }

        private void CheckNestAreaRepeatOut(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            var timeseries = ParkTimeseriesServices.GetTimeseriesesByIORecordID(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            if ((timeseries == null || timeseries.IsExit) && args.CarTypeInfo.RepeatOut == YesOrNo.No)
            {
                rst.ResCode = ResultCode.RepeatOut;
            }
        }

        private void CheckMainAreaRepeatOut(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            ParkIORecord iorecord = null;
            if (args.CardInfo != null)
            {
                iorecord = ParkIORecordServices.GetNoExitIORecordByCardNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
            }
            else
            {
                iorecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            }
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            if ((iorecord == null || iorecord.IsExit) && args.CarTypeInfo.RepeatOut == YesOrNo.No)
            {
                rst.ResCode = ResultCode.RepeatOut;
            }
        }
    }
}
