using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using Common.Entities.Enum;
using Common.Entities.Order;
using Common.Services.WeiXin;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinServices;
using SmartSystem.AliPayServices.Entities;
using SmartSystem.AliPayServices;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.AliPay
{
    public class AliPaymentController : WeiXinController
    {
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkCarPayment(decimal orderId,int source = 0)
        {
            if (string.IsNullOrWhiteSpace(AliPayUserId)) {
                return RedirectToAction("Index", "ErrorPrompt", new { message = "获取用户信息失败" });
            }
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
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/QRCodeParkPayment/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", string.Format("支付失败 orderId:{0};AliUserId:{1}", orderId, AliPayUserId), ex, LogFrom.AliPay);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/QRCodeParkPayment/Index" });
            }
        }
        private string GetParkingName(string parkingId)
        {
            try
            {
                BaseParkinfo model = ParkingServices.QueryParkingByParkingID(parkingId);
                if (model != null)
                {
                    return model.PKName;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", "获取车场名称失败", ex);
                return string.Empty;
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
            string tradeNo = AliPayApiServices.MakeAliPayOrder(order.CompanyID,model);
            if (string.IsNullOrWhiteSpace(tradeNo)){
                throw new MyException("创建交易订单失败");
            }
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
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
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
            string controllerName = "ParkingPayment";
            try
            {

                OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
                if (order != null)
                {
                    OnlineOrderServices.CancelOrder(orderId);
                    if (order.OrderType == OnlineOrderType.MonthCardRecharge)
                    {
                        controllerName = "CardRenewal";
                        actionName = "Index";
                    }
                    if (order.OrderType == OnlineOrderType.ParkFee)
                    {
                        if (source == 0)
                        {
                            controllerName = "ParkingPayment";
                            actionName = "Index";
                        }
                        else
                        {
                            controllerName = "QRCodeParkPayment";
                            actionName = "Index";
                        }

                    }
                    if (order.OrderType == OnlineOrderType.PkBitBooking)
                    {
                        controllerName = "ParkBitBooking";
                        actionName = "Index";

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", string.Format("取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
            }
            return PageAlert(actionName, controllerName, new { RemindUserContent = "取消成功" });
        }
        [HttpPost]
        public ActionResult OrderPaymenting(decimal orderId)
        {
            try
            {
                OnlineOrderServices.OrderPaying(orderId);
                return Json(MyResult.Success());
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPay_Error", string.Format("订单状态更改为支付中失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error("更改未支付中失败"));
            }
        }

        public ActionResult PaySuccess(decimal orderId) {
            return View();
        }
    }
}
