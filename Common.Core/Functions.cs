using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Functions
{
    public static string Upper(string bookingOffice)
    {
        if (string.IsNullOrEmpty(bookingOffice)) return "";
        return bookingOffice.ToUpper();
    }
    public static string CalMonthLongTime(DateTime? StartTimeValue, DateTime? EndTimeValue)
    {
        DateTime StartTime = DateTime.Now;
        if (StartTimeValue != null)
        {
            StartTime = StartTimeValue.Value;
        }

        DateTime EndTime = DateTime.Now;
        if (EndTimeValue != null)
        {
            EndTime = EndTimeValue.Value;
        }
        TimeSpan span = EndTime - StartTime;
        int days = Convert.ToInt32(span.TotalDays);
        int month = days / 30;
        int d = days % 30;
        if (d >= 15)
            month++;
        //总的季度数
        int quarter = month / 3;
        //剩余月份
        int syMonth = month % 3;
        //总年
        int syyear = quarter / 4;
        //剩余季度
        int syquarter = quarter %4;

        string restr = "";
        if (syyear > 0)
        {
            restr = restr + syyear + "年";
        }
        if (syquarter > 0)
        {
            restr = restr + syquarter + "季度";
        }
        if (syMonth > 0)
        {
            restr = restr + syMonth + "月";
        }
        return restr;
    }
    /// <summary>
    /// 计算停车时长
    /// </summary>
    /// <param name="EntranceTime">进场时间</param>
    /// <param name="ExitTime">出场时间</param>
    /// <returns></returns>
    public static string CalLongTime(DateTime EntranceTime, DateTime ExitTime)
    {
        if (EntranceTime == null)
        {
            return "";
        }
        if (ExitTime == DateTime.MinValue)
        {
            ExitTime = DateTime.Now;
        }
        string str = string.Empty;
        TimeSpan span = ExitTime - EntranceTime;
        return (span.Days < 100 ? span.Days.ToString("00") : span.Days.ToString()) + "天" + span.Hours.ToString("00") + "小时" + span.Minutes.ToString("00") + "分";
        //if (span.Days > 0)
        //{
           
        //}
        //if (span.Hours > 0)
        //{
        //    return span.Hours.ToString("00") + "小时" + span.Minutes.ToString("00") + "分";
        //}
        //if (span.Minutes > 0)
        //{
        //    return span.Minutes.ToString("00") + "分" + span.Seconds.ToString("00") + "秒";
        //}
        //else
        //{
        //    return "00分" + span.Seconds.ToString("00") + "秒";
        //}
    }
}
