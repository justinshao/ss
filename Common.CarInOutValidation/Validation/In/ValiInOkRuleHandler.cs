 
using Common.Entities;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    class ValiInOkRuleHandler:RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.ResCode = ResultCode.InOK;
        }
    }
}
