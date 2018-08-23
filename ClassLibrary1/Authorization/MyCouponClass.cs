using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1.PurseData;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace ClassLibrary1.Authorization
{
   public class MyCouponClass
    {
       public static MyCoupon getMyCoupon(string token, int status) {
           string url = string.Format(Weixi.getMyCoupon, status);
           return GET<MyCoupon>(token, url);
       }

       public static T GET<T>(string token, string url) {
           var jsonresult = Get(token, url);
           CheckThrowError(jsonresult);
           return GetJson<T>(jsonresult);
       }

       public static T GetJson<T>(string jsonresult) {
           return JsonConvert.DeserializeObject<T>(jsonresult);
       }

       public static void CheckThrowError(string jsonresult) {
           if (jsonresult.Contains("\"status\":40001")) {
               var Errorresult = GetJson<VerifyCode>(jsonresult);
               if (Errorresult.Status == 40001) {
                   throw new Exception(string.Format("{0},{1}", Errorresult.Status, Errorresult.Result));
               }
           }
       }

       public static string Get(string token, string url,int timeout=30000) {
           var request = (HttpWebRequest)WebRequest.Create(url);
           request.Timeout = timeout;
           request.Method = "GET";
           request.ContentType = "text/html;charset=UTF-8";
           request.Headers.Add("Authorization", token);
           using (var response = (HttpWebResponse)request.GetResponse()) {
               var responsestream = response.GetResponseStream();
               if (response.ContentEncoding.ToLower() == "gzip") {
                   responsestream = new GZipStream(responsestream, CompressionMode.Decompress);
               }
               using(var streamreder=new StreamReader(responsestream,Encoding.UTF8)){
                   return streamreder.ReadToEnd();
               }
           }
       }

       
       
    }
}
