using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace ClassLibrary1.Authorization
{
   public class UnifiedClass
    {
       public static VerifyCode getUnified(string token, string orderNo, int PayType) {
           string url = string.Format(Weixi.getUnifiedOrder, orderNo, PayType);
           return GET<VerifyCode>(token, url);
       }

       public static T GET<T>(string token, string url) {
           var jsonresult = Get(token, url);
           return GetJson<T>(jsonresult);
       }

       public static T GetJson<T>(string jsonresult) {
           return JsonConvert.DeserializeObject<T>(jsonresult);
       }

       public static string Get(string token, string url,int timeout=30000) {
           var request = (HttpWebRequest)WebRequest.Create(url);
           request.Timeout = timeout;
           request.Method = "GET";
           request.ContentType = "text/html;charset=UTF-8";
           request.Headers.Add("Authorization", token);
           using (var response = (HttpWebResponse)request.GetResponse()) {
               var responseStream = response.GetResponseStream();
               if (response.ContentEncoding.ToLower() == "gzip") {
                   responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
               }
               if (responseStream == null)
                   return string.Empty;
               using (var streamreader = new StreamReader(responseStream, Encoding.UTF8)) {
                   return streamreader.ReadToEnd();
               }
           }
           
           
       }
       

    }
}
