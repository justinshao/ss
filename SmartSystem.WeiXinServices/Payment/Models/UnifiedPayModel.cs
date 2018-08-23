using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Services;
using SmartSystem.WeiXinServices.Payment.Models;
using Common.Utilities.Helpers;


namespace SmartSystem.WeiXinServices.Payment
{
    public class UnifiedPayModel
    {
        private Dictionary<string, string> parameters;
        public string AppId;
        private string Key;
        public string PartnerId;
        private string ServerIP;

        private UnifiedPayModel()
        {
            this.parameters = new Dictionary<string, string>();
        }

        public static UnifiedPayModel CreateUnifiedModel(string companyId)
        {
            UnifiedPayModel wxPayModel = new UnifiedPayModel();
            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
            wxPayModel.SetAppId(config.AppId);
            wxPayModel.SetKey(config.PartnerKey);
            wxPayModel.SetPartnerId(config.PartnerId);
            wxPayModel.SetServerIP(config.ServerIP);
            return wxPayModel;
        }
        public void SetAppId(string appId){
            AppId = appId;
        }
        public void SetPartnerId(string partnerId){
            PartnerId = partnerId;
        }
        public void SetKey(string key){
            Key = key;
        }
        public void SetServerIP(string serverIp)
        {
            ServerIP = serverIp;
        }

        /// <summary>
        /// MD5签名
        /// </summary>
        /// <returns></returns>
        public string GetCftPackage(Dictionary<string, string> bizObj)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new Exception("Key为空！");
            }

