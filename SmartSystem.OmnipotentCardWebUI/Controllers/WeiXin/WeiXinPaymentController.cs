using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinServices;
using Common.Entities.Order;
using Common.Entities.Enum;
using SmartSystem.WeiXinServices.Payment;
using Common.Services.WeiXin;
using Common.Entities;
using Common.Services;
using SmartSystem.WXInerface;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class WeiXinPaymentController : WeiXinController
    {
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult ParkCarPayment(decimal orderId,int source=0)
        {
            string returnUrl = source == 0 ? "/ParkingPayment/Index" : "/QRCodeParkPayment/QRCodePaySuccess?orderId=" + orderId;
            try
            {
                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.ParkFee) throw new MyException("支付方法不正确");
                if (!OnlineOrderServices.UpdateSFMCode(order)) throw new MyException("处理订单信息异常【SFM】");

                string sAttach = Convert.ToString(Session["SmartSystem_WeiXinTg_personid"]);
                UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("临停缴费-{0}",order.PlateNo), sAttach);
                WeiXinPaySignModel payModel = GetWeiXinPaySign(order, model);
                ViewBag.MaxWaitTime = DateTime.Now.AddMinutes(WXOtherConfigServices.GetTempParkingWeiXinPayTimeOut(order.CompanyID)).ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.PayModel = payModel;
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.Source = source;
                Session["SmartSystem_WeiXinTg_personid"] = null;
                return View(order);
            }
            catch (MyException ex) {

                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = returnUrl });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = returnUrl });
            }
        }

        private WeiXinPaySignModel GetWeiXinPaySign(OnlineOrder order, UnifiedPayModel model)
        {
            WeiXinPaySignModel payModel = new WeiXinPaySignModel()
            {
                AppId = model.AppId,
                Package = string.Format("prepay_id={0}", order.PrepayId),
                Timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(),
                Noncestr = Util.CreateNoncestr(),
            };

            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            nativeObj.Add("appId", payModel.AppId);
            nativeObj.Add("package", payModel.Package);
            nativeObj.Add("timeStamp", payModel.Timestamp);
            nativeObj.Add("nonceStr", payModel.Noncestr);
            nativeObj.Add("signType", payModel.SignType);
            payModel.PaySign = model.GetCftPackage(nativeObj); //生成JSAPI 支付签名
            return payModel;
        }
        private UnifiedPayModel GetUnifiedPayModel(OnlineOrder order, string description,string attach = "")
        {
            UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(order.CompanyID);
            if (string.IsNullOrWhiteSpace(order.PrepayId))
            {
                string payNotifyAddress = string.Format("{0}{1}", WXApiConfigServices.QueryWXApiConfig(order.CompanyID).Domain, "/WeiXinPayNotify/Index");
                //预支付
                string postData = model.CreatePrePayPackage(order.OrderID.ToString(), ((int)(order.Amount * 100)).ToString(), WeiXinOpenId, description, payNotifyAddress, attach);
                UnifiedPrePayMessage result = PaymentServices.UnifiedPrePay(postData);
                if (result == null || !result.ReturnSuccess || !result.ResultSuccess || string.IsNullOrEmpty(result.Prepay_Id))
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", "预支付",string.Format("postData:{0}",postData), LogFrom.WeiXin);
                    throw new Exception(string.Format("获取PrepayId 失败,Message:{0}",result.ToXmlString()));
                }
                OnlineOrderServices.UpdatePrepayIdById(result.Prepay_Id, order.OrderID);
                order.PrepayId = result.Prepay_Id;
            }
            return model;
        }
        private OnlineOrder CheckOrder(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            if (order == null){
                throw new MyException("获取支付信息失败，请重新支付");
            }
            if (order.PaymentChannel != PaymentChannel.WeiXinPay)
            {
                throw new MyException("支付方法不正确");
            }
            if (order.Status != OnlineOrderStatus.WaitPay){
                throw new MyException("订单不是可支付状态");
            }
            return order;
        }
         [HttpPost]
        public ActionResult CheckOrderState(decimal orderId) {
            try
            {
                OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
                //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(order.PayDetailID);

                int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(order.PayDetailID, order.PKID, order.InOutID);
                if (interfaceOrderState != 1)
                  {
                      string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                      return Json(MyResult.Error(msg));
                   
                  }
                  return Json(MyResult.Success());
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("检查订单是否有效失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error("检查订单是否有效失败"));
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
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("支付超时或用户手动取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
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
                        else {
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
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("取消待支付订单失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
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
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("订单状态更改为支付中失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return Json(MyResult.Error("更改未支付中失败"));
            }
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

                UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("月卡充值-{0}", order.PlateNo));
                WeiXinPaySignModel payModel = GetWeiXinPaySign(order, model);
                ViewBag.PayModel = payModel;
                return View(order);
            }
            catch (MyException ex)
            {

                TxtLogServices.WriteTxtLogEx("WeiXinPayment_Error", "支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/CardRenewal/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("月卡续期支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/CardRenewal/Index" });
            }
        }

        public ActionResult AdvanceParkingPayment(decimal orderId) {
            try
            {
                AdvanceParking order = AdvanceParkingServices.QueryByOrderId(orderId);
                if (order == null) throw new MyException("支付信息不存在");
                if (order.OrderState != 0) throw new MyException("支付状态不正确");

                UnifiedPayModel model = UnifiedPayModel.CreateUnifiedModel(order.CompanyID);
                if (string.IsNullOrWhiteSpace(order.PrepayId))
                {
                    string payNotifyAddress = string.Format("{0}{1}", WXApiConfigServices.QueryWXApiConfig(order.CompanyID).Domain, "/WeiXinPayNotify/AdvanceParking");
                    //预支付
                    UnifiedPrePayMessage result = PaymentServices.UnifiedPrePay(model.CreatePrePayPackage(order.OrderId.ToString(), ((int)(order.Amount * 100)).ToString(), WeiXinOpenId, "预停车支付", payNotifyAddress));
                    if (result == null || !result.ReturnSuccess || !result.ResultSuccess || string.IsNullOrEmpty(result.Prepay_Id))
                    {
                        throw new Exception(string.Format("获取PrepayId 失败,Message:{0}", result.ToXmlString()));
                    }
                    AdvanceParkingServices.UpdatePrepayIdById(result.Prepay_Id, order.OrderId);
                    order.PrepayId = result.Prepay_Id;
                }
                WeiXinPaySignModel payModel = new WeiXinPaySignModel()
                {
                    AppId = model.AppId,
                    Package = string.Format("prepay_id={0}", order.PrepayId),
                    Timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(),
                    Noncestr = Util.CreateNoncestr(),
                };

                Dictionary<string, string> nativeObj = new Dictionary<string, string>();
                nativeObj.Add("appId", payModel.AppId);
                nativeObj.Add("package", payModel.Package);
                nativeObj.Add("timeStamp", payModel.Timestamp);
                nativeObj.Add("nonceStr", payModel.Noncestr);
                nativeObj.Add("signType", payModel.SignType);
                payModel.PaySign = model.GetCftPackage(nativeObj); //生成JSAPI 支付签名

                ViewBag.PayModel = payModel;
                return View(order);
            }
            catch (MyException ex)
            {
                return PageAlert("Index", "AdvanceParking", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex) {

                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("预停车支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return PageAlert("Index", "AdvanceParking", new { RemindUserContent = "支付异常,请重新支付" });
            }
        }

        /// <summary>
        /// 车位预约
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult BookingBitNoPayment(decimal orderId)
        {

            try
            {

                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.PkBitBooking) throw new MyException("支付方法不正确");

                UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("车位预约-{0}", order.PlateNo));
                WeiXinPaySignModel payModel = GetWeiXinPaySign(order, model);
                ViewBag.PayModel = payModel;
                return View(order);
            }
            catch (MyException ex)
            {

                TxtLogServices.WriteTxtLogEx("WeiXinPayment_Error", "支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/ParkBitBooking/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("车位预约支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/ParkBitBooking/Index" });
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

                UnifiedPayModel model = GetUnifiedPayModel(order,string.Format("{0}-商家充值", order.PKName));
                WeiXinPaySignModel payModel = GetWeiXinPaySign(order, model);
                ViewBag.PayModel = payModel;
                return View(order);
            }
            catch (MyException ex)

            {

                TxtLogServices.WriteTxtLogEx("WeiXinPayment_Error", "支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/ParkBitBooking/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("商家充值支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/SellerRecharge/Index" });
            }
        }


        /// <summary>
        /// APP充值
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult BalancePayment(decimal orderId)
        {
            try
            {
                OnlineOrder order = CheckOrder(orderId);
                if (order.OrderType != OnlineOrderType.APPRecharge) throw new MyException("支付方法不正确");

                UnifiedPayModel model = GetUnifiedPayModel(order, string.Format("{0}-APP充值", order.PlateNo));
                WeiXinPaySignModel payModel = GetWeiXinPaySign(order, model);
                ViewBag.PayModel = payModel;
                return View(order);
            }
            catch (MyException ex)
            {

                TxtLogServices.WriteTxtLogEx("WeiXinPayment_Error", "支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/PurseData/Index" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPayment_Error", string.Format("APP充值支付失败 orderId:{0};openId:{1}", orderId, WeiXinOpenId), ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "支付失败，请重新支付", returnUrl = "/PurseData/Index" });
            }
        }
    }
}
