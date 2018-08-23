 
using Common.Entities;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    /// 车牌或卡片识别方式
    /// </summary>
    class ValiLpDistinguishRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            if (args.CarTypeInfo.LpDistinguish == LpDistinguish.CardAndPlate)
            {
                if (args.CardInfo.Usercard.CardNo != args.InCardNo || args.CardInfo.OwnerPlateNumber.PlateNo != args.Plateinfo.LicenseNum)
                {
                    rst.ResCode = ResultCode.CarNoEqLp;
                }
            }
        }
    }
}
