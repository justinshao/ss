using Common.Entities;
using Common.Entities.Validation; 
namespace Common.CarInOutValidation.Validation.In
{
    class ValueInRuleHandler:RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = BaseCarType.StoredValueCar;
        }
    }
}
