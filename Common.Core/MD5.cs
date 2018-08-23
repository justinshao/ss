using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Common.Core
{
    public class MD5
    {
        /// <summary>
        /// MD5加密，使用UTF8字符编码
        /// </summary>
        /// <param name="intput">待加密字符串</param> 
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] md5Data = md5.ComputeHash(data);
            md5.Clear();
            return md5Data.Aggregate("", (current, t) => current + t.ToString("X2"));
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="intput">待加密字符串</param>
        /// <param name="encoding">字符集</param>
        /// <returns></returns>
        public static string Encrypt(string input, Encoding encoding)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = encoding.GetBytes(input);
            byte[] md5Data = md5.ComputeHash(data);
            md5.Clear();
            return md5Data.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        public static string md5_Encode(string str)
        {
            str = System.Web.HttpUtility.UrlEncode(str);
            str = str.ToLower();
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToLower();

        }
        private static string Bytes2Hex(byte[] bytes)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }

    }
}
