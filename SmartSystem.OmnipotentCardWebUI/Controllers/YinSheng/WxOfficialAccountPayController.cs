using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YinSheng.Pay;
using Common.Entities.Order;
using SmartSystem.WeiXinServices;
using Common.Entities.Enum;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.YinSheng
{
    /// <summary>
    /// 银盛微信公众账号支付
    /// </summary>
    public class WxOfficialAccountPayController : Controller
    {
        public ActionResult ParkPayment(decimal orderId,string opendId)
        {
            OnlineOrder order = CheckOrder(orderId);
            try
            {

                if (order.OrderType != OnlineOrderType.ParkFee) throw new MyException("支付方法不正确");

                PayDictionary resultArray = new PayDictionary();
                resultArray.Add("method", "ysepay.online.weChat.app.pay");
                resultArray.Add("partner_id", YinShengConfig.PartnerId);
                resultArray.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                resultArray.Add("charset", "utf-8");
                resultArray.Add("sign_type", "RSA");
                resultArray.Add("version", "3.0");
                resultArray.Add("notify_url","");//异步地址
                resultArray.Add("return_url", "");//同步地址
                resultArray.Add("seller_id", YinShengConfig.SellerId);
                resultArray.Add("timeout_express", "10m");
                resultArray.Add("currency", "CNY");
                resultArray.Add("seller_name","商家名称");
                resultArray.Add("business_code", "01000010");
                resultArray.Add("extra_common_param", "备注说明");
                resultArray.Add("open_id", order.PayAccount);
                resultArray.Add("out_trade_no", orderId.ToString());
                resultArray.Add("subject","临停缴费");
                resultArray.Add("total_amount","0.1");
                resultArray.Sort(PaySortEnum.Asc);
                string par = resultArray.GetParmarStr();
                resultArray.Add("sign", YinShengCommon.SignEncrypt(par));
                ViewBag.PayDictionary = resultArray;
                return View(order);
            }
            catch (MyException ex) {
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/ParkingPayment/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WxOfficialAccountPay_Error", string.Format("支付失败 orderId:{0};openId:{1}", orderId, order.PayAccount), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "提交支付失败", returnUrl = "/ParkingPayment/Index" });
            }
        }
        private OnlineOrder CheckOrder(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            if (order == null)
            {
                throw new MyException("获取支付信息失败，请重新支付");
            }
            if (order.PaymentChannel != PaymentChannel.WeiXinPay)
            {
                throw new MyException("支付方法不正确");
            }
            if (order.Status != OnlineOrderStatus.WaitPay)
            {
                throw new MyException("订单不是可支付状态");
            }
            return order;
        }

    }
}
