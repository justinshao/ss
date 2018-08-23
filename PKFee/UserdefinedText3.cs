using System;
namespace PKFee
{
    public class UserdefinedText3
    {
        public decimal CalcFee(DateTime starttime, DateTime endtime)
        {
          
            return Calc_Day(false, starttime, 0, endtime);

        }
        public decimal Calc_Day(bool loop, DateTime dtLoopStart, decimal total, DateTime ParkingEndTime)
        {
            if (ParkingEndTime <= dtLoopStart)
                return total;

            string StartTime = "8:00";
            string EndTime = "22:00";
            decimal Limit = 0;
            bool Supplement = true;
            int IsFirstFree = 1;
            int LoopType = 1;
            int FreeTime = 15;
            int FirstFee = 2;
            int FirstTime = 30;
            int Loop1PerTime = 15;
            int Loop1PerFee = 2;
            int Loop2Start = 120;
            int Loop2PerTime = 15;
            int Loop2PerFee = 2;

            string strDate = dtLoopStart.Date.ToString("yyyy-MM-dd ");

            DateTime dtThisDayStart = Convert.ToDateTime(strDate + StartTime);

            DateTime dtThisDayEnd = Convert.ToDateTime(strDate + EndTime);

            if (dtLoopStart < dtThisDayStart || dtLoopStart > dtThisDayEnd)
                return Calc_Night(false, dtLoopStart, 0, ParkingEndTime);


            DateTime dtTmpStart = dtLoopStart;

            decimal limit = Limit == 0 ? decimal.MaxValue : Limit;

            decimal sum = 0;

            bool supp = Supplement;



            if ((!loop && IsFirstFree == 1) || (loop && LoopType == 3))
            {

                if (dtTmpStart.AddMinutes(FreeTime) >= dtThisDayEnd && !supp)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes(FreeTime) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, 0, ParkingEndTime);
                }


                if (dtTmpStart.AddMinutes(FreeTime) >= ParkingEndTime)
                    return total;

                //dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }


            if (!loop || (loop && (LoopType == 2 || LoopType == 3)))
            {
                sum = Math.Min(limit, sum + (decimal)FirstFee);

                if (dtTmpStart.AddMinutes(FirstTime) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes(FirstTime) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, sum, ParkingEndTime);
                }

                if (dtTmpStart.AddMinutes((int)FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)FirstTime);
            }



            if (Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + (decimal)Loop1PerFee);

                if (ParkingEndTime <= dtThisDayEnd)
                    return total + sum;

