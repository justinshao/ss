 
using Common.Entities;
using Common.Services.WeiXin;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.Out
{
    class ValiLockCarRuleHander : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            
            var lockcar = WX_LockCarServices.GetLockCar(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            if (lockcar != null && lockcar.Status == 1)
            {
                //已经锁车后 无法出场
                rst.ResCode = ResultCode.CarLocked;
            }
        }
    }
}
