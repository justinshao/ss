 
using Common.Entities;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.Out
{
    class VIPOutRuleHandler:RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = BaseCarType.VIPCar;
        }
    }
}
