using Common.Entities;
using Common.Entities.Parking;
using System;
using System.Linq;

namespace PKFee
{
    public class DayNight : IFeeRule
    {
        public DayNight()
        {

        }
        public DayNight(int isFirstFree)
        {
            IsFirstFree = isFirstFree;
        }
        public override decimal CalcFee()
        {
            //return Calc_Day();
            return Calc_Day(false, ParkingBeginTime, 0);
        }

        /// <summary>
        /// 计算主体，白天
        /// </summary>
        /// <param name="loop">标记"是否循环"</param>
        /// <param name="dtLoopStart">本次大循环内开始计费时间</param>
        /// <param name="total">累计费用</param>
        /// <returns>累计费用（已加上本次大循环的费用）</returns>
        public decimal Calc_Day(bool loop, DateTime dtLoopStart, decimal total)
        {
            if (ParkingEndTime <= dtLoopStart)
                return total;

            //规则详情
            ParkFeeRuleDetail RuleDetail = (from d in listRuleDetail
                                            let start = Convert.ToDateTime(d.StartTime)
                                            let end = Convert.ToDateTime(d.EndTime)
                                            where start < end
                                            select d).FirstOrDefault();

            //本白天段所属日期取整
            string strDate = dtLoopStart.Date.ToString("yyyy-MM-dd ");
            //本白天段开始时间
            DateTime dtThisDayStart = Convert.ToDateTime(strDate + RuleDetail.StartTime);
            //本白天段结束时间
            DateTime dtThisDayEnd = Convert.ToDateTime(strDate + RuleDetail.EndTime);
            //如果入场时间不在本白天段内，从黑夜段开始算
            if (dtLoopStart < dtThisDayStart || dtLoopStart > dtThisDayEnd)
                return Calc_Night(false, dtLoopStart, 0);


            //小段开始时间
            DateTime dtTmpStart = dtLoopStart;
            //本白天段收费限额
            decimal limit = RuleDetail.Limit == 0 ? decimal.MaxValue : (decimal)RuleDetail.Limit;
            //本白天段费用
            decimal sum = 0;
            //是否补时
            bool supp = (bool)RuleDetail.Supplement;


            #region 免费时段
            if ((!loop && IsFirstFree == 1) || (loop && RuleDetail.LoopType == LoopType.AllContain))
            {
                //免费时段超过本白天段结束时间，跳过本白天段算下一个夜晚段
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= dtThisDayEnd && !supp)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, 0);
                }

