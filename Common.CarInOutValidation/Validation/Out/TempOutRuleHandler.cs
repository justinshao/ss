
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Enum;
using Common.Entities.Validation;
using Common.Services;

namespace Common.CarInOutValidation.Validation.Out
{
    class TempOutRuleHandler : RuleHandler
    {
        public string[] FreeCarStartContains = new string[] { "WJ", "军", "空", "海", "北", "沈", "兰", "济", "南", "广", "成", "甲", "V", "Z", "B", "N", "G", "S", "C", "L", "J", "K", "H" };
        public string[] FreeCarEndContains = new string[] { "警" };
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = BaseCarType.TempCar;
            string errorMsg = "";
            if (args.CarTypeInfo.IsIgnoreHZ && args.Plateinfo.LicenseNum.Length >= 7)
            {
                var plateNumber = args.Plateinfo.LicenseNum.Substring(1, args.Plateinfo.LicenseNum.Length - 1);
                var iorecord = ParkIORecordServices.QueryInCarTempIORecordByLikePlateNumber(args.AreadInfo.PKID, plateNumber, out errorMsg);
                if (iorecord != null)
                {
                    args.Plateinfo.LicenseNum = iorecord.PlateNumber;
                }
            }
            IsPoliceFree(args, rst);

            var blackList = ParkBlacklistServices.GetBlacklist(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            if (blackList != null)
            {
                if (blackList.Status == BlackListStatus.NotEnterAndOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                    return;
                }
                if (args.GateInfo.IoState == IoState.GoOut
                    && blackList.Status == BlackListStatus.CanEnterAndNotOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                    return;
                }
            }
            var vistar = ParkVisitorServices.GetNormalVisitor(args.AreadInfo.Parkinfo.VID, args.Plateinfo.LicenseNum, out errorMsg);
            if (vistar != null && args.Plateinfo.TriggerTime >= vistar.BeginDate && args.Plateinfo.TriggerTime <= vistar.EndDate
                && (vistar.VisitorCount > 0 || vistar.VisitorCount == -1))
            { 
                var vistarcar = ParkVisitorServices.GetVisitorCar(args.AreadInfo.PKID, vistar.RecordID, out errorMsg);
                if (vistarcar != null && (vistar.VisitorCount == -1 || vistar.VisitorCount > vistarcar.AlreadyVisitorCount))
                {
                    //访客表中  在时段内 且进出次数大于已经进出次数时 允许进出 
                    var carModelid = vistar.CarModelID;
                    if (!carModelid.IsEmpty())
                    {
                        var carModel = ParkCarModelServices.QueryByRecordId(vistar.CarModelID);
                        if (carModel != null)
                        {
                            args.CarModel = carModel;
                        }
                    }
                    rst.ValidMsg = "访客车辆";
                    rst.IsVisitorCar = true;
                    rst.VisitorCar = vistarcar;
                    return;
                }
                else if (args.GateInfo.IsTempInOut == YesOrNo.No)
                {
                    rst.ResCode = ResultCode.NoPermissionsInOut;
                    return;
                }
            }
            else  if (args.GateInfo.IsTempInOut == YesOrNo.No)//子区域允许月卡进出
            {
                if (!rst.IsPoliceFree)
                {
                    rst.ResCode = ResultCode.NoPermissionsInOut;
                    return;
                }
            }
        }

    }
}
