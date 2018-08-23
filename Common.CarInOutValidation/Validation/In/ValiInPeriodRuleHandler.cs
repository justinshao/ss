using System.Collections.Generic;
using System.Linq;
using System;
using Common.Entities;
using Common.Services.Park;
using Common.Core.Expands;
using Common.Services;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    ///  验证有效期
    /// </summary>
    class ValiInPeriodRuleHandler : RuleHandler
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
            if (args.GateInfo.IoState == IoState.GoIn)
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
            if (args.CardInfo.PKLot.IsEmpty() || args.GateInfo.IoState != IoState.GoIn)
            {
                return;
            }
            if (args.CardInfo.PKLotNum <= 0)//没有车位忽略  如果有车位的话 根据车位查找在场车辆
            {
                return;
            }
            if(args.AreadInfo.IsNestArea)
            {
                if(args.NestAreaIORecord==null)
                {
                    return;
                }
                var interimes = ParkInterimServices.GetInterimByIOrecord(args.NestAreaIORecord.RecordID, out error);
                if (interimes != null && interimes.Find(i => i.EndInterimTime == DateTime.MinValue) != null)//没有正在转临停 说明是月卡
                {  
                    rst.InOutBaseCardType = BaseCarType.TempCar;
                    args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First();// ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                    args.CardInfo = null;
                    rst.ResCode = ResultCode.OverdueToTemp;
                    rst.IsOverdueToTemp = true;
                    rst.ValidMsg = "车位占用转临时车";
                    rst.EnterType = 2;
                } 
            }
            else
            {

                int occupyCount = 0;//如果有多个车位 且多个车位都被占用时 根据配置是否能进或转临停  
                var usercards = ParkGrantServices.GetCardgrantsByLot(args.AreadInfo.PKID, args.CardInfo.PKLot, out error);

                foreach (var card in usercards)
                {
                    var ownerPlateNumber = EmployeePlateServices.GetPlateNumber(args.AreadInfo.Parkinfo.VID, card.PlateID, out error);
                    if (ownerPlateNumber == null)
                    {
                        continue;
                    }
                    if (ownerPlateNumber.PlateNo == args.Plateinfo.LicenseNum)
                    {
                        continue;
                    }
                    var iorecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, ownerPlateNumber.PlateNo, out error);
                    if (iorecord == null)
                    {
                        continue;
                    }

                    var cardtype = ParkCarTypeServices.QueryParkCarTypeByRecordId(iorecord.CarTypeID);
                    var interimes = ParkInterimServices.GetInterimByIOrecord(iorecord.RecordID, out error);
                    if (interimes == null || interimes.Find(i => i.EndInterimTime == DateTime.MinValue) == null)//没有正在转临停 说明是月卡
                    {
                        occupyCount++;
                        continue;
                    }
                }

                if (occupyCount >= args.CardInfo.PKLotNum)
                {
                    if (args.CarTypeInfo.LotOccupy == LotOccupy.ProhibitedInAndOut)//不允许入场
                    {
                        rst.ResCode = ResultCode.NoPermissionsInOut;
                    }
                    else//固定转临时停车
                    {
                        rst.InOutBaseCardType = BaseCarType.TempCar;
                        args.CarTypeInfo = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar).First();// ShareData.CarTypes.Find(o => o.BaseTypeID == BaseCarType.TempCar);
                        args.CardInfo = null;
                        rst.ResCode = ResultCode.OverdueToTemp;
                        rst.IsOverdueToTemp = true;
                        rst.ValidMsg = "车位占用转临时车";
                        rst.EnterType = 2;
                    }
                }
            }
        }
    }
}
