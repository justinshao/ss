using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Base.Common;
using System.Xml;
using SmartSystem.WeiXinServices.Payment;
using Common.Services;
using Base.Common.Xml;
using Common.Entities;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;
using Common.Entities.Order;
using Common.Entities.WX;
using Common.Entities.Statistics;
using Common.Services.Statistics;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class WeiXinPayNotifyController : Controller
    {
        /// <summary>
        /// 订单支付通道
        /// </summary>
        public void Index()
        {
            ReturnMessage returnMsg = new ReturnMessage() { Return_Code = "SUCCESS", Return_Msg = "" };
            string xmlString = GetXmlString(Request);
            NotifyMessage message = null;
            try
            {
                TxtLogServices.WriteTxtLogEx("WeiXinPayNotify", xmlString);
                //此处应记录日志
                message = XmlHelper.Deserialize<NotifyMessage>(xmlString);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                string sign = string.Empty;
                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if (node.Name.ToLower() != "sign")
                        dic.Add(node.Name, node.InnerText);
                    else
                        sign = node.InnerText;
                }
                //处理通知
                decimal orderId;
                if (!dic.ContainsKey("out_trade_no") || !decimal.TryParse(dic["out_trade_no"], out orderId))
                    throw new MyException("支付回调订单编号格式不正确");

                OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
                if (order == null) throw new MyException("订单存在,orderId:" + orderId);

                UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(order.CompanyID);
                if (!model.ValidateMD5Signature(dic, sign))
                {
                    throw new Exception("签名未通过！");

                }
               
                if (!dic.ContainsKey("transaction_id")) throw new MyException("支付流水号不存在");

                string payTradeNo = dic["transaction_id"];

                DateTime payTime = DateTime.Now;
                if (dic.ContainsKey("time_end") && dic["time_end"].Length == 14)
                {
                    string strDate = dic["time_end"].Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
                    if (!DateTime.TryParse(strDate, out payTime))
                    {
                        payTime = DateTime.Now;
                    }
                }
                string payAccount = string.Empty;
                if (dic.ContainsKey("attach") && dic["attach"] == "MWEB" && dic.ContainsKey("openid") && !string.IsNullOrWhiteSpace(dic["openid"]))
                {
                    payAccount = dic["openid"];
                }
                else if (dic.ContainsKey("attach") && dic["attach"].Length > 0)
                {
                    //统计二维码推广订单信息
                    string sAgendID = dic["attach"];
                    tgPerson person = tgPersonServices.QueryPersonByID(int.Parse(sAgendID));
                    TgOrder to = new TgOrder();
                    to.OrderID = order.OrderID;
                    to.PKID = order.PKID;
                    to.PKName = order.PKName;
                    to.PlateNo = order.PlateNo;
                    to.Amount = order.Amount;
                    to.RealPayTime = payTime;
                    to.PersonId = person.id;
                    to.PersonName = person.name;
                    TgOrderService.Addperson(to);
                }

                //if (Request.Cookies["SmartSystem_WeiXinTg_personid"] != null)
                //{

                //}
                WeiXinInerface.ParkingFeeService.DeleteParkingFee(order.PlateNo + order.PKID);
                OnlineOrderServices.PaySuccess(orderId, payTradeNo, payTime, payAccount);
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayNotify", "支付通知出错:"+ex.Message, ex, LogFrom.WeiXin);
                //此处记录异常日志
                returnMsg.Return_Code = "FAIL";
                returnMsg.Return_Msg = ex.Message;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayNotify", "支付通知出错", ex, LogFrom.WeiXin);
                //此处记录异常日志
                returnMsg.Return_Code = "FAIL";
                returnMsg.Return_Msg = ex.Message;
            }
            Response.Write(returnMsg.ToXmlString());
            Response.End();
        }
        public void AdvanceParking() {
            ReturnMessage returnMsg = new ReturnMessage() { Return_Code = "SUCCESS", Return_Msg = "" };
            string xmlString = GetXmlString(Request);
            NotifyMessage message = null;
            try
            {
                TxtLogServices.WriteTxtLogEx("AdvanceParkingPayNotify", xmlString);
                //此处应记录日志
                message = XmlHelper.Deserialize<NotifyMessage>(xmlString);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                string sign = string.Empty;
                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if (node.Name.ToLower() != "sign")
                        dic.Add(node.Name, node.InnerText);
                    else
                        sign = node.InnerText;
                }
                //处理通知
                decimal orderId;
                if (!dic.ContainsKey("out_trade_no") || !decimal.TryParse(dic["out_trade_no"], out orderId))
                    throw new MyException("支付回调订单编号格式不正确");

                AdvanceParking order = AdvanceParkingServices.QueryByOrderId(orderId);
                if (order == null) throw new MyException("获取预订单失败");

                UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(order.CompanyID);
                if (!model.ValidateMD5Signature(dic, sign))
                {
                    throw new Exception("签名未通过！");

                }
              

                if (!dic.ContainsKey("transaction_id")) throw new MyException("支付流水号不存在");

                string payTradeNo = dic["transaction_id"];

                DateTime payTime = DateTime.Now;
                if (dic.ContainsKey("time_end") && dic["time_end"].Length == 14)
                {
                    string strDate = dic["time_end"].Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
                    if (!DateTime.TryParse(strDate, out payTime))
                    {
                        payTime = DateTime.Now;
                    }
                }
                AdvanceParkingServices.PaySuccess(orderId, payTradeNo, payTime);
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AdvanceParkingPayNotify", "支付通知出错:" + ex.Message, ex, LogFrom.WeiXin);
                //此处记录异常日志
                returnMsg.Return_Code = "FAIL";
                returnMsg.Return_Msg = ex.Message;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AdvanceParkingPayNotify", "支付通知出错", ex, LogFrom.WeiXin);
                //此处记录异常日志
                returnMsg.Return_Code = "FAIL";
                returnMsg.Return_Msg = ex.Message;
            }
            Response.Write(returnMsg.ToXmlString());
            Response.End();
        }
        /// <summary>
        /// 获取Post Xml数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetXmlString(HttpRequestBase request)
        {
            using (System.IO.Stream stream = request.InputStream)
            {
                Byte[] postBytes = new Byte[stream.Length];
                stream.Read(postBytes, 0, (Int32)stream.Length);
                return System.Text.Encoding.UTF8.GetString(postBytes);
            }
        }
    }
}
