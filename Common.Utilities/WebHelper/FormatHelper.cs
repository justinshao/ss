using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities.WebHelper
{
    public static class FormatHelper
    {
        public static int? ToIntWithNull(string val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(val.Trim()))
                return null;
            return Convert.ToInt32(val.Trim());
        }

        public static float? ToFloatWithNull(string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            return Convert.ToSingle(val.Trim());
        }

        public static double? ToDoubleWithNull(string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            return Convert.ToDouble(val.Trim());
        }

        public static decimal? ToDecimalWithNull(string val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(val.Trim()))
                return null;
            return Convert.ToDecimal(val.Trim());
        }

        public static decimal? ToDecimalWithNull(string val,Func<decimal,decimal> convert )
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(val.Trim()))
                return null;
            return convert(Convert.ToDecimal(val.Trim()));
        }

        public static DateTime? ToDateTimeWithNull(string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            return Convert.ToDateTime(val.Trim());
        }

        /// <summary>
        /// 从数字转换为星期字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetWeekdayInfo(int? value)
        {
            if (value == null)
                return "1/2/3/4/5/6/7";

            int[] weeks = new int[] { 1, 2, 4, 8, 16, 32, 64 };
            StringBuilder result = new StringBuilder(14);
            
            for (int i = 0; i < weeks.Length; i++)
            {
                if (result.Length > 0)
                    result.Append("/");

                if ((weeks[i] & value) == weeks[i])
                {
                    result.Append(i + 1);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 佣金截位，返回小于或者等于指定小数的最大整数
        /// </summary>
        /// <param name="commission">佣金</param>
        /// <returns>佣金</returns>
        public static decimal FormatCommission(decimal commission)
        {
            return Math.Floor(commission);
        }

        /// <summary>
        /// 折扣四舍五入
        /// </summary>
        /// <param name="commission">折扣</param>
        /// <returns>折扣</returns>
        public static decimal FormatReplyDiscount(decimal discount)
        {
            return Calculator.Round(discount, -2);
        }

        /// <summary>
        /// 航空公司排序(按照国航，南航，东航，海航，川航排序)
        /// </summary>
        /// <param name="ie">航空公司列表</param>
        /// <returns>排序结果</returns>
        public static IEnumerable<string> SortAirlines(IEnumerable<string> ie)
        {
            string array = "3U;HU;MU;CZ;CA";
            if (ie == null)
                return Enumerable.Empty<string>();
            return ie.OrderBy(s => -array.IndexOf(s));
        }

        /// <summary>
        /// 将字符串中的无效 XML 字符替换为与其等效的有效 XML 字符
        /// </summary>
        /// <param name="content">xml字符串</param>
        /// <returns>有效 XML 字符</returns>
        public static string EscapeXML(string content)
        {
            return System.Security.SecurityElement.Escape(content);
        }

        public static string ConvertToString(DateTime? dateTime) {
            if (!dateTime.HasValue) return string.Empty;
            return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
