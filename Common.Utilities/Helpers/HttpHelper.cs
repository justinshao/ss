using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Base.Common.Xml;

namespace Common.Utilities.Helpers
{
    public class HttpHelper
    {
        public static string Get(string url, int timeout = 30000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            var response = (HttpWebResponse)request.GetResponse();
            using (var myResponseStream = response.GetResponseStream())
            {
                if (myResponseStream == null)
                    return string.Empty;
                using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                {
                    return myStreamReader.ReadToEnd();
                }
            }
        }


        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">提交到的链接</param>
        /// <param name="postdata">提交的对象</param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string Post(string url, object postdata, CookieContainer cookieContainer = null)
        {
            var dataString = postdata == null ? string.Empty : JsonHelper.GetJsonString(postdata);
            return Post(url, dataString, cookieContainer);
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">提交到的链接</param>
        /// <param name="dataString">提交的字符串</param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string Post(string url, string dataString, CookieContainer cookieContainer = null)
        {
            var formDataBytes = Encoding.UTF8.GetBytes(dataString);
            var ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            return Post(url, ms, cookieContainer);
        }

        public static string Post(string url, Stream postStream = null, CookieContainer cookieContainer = null)
        {
            return Post(url, cookieContainer, postStream, null, null, Encoding.UTF8);
        }

        public static string Post(string url, CookieContainer cookieContainer = null, Stream postStream = null, NameValueCollection fileValueCollection = null, string refererUrl = null, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            #region 文件上传 & 设置ContentType
            if (fileValueCollection != null && fileValueCollection.Count > 0)
            {
                //通过表单上传文件
                postStream = new MemoryStream();

                var boundary = "----" + DateTime.Now.Ticks.ToString("x");
                var formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

                foreach (string fileName in fileValueCollection.Keys)
                {
                    var filePath = fileValueCollection[fileName];
                    //准备文件流
                    using (var fileStream = FileHelper.GetFileStream(filePath))
                    {
                        var formdata = string.Format(formdataTemplate, fileName, filePath /*Path.GetFileName(fileName)*/);
                        var formdataBytes = encoding.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                        postStream.Write(formdataBytes, 0, formdataBytes.Length);

                        //写入文件
                        var buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            postStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                //结尾
                var footer = encoding.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            #endregion

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.Timeout = 100000;   // 100 secends

            if (!string.IsNullOrEmpty(refererUrl))
            {
                request.Referer = refererUrl;
            }
            request.UserAgent = String.Format("xNet HttpUtils v1.0");

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                using (var requestStream = request.GetRequestStream())
                {
                    var buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                    postStream.Close();//关闭文件访问
                }
            }
            #endregion

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        return string.Empty;
                    using (var myStreamReader = new StreamReader(responseStream, encoding))
                    {
                        return myStreamReader.ReadToEnd();
                    }
                }
            }
        }

        public static void DownloadFile(string url, string savePath, bool replace = false)
        {
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(savePath))
            {
                return;
            }
            var fileInfo = new FileInfo(savePath);
            // 如果文件存在并且不替换则直接退出
            if (fileInfo.Exists)
            {
                if (replace)
                {
                    fileInfo.Delete();
                }
                else
                {
                    return;
                }
            }
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            using (var wc = new WebClient())
            {
                wc.DownloadFile(url, savePath);
            }
        }

        public static Stream GetStream(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        public static string GetClientIp(HttpRequestBase request)
        {
            var clientIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrWhiteSpace(clientIp))
            {
                clientIp = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrWhiteSpace(clientIp))
            {
                clientIp = request.UserHostAddress;
            }
            return clientIp;
        }
        /// <summary>
        /// 请求WEB页
        /// </summary>
        public static string RequestUrl(string webPageUrl, List<KeyValuePair<string, string>> parameters, HttpMethod method = HttpMethod.POST, int timeout = 50000, string encode = "")
        {
            StringBuilder builder = new StringBuilder();
            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    builder.Append(pair.Key).Append('=');
                    if (encode.ToUpper() == "UTF8")
                    {
                        builder.Append(HttpUtility.UrlEncode(pair.Value, Encoding.UTF8)).Append('&');
                    }
                    else if (encode.ToUpper() == "GB2312")
                    {
                        builder.Append(HttpUtility.UrlEncode(pair.Value, Encoding.GetEncoding("gb2312"))).Append('&');
                    }
                    else
                    {
                        builder.Append(HttpUtility.UrlEncode(pair.Value)).Append('&');
                    }
                }
            }
            if (method == HttpMethod.GET)
            {
                webPageUrl = string.Format("{0}?{1}", webPageUrl, builder.ToString().TrimEnd('&'));
            }

            HttpWebRequest web = (HttpWebRequest)WebRequest.Create(webPageUrl);
            web.Method = method.ToString();
            web.Timeout = timeout;
            if (method == HttpMethod.POST)
            {
                byte[] buff = Encoding.UTF8.GetBytes(builder.ToString().TrimEnd('&'));
                web.ContentType = "application/x-www-form-urlencoded";
                web.ContentLength = buff.Length;

                Stream sreq = web.GetRequestStream();
                sreq.Write(buff, 0, buff.Length);
            }

            using (Stream stream = web.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class,new()
        {
            string returnValue = string.Empty;
            byte[] byteData = Encoding.UTF8.GetBytes(xmlString);
            Uri uri = new Uri(url);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
            webReq.Method = "POST";
            webReq.ContentType = "application/json";
            webReq.ContentLength = byteData.Length;
            //定义Stream信息
            Stream stream = webReq.GetRequestStream();
            stream.Write(byteData, 0, byteData.Length);
            stream.Close();
            //获取返回信息
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            returnValue = streamReader.ReadToEnd();
            //关闭信息
            streamReader.Close();
            response.Close();
            stream.Close();
            T result = default(T);
            result = XmlHelper.Deserialize<T>(returnValue);
            return result;
        }
    }

    public enum HttpMethod
    {
        GET = 0,
        POST = 1
    }
}
