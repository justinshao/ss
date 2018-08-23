
using Common.Entities.Parking;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Validation;
using Common.Services;
using System;

namespace Common.CarInOutValidation.Validation.Out
{
    /// <summary>
    /// 重复出场  车牌未找到 
    /// 开启模糊识别时：未找到出场纪录，表示车牌未找到 
    /// </summary>
    class ValiTempOutRecordRuleHandler : RuleHandler
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
            ParkTimeseries timeseries = null;
            string errorMsg = "";
            if (args.NestAreaIORecord != null)
            {
                timeseries = ParkTimeseriesServices.GetTimeseriesesByIORecordID(args.AreadInfo.PKID, args.NestAreaIORecord.RecordID, out errorMsg);
            }
            if (timeseries != null)
            {
                args.Timeseries = timeseries;
                rst.InDate = args.Timeseries.EnterTime;
                rst.OutDate = args.Plateinfo.TriggerTime;
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.TempCar;
            }
            else if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)//是否模糊识别 
            {
                rst.ResCode = ResultCode.OnFindNo;
                rst.InOutBaseCardType = BaseCarType.TempCar;
            }
            else //重复出场判断
            {
                if (args.CarTypeInfo.RepeatOut == YesOrNo.No && args.AreadInfo.NeedToll == YesOrNo.Yes)
                {
                    rst.ResCode = ResultCode.RepeatOut;
                }
                else
                {
                    rst.ResCode = ResultCode.OutOK;
                    rst.InOutBaseCardType = BaseCarType.TempCar;
                }
            }
        }

        private void CheckMainAreaRepeatOut(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            ParkIORecord isfindno = null;
            if (args.CardInfo != null)
            {
                isfindno = ParkIORecordServices.GetNoExitIORecordByCardNo(args.AreadInfo.PKID, args.CardInfo.Usercard.CardNo, out errorMsg);
            }
            else
            {
                isfindno = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            }
            if (isfindno != null)
            {
                args.IORecord = isfindno;
                if (!isfindno.CarModelID.IsEmpty() && (isfindno.CarModelID.IsEmpty() || (!isfindno.CarModelID.IsEmpty() && (args.CarModel != null && args.CarModel.CarModelID != isfindno.CarModelID))))
                {
                    args.CarModel = ParkCarModelServices.QueryByRecordId(isfindno.CarModelID);
                }

                var list = ParkCarModelServices.QueryByParkingId(args.AreadInfo.PKID);
                foreach (var item in list)
                {
                    if (item.PlateColor.ToInt() != 0 && item.PlateColor.ToInt() == args.Plateinfo.CarModelType)
                    {
                        args.CarModel = item;
                        break;
                    }
                }
                if (args.CarModel == null)
                {
                    //获取卡对应的车类型 
                    var carmode = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                    if (carmode != null)
                    {
                        args.CarModel = carmode;
                    }
                }

                rst.InDate = args.IORecord.EntranceTime;
                rst.OutDate = args.Plateinfo.TriggerTime;
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.TempCar;

            }
            else if (rst.IsPoliceFree)
            {
                rst.ResCode = ResultCode.OutOK;
                rst.InOutBaseCardType = BaseCarType.TempCar;
            }
            else if (args.CarTypeInfo.CarNoLike == YesOrNo.Yes)//是否模糊识别 
            {
                rst.ResCode = ResultCode.OnFindNo;
                rst.InOutBaseCardType = BaseCarType.TempCar;
            }
            else //重复出场判断
            {
                if (args.CarTypeInfo.RepeatOut == YesOrNo.No)
                {
                    rst.ResCode = ResultCode.RepeatOut;
                }
                else
                {
                    rst.ResCode = ResultCode.OutOK;
                    rst.InOutBaseCardType = BaseCarType.TempCar;
                }
            }
        }
    }
}
