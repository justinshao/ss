using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class DataEncryption
    {
        public static string ToMD5(this string source)
        {
            return GetMD5(source);
        }
        public static string GetMD5(string info)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info, "MD5");
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">明文</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string key , string value)
        {
            DES des = DES.Create();
            byte[] data = Encoding.UTF8.GetBytes(value);
            string s = GetMD5(key).ToLower().Substring(0, 8);
            des.Key = Encoding.ASCII.GetBytes(s);
            des.IV = des.Key;
            byte[] result;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    result = ms.ToArray();
                }
            }

            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
