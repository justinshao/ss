using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Core.Security
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
    }
}