using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;
using SmartSystem.WeiXinServices;
using Common.Entities.Order;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5OrderController : H5WeiXinController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AliPayRequest(decimal orderId,int requestSource) {
            try
            {
                OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
                if (order == null) throw new MyException("获取订单信息失败");
                if (order.Status != Common.Entities.Enum.OnlineOrderStatus.WaitPay) throw new MyException("订单不是可支付状态");

                if (string.IsNullOrWhiteSpace(GetAliPayUserId))
                {
                    string id = string.Format("H5Order_AliPayRequest_orderId={0}^companyId={1}^requestSource={2}", order.OrderID, order.CompanyID, requestSource);
                    return RedirectToAction("Index", "AliPayAuthorize", new { id = id });
                }

                bool result = OnlineOrderServices.UpdatePayAccount(orderId, GetAliPayUserId);
                if (!result) throw new MyException("更改支付账号失败");
                if (requestSource == 1) {
                    return RedirectToAction("ParkCarPayment", "H5AliPayment", new { orderId = orderId });
                }
                if (requestSource == 2)
                {
                    return RedirectToAction("MonthCardPayment", "H5AliPayment", new { orderId = orderId });
                }
                throw new MyException("未知请求来源");
            }
            catch (MyException ex) {
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5OrderError", string.Format("更改支付人信息失败,订单编号：{0},单位编号：{1}",orderId,GetRequestCompanyId),ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "更改支付账号失败！" });
            }
        }
        public ActionResult PaySuccess(decimal orderId) {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            return View(order);
        }
    }
}
