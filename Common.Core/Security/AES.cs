using System;
using System.Text;
using System.Security.Cryptography;

namespace Common.Core.Security
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public class AES
    {
        /// <summary>
        /// 默认密钥
        /// </summary>
        private static string _key = ")O[N@]g,!F}+sfc$j{+#ESb^*8>Z'e&M";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的base64数据</returns>
        public static string Encrypt(string toEncrypt, string key = null)
        {
            if (key == null)
            {
                key = _key;
            }
            byte[] keyArray = Encoding.Default.GetBytes(key);
            byte[] toEncryptArray = Encoding.Default.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length, Base64FormattingOptions.None);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">要解密的base64数据</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string toDecrypt, string key = null)
        {
            if (key == null)
            {
                key = _key;
            }
            byte[] keyArray = Encoding.Default.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.Default.GetString(resultArray).TrimEnd('\0');
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的数据</returns>
        public static byte[] EncryptToBytes(string toEncrypt, string key = null)
        {
            if (key == null)
            {
                key = _key;
            }
            byte[] keyArray = Encoding.Default.GetBytes(key);
            byte[] toEncryptArray = Encoding.Default.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">要解密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptFromBytes(byte[] toDecrypt, string key = null)
        {
            if (key == null)
            {
                key = _key;
            }
            byte[] keyArray = Encoding.Default.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);

            return Encoding.Default.GetString(resultArray).TrimEnd('\0');
        }
    }
}
