
using Common.Entities;
using Common.Entities.Validation; 

namespace Common.CarInOutValidation.Validation.Out
{
    class NestAreaOutRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (!args.AreadInfo.IsNestArea)
            {
                return;
            }
            if (args.NestAreaIORecord == null)
            {
                if (args.AreadInfo.NeedToll == YesOrNo.Yes && args.CarTypeInfo.CarNoLike == YesOrNo.Yes)
                {
                    rst.ResCode = ResultCode.OnFindNo;
                }
                else if (args.CarTypeInfo.RepeatOut == YesOrNo.No)
                {
                    rst.ResCode = ResultCode.RepeatOut;
                }
            }
        }
    }
}
