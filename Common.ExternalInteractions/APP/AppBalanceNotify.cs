using Common.Services;
using Common.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.ExternalInteractions.APP
{
    public class AppBalanceNotify
    {
        /// <summary>
        /// 通知APP端 
        /// </summary>
        /// <param name="sOrderID">订单ID</param>
        /// <param name="sAmount">金额</param>
        /// <returns></returns>
        public static int BalanceRechargeNotify(string sOrderID,string sUserName, decimal dAmount)
        {
            string sTime = "";
            string sAmount = ((int)(dAmount * 100)).ToString();
            
            //请求时间
            string sTUrl = "http://spsapp.spsing.com/api/Home/GetSysTime";

            string sResult = GetHttpResponse(sTUrl);
            //
            string sCode = GetRegexString("(?<=\"Status\":)[\\d]+(?=,)", sResult, 0);
            sTime = GetRegexString("(?<=\"Result\":)[\\d]+(?=})", sResult, 0);
            //是否正确
            if (sCode != "1")
            {
                TxtLogServices.WriteTxtLogEx("SPSError", "Time Result=" + sResult);
                return 0;
            }

            string t = sTime + sAmount + sUserName;
            string sUrl = "http://spsapp.spsing.com/api/User/WxNotice?t=" + t;
            
            string sNotifyResult = GetHttpResponse(sUrl);
            sCode = GetRegexString("(?<=\"Status\":)[\\d]+(?=,)", sNotifyResult, 0);
            //是否正确
            if (sCode != "1")
            {
                TxtLogServices.WriteTxtLogEx("SPSError", "Notify Result=" + sNotifyResult);
                return 0;
            }

            return 1;
        }


        private static string GetHttpResponse(string sUrl)
        {


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sUrl);

            myRequest.Method = "GET";
            //myRequest.ContentType = "application/json; charset=UTF-8";
            //
            myRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            //myRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            myRequest.Headers.Add("Accept-Language", "text/html, application/xhtml+xml, image/jxr, */*");
            myRequest.Timeout = 5000;
            string sRetResult = "";

            try
            {
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    Stream deStream = myResponse.GetResponseStream();
                    if (myResponse.ContentEncoding.ToLower() == "gzip")
                    {
                        deStream = new GZipStream(deStream, CompressionMode.Decompress);
                    }


                    StreamReader sr = new StreamReader(deStream, Encoding.UTF8);
                    sRetResult = sr.ReadToEnd();

                    return sRetResult;
                }
            }
            catch (Exception e)
            {
                sRetResult = "";

                TxtLogServices.WriteTxtLogEx("SPSError", string.Format("PayNotify result:{0}", e.Message),e);

                return sRetResult;
            }
            finally
            {
                TxtLogServices.WriteTxtLogEx("SPSInfo", string.Format("请求SPS接口：{0}\r\n参数：{1}\r\n返回结果：{2}", sUrl, "", sRetResult));
            }
        }
        private static string GetRegexString(string sRegex, string sValue, int iIndex)
        {
            if (sValue.IsEmpty())
            {
                return "";
            }
            Regex mRegex = new Regex(sRegex);
            MatchCollection mMatchCollection = mRegex.Matches(sValue);
            if (mMatchCollection.Count > iIndex)
            {
                return mMatchCollection[iIndex].Value;
            }
            return "";
        }



    }
}
