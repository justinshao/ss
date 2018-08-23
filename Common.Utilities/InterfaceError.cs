using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class InterfaceError
    {
        public static readonly string BaseFormat = "<l ecode=\"{0}\" emsg=\"{1}\">{2}</l>";
         public static string Get(string code)
        {
            if (ErrorInfo.ContainsKey(code))
                return ErrorInfo[code];
            return code;
        }
        /// <summary>
        /// 在异常创建时调用解析
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Decode(string code)
        {
            return string.Format(BaseFormat, code, Get(code), string.Empty);
        }
        public static string Success(string xml) {
            return string.Format(BaseFormat, "0000", Get("0000"), xml);
        }
        public static readonly Dictionary<string, string> ErrorInfo = new Dictionary<string, string>();

        static InterfaceError()
        {
            ErrorInfo.Add("0000", "调用成功");
            ErrorInfo.Add("0001", "未知错误");
            ErrorInfo.Add("0002", "获取楼盘信息失败");
            ErrorInfo.Add("0003", "参数houseId格式不正确（只能为数字）");
            ErrorInfo.Add("0004", "无此楼盘信息");
            ErrorInfo.Add("0005", "获取楼盘详情失败");
            ErrorInfo.Add("0006", "获取楼盘图片失败");
        }
        public static readonly string NoError = "0000";
        public static readonly string UnknownError = "0001";
    }
}
