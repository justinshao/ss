
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Entities;
using Common.Entities.Validation;
using Common.Services;

namespace Common.CarInOutValidation.Validation.Out
{
    /// <summary>
    /// 查找入场记录
    /// </summary>
    class ValiMonthInRecordRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (args.AreadInfo.IsNestArea)
            {
                FindNestAreaIorecord(args, rst);
            }
            else
            {
                FindMainAreaIorecord(args, rst);
            }
        }
        /// <summary>
        /// 主区域入场记录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="rst"></param>
        private void FindMainAreaIorecord(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            var iorecord = ParkIORecordServices.GetNoExitIORecordByCardNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
            if (iorecord == null) //根据卡找未找到 可能是进场固定转临停 这个时候根据车牌找记录 找到后更新进出记录
            {
                iorecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.CardInfo.OwnerPlateNumber.PlateNo, out errorMsg);
                if (iorecord != null)
                {
                    iorecord.CarTypeID = args.CarTypeInfo.CarTypeID;
                    iorecord.CardNo = args.CardInfo.Usercard.CardNo;
                    iorecord.CardNumb = args.CardInfo.Usercard.CardNumb;
                    iorecord.CarTypeID = args.CarTypeInfo.CarTypeID;
                    ParkIORecordServices.ModifyIORecord(iorecord, out errorMsg);
                    //SynchroService.UpLoadModle<ParkIORecord>(iorecord);
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
                rst.InOutBaseCardType = BaseCarType.MonthlyRent;
            }
            else
            {
                if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)
                {
                    rst.ResCode = ResultCode.OnFindNo;
                    rst.InOutBaseCardType = BaseCarType.MonthlyRent;
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
                        rst.InOutBaseCardType = BaseCarType.MonthlyRent;
                    }
                }
            }
        }

        /// <summary>
        /// 内场在场记录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="rst"></param>
        private void FindNestAreaIorecord(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            ParkTimeseries timeseries = null;
            if (args.NestAreaIORecord != null)
            {
                timeseries = ParkTimeseriesServices.GetTimeseriesesByIORecordID(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
            }
            if (timeseries != null && !timeseries.IsExit)
            {
                args.Timeseries = timeseries;
                rst.InDate = args.Timeseries.EnterTime;
                rst.OutDate = args.Plateinfo.TriggerTime;
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.MonthlyRent;
            }
            else
            {
                if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)
                {
                    rst.ResCode = ResultCode.OnFindNo;
                    rst.InOutBaseCardType = BaseCarType.MonthlyRent;
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
                        rst.InOutBaseCardType = BaseCarType.MonthlyRent;
                    }
                }
            }
        }
    }
}
