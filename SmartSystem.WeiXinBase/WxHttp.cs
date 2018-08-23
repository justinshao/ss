using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Common.Utilities.Helpers;
using Common.Utilities;
using Common.Services;

namespace SmartSystem.WeiXinBase
{
    public static class WxHttp
    {
        #region Get & Post
        public static ResError Get(string url)
        {
            return Get<ResError>(url);
        }
        public static T Get<T>(string url)
        {
            var returnJson = HttpHelper.Get(url);


            TxtLogServices.WriteTxtLogEx("WeiXinReturn", "拉取微信用户信息,OPENID：{0}:{1}", url, returnJson);
            CheckThrowError(returnJson);
            return JsonHelper.GetJson<T>(returnJson);
        }

        public static ResError Post(string url, object data)
        {
            var jsonString = JsonHelper.GetJsonString(data);
            using (var ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var returnJson = HttpHelper.Post(url, null, ms);
                return JsonHelper.GetJson<ResError>(returnJson);
            }
        }

        public static T Post<T>(string url, object data)
        {
            var jsonString = JsonHelper.GetJsonString(data);
            using (var ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var returnJson = HttpHelper.Post(url, null, ms);
                CheckThrowError(returnJson);
                return JsonHelper.GetJson<T>(returnJson);
            }
        }

        public static T Post<T>(string url, string fileStream)
        {
            var c = new WebClient();
            var result = c.UploadFile(url, fileStream);
            var returnJson = Encoding.UTF8.GetString(result);
            //var returnJson = HttpUtils.Post(url, null, fileStream);
            CheckThrowError(returnJson);
            return JsonHelper.GetJson<T>(returnJson);
        }

        public static void CheckThrowError(string returnJson)
        {
            if (returnJson.Contains("errcode"))
            {
                //可能发生错误
                var errorResult = JsonHelper.GetJson<ResError>(returnJson);
                if (errorResult.errcode != ResCode.请求成功)
                {
                    throw new Exception(string.Format("微信请求发生错误！错误代码：{0}，说明：{1}", (int)errorResult.errcode, errorResult.errmsg));
                }
            }
        }
        #endregion


        /// <summary>
        /// 请求是否发起自微信客户端的浏览器
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsWxClientRequest(this HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Request.UserAgent) &&
                   httpContext.Request.UserAgent.Contains("MicroMessenger");
        }
    }
}
