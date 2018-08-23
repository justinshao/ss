using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.IO.Compression;
using ClassLibrary1.PurseData;


namespace ClassLibrary1
{
    public class wxApi
    {
        public static AccessToken GetToken(string AppID, string AppSecret, string grant_type = "client_credential")
        {
            var url = string.Format(Weixi.getAccessToken, grant_type, AppID, AppSecret);
            return Get<AccessToken>(url);
        }

        public static VerifyCode getPhone(string phoneNo) {
            var url = string.Format(Weixi.getPhone, phoneNo);
            return Get<VerifyCode>(url);
        }

        public static VerifyCode getThirdLogin(string ID, string DeviceID)
        {
            var url = Weixi.getThirdLogin;
            return PostThird<VerifyCode>(url, ID, DeviceID);
        }

        public static VerifyCode getBingding(string Phone, string Code, string ID, string DeviceID)
        {
            var url = Weixi.getBingding;
            return PostBingding<VerifyCode>(url, Phone, Code, ID, DeviceID);
        }

        public static CarManage getCarManage(string token)
        {
            var url = string.Format(Weixi.getCarManage);
            return GetChange<CarManage>(url, token);
        }

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserInfo getUserInfo(string token) {
            var url = string.Format(Weixi.getUserInfo);
            return GetNewChange<UserInfo>(url, token);
        
        }


        /// <summary>
        /// 查询可用优惠券
        /// </summary>
        /// <param name="token"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static Coupon getCoupon(string token, int price) {
            var url = string.Format(Weixi.getBalaceGetCoupon, price);
            return GetNewChange<Coupon>(url, token);
        }
        



        public static VerifyCode getCarline(string token) {
            var url = string.Format(Weixi.getCarLine);
            return GetChange<VerifyCode>(url,token);
        }


        public static Errcode Token(string IsToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", IsToken);
            return Get<Errcode>(url);
        }
        

        public static userin GetNickname(string accessToken, string openid) {
            var urlstr = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", accessToken, openid);
            return Get<userin>(urlstr);

        }

        
        public static ewm CreateQrCode(string companyId, string accessToken, int sceneId, int expireSeconds = 0)
        {
            var url = string.Format(Weixi.GetTicketStr,accessToken);
            var guid = Guid.NewGuid().ToString("N");
            object data;
            if (expireSeconds > 0)
            {
                data = new
                {
                    expire_seconds = expireSeconds,
                    action_name = "QR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
                //LogServices.WriteTxtLogEx("CreateQrCode", "Create sceneId:{0},expire_seconds:{1},guid:{2}", sceneId, expireSeconds, guid);
            }
            else
            {
                data = new
                {
                    action_name = "QR_LIMIT_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
                //LogServices.WriteTxtLogEx("CreateQrCode", "Create static sceneId:{0},guid:{1}", sceneId, guid);
            }
            var qrCode = Post<ewm>(url, data);
            //LogServices.WriteTxtLogEx("CreateQrCode", "Create sceneId:{0},guid:{1},ticket:{2}", sceneId, guid, qrCode.Ticket);
            return qrCode;
        }


        public static T PostBingding<T>(string url,string Phone, string Code, string ID, string DeviceID)
        {
            using (var ms = new MemoryStream())
            {
                var returnJson = postbingdin(url, Phone, Code, ID, DeviceID);
                CheckThrowError(returnJson);
                return GetJson<T>(returnJson);
            }
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



        public static T PostThird<T>(string url, string ID, string DeviceID)
        {
            using (var ms = new MemoryStream())
            {
                var returnJson = postauto(url, ID, DeviceID);
                CheckThrowError(returnJson);
                return GetJson<T>(returnJson);
            }
        }

     



        public static string postauto(string url, string ID,string DeviceID)
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

        public static string postbingdin(string url, string Phone, string Code, string ID, string DeviceID) {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var postData = "Phone=" + Phone + "";
            postData += "&Type=1";
            postData += "&Code=" + Code + "";
            postData += "&ID=" + ID + "";
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
                    using (var fileStream =GetFileStream(filePath))
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


        public static T Get<T>(string url)
        {
            var returnJson = Get(url);
            CheckThrowError(returnJson);
            return GetJson<T>(returnJson);
        }

        public static T GetChange<T>(string url, string token) {
            var returnJson = GetChange(url, token);
            if (returnJson.Contains("\"Status\":40001"))
            {
                var obj = default(T);
                if (typeof(T) == Type.GetType("CarManage"))
                {
                    CarManage m = new CarManage();
                    m.Status = 40001;
                    return m as dynamic;
                }
                else
                {
                    return obj;
                }
            }
            CheckThrowError(returnJson);
            return GetJson<T>(returnJson);
        }

       

        public static T GetNewChange<T>(string url, string token) { 
            var returnJson=GetChange(url,token);
            if (returnJson.Contains("\"Status\":40001")) {
                var obj = default(T);
                return obj;
            }
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


        public static string GetChange(string url, string token,int timeout = 30000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Headers.Add("Authorization", token);

            using (var response = (HttpWebResponse)request.GetResponse())
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

     



        public static string Get(string url, int timeout = 30000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";


            using ( var response = (HttpWebResponse)request.GetResponse())
            {  
                var myResponseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower() == "gzip") {
                    myResponseStream =new GZipStream(myResponseStream, CompressionMode.Decompress);
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



    }
}