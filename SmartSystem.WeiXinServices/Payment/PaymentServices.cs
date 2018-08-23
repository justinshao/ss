using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinServices.Payment;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Services.WeiXin;
using Common.Entities.WX;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Base.Common.Xml;

namespace SmartSystem.WeiXinServices
{
    public class PaymentServices
    {
        /// <summary>
        /// V3 统一支付接口
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="isApp"></param>
        /// <returns></returns>
        public static UnifiedPrePayMessage UnifiedPrePay(string postData)
        {
            string url = WeiXinPayConst.WeiXin_Pay_UnifiedPrePayUrl;
            var message = HttpHelper.PostXmlResponse<UnifiedPrePayMessage>(url, postData);
            return message;
        }


        /// <summary>
        /// V3 统一订单查询接口
        /// </summary>
        /// <param name="postXml"></param>
        /// <returns></returns>
        public static UnifiedOrderQueryMessage UnifiedOrderQuery(string companyId,string transactionId, string orderId)
        {
            UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(companyId);
            string postXml = model.CreateOrderQueryXml(transactionId, orderId);
            string url = WeiXinPayConst.WeiXin_Pay_UnifiedOrderQueryUrl;
            UnifiedOrderQueryMessage message = HttpHelper.PostXmlResponse<UnifiedOrderQueryMessage>(url, postXml);
            return message;
        }

        #region V3 申请退款

        /// <summary>
        /// 申请退款（V3接口）
        /// </summary>
        /// <param name="postData">请求参数</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        public static bool Refund(string comapnyId,string transaction_Id, string out_trade_no, string total_fee, string refund_fee, string refundNo, string certpath)
        {
            TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund", "transaction_Id:{0}；out_trade_no:{1}；total_fee：{2}；refund_fee：{3}；refundNo：{4}", transaction_Id, out_trade_no, total_fee, refund_fee, refundNo);
            UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(comapnyId);
            string postData = model.CreateOrderRefundXml(out_trade_no, transaction_Id, total_fee, refundNo, refund_fee);
            TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund", "postData xml:{0}", postData);
            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(comapnyId);
            TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund", "config:{0}", config==null?"is null":"is not null");
            if(config==null || string.IsNullOrWhiteSpace(config.CertPath) || string.IsNullOrWhiteSpace(config.CertPwd)){
                throw new MyException("获取支付配置失败");
            }
            string path = string.Format("{0}{1}", certpath, config.CertPath.TrimStart('/'));
            string url = WeiXinPayConst.WeiXin_Pay_UnifiedOrderRefundUrl;
            RefundMessage message = PostXmlResponse<RefundMessage>(url, postData, path, config.CertPwd);
            TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund","OrderId:{0}；return Xml:{1}", out_trade_no, message.ToXmlString());

            return message.Success;
        }
        /// <summary>
        /// 证书验证的 post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="postData">post数据</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <returns></returns>
        private static T PostXmlResponse<T>(string url, string postData, string certPath, string certPwd) where T : class,new()
        {
           TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund",string.Format("postData:{0}；", postData));
            HttpWebRequest hp = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            var cer = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            hp.ClientCertificates.Add(cer);
            var encoding = System.Text.Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            hp.Method = "POST";
            hp.ContentType = "application/x-www-form-urlencoded";
            hp.ContentLength = data.Length;
            using (Stream ws = hp.GetRequestStream())
            {
                // 发送数据
                ws.Write(data, 0, data.Length);
                ws.Close();

                using (HttpWebResponse wr = (HttpWebResponse)hp.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream(), encoding))
                    {
                        return XmlHelper.Deserialize<T>(sr.ReadToEnd());
                    }
                }
            }
        }

        //验证服务器证书
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            TxtLogServices.WriteTxtLogEx("WeiXin_Request_Refund", string.Format("CheckValidationResult:{0}；", (int)errors));
            return false;
        }
        #endregion
        #region V3 关闭待支付订单
        public static bool CloseOrderPay(string companyId,string out_trade_no)
        {
            UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(companyId);
            string postData = model.CreateClosePayPackage(out_trade_no);
            TxtLogServices.WriteTxtLogEx("CloseOrderPay", "OrderId:{0},Request Xml:{1}", out_trade_no, postData);

            string url = WeiXinPayConst.WeiXin_Pay_ClosePayUrl;
            var message = HttpHelper.PostXmlResponse<ClosePayMessage>(url, postData);
            TxtLogServices.WriteTxtLogEx("CloseOrderPay"," OrderId:{0}, Return Xml:{1},Result:{2}", out_trade_no, message.ToXmlString(), message.Success ? "成功" : "失败");
            return message.Success;
        }
        #endregion
    }
}
