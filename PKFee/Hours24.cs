using Common.Entities;
using Common.Entities.Parking;
using System;

namespace PKFee
{
    public class Hours24 : IFeeRule
    {
        public Hours24()
        {

        }
        public Hours24(int isFirstFree)
        {
            IsFirstFree = isFirstFree;
        }

        public override decimal CalcFee()
        {
            return Calc(false, ParkingBeginTime, 0);
        }

        /// <summary>
        /// 计算主体，大循环
        /// </summary>
        /// <param name="loop">标记"是否循环"</param>
        /// <param name="dtLoopStart">本次大循环内开始计费时间</param>
        /// <param name="total">累计费用</param>
        /// <returns>累计费用（已加上本次大循环的费用）</returns>
        public decimal Calc(bool loop, DateTime dtLoopStart, decimal total)
        {
            if (ParkingEndTime <= dtLoopStart)
                return total;

            //规则详情
            ParkFeeRuleDetail RuleDetail = listRuleDetail[0];
            //小段开始时间
            DateTime dtTmpStart = dtLoopStart;
            //本次大循环收费限额
            decimal limit = RuleDetail.Limit == 0 ? int.MaxValue : (int)RuleDetail.Limit;
            //本次大循环费用
            decimal sum = 0;

            #region 免费时段
            if ((!loop && IsFirstFree == 1) || (loop && RuleDetail.LoopType == LoopType.AllContain))
            {
                //出场时间小于免费时段结束时间
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= ParkingEndTime)
                    return total;
                //dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }
            #endregion 免费时段

            #region 首时段
            if (!loop || (loop && (RuleDetail.LoopType == LoopType.OnlyFirstTime || RuleDetail.LoopType == LoopType.AllContain)))
            {
                sum = Math.Min(limit, sum + RuleDetail.FirstFee);
                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FirstTime);
            }
            #endregion 首时段

            #region 按次收费
            if (RuleDetail.Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + RuleDetail.Loop1PerFee);
                //如果出场时间在24小时以内
                if (dtLoopStart.AddDays(1) >= ParkingEndTime)
                    return total + sum;
                //超过24小时，再来一个大循环
                return Calc(true, dtLoopStart.AddDays(1), total + sum);
            }
            #endregion 按次收费

            #region 小循环段1
            //小循环段1总结束时间
            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)RuleDetail.Loop2Start);
            //没设定小循环段2，表示小循环段1循环到入场24小时结束
            if (RuleDetail.Loop2Start == 0)
                dtLoop1End = dtLoopStart.AddDays(1);
            //开始小循环
            while (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + RuleDetail.Loop1PerFee);
                //出场时间小于本小循环段结束时间
                if (dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime);
            }
            #endregion 小循环段1

            #region 小循环段2
            if (RuleDetail.Loop2Start > 0 && RuleDetail.Loop2PerTime > 0)
            {
                //小循环段2总开始时间，其实就是小循环段1总结束时间
                //本小段开始时间
                dtTmpStart = dtLoopStart.AddMinutes((int)RuleDetail.Loop2Start);
                //小循环段2循环到入场24小时结束
                while (dtTmpStart < dtLoopStart.AddDays(1))
                {
                    sum = Math.Min(limit, sum + RuleDetail.Loop2PerFee);
                    if (sum == limit)
                        break;
                    //出场时间小于本小循环段结束时间
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime);
                }
            }
            #endregion 小循环段2

            //出场时间>本次大循环24小时
            if (ParkingEndTime > dtLoopStart.AddDays(1))
                return Calc(true, dtLoopStart.AddDays(1), total + sum);

            return total + sum;
        }
    }
}
