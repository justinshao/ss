using Common.Entities;
using Common.Entities.Enum;
using Common.Entities.Validation;
using Common.Services.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.CarInOutValidation.Validation
{
    /// <summary>
    /// 黑名单 单双号严重
    /// </summary>
    class VailBlacklistRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            var blackList = ParkBlacklistServices.GetBlacklist(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            if (blackList != null)
            {

                if (blackList.Status == BlackListStatus.NotEnterAndOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                }
                else if (args.GateInfo.IoState == IoState.GoOut
                    && blackList.Status == BlackListStatus.CanEnterAndNotOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                }
                else
                {
                    rst.ValidMsg = "黑名单车辆";
                }
            }
            if (args.CardInfo != null && args.CarTypeInfo != null)
            {
                var siglelist = ParkCarTypeSingleServices.QueryParkCarTypeByCarTypeID(args.CarTypeInfo.CarTypeID);
                if (siglelist == null || siglelist.Count <= 0)
                {
                    return;
                }
                var sigle = siglelist.Find(s => s.Week == (int)DateTime.Now.DayOfWeek);
                if (sigle == null)
                {
                    return;
                }
                if (sigle.SingleType == 0)
                {
                    return;
                }
                // sigle.SingleType 0 单双可进出 1 单可进出 2 双可进出
                if (args.Plateinfo.LicenseNum.Length < 1)
                {
                    return;
                }
                List<string> stringplates = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
                    "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                var cplate = args.Plateinfo.LicenseNum.Substring(args.Plateinfo.LicenseNum.Length - 2, 1);
                var index = 0;
                if (cplate.IsInteger())
                {
                    index = cplate.ToInt();
                }
                else
                {
                    index = stringplates.IndexOf(cplate) + 1;
                }
                if (index.IsEvennumber() && sigle.SingleType == 2)//偶数
                {
                    return;
                }
                else if (!index.IsEvennumber() && sigle.SingleType == 1)//奇数
                {
                    return; 
                }
                else
                {
                    rst.ResCode = ResultCode.NoPermissionsInOut; 
                    rst.ValidMsg = "车辆在限行时间";
                }
            }
        }
    }
}
