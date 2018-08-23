using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.AliPayServices;
using System.Collections.Specialized;
using Common.Services;
using SmartSystem.WeiXinServices;
using Common.Entities;
using Common.Entities.Order;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.AliPay
{
    /// <summary>
    /// 支付通知
    /// </summary>
    public class AliPayNotifyController : Controller
    {
        public void Index()
        {
            //TxtLogServices.WriteTxtLogEx("AliPayNotify", Request.Url.AbsoluteUri + ":" + Request.Form.ToString());

            try
            {
                string notify_time = Request.Params["notify_time"];
                string notify_type = Request.Params["notify_type"];
                string notify_id = Request.Params["notify_id"];
                string app_id = Request.Params["app_id"];
                string charset = Request.Params["charset"];
                string version = Request.Params["version"];
                string sign_type = Request.Params["sign_type"];
                string sign = Request.Params["sign"];
                string trade_no = Request.Params["trade_no"];
                string out_trade_no = Request.Params["out_trade_no"];
                string out_biz_no = Request.Params["out_biz_no"];
                string buyer_id = Request.Params["buyer_id"];
                string buyer_logon_id = Request.Params["buyer_logon_id"];
                string seller_id = Request.Params["seller_id"];
                string seller_email = Request.Params["seller_email"];
                string trade_status = Request.Params["trade_status"];
                string total_amount = Request.Params["total_amount"];
                string receipt_amount = Request.Params["receipt_amount"];
                string invoice_amount = Request.Params["invoice_amount"];
                string buyer_pay_amount = Request.Params["buyer_pay_amount"];
                string point_amount = Request.Params["point_amount"];
                string refund_fee = Request.Params["refund_fee"];
                string subject = Request.Params["subject"];
                string gmt_create = Request.Params["gmt_create"];
                string gmt_payment = Request.Params["gmt_payment"];
                string gmt_refund = Request.Params["gmt_refund"];
                string gmt_close = Request.Params["gmt_close"];
                string fund_bill_list = Request.Params["fund_bill_list"];
                string passback_params = Request.Params["passback_params"];
                string voucher_detail_list = Request.Params["voucher_detail_list"];

                
                OnlineOrder order = OnlineOrderServices.QueryByOrderId(decimal.Parse(out_trade_no));
                if (order == null) throw new Exception("获取订单失败");

                if (!CheckNotifySign(order.CompanyID)) throw new Exception("验证签名失败");

                TxtLogServices.WriteTxtLogEx("AliPayNotify", "验签成功：out_trade_no:" + out_trade_no);
                DateTime payTime = DateTime.Now;
                DateTime.TryParse(gmt_payment, out payTime);
                WeiXinInerface.ParkingFeeService.DeleteParkingFee(order.PlateNo + order.PKID);
                bool result = OnlineOrderServices.PaySuccess(decimal.Parse(out_trade_no), trade_no, payTime, buyer_id);
                if (!result) throw new Exception("修改订单状态未已支付失败");

                TxtLogServices.WriteTxtLogEx("AliPayNotify", string.Format("AliPayShowResult:{0}支付完成", trade_no));
                Response.Write("success");
                Response.End();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPayNotify", "支付通知出错", ex, LogFrom.AliPay);
                Response.Write("fail");
                Response.End();
            }
        }
        private bool CheckNotifySign(string companyId)
        {
            try
            {
                IDictionary<string, string> paramsMap = GetRequestPost();
                return AliPayApiServices.RSACheckV1(companyId,paramsMap);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayNotify", "验签报错：" + ex.StackTrace);
                return false;
            }
        }

        public IDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            IDictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        }

    }
}