                //免费时段结束时间超过出场时间
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= ParkingEndTime)
                    return total;

                 //dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }
            #endregion 免费时段

            #region 首时段
            if (!loop || (loop && (RuleDetail.LoopType == LoopType.OnlyFirstTime || RuleDetail.LoopType == LoopType.AllContain)))
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.FirstFee);
                //首时段超过本白天段结束时间，继续下一个夜晚段
                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, sum);
                }

                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FirstTime);
            }
            #endregion 首时段

            #region 按次收费
            if (RuleDetail.Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //如果出场时间不超过本白天段
                if (ParkingEndTime <= dtThisDayEnd)
                    return total + sum;
                //超过本白天段，继续算下一个夜晚段
                return Calc_Night(true, dtThisDayEnd, total + sum);
            }
            #endregion 按次收费

            #region 小循环段1
            //小循环段1总结束时间
            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)RuleDetail.Loop2Start);
            //没设定小循环段2，表示小循环段1循环到本白天段结束
            if (RuleDetail.Loop2Start == 0)
                dtLoop1End = dtThisDayEnd;
            //开始小循环
            while (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //本小循环段结束时间超过本白天段，继续算下一个夜晚段
                if (dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, total + sum);
                }

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
                //小循环段2循环到本白天段结束
                while (dtTmpStart < dtThisDayEnd)
                {
                    sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop2PerFee);
                    if (sum == limit)
                        break;

                    //本小循环段结束时间超过本白天段，继续算下一个夜晚段
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= dtThisDayEnd)
                    {
                        TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) - dtThisDayEnd : new TimeSpan());
                        return Calc_Night(true, dtThisDayEnd + tspan, total + sum);
                    }

                    //本小循环段结束时间超过出场时间
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime);
                }
            }
            #endregion 小循环段2

            //出场时间大于本白天段结束时间
            if (ParkingEndTime > dtThisDayEnd)
                return Calc_Night(true, dtThisDayEnd, total + sum);

            return total + sum;
        }


        /// <summary>
        /// 计算主体，夜晚
        /// </summary>
        /// <param name="loop">标记"是否循环"</param>
        /// <param name="dtLoopStart">本次大循环内开始计费时间</param>
        /// <param name="total">累计费用</param>
        /// <returns>累计费用（已加上本次大循环的费用）</returns>
        public decimal Calc_Night(bool loop, DateTime dtLoopStart, decimal total)
        {
            if (ParkingEndTime <= dtLoopStart)
                return total;

            //规则详情
            ParkFeeRuleDetail RuleDetail = (from d in listRuleDetail
                                            let start = Convert.ToDateTime(d.StartTime)
                                            let end = Convert.ToDateTime(d.EndTime)
                                            where start > end
                                            select d).FirstOrDefault();
            //本夜晚段所属日期取整
            string strDate = dtLoopStart.Date.ToString("yyyy-MM-dd ");
            //本夜晚段开始时间
            DateTime dtThisNightStart = Convert.ToDateTime(strDate + RuleDetail.StartTime);
            //本夜晚段结束时间
            DateTime dtThisNightEnd = Convert.ToDateTime(strDate + RuleDetail.EndTime);
            if (dtLoopStart < dtThisNightEnd)
            {
                dtThisNightStart = dtThisNightStart.AddDays(-1);
            }
            else if (dtLoopStart >= dtThisNightStart)
            {
                dtThisNightEnd = dtThisNightEnd.AddDays(1);
            }

            //小段开始时间
            DateTime dtTmpStart = dtLoopStart;
            //本夜晚段收费限额
            decimal limit = RuleDetail.Limit == 0 ? decimal.MaxValue : (decimal)RuleDetail.Limit;
            //本夜晚段费用
            decimal sum = 0;
            //是否补时
            bool supp = (bool)RuleDetail.Supplement;


            #region 免费时段
            if ((!loop && IsFirstFree == 1) || (loop && RuleDetail.LoopType == LoopType.AllContain))
            {
                //免费时段超过本夜晚段结束时间，跳过本夜晚段算下一个白天段
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= dtThisNightEnd && !supp)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, 0);
                }

                //免费时段结束时间超过出场时间
                if (dtTmpStart.AddMinutes((int)RuleDetail.FreeTime) >= ParkingEndTime)
                    return total;

               // dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }
            #endregion 免费时段

            #region 首时段
            if (!loop || (loop && (RuleDetail.LoopType == LoopType.OnlyFirstTime || RuleDetail.LoopType == LoopType.AllContain)))
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.FirstFee);
                //首时段超过本夜晚段结束时间，继续下一个白天段
                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= dtThisNightEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, sum);
                }

                if (dtTmpStart.AddMinutes((int)RuleDetail.FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FirstTime);
            }
            #endregion 首时段

            #region 按次收费
            if (RuleDetail.Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //如果出场时间不超过本夜晚段
                if (ParkingEndTime <= dtThisNightEnd)
                    return total + sum;
                //超过本夜晚段，继续算下一个白天段
                return Calc_Day(true, dtThisNightEnd, total + sum);
            }
            #endregion 按次收费

            #region 小循环段1
            //小循环段1总结束时间
            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)RuleDetail.Loop2Start);
            //没设定小循环段2，表示小循环段1循环到本夜晚段结束
            if (RuleDetail.Loop2Start == 0)
                dtLoop1End = dtThisNightEnd;
            //开始小循环
            while (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop1PerFee);
                //本小循环段结束时间超过本夜晚段，继续算下一个白天段
                if (dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) >= dtThisNightEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.Loop1PerTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, total + sum);
                }

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
                //小循环段2循环到本夜晚段结束
                while (dtTmpStart < dtThisNightEnd)
                {
                    sum = Math.Min(limit, sum + (decimal)RuleDetail.Loop2PerFee);
                    if (sum == limit)
                        break;

                    //本小循环段结束时间超过本夜晚段，继续算下一个白天段
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= dtThisNightEnd)
                    {
                        TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) - dtThisNightEnd : new TimeSpan());
                        return Calc_Day(true, dtThisNightEnd + tspan, total + sum);
                    }

                    //本小循环段结束时间超过出场时间
                    if (dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.Loop2PerTime);
                }
            }
            #endregion 小循环段2

            //出场时间大于本夜晚段结束时间
            if (ParkingEndTime > dtThisNightEnd)
                return Calc_Day(true, dtThisNightEnd, total + sum);

            return total + sum;
        }

        /// <summary>
        /// 惠州公园
        /// </summary>
        /// <returns></returns>
        public decimal Calc_Day()
        {
            decimal total = 0;
            TimeSpan ts = ParkingEndTime - ParkingBeginTime;
            long min = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
            if (min == 0)
            {
                return 0;
            }
           
            //规则详情
            ParkFeeRuleDetail DayRuleDetail = (from d in listRuleDetail
                                            let start = Convert.ToDateTime(d.StartTime)
                                            let end = Convert.ToDateTime(d.EndTime)
                                            where start < end
                                            select d).FirstOrDefault();
            //规则详情
            ParkFeeRuleDetail RuleDetail = (from d in listRuleDetail
                                            let start = Convert.ToDateTime(d.StartTime)
                                            let end = Convert.ToDateTime(d.EndTime)
                                            where start > end
                                            select d).FirstOrDefault();
            //本夜晚段所属日期取整
            string strDate = ParkingBeginTime.Date.ToString("yyyy-MM-dd ");
            //本白天段开始时间
            DateTime dtThisDayStart = Convert.ToDateTime(strDate + DayRuleDetail.StartTime);
            //本白天段结束时间
            DateTime dtThisDayEnd = Convert.ToDateTime(strDate + DayRuleDetail.EndTime);
            //如果入场时间不在本白天段内，从黑夜段开始算
            if (ParkingBeginTime < dtThisDayStart || ParkingBeginTime > dtThisDayEnd)
            {
                if (min > RuleDetail.FreeTime)
                {
                    total += ts.Days * RuleDetail.Limit;
                    if (min % (24 * 60) > RuleDetail.FreeTime)
                    {
                        total += RuleDetail.FirstFee;
                    }
                }
            }
            else 
            {
                if (min > DayRuleDetail.FreeTime)
                {
                    total += ts.Days * DayRuleDetail.Limit;
                    if (min  % (24 * 60) > DayRuleDetail.FreeTime)
                    {
                        total += DayRuleDetail.FirstFee;
                    }
                }
            }
            
            
            return total;
        }
    }
}
