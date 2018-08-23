using System;
namespace PKFee
{
    public class UserdefinedText
    {
        public decimal CalcFee(DateTime starttime, DateTime endtime)
        {
            starttime = DateTime.Parse(starttime.ToString("yyyy-MM-dd HH:mm:ss"));
            endtime = DateTime.Parse(endtime.ToString("yyyy-MM-dd HH:mm:ss"));
            decimal money = 0;

            TimeSpan ts = endtime - starttime;
            long min = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
            if (ts.Seconds > 0)
            {
                min += 1;
            }
            if (min <= 30)
            {
                return 0;
            }

            money += min / 1440 * 75;
            long min2 = min % 1440;
            if (min2 > 30 && min2 <= 2 * 60)
            {
                money += 20;
            }
            else if (min2 > 2 * 60 && min2 <= 6 * 60)
            {
                money += 40;
            }
            else if (min2 > 6 * 60 && min2 <= 12 * 60)
            {
                money += 60;
            }
            else if (min2 > 12 * 60 && min2 <= 24 * 60)
            {
                money += 75;
            }
            return money;
        }

    }
}
