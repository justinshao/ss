
using Common.Entities;
using Common.Entities.Validation; 

namespace Common.CarInOutValidation.Validation.Out
{
    /// <summary>
    /// 月卡出验证
    /// </summary>
    class MonthOutRuleHandler : RuleHandler
    {

        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = BaseCarType.MonthlyRent;
        }
    }
}
