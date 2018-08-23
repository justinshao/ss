using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utilities.Helpers
{
    public class StringHelper
    {
        private static Random random = new Random();
        /// <summary>
        /// 返回几位的随机数字,第一位不为零
        /// </summary>
        public static string GetRndNumberString(int digit)
        {
            if (digit <= 0)
                return string.Empty;
            const string ALL_CHAR = "0123456789";
            var strRnd = new StringBuilder();
            //var random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            lock (random)
            {
                for (int i = 0; i < digit; i++)
                {
                    if (i > 0)
                        strRnd.Append(ALL_CHAR[random.Next(ALL_CHAR.Length)]);
                    else
                        strRnd.Append(ALL_CHAR[random.Next(1, ALL_CHAR.Length)]);
                }
            }
            return strRnd.ToString();
        }

        /// <summary>
        /// 返回几位的随机字符串,由字母及数字组成,不含字母O及数字0,即ABCDEFGHIJKLMNPQRSTUVWXYZ123456789
        /// </summary>       
        public static string GetRndString(int digit)
        {
            if (digit <= 0)
                return string.Empty;
            const string ALL_CHAR = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            var strRnd = new StringBuilder();
            lock (random)
            {
                for (int i = 0; i < digit; i++)
                {
                    strRnd.Append(ALL_CHAR[random.Next(ALL_CHAR.Length)]);
                }
            }
            return strRnd.ToString();
        }

        /// <summary>
        /// 返回源字符串的16或32位MD5加密结果
        /// </summary>
        public static string GetMD5String(string source, int digit)
        {
            if (string.IsNullOrEmpty(source))
                source = string.Empty;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(source));
            string strResult = BitConverter.ToString(bytResult).Replace("-", "");

            if (digit == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return strResult.Substring(8, 16);
                //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Source, "MD5").Substring(8, 16);
            }
            if (digit == 32) //32位加密 
            {
                return strResult;
                //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Source, "MD5");
            }
            return "00000000000000000000000000000000";
        }

        public static string GetMD5String(string source)
        {
            return GetMD5String(source, 32);
        }

        /// <summary>
        /// 将UBB编码转换成WML编码
        /// </summary>
        public static string UBBToWML(string strUBBBody)
        {
            if (string.IsNullOrEmpty(strUBBBody))
                return string.Empty;
            System.Text.RegularExpressions.Regex r;
            Match m;
            #region 处理&号
            strUBBBody = strUBBBody.Replace("&", "&amp;");
            #endregion
            #region 处理空格
            strUBBBody = strUBBBody.Replace(" ", "&nbsp;");
            #endregion
            #region 处理单引号
            strUBBBody = strUBBBody.Replace("'", "'");
            #endregion
            #region 处理双引号
            strUBBBody = strUBBBody.Replace("\"", "&quot;");
            #endregion
            #region html标记符
            strUBBBody = strUBBBody.Replace("<", "&lt;");
            strUBBBody = strUBBBody.Replace(">", "&gt;");
            #endregion
            #region 处理回车
            strUBBBody = strUBBBody.Replace("\r\n", "<br/>");
            #endregion
            #region 处[b][/b]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[b\])([ \S\t]*?)(\[\/b\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(), "<b>" + m.Groups[2].ToString() + "</b>");
            }
            #endregion
            #region 处[i][/i]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[i\])([ \S\t]*?)(\[\/i\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(), "<i>" + m.Groups[2].ToString() + "</i>");
            }
            #endregion
            #region 处[u][/u]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[U\])([ \S\t]*?)(\[\/U\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(), "<u>" + m.Groups[2].ToString() + "</u>");
            }
            #endregion
            #region 处[IMG][/IMG]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[IMG\])(.[^\[]*)(\[\/IMG\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(), "<img src=\"" + m.Groups[2].ToString() + "\" alt=\"Loading\" />");
            }
            #endregion
            #region 处理图片链接[imglink][/imglink]标记
            //处理图片链接
            r = new System.Text.RegularExpressions.Regex(@"(\[IMGLINK\])(http|https|ftp):\/\/([ \S\t]*?)(\[\/IMGLINK\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(), "<a href=\"" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + "\"><img src=\"" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + "\" alt=\"Loading...\" /></a>");
            }
            #endregion
            #region 处[url][/url]标记
            //处[url][/url]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[URL\])(.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(),
                    "<a href=\"" + m.Groups[2].ToString() + "\">" +
                    m.Groups[2].ToString() + "</a>");
            }
            #endregion
            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new System.Text.RegularExpressions.Regex(@"(\[URL=(.[^\]]*)\])(.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
            for (m = r.Match(strUBBBody); m.Success; m = m.NextMatch())
            {
                strUBBBody = strUBBBody.Replace(m.Groups[0].ToString(),
                    "<a href=\"" + m.Groups[2].ToString() + "\">" +
                    m.Groups[3].ToString() + "</a>");
            }
            #endregion
            #region 处理[center][left][right]标记
            strUBBBody = strUBBBody.Replace("<br/>[center]", "</p><p align=\"center\">");
            strUBBBody = strUBBBody.Replace("[center]", "</p><p align=\"center\">");
            strUBBBody = strUBBBody.Replace("<br/>[left]", "</p><p align=\"left\">");
            strUBBBody = strUBBBody.Replace("[left]", "</p><p align=\"left\">");
            strUBBBody = strUBBBody.Replace("<br/>[right]", "</p><p align=\"right\">");
            strUBBBody = strUBBBody.Replace("[right]", "</p><p align=\"right\">");
            #endregion

            return strUBBBody;
        }

        /// <summary>
        /// 将编码转换成可以在文本框中显示的符号
        /// </summary>
        public static string ToInputBox(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            //input = input.Replace("\n", "&#10;");
            //input = input.Replace("\r", "&#13;");
            //input = input.Replace(">", "&#62;");
            //input = input.Replace("\"", "&#34;");
            input = input.Replace("\"", "'");
            input = input.Replace("&", "&#38;");
            input = input.Replace("<", "&#60;");
            input = input.Replace("$", "$$");
            return input;
        }

        /// <summary>
        /// 验证用户名是否符合指定条件(长短,大小写字母,数字加下划线)
        /// </summary>
        public static bool IsValidateUsername(string username, int minlength, int maxlength)
        {
            if (!string.IsNullOrEmpty(username))
                return System.Text.RegularExpressions.Regex.IsMatch(username, @"^\w{" + minlength.ToString() + "," + maxlength.ToString() + "}$");
            return false;
        }

        /// <summary>
        /// 验证密码是否符合指定条件(长短,字符)
        /// </summary>
        public static bool IsValidatePassword(string password, int minlength, int maxlength)
        {
            if (!string.IsNullOrEmpty(password))
                return System.Text.RegularExpressions.Regex.IsMatch(password, @"^.{" + minlength + "," + maxlength + "}$");
            return false;
        }

        /// <summary>
        /// 验证邮件地址是否正确(Beta,待测试,改进.未对如xxx.com.cn,.mobi等域名做测试)
        /// </summary>
        public static bool IsValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return System.Text.RegularExpressions.Regex.IsMatch(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return false;
        }

        public static bool IsValidateIP(string ip)
        {
            if (!string.IsNullOrEmpty(ip))
                return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^(([01]?\d?\d|2[0-4]\d|25[0-5])\.){3}([01]?\d?\d|2[0-4]\d|25[0-5])$");
            return false;
        }

        public static bool IsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            System.Text.RegularExpressions.Regex RegNumber = new System.Text.RegularExpressions.Regex("^[0-9]+$");
            Match m = RegNumber.Match(str);
            return m.Success;
            //for (int i = 0; i < str.Length; i++)
            //{
            //    if (str[i] < '0' || str[i] > '9')
            //        return false;
            //}
            //return true;
        }

        /// <summary>
        /// 取按指定宽度截取的字符串,截取部分以"..."代替
        /// </summary>
        public static string GetCutedString(object obj, int width)
        {
            if (obj == null)
                return string.Empty;
            return GetCutedString(obj.ToString(), width, true);
        }

        public static string GetCutedString(object obj, int width, bool IsAddDot)
        {
            if (obj == null)
                return string.Empty;
            string str = obj.ToString();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            Encoding encoding = Encoding.GetEncoding(936);
            int bytesCount = encoding.GetByteCount(str);
            if (bytesCount <= width)
                return str;
            StringBuilder strCuted = new StringBuilder();
            int i = 0;
            int length = 0;
            while (length < width)
            {
                strCuted.Append(str.Substring(i, 1));
                length = encoding.GetByteCount(strCuted.ToString());
                i++;
            }
            if (encoding.GetByteCount(strCuted.ToString()) >= width && strCuted.Length > 1)
                strCuted.Remove(strCuted.Length - 1, 1);
            if (IsAddDot)
            {
                if (strCuted.Length > 1 && encoding.GetByteCount(strCuted.ToString()) >= width)
                    strCuted.Remove(strCuted.Length - 1, 1);
                strCuted.Append("...");
            }
            return strCuted.ToString();
        }

        /// <summary>
        /// 如source: "xxx=yyy&yyy=zzz", 则GetQueryStringValue(source,"xxx")的返回值为"yyy"
        /// </summary> 
        public static string GetQueryStringValue(string source, string name)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(string.Format(@"(^|&){0}=(?<value>(.*?))(&|$)", name), RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(source);
            if (mc.Count > 0)
            {
                return mc[0].Result("${value}");
            }
            return string.Empty;
        }

        /// <summary>
        /// 是否为手机号码,即13,14,15,17,18开头的11位数字  20140828新增17
        /// </summary> 
        public static bool IsValidateMobile(string strMobile)
        {
            if (string.IsNullOrEmpty(strMobile))
                return false;
            return System.Text.RegularExpressions.Regex.Match(strMobile, @"^1(3|4|5|7|8)[0-9]{9}$").Success;
        }

        /// <summary>
        /// 是否为手机号码或小灵通号码
        /// </summary> 
        public static bool IsValideMobileExt(string strMobile)
        {
            if (string.IsNullOrEmpty(strMobile))
                return false;
            return System.Text.RegularExpressions.Regex.Match(strMobile, @"^((1(3|4|5|7|8)[0-9]{9})|(0([0-9]{10,11})))$").Success;
        }

        /// <summary>
        /// 取号码运营商
        /// </summary> 
        public static TelecomsOperators GetTelecomsOperator(string mobile)
        {
            if (!IsValideMobileExt(mobile))
                return TelecomsOperators.Unknown;
            //移动号段 134.135.136.137.138.139.147.150.151.152.157.158.159.182.183.187.188  //2011-09-08 新增183号段 20120509新增181,20140815新增184、178
            if (System.Text.RegularExpressions.Regex.Match(mobile, @"^1(3[4-9]|47|5[012789]|7[8]|8[23478])\d{8}|705\d{7}$").Success)
                return TelecomsOperators.ChinaMobile;
            //新联通 130.131.132.155.156.185.186.145   //2011-11-12 新增145号段 20140815新增176 20140828 新增1709
            if (System.Text.RegularExpressions.Regex.Match(mobile, @"^1(3[012]|45|5[56]|7[6]|8[56])\d{8}|709\d{7}$").Success)
                return TelecomsOperators.ChinaUnicom;
            //新电信 133.153.180.189.181 20140815新增177,20140828 新增1700
            if (System.Text.RegularExpressions.Regex.Match(mobile, @"^1(33|53|77|8[019])\d{8}|700\d{7}$").Success)
                return TelecomsOperators.ChinaTelecom;
            return TelecomsOperators.Other;
        }

        public enum TelecomsOperators
        {
            Unknown = 0,
            ChinaMobile = 1,
            ChinaUnicom = 2,
            ChinaTelecom = 3,
            Other = 4   //其它如小灵通号码无法识别是联通还是电信的
        }
        public static string ToUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }
        public static string FromUnicodeString(string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return System.Text.RegularExpressions.Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }
    }
}
