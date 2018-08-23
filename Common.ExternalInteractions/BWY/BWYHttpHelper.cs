using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Common.ExternalInteractions.BWY
{
    public class BWYHttpHelper
    {
        public static string RequestUrl(string webPageUrl,string sessionId,int timeout = 50000, string encode = "")
        {
            HttpWebRequest web = (HttpWebRequest)WebRequest.Create(webPageUrl);
            web.Headers.Add("SessionID", sessionId);
            web.Method = "GET";
            web.Timeout = timeout;

            using (Stream stream = web.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
        public static string RequestUrl(string url, string sessionId, string content, string requestMethod = "POST")
        {
            string restr = "";
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            try
            {

                byte[] data = Encoding.UTF8.GetBytes(content);
                // 设置参数  
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Headers.Add("SessionID", sessionId);
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = requestMethod;
                request.ContentType = "application/json";  //application/xml
                request.ContentLength = data.Length;

                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码  
                restr = sr.ReadToEnd();
            }
            catch { }
            return restr;
        }
    }
}
