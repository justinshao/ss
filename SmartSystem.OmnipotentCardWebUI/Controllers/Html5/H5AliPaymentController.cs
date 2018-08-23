using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.Order;
using Common.Entities.Enum;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;
using Common.Services;
using SmartSystem.AliPayServices.Entities;
using Common.Utilities.Helpers;
using SmartSystem.AliPayServices;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5AliPaymentController : H5WeiXinController
    {
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkCarPayment(decimal orderId)
        {
            try
            {
                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.ParkFee) throw new MyException("支付方法不正确");
             
                string tradeNo = MakeAlipayTradeOrder(order);
                OnlineOrderServices.UpdatePrepayIdById(tradeNo, order.OrderID);
                order.PrepayId = tradeNo;
                ViewBag.MaxWaitTime = DateTime.Now.AddMinutes(WXOtherConfigServices.GetTempParkingWeiXinPayTimeOut(order.CompanyID)).ToString("yyyy-MM-dd HH:mm:ss");
                return View(order);
            }
            catch (MyException ex)
            {
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/H5ParkingPayment/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5AliPayment_Error", string.Format("支付失败 orderId:{0};AliUserId:{1}", orderId, GetAliPayUserId), ex, LogFrom.AliPay);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/QRCodeParkPayment/Index" });
            }
        }
      
        private string MakeAlipayTradeOrder(OnlineOrder order)
        {

            string parkid = string.Empty;
            AlipayTradeOrderModel model = new AlipayTradeOrderModel();
            model.out_trade_no = order.OrderID.ToString();
            model.total_amount = order.Amount.ToString("F2");
            model.discountable_amount = "";
            model.undiscountable_amount = "";
            model.seller_id = order.PayeeAccount;
            model.buyer_logon_id = "";
            model.subject = order.PKName + "," + order.PlateNo + "," + order.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss");
            model.buyer_id = order.PayAccount;
            model.store_id = parkid;
            model.body = string.Format("临停缴费-{0}-{1}", order.PKName, order.PlateNo);
            TxtLogServices.WriteTxtLogEx("AliPayApiServices", JsonHelper.GetJsonString(model));
            string tradeNo = AliPayApiServices.MakeAliPayOrder(order.CompanyID, model);
            if (string.IsNullOrWhiteSpace(tradeNo)) throw new MyException("创建交易订单失败");
            return tradeNo;
        }
        private OnlineOrder CheckOrder(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            if (order == null)
            {
                throw new MyException("获取支付信息失败，请重新支付");
            }
            if (order.PaymentChannel != PaymentChannel.AliPay)
            {
                throw new MyException("支付方法不正确");
            }
            if (order.Status != OnlineOrderStatus.WaitPay)
            {
                throw new MyException("订单不是可支付状态");
            }
            return order;
        }
        [HttpPost]
        public ActionResult CheckPayTimeOut(string eTime, decimal orderId)
        {
            DateTime endtime;
            if (!DateTime.TryParse(eTime, out endtime))
            {
                return Json(MyResult.Error("支付异常"));
            }


            DateTime nowTime = DateTime.Now;
            if (endtime < nowTime)
            {
                return Json(MyResult.Error(string.Format("{0}分{1}秒", "00", "00")));
            }
            TimeSpan countdownSpan = endtime - nowTime;
            if (countdownSpan.TotalSeconds > 0)
            {

                string mm = countdownSpan.Minutes >= 10 ? countdownSpan.Minutes.ToString() : "0" + countdownSpan.Minutes.ToString();
                string ss = countdownSpan.Seconds >= 10 ? countdownSpan.Seconds.ToString() : "0" + countdownSpan.Seconds.ToString();
                string syTime = string.Format("{0}分{1}秒", mm, ss);
                return Json(MyResult.Success(syTime));

            }
            return Json(MyResult.Error("支付超过时限"));
        }
        [HttpPost]
        public ActionResult AsynCancelOrder(decimal orderId)
        {
            try
            {
                OnlineOrderServices.CancelOrder(orderId);
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5AliPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};AliPayUserId:{1}", orderId, GetAliPayUserId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5AliPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};AliPayUserId:{1}", orderId, GetAliPayUserId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error("取消订单失败"));
            }
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult CancelOrder(decimal orderId, int source = 0)
        {
            string actionName = "Index";
            string controllerName = "H5ParkingPayment";
            try
            {
                OnlineOrderServices.CancelOrder(orderId);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5AliPayment_Error", string.Format("取消待支付订单失败 orderId:{0};AliPayUserId:{1}", orderId, GetAliPayUserId), ex, LogFrom.WeiXin);
            }
            return PageAlert(actionName, controllerName, new { RemindUserContent = "取消成功" });
        }

    }
}