            string unSignParaString = Util.FormatBizQueryParaMapForUnifiedPay(bizObj);
            return WxMD5Sign.Sign(unSignParaString, Key);
        }
        public bool ValidateMD5Signature(Dictionary<string, string> bizObj, string sign)
        {
            string signValue = GetCftPackage(bizObj);
            return signValue == sign;
        }

        /// <summary>
        /// 生成 预支付 请求参数（XML）
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tradeNo"></param>
        /// <param name="totalFee"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string CreatePrePayPackage(string tradeNo, string totalFee, string openid, string description, string notifyUrl,string attach = "")
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();

            //自定义的参数
            if (!attach.IsEmpty())
            {
                nativeObj.Add("attach", attach);
            }
            nativeObj.Add("appid", AppId);
            nativeObj.Add("mch_id", PartnerId);
            nativeObj.Add("nonce_str", Util.CreateNoncestr());
            nativeObj.Add("body", description);
            nativeObj.Add("out_trade_no", tradeNo);
            nativeObj.Add("total_fee", totalFee);
            nativeObj.Add("spbill_create_ip", ServerIP);
            nativeObj.Add("notify_url", notifyUrl);
            nativeObj.Add("trade_type", "JSAPI");
            nativeObj.Add("openid", openid);
            nativeObj.Add("sign", GetCftPackage(nativeObj));

            return DictionaryToXmlString(nativeObj);
        }
        /// <summary>
        /// 生产关闭订单 请求(XML)
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        public string CreateClosePayPackage(string tradeNo)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            nativeObj.Add("appid", AppId);
            nativeObj.Add("mch_id", PartnerId);
            nativeObj.Add("out_trade_no", tradeNo);
            nativeObj.Add("nonce_str", Util.CreateNoncestr());
            nativeObj.Add("sign", GetCftPackage(nativeObj));
            return DictionaryToXmlString(nativeObj);
        }
        /// <summary>
        /// 创建订单查询 XML
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public string CreateOrderQueryXml(string transactionId,string orderNo)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();

            nativeObj.Add("appid", AppId);
            nativeObj.Add("mch_id", PartnerId);
            if (string.IsNullOrWhiteSpace(transactionId) && string.IsNullOrWhiteSpace(orderNo))
                throw new Exception("交易单号或订单号必须一项不能为空");

            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                nativeObj.Add("transaction_id", transactionId);
            }
            else {
                nativeObj.Add("out_trade_no", orderNo);
            }
           
            nativeObj.Add("nonce_str", Util.CreateNoncestr());
            nativeObj.Add("sign", GetCftPackage(nativeObj));

            return DictionaryToXmlString(nativeObj);
        }
        /// <summary>
        /// 创建订单退款 XML
        /// </summary>
        /// <param name="orderNo">商户订单号</param>
        /// <param name="transactionId">微信订单号</param>
        /// <param name="totalFee">总金额</param>
        /// <param name="refundNo">退款订单号</param>
        /// <param name="refundFee">退款金额</param>
        /// <returns></returns>
        public string CreateOrderRefundXml(string orderNo, string transactionId, string totalFee, string refundNo, string refundFee)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            nativeObj.Add("appid", AppId);
            nativeObj.Add("mch_id",PartnerId);
            nativeObj.Add("nonce_str", Util.CreateNoncestr());
            if (string.IsNullOrEmpty(transactionId))
            {
                if (string.IsNullOrEmpty(orderNo))
                    throw new Exception("缺少订单号！");
                nativeObj.Add("out_trade_no", orderNo);
            }
            else
            {
                nativeObj.Add("transaction_id", transactionId);
            }

            nativeObj.Add("out_refund_no", refundNo);
            nativeObj.Add("total_fee", totalFee);
            nativeObj.Add("refund_fee", refundFee);
            nativeObj.Add("op_user_id", PartnerId); //todo:配置
            nativeObj.Add("sign", GetCftPackage(nativeObj));

            return DictionaryToXmlString(nativeObj);
        }

        /// <summary>
        /// dictionary转为xml 字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static string DictionaryToXmlString(Dictionary<string, string> dic)
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            foreach (string key in dic.Keys)
            {
                xmlString.Append(string.Format("<{0}><![CDATA[{1}]]></{0}>", key, dic[key]));
            }
            xmlString.Append("</xml>");
            return xmlString.ToString();
        }
        /// <summary>
        /// xml字符串 转换为  dictionary
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlToDictionary(string xmlString)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(xmlString);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            var nodes = document.FirstChild.ChildNodes;

            foreach (System.Xml.XmlNode item in nodes)
            {
                dic.Add(item.Name, item.InnerText);
            }
            return dic;
        }
      /// <summary>
        ///  H5生成 预支付 请求参数（XML）
      /// </summary>
      /// <param name="tradeNo"></param>
      /// <param name="totalFee"></param>
      /// <param name="description"></param>
      /// <param name="notifyUrl">通知地址</param>
      /// <param name="clientIp">客户端ip</param>
        /// <param name="wap_url">WAP网站URL地址</param>
      /// <param name="wap_name">WAP 网站名</param>
      /// <returns></returns>
        public string CreateH5PrePayPackage(string tradeNo, string totalFee, string description, string notifyUrl, string clientIp, string wap_url, string wap_name)
        {
            SceneInfoModel sceneInfo = new SceneInfoModel();
            SceneInfo model = new SceneInfo();
            model.type = "Wap";
            model.wap_url = wap_url;
            model.wap_name = wap_name;
            sceneInfo.h5_info = model;

            Dictionary<string, string> nativeObj = new Dictionary<string, string>();

            nativeObj.Add("appid", AppId);
            nativeObj.Add("mch_id", PartnerId);
            nativeObj.Add("nonce_str", Util.CreateNoncestr());
            nativeObj.Add("body", description);
            nativeObj.Add("out_trade_no", tradeNo);
            nativeObj.Add("total_fee", totalFee);
            nativeObj.Add("spbill_create_ip", clientIp);
            nativeObj.Add("notify_url", notifyUrl);
            nativeObj.Add("attach", "MWEB");
            nativeObj.Add("scene_info", JsonHelper.GetJsonString(sceneInfo));
            nativeObj.Add("trade_type", "MWEB");
            nativeObj.Add("sign", GetCftPackage(nativeObj));

            return DictionaryToXmlString(nativeObj);
        }
    }
}
