using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.Utilities.WebHelper
{
    public static class DecodeHelper
    {   
        /// <summary>
        /// 解码对Base64Encode和encodeURIComponent UTF8
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Base64Decode(string val)
        {
            try
            {
                if (string.IsNullOrEmpty(val)) return string.Empty;
                if(string.Equals(val,"undefined",StringComparison.OrdinalIgnoreCase)) return  string.Empty;
                byte[] base64StringGoTicketPrice = Convert.FromBase64String(HttpUtility.UrlDecode(val));
                return Encoding.UTF8.GetString(base64StringGoTicketPrice);
            }
            catch{
               return val;
            }
        }
        /// <summary>
        /// 通过指定编码方式获取Http请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="getEncoding">get请求方式编码</param>
        /// <param name="postEncoding">post请求方式编码</param>
        /// <returns></returns>
        public static NameValueCollection GetRequestParameters(HttpRequest request,Encoding getEncoding,Encoding postEncoding)
        {
            if(request .HttpMethod .ToUpper()=="GET")
            {
                return HttpUtility.ParseQueryString(request.Url.Query, getEncoding);
            }

            using(var reader=new StreamReader(request .InputStream,postEncoding))
            {
                return HttpUtility.ParseQueryString(reader.ReadToEnd(), postEncoding);
            }
        }
    }
}
