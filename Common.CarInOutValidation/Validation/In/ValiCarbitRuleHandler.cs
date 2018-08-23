
using Common.Entities;
using Common.Services.Park;
using Common.Core.Expands;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.In
{
    /// <summary>
    /// 验证车位信息
    /// </summary>
    class ValiCarbitRuleHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = args.CarTypeInfo.BaseTypeID;
            string errorMsg = "";
            if (args.AreadInfo.IsNestArea)
            {
                uint count = ParkTimeseriesServices.GetIsEditCarNum(args.AreadInfo.AreaID, out errorMsg).ToUInt();
                if (args.CarBitNumes == null)
                {
                    return;
                }
                var carBitNum = args.CarBitNumes.Find(a => a.Area.AreaID == args.AreadInfo.AreaID);
                uint surplusCount = 0;
                if (count >= carBitNum.CarBitCount)
                {
                    surplusCount = 0;
                    if (args.CarTypeInfo.AllowLose == YesOrNo.No && args.GateInfo.IoState == IoState.GoIn)
                    {
                        rst.ResCode = ResultCode.NoCarBit;
                    }
                }
                else
                {
                    surplusCount = carBitNum.CarBitCount - count;
                }
                if (surplusCount != carBitNum.SurplusCount)//如果和内存中的车位剩余数量不同时，更新所有的车位信息
                { 
                    carBitNum.InParkCarNum = count;
                }
            }
            else
            {
                uint count = ParkIORecordServices.GetIsEditCarNum(args.AreadInfo.AreaID, out errorMsg).ToUInt();
                if (args.CarBitNumes == null)
                {
                    return;
                }
                var carBitNum = args.CarBitNumes.Find(a => a.Area.AreaID == args.AreadInfo.AreaID);
                if (carBitNum == null)
                {
                    return;
                }
                uint surplusCount = 0;
                if (count >= carBitNum.CarBitCount)
                {
                    surplusCount = 0;
                    if (args.CarTypeInfo.AllowLose == YesOrNo.No && args.GateInfo.IoState == IoState.GoIn)
                    {
                        rst.ResCode = ResultCode.NoCarBit;
                    }
                }
                else
                {
                    surplusCount = carBitNum.CarBitCount - count;
                } 
                if (surplusCount != carBitNum.SurplusCount)//如果和内存中的车位剩余数量不同时，更新所有的车位信息
                {
                    carBitNum.InParkCarNum = count;
                }
            }
        }
    }
}
