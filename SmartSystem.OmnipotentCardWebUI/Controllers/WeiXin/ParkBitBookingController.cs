using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinInerface;
using Common.Entities.WX;
using Common.Services;
using Common.Entities;
using Common.Entities.Order;
using Common.Services.WeiXin;
using Common.Utilities;
using Common.Entities.Enum;
using SmartSystem.WeiXinServices;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Parking;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 车位预订
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    public class ParkBitBookingController : WeiXinController
    {
        public ActionResult Index()
        {
            string plateNumber = OnlineOrderServices.QueryLastPaymentPlateNumber(PaymentChannel.WeiXinPay, WeiXinUser.OpenID);
            ViewBag.PlateNumber = plateNumber;
            return View();
        }
        public ActionResult SaveBooking(string plateNo, string parkingId, string areaId, DateTime startTime, DateTime endTime)
        {
            try
            {
                WXReserveBitResult result = PkBitBookingServices.WXReservePKBit(WeiXinUser.AccountID, string.Empty, parkingId, areaId, plateNo, startTime, endTime);
                if (result.code == 0) {
                    if (parkingId != result.Order.PKID) throw new MyException("车场编号不一致");

                    BaseParkinfo parking= ParkingServices.QueryParkingByParkingID(parkingId);
                    if (parking == null) throw new MyException("获取车场信息失败");
                    ViewBag.ParkingName = parking.PKName;

                    ParkArea area = ParkAreaServices.QueryByRecordId(areaId);
                    if (area == null) throw new MyException("获取区域信息失败");
                    ViewBag.AreaName = area.AreaName;
                    ViewBag.PlateNo = plateNo;
                    ViewBag.StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ViewBag.EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ViewBag.AreaId = areaId;
                    return View(result);
                }
                if (result.code == 1) throw new MyException("您已经预约过了，请勿重复预约");
                throw new MyException("预约失败["+result.message+"]");
            }
            catch (MyException ex) {
                return PageAlert("Index", "ParkBitBooking", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinWeb", "预约失败", ex, LogFrom.WeiXin);
                return PageAlert("Index", "ParkBitBooking", new { RemindUserContent = "预约失败!" });
            }
        }
        [HttpPost]
        public ActionResult SubmitBookingPayment(OnlineOrder model)
        {
            try
            {

                BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
                if (company == null) throw new MyException("获取单位信息失败");

                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                if (config == null)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                }
                if (!config.Status)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });
                }
                if (config.CompanyID != WeiXinUser.CompanyID)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "微信用户所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, WeiXinUser.CompanyID), LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "微信用户所属公众号和当前公众号不匹配，不能支付！" });
                }
                if (CurrLoginWeiXinApiConfig == null || config.CompanyID != CurrLoginWeiXinApiConfig.CompanyID)
                {
                    string loginCompanyId = CurrLoginWeiXinApiConfig != null ? CurrLoginWeiXinApiConfig.CompanyID : string.Empty;
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "车场所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "车场所属公众号和当前公众号不匹配，不能支付！" });
                }

                model.OrderID = IdGenerator.Instance.GetId();
                model.Status = OnlineOrderStatus.WaitPay;
                model.PayAccount = WeiXinUser.OpenID;
                model.Payer = WeiXinUser.OpenID;
                model.PayeeUser = config.SystemName;
                model.OrderType = OnlineOrderType.PkBitBooking;
                model.AccountID = WeiXinUser.AccountID;
                model.OrderTime = DateTime.Now;
                model.CompanyID = config.CompanyID;
                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("生成车位预约订单失败");
                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("BookingBitNoPayment", "WeiXinPayment", new { orderId = model.OrderID});
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return PageAlert("SaveBooking", "ParkBitBooking", new { plateNo = model.PlateNo, parkingId = model.PKID, areaId = model.CardId, startTime=model.BookingStartTime,endTime=model.BookingEndTime, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinWeb", "预约失败", ex, LogFrom.WeiXin);
                return PageAlert("SaveBooking", "ParkBitBooking", new { plateNo = model.PlateNo, parkingId = model.PKID, areaId = model.CardId, startTime = model.BookingStartTime, endTime = model.BookingEndTime, RemindUserContent = "预约失败" });
            }
        }
        public ActionResult PaymentSuccess(decimal orderId) {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            return View(order);
        }
        public ActionResult BitBookingRecord() {
            List<ParkReserveBit> models = PkBitBookingServices.WXReserveBitPay(WeiXinUser.AccountID);
            return View(models);
        }
    }
}
