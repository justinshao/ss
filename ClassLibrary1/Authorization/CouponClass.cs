using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1.PurseData;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace ClassLibrary1.Authorization
{
   public class CouponClass
    {
       public static Coupon getCoupon(string token, int price) {
           string url = string.Format(Weixi.getBalaceGetCoupon, price);
           return GET<Coupon>(token, url);
       }

       public static T GET<T>(string token, string url) {
           var jsonresult = GET(token, url);
           CheckThrowError(jsonresult);
           return GetJson<T>(jsonresult);
       }

       public static T GetJson<T>(string jsonresult) {
           return JsonConvert.DeserializeObject<T>(jsonresult);
       }


       public static void CheckThrowError(string returnjson) {
           if (returnjson.Contains("\"status\":40001")) {
               var errorresult = GetJson<VerifyCode>(returnjson);
               if (errorresult.Status == 40001) {
                   throw new Exception(string.Format("{0},{1}", errorresult.Status, errorresult.Result));
               }
           }
       }


       public static string GET(string token, string url,int timeout=30000) {
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
               using (var streamReader = new StreamReader(responseStream, Encoding.UTF8)) {
                   return streamReader.ReadToEnd();
               }
           }
       }


    }
}
