using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using ClassLibrary1.PurseData;
using Newtonsoft.Json;

namespace ClassLibrary1.Authorization
{
    public class Purses
    {
        public static UserInfo getUserInfo(string token)
        {
            string url = string.Format(Weixi.getUserInfo);
            return getUser<UserInfo>(url, token);
    
        }

        public static T getUser<T>(string url, string token) {
            var jsonResult = GET(url, token);
            CheckThrowError(jsonResult);
            return GetJson<T>(jsonResult);
        }

        public static T GetJson<T>(string jsonresult) {
            return JsonConvert.DeserializeObject<T>(jsonresult);
        }

        public static void CheckThrowError(string returnjson) {
            if (returnjson.Contains("\"Status\":40001"))
            {
                var errorResult = GetJson<VerifyCode>(returnjson);
                if (errorResult.Status == 40001)
                {
                    throw new Exception(string.Format("{0},{1}", errorResult.Status, errorResult.Result));
                }
            }
        }

        public static string GET(string url,string token,int timeout=30000) {
            var myrequest = (HttpWebRequest)WebRequest.Create(url);
            myrequest.Timeout = timeout;
            myrequest.Method = "GET";
            myrequest.ContentType = "text/html;charset=UTF-8";
            myrequest.Headers.Add("Authorization", token);
            using (var myresponse = (HttpWebResponse)myrequest.GetResponse()) {
                var myresponseStream = myresponse.GetResponseStream();
                if (myresponse.ContentEncoding.ToLower() == "gzip") {
                    myresponseStream = new GZipStream(myresponseStream, CompressionMode.Decompress);
                }
                if (myresponseStream == null)
                    return string.Empty;
                using (var myStreamReader = new StreamReader(myresponseStream, Encoding.UTF8)) {
                    return myStreamReader.ReadToEnd();
                }
            }
        }

    }
}
