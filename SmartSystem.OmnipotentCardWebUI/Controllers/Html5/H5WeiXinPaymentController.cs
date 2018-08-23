using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinServices.Payment;
using Common.Utilities;
using Common.Services.WeiXin;
using SmartSystem.WeiXinServices;
using Common.Services;
using Common.Entities;
using Common.Entities.Order;
using Common.Entities.Enum;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5WeiXinPaymentController : Controller
    {
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult ParkCarPayment(decimal orderId)
        {
            try
            {
                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.ParkFee) throw new MyException("支付方法不正确");
                if (string.IsNullOrWhiteSpace(order.MWebUrl))
                {
                    UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("临停缴费-{0}-{1}", order.PKName, order.PlateNo));
                }
                ViewBag.MaxWaitTime = DateTime.Now.AddMinutes(WXOtherConfigServices.GetTempParkingWeiXinPayTimeOut(order.CompanyID)).ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.MWeb_Url = order.MWebUrl;
                return View(order);
            }
            catch (MyException ex) {
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/H5ParkingPayment/Index" });
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("H5WeiXinPayment_Error", string.Format("Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/H5ParkingPayment/Index" });
            }
           
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
                ExceptionsServices.AddExceptionToDbAndTxt("H5WeiXinPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};", orderId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5WeiXinPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};", orderId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error("取消订单失败"));
            }
        }
        private UnifiedPayModel GetUnifiedPayModel(OnlineOrder order, string description)
        {
            UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(order.CompanyID);
            if (string.IsNullOrWhiteSpace(order.PrepayId))
            {
              WX_ApiConfig apiConfig =  WXApiConfigServices.QueryWXApiConfig(order.CompanyID);

              string payNotifyAddress = string.Format("{0}{1}", apiConfig.Domain, "/WeiXinPayNotify/Index");
                //预支付
                string clientIp = AddressLoader.GetClientIP();
                string postData = model.CreateH5PrePayPackage(order.OrderID.ToString(), ((int)(order.Amount * 100)).ToString(), description, payNotifyAddress, clientIp, apiConfig.Domain, "智慧停车系统");
                UnifiedPrePayMessage result = PaymentServices.UnifiedPrePay(postData);
                if (result == null || !result.ReturnSuccess || !result.ResultSuccess || string.IsNullOrEmpty(result.Prepay_Id) || string.IsNullOrWhiteSpace(result.MWeb_Url))
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", "预支付", string.Format("postData:{0}", postData), LogFrom.WeiXin);
                    throw new Exception(string.Format("获取PrepayId 失败,Message:{0}", result.ToXmlString()));
                }
                OnlineOrderServices.UpdatePrepayIdById(result.Prepay_Id,result.MWeb_Url, order.OrderID);
                order.PrepayId = result.Prepay_Id;
                order.MWebUrl = result.MWeb_Url;
            }
            return model;
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
        /// <summary>
        /// 月卡续期
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult MonthCardPayment(decimal orderId)
        {
            try
            {
                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.MonthCardRecharge) throw new MyException("支付方法不正确");
                if (string.IsNullOrWhiteSpace(order.MWebUrl))
                {
                    UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("月卡充值-{0}-{1}", order.PKName, order.PlateNo));
                }
                ViewBag.MWeb_Url = order.MWebUrl;
                return View(order);
            }
            catch (MyException ex)
            {

                TxtLogServices.WriteTxtLogEx("H5WeiXinPayment_Error", "支付失败 orderId:{0};", orderId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/H5CardRenewal/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5WeiXinPayment_Error", string.Format("月卡续期支付失败 orderId:{0};", orderId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/H5CardRenewal/Index" });
            }
        }
        /// <summary>
        /// 商家充值
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult SellerRechargePayment(decimal orderId)
        {
            try
            {

                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.SellerRecharge) throw new MyException("支付方法不正确");

                if (string.IsNullOrWhiteSpace(order.MWebUrl))
                {
                    UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("商家充值-{0}", order.PKName));
                }
                ViewBag.MWeb_Url = order.MWebUrl;
                return View(order);
            }
            catch (MyException ex)
            {

                TxtLogServices.WriteTxtLogEx("H5WeiXinPayment_Error", "支付失败 orderId:{0};", orderId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/H5Seller/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5WeiXinPayment_Error", string.Format("商家充值支付失败 orderId:{0};", orderId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/H5Seller/Index" });
            }
        }
    }
}
