using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities.Helpers
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Unix起始时间
        /// </summary>
        public static DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 将Unix时间戳转换为当前时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳</param>
        /// <returns></returns>
        public static DateTime TransferUnixDateTime(long timeStamp)
        {
            return UnixTime.AddTicks((timeStamp + 8 * 60 * 60) * 10000000);
        }

        /// <summary>
        /// 将Unix时间戳转换为当前时间
        /// </summary>
        /// <param name="strTimeStamp">Unix时间戳</param>
        /// <returns></returns>
        public static DateTime TransferUnixDateTime(string strTimeStamp)
        {
            return TransferUnixDateTime(long.Parse(strTimeStamp));
        }

        /// <summary>
        /// 将时间转换为Unix时间戳
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns></returns>
        public static long TransferUnixDateTime(DateTime time)
        {
            return (time.Ticks - UnixTime.Ticks) / 10000000 - 8 * 60 * 60;
        }
    }
}
