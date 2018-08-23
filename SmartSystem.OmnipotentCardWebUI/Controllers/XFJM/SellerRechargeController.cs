using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Order;
using Common.Utilities;
using Common.Entities.Enum;
using Common.Entities.WX;
using Common.Services.WeiXin;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinServices;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.XFJM
{
    /// <summary>
    /// 商家充值
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    [CheckSellerPurview]
    public class SellerRechargeController : XFJMController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveRecharge(decimal Amount)
        {
            try
            {
                BaseVillage village = VillageServices.QueryVillageByRecordId(SellerLoginUser.VID);
                if (village == null) throw new MyException("获取小区信息失败");

                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(village.CPID);
                if (config == null)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取微信配置信息失败", "单位编号：" + village.CPID, LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                }
                if (!config.Status)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "该车场暂停使用微信支付", "单位编号：" + village.CPID, LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });
                }
                if (config.CompanyID != WeiXinUser.CompanyID)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "微信用户所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, WeiXinUser.CompanyID), LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "微信用户所属公众号和当前公众号不匹配，不能支付！" });
                }
                if (CurrLoginWeiXinApiConfig == null || config.CompanyID != CurrLoginWeiXinApiConfig.CompanyID)
                {
                    string loginCompanyId = CurrLoginWeiXinApiConfig != null ? CurrLoginWeiXinApiConfig.CompanyID : string.Empty;
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "车场所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "车场所属公众号和当前公众号不匹配，不能支付！" });
                }

                OnlineOrder order = new OnlineOrder();
                order.OrderID = IdGenerator.Instance.GetId();
                order.InOutID = SellerLoginUser.SellerID;
                order.PKID = SellerLoginUser.SellerID;
                order.Status = OnlineOrderStatus.WaitPay;
                order.PayAccount = WeiXinUser.OpenID;
                order.Payer = WeiXinUser.OpenID;
                order.PKName = SellerLoginUser.SellerName;
                order.Amount = Amount;
                order.PayeeUser = config.SystemName;
                order.PayeeAccount = config.PartnerId;
                order.OrderType = OnlineOrderType.SellerRecharge;
                order.PaymentChannel = PaymentChannel.WeiXinPay;
                order.PayeeChannel = PaymentChannel.WeiXinPay;
                order.AccountID = WeiXinUser.AccountID;
                order.Amount = Amount;
                order.CardId = WeiXinUser.AccountID;
                order.CompanyID = config.CompanyID;
                order.OrderTime = DateTime.Now;
                order.Remark = "商家充值";
                bool result = OnlineOrderServices.Create(order);
                if (!result) throw new MyException("充值失败[保存订单失败]");

                return RedirectToAction("SellerRechargePayment", "WeiXinPayment", new { orderId = order.OrderID });
            }
            catch (MyException ex)
            {
                return PageAlert("Index", "SellerRecharge", new {RemindUserContent = ex.Message });
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "提交商家充值信息失败", ex, LogFrom.WeiXin);
                return PageAlert("Index", "SellerRecharge", new { RemindUserContent = "提交商家充值信息失败" });
            }
        }
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeRecord()
        {
            ViewBag.StartTime = DateTime.Now.AddDays(-7).Date.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.OrderSource = EnumHelper.GetEnumContextList(typeof(OrderSource), true);
            return View();
        }
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRechargeRecordData(int orderSource, DateTime? start, DateTime? end, int page)
        {
            try
            {
                int pageSize = 10;
                int total = 0;
                List<ParkOrder> orders = ParkOrderServices.GetSellerRechargeOrder(SellerLoginUser.SellerID,orderSource,start,end, page, pageSize, out total);
                var models = from p in orders
                             select new
                             {
                                 OrderNo = p.OrderNo,
                                 OrderType = p.OrderType.GetDescription(),
                                 PayWay = p.PayWay.GetDescription(),
                                 Amount = p.Amount,
                                 OrderSource = p.OrderSource.GetDescription(),
                                 OrderTime = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Balance=p.NewMoney
                             };
                return Json(MyResult.Success("", models));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取商家充值记录失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取商家充值记录失败"));
            }
        }
        public ActionResult PaymentSuccess(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            return View(order);
        }
    }
}
