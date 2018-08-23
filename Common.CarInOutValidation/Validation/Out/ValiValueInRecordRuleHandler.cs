using Common.Entities;
using Common.Entities.Validation;
using Common.Services;
using Common.Services.Park;

namespace Common.CarInOutValidation.Validation.Out
{
    class ValiValueInRecordRuleHandler : RuleHandler
    {

        protected override void ProcessRule(InputAgs args, ResultAgs rst)
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

        private void CheckNestAreaRepeatOut(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            var timeseries = ParkTimeseriesServices.GetTimeseriesesByIORecordID(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
            if (timeseries != null && !timeseries.IsExit)
            {
                args.Timeseries = timeseries;
                rst.InDate = args.Timeseries.EnterTime;
                rst.OutDate = args.Plateinfo.TriggerTime;
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.StoredValueCar;
            }
            else if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)
            {
                rst.ResCode = ResultCode.OnFindNo;
                rst.InOutBaseCardType = BaseCarType.StoredValueCar;
            }
            else
            {
                if (args.CarTypeInfo.RepeatOut == YesOrNo.No)
                {
                    rst.ResCode = ResultCode.RepeatOut;
                }
                else
                {
                    rst.ResCode = ResultCode.OutOK;
                    rst.InOutBaseCardType = BaseCarType.StoredValueCar;
                }
            }
        }

        private void CheckMainAreaRepeatOut(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            var iorecord = ParkIORecordServices.GetNoExitIORecordByCardNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
            if (iorecord == null)//可能是进场后再添加的储值卡
            {
                iorecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.PlayPlateNmber, out errorMsg);
                if (iorecord != null)
                {
                    iorecord.CarTypeID = args.CarTypeInfo.CarTypeID;
                    iorecord.CardNo = args.CardInfo.Usercard.CardNo;
                    iorecord.CardNumb = args.CardInfo.Usercard.CardNumb;
                    iorecord.CarTypeID = args.CarTypeInfo.CarTypeID;
                    ParkIORecordServices.ModifyIORecord(iorecord, out errorMsg);
                    //SynchroService.UpLoadModle(iorecord);
                }
            }
            if (iorecord != null && !iorecord.IsExit)
            {
                args.IORecord = iorecord;
                if (!iorecord.CarModelID.IsEmpty() && (iorecord.CarModelID.IsEmpty() || (!iorecord.CarModelID.IsEmpty() && args.CarModel.CarModelID != iorecord.CarModelID)))
                {
                    args.CarModel = ParkCarModelServices.QueryByRecordId(iorecord.CarModelID);
                }
                rst.InDate = args.IORecord.EntranceTime;
                rst.OutDate = args.Plateinfo.TriggerTime;
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.StoredValueCar;
            }
            else if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)
            {
                rst.ResCode = ResultCode.OnFindNo;
                rst.InOutBaseCardType = BaseCarType.StoredValueCar;
            }
            else
            {
                if (args.CarTypeInfo.RepeatOut == YesOrNo.No)
                {
                    rst.ResCode = ResultCode.RepeatOut;
                }
                else
                {
                    rst.ResCode = ResultCode.OutOK;
                    rst.InOutBaseCardType = BaseCarType.StoredValueCar;
                }
            }
        }
    }
}
