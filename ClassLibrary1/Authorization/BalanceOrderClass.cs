using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace ClassLibrary1.Authorization
{
    public class BalanceOrderClass
    {
        public static VerifyCode getBalanceOrder(string token, int Price, string CouponID) {
            string url = string.Format(Weixi.getBalanceOrder);
            return POST<VerifyCode>(token, Price, CouponID, url);
             
        }

        public static T POST<T>(string token, int price, string coupon,string url) {
            var jsonresult = POST(token, price, coupon, url);
            return GetJson<T>(jsonresult);
        }

        public static T GetJson<T>(string returnjson) {
            return JsonConvert.DeserializeObject<T>(returnjson);
        }

        public static string POST(string token, int Price, string CouponID, string url, int timeout = 30000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "Price=" + Price + "";
            postData += "&CouponID='" + CouponID + "'";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("Authorization", token);

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
