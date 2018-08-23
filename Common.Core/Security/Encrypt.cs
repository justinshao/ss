using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Core.Security
{
    /// <summary>
    /// ��Rijndael�㷨�ļ���/���ܵ���.
    /// </summary>
    public class Encrypt {
        private static Encoding encoding = Encoding.Default;

        /// <summary>
        ///  ��ָ������Կ��ָ�����ַ������ܡ�
        /// </summary>
        /// <param name="originalStr">Դ�ַ���</param>
        /// <returns>�ɹ����ؼ��ܺ���ַ�,ʧ�ܷ���ԭ�ַ���/���ַ���</returns>
        public static string EncryptString(string originalStr) {
            return EncryptString(originalStr, "*&%$$FGad232f%^&&*()GTH$%3245gth", "@#%9286(^$jk$%#?", true);
        }

        public static string EncryptString(string originalStr, string key, string IV, bool returnBase64Code) {
            return EncryptString(originalStr, encoding.GetBytes(key), encoding.GetBytes(IV), returnBase64Code);
        }

        public static string EncryptString(byte[] data, string key, string IV, bool returnBase64Code)
        {
            return EncryptString(encoding.GetString(data,0,data.Length), encoding.GetBytes(key), encoding.GetBytes(IV), returnBase64Code);
        }

        public static string EncryptString(string originalStr, byte[] key, byte[] IV, bool returnBase64Code) {
            try {
                if (originalStr.Length == 0) return string.Empty;
                byte[] originalBytes = encoding.GetBytes(originalStr);
                byte[] sourceBytes;
                using (MemoryStream ms = new MemoryStream(originalBytes.Length)) {
                    using (RijndaelManaged rijndael = new RijndaelManaged()) {
                        using (ICryptoTransform rdTransform = rijndael.CreateEncryptor(key, IV)) {
                            using (CryptoStream cs = new CryptoStream(ms, rdTransform, CryptoStreamMode.Write)) {
                                cs.Write(originalBytes, 0, originalBytes.Length);
                                cs.FlushFinalBlock();
                                sourceBytes = ms.ToArray();
                            }
                        }
                        rijndael.Clear();
                    }
                }
                return (returnBase64Code ? Convert.ToBase64String(sourceBytes) : encoding.GetString(sourceBytes));
            }
            catch { }
            return string.Empty;
        }

        /// <summary>
        /// ��ָ������Կ��ָ�����ַ�������.
        /// </summary>
        /// <param name="originalStr">Դ�ַ���</param>
        /// <returns>���ܺ��ַ���</returns>
        public static string DecryptString(string originalStr) {
            return DecryptString(originalStr, "*&%$$FGad232f%^&&*()GTH$%3245gth", "@#%9286(^$jk$%#?", true);
        }

        public static string DecryptString(string originalStr, string key, string IV, bool isBase64Code) {
            if (originalStr.Length == 0) return string.Empty;

            byte[] originalBytes = (isBase64Code ? Convert.FromBase64String(originalStr) :
                encoding.GetBytes(originalStr));
            byte[] sourceBytes = new byte[originalBytes.Length];
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] IVBytes = encoding.GetBytes(IV);
            using (MemoryStream ms = new MemoryStream(originalBytes)) {
                using (RijndaelManaged rijndael = new RijndaelManaged()) {
                    using (ICryptoTransform rdTransform = rijndael.CreateDecryptor(keyBytes, IVBytes)) {
                        using (CryptoStream cs = new CryptoStream(ms, rdTransform, CryptoStreamMode.Read)) {
                            cs.Read(sourceBytes, 0, sourceBytes.Length);
                        }
                    }
                    rijndael.Clear();
                }
            }
            return encoding.GetString(sourceBytes).Trim(new char[] { '\0' });
        }

        public static string MD5Encrypt(string inputString)
        {
            if(string.IsNullOrEmpty(inputString)) return string.Empty;
            byte[] inputBytes = encoding.GetBytes(inputString);
            return MD5Encrypt(inputBytes);
        }

        public static string MD5Encrypt(byte[] inputBytes)
        {
            if(inputBytes == null || inputBytes.Length==0) return string.Empty;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptedBytes = md5.ComputeHash(inputBytes);
            md5.Clear();

            StringBuilder sb = new StringBuilder(encryptedBytes.Length);            
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);                
            }

            return sb.ToString().ToUpper();
        }


        public static Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
    }
}
