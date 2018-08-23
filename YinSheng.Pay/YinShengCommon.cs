using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web;
using System.IO;

namespace YinSheng.Pay
{
    public class YinShengCommon
    {
        /// <summary>
        /// 签名加密
        /// </summary>
        /// <returns></returns>
        public static string SignEncrypt(string value)
        {
            try
            {
                X509Certificate2 pc = new X509Certificate2(AppDomain.CurrentDomain.BaseDirectory + YinShengConfig.PrivateLucPath, YinShengConfig.PrivateLucPwd);
                RSACryptoServiceProvider p = (RSACryptoServiceProvider)pc.PrivateKey;
                byte[] enBytes = p.SignData(Encoding.UTF8.GetBytes(value), new SHA1CryptoServiceProvider());
                p.Dispose();
                return Convert.ToBase64String(enBytes);
            }
            catch (Exception e)
            {
                //处理异常
            }
            return string.Empty;
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        public static bool SignVerify(string par, string sign)
        {
            try
            {
                X509Certificate2 pc = new X509Certificate2(AppDomain.CurrentDomain.BaseDirectory + YinShengConfig.BusinessgatePath);
                RSACryptoServiceProvider provider = (RSACryptoServiceProvider)pc.PublicKey.Key;
                bool result = provider.VerifyData(Encoding.UTF8.GetBytes(HttpUtility.UrlDecode(par)), new SHA1CryptoServiceProvider(), Convert.FromBase64String(sign));
                provider.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                //日志记录
            }
            return false;
        }

        /// <summary>
        /// 转换 QueryString 参数值
        /// </summary>
        public static PayDictionary TransQueryString(string msg)
        {
            PayDictionary par = new PayDictionary();
            string[] split = msg.Split(new Char[] { '&' });
            for (int i = 0; i < split.Length; i++)
            {
                int pos = split[i].IndexOf('=');
                if (pos > 0)
                {
                    if (pos < split[i].Length - 1)
                    {
                        par.Add(split[i].Substring(0, pos), split[i].Substring(pos + 1));
                    }
                    else
                    {
                        par.Add(split[i].Substring(0, pos), "");
                    }
                }
            }
            return par;
        }

        private static string key = "shanghu_test";

        /**/
        /// <summary> 
        /// 对称加密解密的密钥 
        /// </summary> 
        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /**/
        /// <summary> 
        /// DES加密 
        /// </summary> 
        /// <param name="encryptString"></param> 
        /// <returns></returns> 
        public static string DesEncrypt(string encryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /**/
        /// <summary> 
        /// DES解密 
        /// </summary> 
        /// <param name="decryptString"></param> 
        /// <returns></returns> 
        public static string DesDecrypt(string decryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        } 
    }
}
