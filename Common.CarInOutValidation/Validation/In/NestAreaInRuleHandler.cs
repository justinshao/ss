
using Common.Entities;
using Common.Entities.Validation; 

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    /// 小车场入场验证
    /// </summary>
    class NestAreaInRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (!args.AreadInfo.IsNestArea)
            {
                return;
            }
            if (args.NestAreaIORecord == null && (args.CarTypeInfo.BaseTypeID == BaseCarType.TempCar
                || args.CarTypeInfo.BaseTypeID == BaseCarType.StoredValueCar))//临时卡和储值卡不让进
            {
                if (args.GateInfo.IoState == IoState.GoIn)
                {

                    if (args.CarTypeInfo.RepeatIn != YesOrNo.Yes && rst.EnterType == 0)
                    {
                        rst.ResCode = ResultCode.OnFindNo;
                    }
                }
                else
                {
                    if (args.CarTypeInfo.RepeatOut != YesOrNo.Yes && rst.EnterType == 0)
                    {
                        rst.ResCode = ResultCode.RepeatOut;
                    }
                }
            }
        }
    }
}
