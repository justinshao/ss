using System;
using Common.Entities.Parking;
using Common.Entities;

namespace PKFee
{
    public class NaturalDay : IFeeRule
    {
        public NaturalDay()
        {
            
        }
        public NaturalDay(int isFirstFree)
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
            //今天结束时间
            DateTime dtThisDayEnd = dtLoopStart.AddDays(1).Date;
            //下一个自然天开始时间
            DateTime dtNextDayStart = dtLoopStart.AddDays(1).Date;
            //小段开始时间
            DateTime dtTmpStart = dtLoopStart;
            //今天收费限额
            decimal limit = RuleDetail.Limit == 0 ? int.MaxValue : (int)RuleDetail.Limit;
            //今天费用
            decimal sum = 0;

            #region 免费时段
            if ((!loop && IsFirstFree == 1) || (loop && RuleDetail.LoopType == LoopType.AllContain))
            {
                //免费时段超过今天结束时间，跳过今天算下一个自然天
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= dtThisDayEnd)
                    return Calc(true, dtNextDayStart, 0);

                //出场时间小于免费时段结束时间
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= ParkingEndTime)
                    return total;

               // dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }
            #endregion 免费时段

            #region 首时段
            if (!loop || (loop && (RuleDetail.LoopType == LoopType.OnlyFirstTime || RuleDetail.LoopType == LoopType.AllContain)))
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.FirstFee);
                //首时段超过今天结束时间，继续下一个自然天
                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= dtThisDayEnd)
                    return Calc(true, dtNextDayStart, sum);

                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FirstTime);
            }
            #endregion 首时段

            #region 按次收费
            if (RuleDetail.Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //如果出场时间不超过今天
                if (ParkingEndTime <= dtThisDayEnd)
                    return total + sum;
                //超过今天，继续算下一个自然天
                return Calc(true, dtNextDayStart, total + sum);
            }
            #endregion 按次收费

            #region 小循环段1
            //小循环段1总结束时间
            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)RuleDetail.Loop2Start);
            //没设定小循环段2，表示小循环段1循环到今天结束
            if (RuleDetail.Loop2Start == 0)
                dtLoop1End = dtThisDayEnd;
            //开始小循环
            while (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //本小循环段结束时间超过今天，继续算下一个自然天
                if (dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) >= dtThisDayEnd)
                    return Calc(true, dtNextDayStart, total + sum);
                //本小循环段结束时间超过出场时间
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
                //小循环段2循环到今天结束
                while (dtTmpStart < dtThisDayEnd)
                {
                    sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop2PerFee);
                    if (sum == limit)
                        break;
                    //本小循环段结束时间超过出场时间
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime);
                }
            }
            #endregion 小循环段2

            //出场时间大于今天结束时间
            if (ParkingEndTime > dtThisDayEnd)
                return Calc(true, dtNextDayStart, total + sum);

            return total + sum;
        }
    }
}
