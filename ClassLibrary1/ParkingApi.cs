using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.IO.Compression;

namespace ClassLibrary1
{
    public class ParkingApi
    {
        public static TPLogin aa(string ID,string DeviceID)
        {
            var url = Weixi.getThirdLogin;
            return PostThird<TPLogin>(url, ID, DeviceID);
        }
        public static ParkingList bb(int status, int pageIndex, int pageSize, string auth)
        {
            ////
            var url = string.Format("http://spsapp.spsing.com/api/Order/MyParkingList?status={0}&pageIndex={1}&pageSize={2}", status, pageIndex, pageSize);
            return Get<ParkingList>(url,auth);
        }
        public static ParkingDetail cc(string orderid, int type, string auth)
        {
            var url = string.Format("http://spsapp.spsing.com/api/Order/ParkingDetail?orderID={0}&type={1}", orderid, type);
            return Get<ParkingDetail>(url, auth);
        }


        public static PresenceOrderList GetPresenceOrder(string auth)
        {
            var url = "http://spsapp.spsing.com/api/Home/GetPresenceOrder";
            return Get<PresenceOrderList>(url, auth);
        }

        public static T Post<T>(string url, object data)
        {
            var jsonString = GetJsonString(data);
            using (var ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var returnJson = Post(url, null, ms);
                CheckThrowError(returnJson);
                return GetJson<T>(returnJson);
            }
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
                    using (var fileStream = GetFileStream(filePath))
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

                var responseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower() == "gzip")
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }
                if (responseStream == null)
                    return string.Empty;
                using (var myStreamReader = new StreamReader(responseStream, encoding))
                {
                    return myStreamReader.ReadToEnd();
                }
                
            }
        }


        /// <summary>
        /// 对象转换为Json字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetJsonString(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T Get<T>(string url,string head)
        {
            var returnJson = Get(url,head);
            CheckThrowError(returnJson);
            return GetJson<T>(returnJson);
        }

         /// <summary>
        /// Json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T GetJson<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string Get(string url, string head,int timeout = 30000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            request.Headers.Add("Authorization",head);
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var myResponseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower() == "gzip")
                {
                    myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
                }
                if (myResponseStream == null)
                    return string.Empty;
                using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                {
                    return myStreamReader.ReadToEnd();
                }
            }
        }

        public static void CheckThrowError(string returnJson)
        {
            if (returnJson.Contains("errcode"))
            {
                //可能发生错误
                var errorResult = GetJson<ResError>(returnJson);
                if (errorResult.errcode != ResCode.请求成功)
                {
                    throw new Exception(string.Format("微信请求发生错误！错误代码：{0}，说明：{1}", (int)errorResult.errcode, errorResult.errmsg));
                }
            }
        }

        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }

        public static T PostThird<T>(string url, string ID, string DeviceID)
        {
            using (var ms = new MemoryStream())
            {
                var returnJson = postauto(url, ID, DeviceID);
                CheckThrowError(returnJson);
                return GetJson<T>(returnJson);
            }
        }

        public static string postauto(string url, string ID, string DeviceID)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "ID=" + ID + "";
            postData += "&Type=1";
            postData += "&DeviceID=" + DeviceID + "";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var myResponseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower() == "gzip")
                {
                    myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
                }

                using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                {
                    return myStreamReader.ReadToEnd();
                }
            }
        }
    }
}
