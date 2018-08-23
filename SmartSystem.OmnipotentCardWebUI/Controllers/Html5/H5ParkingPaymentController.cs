using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinInerface;
using SmartSystem.WeiXinServices;
using Common.Services.Park;
using Common.Entities.Parking;
using System.Text;
using Common.Entities.Order;
using Common.Entities.WX;
using SmartSystem.WXInerface;
using Common.Services;
using Common.Entities;
using Common.Utilities;
using Common.Entities.Enum;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    /// <summary>
    /// 临停缴费
    /// </summary>
    public class H5ParkingPaymentController : H5WeiXinController
    {
        /// <summary>
        /// 车牌缴费
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(bool updatePlateNo=false)
        {
            List<string> myLicensePlates = new List<string>();
            string defaultPlateNumber = string.Empty;
            if (UserAccount != null) {
                myLicensePlates = CarService.GetTempCarInfoIn(UserAccount.AccountID);
                defaultPlateNumber = OnlineOrderServices.QueryLastPaymentPlateNumber(UserAccount.AccountID);
                if (!string.IsNullOrWhiteSpace(defaultPlateNumber) && !updatePlateNo)
                {
                    ParkIORecord model = ParkIORecordServices.QueryLastExitIORecordByPlateNumber(defaultPlateNumber);
                    if (model != null)
                    {
                        return RedirectToAction("ComputeParkingFee", "H5ParkingPayment", new { licensePlate = defaultPlateNumber, parkingId = model.ParkingID });
                    }
                }

                if (!string.IsNullOrWhiteSpace(defaultPlateNumber))
                {
                    if (myLicensePlates.Contains(defaultPlateNumber))
                    {
                        myLicensePlates.Remove(defaultPlateNumber);
                    }
                }
                else if (myLicensePlates.Count > 0)
                {
                    defaultPlateNumber = myLicensePlates.First();
                    myLicensePlates.Remove(defaultPlateNumber);
                }
            }
            ViewBag.PlateNumber = defaultPlateNumber;
            return View(myLicensePlates);
        }
        public ActionResult ComputeParkingFee(string licensePlate, string parkingId)
        {
            licensePlate = licensePlate.ToPlateNo();
            if (!string.IsNullOrWhiteSpace(licensePlate) && licensePlate.Length > 2)
            {
                string firstPlate = HttpUtility.UrlEncode(licensePlate.Substring(0, 2), Encoding.GetEncoding("UTF-8"));
                Response.Cookies.Add(new HttpCookie("SmartSystem_WeiXinUser_DefaultPlate", firstPlate));
            }
            try
            {
                OnlineOrder model = new OnlineOrder();
                model.OrderTime = DateTime.Now;

                TempParkingFeeResult result = RechargeService.WXTempParkingFee(licensePlate, parkingId, LoginAccountID, model.OrderTime);
                if (result.Result == APPResult.NoNeedPay || result.Result == APPResult.RepeatPay)
                {
                    int type = result.Result == APPResult.NoNeedPay ? 0 : 1;
                    return RedirectToAction("NotNeedPayment", "ParkingPayment", new { licensePlate = licensePlate, type = type, surplusMinutes = result.OutTime, entranceTime = result.EntranceDate });
                }
                RechargeService.CheckCalculatingTempCost(result.Result);
                if (result.OrderSource == PayOrderSource.Platform)
                {
                    bool testResult = CarService.WXTestClientProxyConnectionByPKID(result.ParkingID);
                    if (!testResult)
                    {
                        throw new MyException("车场网络异常，暂无法缴停车费！");
                    }
                    int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo);
                    if (interfaceOrderState != 1)
                    {
                        string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                        return PageAlert("Index", "H5ParkingPayment", new { RemindUserContent = msg });
                    }
                }
                model.ParkCardNo = result.CardNo;
                model.PKID = result.ParkingID;
                model.PKName = result.ParkName;
                model.InOutID = result.Pkorder.TagID;
                model.PlateNo = result.PlateNumber;
                model.EntranceTime = result.EntranceDate;
                model.ExitTime = model.OrderTime.AddMinutes(result.OutTime);
                model.Amount = result.Pkorder.Amount;
                model.PayDetailID = result.Pkorder.OrderNo;
                model.DiscountAmount = result.Pkorder.DiscountAmount;
                model.OrderSource = result.OrderSource;
                model.ExternalPKID = result.ExternalPKID;
                ViewBag.Result = result.Result;
                ViewBag.PayAmount = result.Pkorder.PayAmount;
                return View(model);

            }
            catch (MyException ex)
            {
                return PageAlert("Index", "H5ParkingPayment", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "计算缴费金额失败", ex, LogFrom.WeiXin);
                return PageAlert("Index", "H5ParkingPayment", new { RemindUserContent = "计算缴费金额失败" });
            }
        }
  
        /// <summary>
        /// 不需要缴费提醒页面
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="type">提醒类型0-不需要缴费 1-缴过费了，目前还不需要再次缴费</param>
        /// <returns></returns>
        public ActionResult NotNeedPayment(string licensePlate, int type, int surplusMinutes, DateTime entranceTime, string backUrl = "/QRCodeParkPayment/Index")
        {
            licensePlate = licensePlate.ToPlateNo();
            ViewBag.BackUrl = backUrl;
            ViewBag.Type = type;
            ViewBag.SurplusMinutes = surplusMinutes;
            ViewBag.EntranceTime = entranceTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.ParkingTotalDuration = entranceTime.GetParkingDuration(DateTime.Now);
            return View();
        }
        public ActionResult SubmitParkingPaymentRequest(OnlineOrder model)
        {
            try
            {
                if (model.OrderSource == PayOrderSource.Platform)
                {
                    int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(model.PayDetailID);
                    if (interfaceOrderState != 1)
                    {
                        string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                        return RedirectToAction("Index", "ErrorPrompt", new { message = msg, returnUrl = "/H5ParkingPayment/Index" });
                    }
                }
                model.OrderID = IdGenerator.Instance.GetId();
                model.Status = OnlineOrderStatus.WaitPay;
                model.OrderType = OnlineOrderType.ParkFee;
                model.Amount = model.Amount;

                BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
                if (company == null) throw new MyException("获取单位信息失败");

                if (model.PaymentChannel == PaymentChannel.AliPay)
                {
                    AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "获取支付宝配置信息失败[0001]", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "该支付宝暂停使用", "单位编号：" + config.CompanyID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用支付宝支付！" });
                    }
                    AliPayApiConfig requestConfig = AliPayApiConfigServices.QueryAliPayConfig(GetRequestCompanyId);
                    if (requestConfig == null) throw new MyException("获取请求单位微信配置失败");

                    if (config.CompanyID != requestConfig.CompanyID)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "支付的支付宝配置和请求的支付配置不匹配，不能支付", string.Format("支付单位：{0},请求单位：{1}", config.CompanyID, requestConfig.CompanyID), LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "支付的支付宝配置和请求的支付配置不匹配，不能支付！" });
                    }
                    model.AccountID = LoginAccountID;
                    model.CardId = string.Empty;
                    model.PayeeChannel = PaymentChannel.AliPay;
                    model.PaymentChannel = PaymentChannel.AliPay;
                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PayeeAccount;
                    model.PayAccount = string.Empty;
                    model.Payer = model.PayAccount;
                    model.CompanyID = config.CompanyID;
                }
                else
                {
                  
                    WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });
                    }
                    WX_ApiConfig requestConfig = WXApiConfigServices.QueryWXApiConfig(GetRequestCompanyId);
                    if (requestConfig == null) throw new MyException("获取请求单位微信配置失败");

                    if (config.CompanyID != requestConfig.CompanyID)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "微信支付配置和当前请求微信支付配置不匹配，不能支付", string.Format("支付单位：{0},请求单位：{1}", config.CompanyID, requestConfig.CompanyID), LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "微信支付配置和当前请求微信支付配置不匹配，不能支付！" });
                    }
                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PartnerId;
                    model.PayAccount = string.Empty;
                    model.Payer = model.PayAccount;
                    model.AccountID = LoginAccountID;
                    model.CardId = model.AccountID;
                    model.PayeeChannel = PaymentChannel.WeiXinPay;
                    model.PaymentChannel = PaymentChannel.WeiXinPay;
                    model.CompanyID = config.CompanyID;
                }

                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("生成待缴费订单失败");

                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("ParkCarPayment", "H5WeiXinPayment", new { orderId = model.OrderID});
                        }
                    case PaymentChannel.AliPay:
                        {
                            return RedirectToAction("AliPayRequest", "H5Order", new { orderId = model.OrderID, requestSource=1 });
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return RedirectToAction("ComputeParkingFee", "H5CodeParkPayment", new { licensePlate = model.PlateNo, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5ParkingPaymentError", "保存临停缴费订单失败", ex, LogFrom.WeiXin);
                return RedirectToAction("ComputeParkingFee", "H5CodeParkPayment", new { licensePlate = model.PlateNo, RemindUserContent = "提交支付失败" });
            }
        }
    }
}
