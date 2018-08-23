using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common.Core.Security
{
    /// <summary>
    /// DES加密解密
    /// </summary>
    public class DES
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string _key = @"(@+$w1XZ"; 

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string IV
        {
            get { return @"L%n67}G\Mk@k%:~Y"; }
        }

        
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainStr">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的base64字符串</returns>
        public static string Encrypt(string plainStr, string key = null)
        {
            if (key == null)
            {
                key = _key;    
            }
            byte[] bKey = Encoding.UTF8.GetBytes(key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = System.Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return encrypt;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptStr">需要解密的base64字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的数据</returns>
        public static string Decrypt(string encryptStr, string key = null)
        {
            if (key == null)
            {
                key = _key;
            }
            byte[] bKey = Encoding.UTF8.GetBytes(key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return decrypt;
        }
    }
}
