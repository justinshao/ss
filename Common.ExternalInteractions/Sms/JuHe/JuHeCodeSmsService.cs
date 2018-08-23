using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.Helpers;
using System.Web;
using System.Configuration;

namespace Common.ExternalInteractions.Sms.JuHe
{
    public class JuHeCodeSmsService
    {
        public static JuHeSmsResult SendCodeSms(string code, string mobile)
        {
            string appkey = ConfigurationManager.AppSettings["JuHeSmsAppKey"] ?? string.Empty;
            string modeId = ConfigurationManager.AppSettings["JuHeSmsModeId"] ?? string.Empty;
            return SendCodeSms(appkey,modeId,code, mobile);
        }
        public static JuHeSmsResult SendCodeSms(string appkey, string modeid, string code, string mobile)
        {
            if (string.IsNullOrWhiteSpace(appkey)){
                return new JuHeSmsResult { error_code = 1, reason = "获取AppKey失败" };
            }
            if (string.IsNullOrWhiteSpace(modeid))
            {
                return new JuHeSmsResult { error_code = 1, reason = "获取模板编号失败" };
            }
            //1.屏蔽词检查测
            string checkurl = "http://v.juhe.cn/sms/black";
            string word = HttpUtility.UrlEncode(code, Encoding.UTF8);

            string result = HttpHelper.RequestUrl(checkurl,
                            new List<KeyValuePair<string, string>>
                              {
                                                new KeyValuePair<string,string>("word",word),
                                                new KeyValuePair<string, string>("key",appkey)

                              }, HttpMethod.GET);

            JuHeSmsResult checkResult = JsonHelper.GetJson<JuHeSmsResult>(result);
            if (!checkResult.SendResult)
            {
                return checkResult;
            }


            //2.发送短信
            string sendUrl = "http://v.juhe.cn/sms/send";

            string sendResult = HttpHelper.RequestUrl(sendUrl,
                            new List<KeyValuePair<string, string>>
                              {
                                                new KeyValuePair<string,string>("mobile",mobile),
                                                new KeyValuePair<string, string>("tpl_id",modeid),
                                                 new KeyValuePair<string, string>("tpl_value",string.Format("#code#={0}",code)),
                                                  new KeyValuePair<string, string>("key",appkey),
                                                   new KeyValuePair<string, string>("dtype","json")

                              }, HttpMethod.GET);

            JuHeSmsResult smsResult = JsonHelper.GetJson<JuHeSmsResult>(sendResult);

            //SaveSendSms(smsResult,code,mobile);
            return smsResult;
        }

        /// <summary>
        /// 发送通道拥堵短信  #GateName#于#Date#发生拥堵，待放行车辆#Number#,请及时处理
        /// </summary>
        /// <param name="gateName">通道名称</param>
        /// <param name="date">拥堵时间</param>
        /// <param name="number">拥堵车牌号码</param>
        /// <returns></returns>
        public static JuHeSmsResult SendGateCongestionSms(string appkey, string modeid, string mobile, string gateName, DateTime date, string number, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(appkey))
            {
                return new JuHeSmsResult { error_code = 1, reason = "获取AppKey失败" };
            }
            var parameters1 = new Dictionary<string, string>();

            parameters1.Add("gateName", gateName);
            string strDate = string.Format("{0}时{1}分{2}秒", date.Hour, date.Minute, date.Second); 
            parameters1.Add("date", strDate);
            if (amount > 0)
            {
                number = number + " 收费" + amount + "元";
            }
            parameters1.Add("number", number);
           
            //1.屏蔽词检查测
            foreach (var item in parameters1)
            {
                string checkWork = HttpUtility.UrlEncode(item.Value, Encoding.UTF8);
                JuHeSmsResult checkResult = CheckWord(checkWork, appkey);
                if (!checkResult.SendResult)
                {
                    return checkResult;
                }
            }
            //2.发送短信
            //#GateName#于#Date#发生拥堵，待放行车辆#Number#,请及时处理

            string sendUrl = "http://v.juhe.cn/sms/send";
            string tpl_value = HttpUtility.UrlEncode(string.Format("#GateName#={0}&#Date#={1}&#Number#={2}", parameters1["gateName"], parameters1["date"], parameters1["number"]), Encoding.UTF8);//urlencode("#code#=1234&#company#=聚合数据")
            string sendResult = HttpHelper.RequestUrl(sendUrl,
                            new List<KeyValuePair<string, string>>
                              {
                                                new KeyValuePair<string,string>("mobile",mobile),
                                                new KeyValuePair<string, string>("tpl_id",modeid),
                                                 new KeyValuePair<string, string>("tpl_value",tpl_value),
                                                  new KeyValuePair<string, string>("key",appkey),
                                                   new KeyValuePair<string, string>("dtype","json")

                              }, HttpMethod.GET);

            JuHeSmsResult smsResult = JsonHelper.GetJson<JuHeSmsResult>(sendResult);
            return smsResult;
        }
        private static JuHeSmsResult CheckWord(string word, string appkey)
        {
            //1.屏蔽词检查测
            string checkurl = "http://v.juhe.cn/sms/black";
            string sendWord = HttpUtility.UrlEncode(word, Encoding.UTF8);

            string result = HttpHelper.RequestUrl(checkurl,
                            new List<KeyValuePair<string, string>>
                              {
                                                new KeyValuePair<string,string>("word",sendWord),
                                                new KeyValuePair<string, string>("key",appkey)

                              }, HttpMethod.GET);

            return JsonHelper.GetJson<JuHeSmsResult>(result);

        }
    }
}
