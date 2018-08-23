using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    public class WxService
    {
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <param name="token">工作账号所设置的token</param>
        /// <param name="timestamp">Request请求的timestamp时间戳</param>
        /// <param name="nonce">Request请求的nonce随机数</param>
        /// <param name="signature">Request请求的signature微信加密签名</param>
        /// <returns></returns>
        public static bool CheckSignature(string token, string timestamp, string nonce, string signature)
        {
            return signature == GetSignature(token, timestamp, nonce);
        }
        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="token">工作账号所设置的token</param>
        /// <param name="timestamp">Request请求的timestamp时间戳</param>
        /// <param name="nonce">Request请求的nonce随机数</param>
        /// <returns></returns>
        public static string GetSignature(string token, string timestamp, string nonce)
        {
            string[] arr = { token, timestamp, nonce };
            Array.Sort(arr);   //字典排序
            var arrString = string.Join("", arr);
            return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(arrString))).Replace("-", "").ToLower();
        }
        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="token">工作账号所设置的token</param>
        /// <param name="timestamp">Request请求的timestamp时间戳</param>
        /// <param name="nonce">Request请求的nonce随机数</param>
        /// <returns></returns>
        public static string GetJsApiSignature(string noncestr, string jsapi_ticket, string timestamp, string url)
        {
            string[] arr =
            {
                string.Format("noncestr={0}", noncestr),
                string.Format("timestamp={0}", timestamp),
                string.Format("jsapi_ticket={0}", jsapi_ticket),
                string.Format("url={0}", url)
            };

            Array.Sort(arr); //字典排序
            var arrString = string.Join("&", arr);
            return
                BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(arrString)))
                    .Replace("-", "")
                    .ToLower();
        }
    }
}