                return Calc_Night(true, dtThisDayEnd, total + sum, ParkingEndTime);
            }



            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)Loop2Start);

            if (Loop2Start == 0)
                dtLoop1End = dtThisDayEnd;

            if (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)2);

                if (dtTmpStart.AddMinutes((int)15) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)15) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, total + sum, ParkingEndTime);
                }


                if (dtTmpStart.AddMinutes((int)15) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)15);
            }


            if (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)2);

                if (dtTmpStart.AddMinutes((int)15) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)75) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, total + sum, ParkingEndTime);
                }
                if (dtTmpStart.AddMinutes((int)15) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)15);
            }

            if (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)2);

                if (dtTmpStart.AddMinutes((int)15) >= dtThisDayEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)75) - dtThisDayEnd : new TimeSpan());
                    return Calc_Night(true, dtThisDayEnd + tspan, total + sum, ParkingEndTime);
                }
                if (dtTmpStart.AddMinutes((int)60) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)60);
            }

            if (Loop2Start > 0 && Loop2PerTime > 0)
            {

                dtTmpStart = dtLoopStart.AddMinutes((int)Loop2Start);

                while (dtTmpStart < dtThisDayEnd)
                {
                    if (dtTmpStart.AddMinutes((int)Loop2PerTime) <= ParkingEndTime)
                    {
                        sum = Math.Min(limit, sum + (decimal)Loop2PerFee);
                    }
                    if (sum == limit)
                        break;


                    if (dtTmpStart.AddMinutes((int)Loop2PerTime) >= dtThisDayEnd)
                    {
                        TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)Loop2PerTime) - dtThisDayEnd : new TimeSpan());
                        return Calc_Night(true, dtThisDayEnd + tspan, total + sum, ParkingEndTime);
                    }


                    if (dtTmpStart.AddMinutes((int)Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)Loop2PerTime);
                }
            }



            if (ParkingEndTime > dtThisDayEnd)
                return Calc_Night(true, dtThisDayEnd, total + sum, ParkingEndTime);

            return total + sum;
        }
        public decimal Calc_Night(bool loop, DateTime dtLoopStart, decimal total, DateTime ParkingEndTime)
        {
            if (ParkingEndTime <= dtLoopStart)
                return total;
            string strDate = dtLoopStart.Date.ToString("yyyy-MM-dd ");
            string StartTime = "21:00";
            string EndTime = "7:00";
            DateTime dtThisNightStart = Convert.ToDateTime(strDate + StartTime);
            DateTime dtThisNightEnd = Convert.ToDateTime(strDate + EndTime);
            if (dtLoopStart < dtThisNightEnd)
            {
                dtThisNightStart = dtThisNightStart.AddDays(-1);
            }
            else if (dtLoopStart >= dtThisNightStart)
            {
                dtThisNightEnd = dtThisNightEnd.AddDays(1);
            }

            DateTime dtTmpStart = dtLoopStart;

            decimal limit = decimal.MaxValue;

            decimal sum = 0;

            bool supp = true;
            int IsFirstFree = 1;
            int LoopType = 1;
            int FreeTime = 15;
            decimal FirstFee = 1;
            int FirstTime = 120;
            int Loop1PerTime = 120;
            int Loop1PerFee = 1;
            int Loop2Start = 0;
            int Loop2PerTime = 0;
            decimal Loop2PerFee = 0;

            if ((!loop && IsFirstFree == 1) || (loop && LoopType == 3))
            {

                if (dtTmpStart.AddMinutes((int)FreeTime) >= dtThisNightEnd && !supp)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)FreeTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, 0, ParkingEndTime);
                }


                if (dtTmpStart.AddMinutes((int)FreeTime) >= ParkingEndTime)
                    return total;

                // dtTmpStart = dtTmpStart.AddMinutes((int)RuleDetail.FreeTime);
            }


            if (!loop || (loop && (LoopType == 2 || LoopType == 3)))
            {
                sum = Math.Min(limit, sum + FirstFee);

                if (dtTmpStart.AddMinutes((int)FirstTime) >= dtThisNightEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)FirstTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, sum, ParkingEndTime);
                }

                if (dtTmpStart.AddMinutes((int)FirstTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)FirstTime);
            }

            if (Loop1PerTime == 0)
            {
                sum = Math.Min(limit, sum + (decimal)Loop1PerFee);

                if (ParkingEndTime <= dtThisNightEnd)
                    return total + sum;

                return Calc_Day(true, dtThisNightEnd, total + sum, ParkingEndTime);
            }


            DateTime dtLoop1End = dtLoopStart.AddMinutes((int)Loop2Start);

            if (Loop2Start == 0)
                dtLoop1End = dtThisNightEnd;

            while (dtTmpStart < dtLoop1End)
            {
                sum = Math.Min(limit, sum + (decimal)Loop1PerFee);

                if (dtTmpStart.AddMinutes((int)Loop1PerTime) >= dtThisNightEnd)
                {
                    TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)Loop1PerTime) - dtThisNightEnd : new TimeSpan());
                    return Calc_Day(true, dtThisNightEnd + tspan, total + sum, ParkingEndTime);
                }


                if (dtTmpStart.AddMinutes((int)Loop1PerTime) >= ParkingEndTime)
                    return total + sum;
                dtTmpStart = dtTmpStart.AddMinutes((int)Loop1PerTime);
            }

            if (Loop2Start > 0 && Loop2PerTime > 0)
            {

                dtTmpStart = dtLoopStart.AddMinutes((int)Loop2Start);

                while (dtTmpStart < dtThisNightEnd)
                {
                    sum = Math.Min(limit, sum + (decimal)Loop2PerFee);
                    if (sum == limit)
                        break;


                    if (dtTmpStart.AddMinutes((int)Loop2PerTime) >= dtThisNightEnd)
                    {
                        TimeSpan tspan = (supp ? dtTmpStart.AddMinutes((int)Loop2PerTime) - dtThisNightEnd : new TimeSpan());
                        return Calc_Day(true, dtThisNightEnd + tspan, total + sum, ParkingEndTime);
                    }


                    if (dtTmpStart.AddMinutes((int)Loop2PerTime) >= ParkingEndTime)
                        return total + sum;
                    dtTmpStart = dtTmpStart.AddMinutes((int)Loop2PerTime);
                }
            }


            if (ParkingEndTime > dtThisNightEnd)
                return Calc_Day(true, dtThisNightEnd, total + sum, ParkingEndTime);

            return total + sum;
        }

    }
}
