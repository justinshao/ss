using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Common;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinInerface;
using Common.Entities.Order;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;
using SmartSystem.WXInerface;
using Common.Services.WeiXin;
using Common.Utilities;
using Common.Entities.Enum;
using SmartSystem.WeiXinServices;
using System.Text;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Utilities.Helpers;
using ClassLibrary1;


namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 停车缴费
    /// </summary>
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class ParkingPaymentController : WeiXinController
    {
        public ActionResult Index()
        {
            bool menuEnter = false;
            if (!string.IsNullOrWhiteSpace(Request["moduleid"]))
            {
                menuEnter = true;
            }
            

            //string sToken = AppUserToken;

            //if (sToken.IsEmpty())
            //{
            //    //没有登录
            //    //
            //    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
            //}

            return RedirectToAction("LicensePlatePayment", "ParkingPayment", new { menuEnter = menuEnter });

            //TxtLogServices.WriteTxtLogEx("ParkingPayment", "用户自动登录，id:{0}, ", sToken);
            //if (sToken.IsEmpty())
            //{
            //    VerifyCode verify = wxApi.getThirdLogin(WeiXinUser.OpenID, WeiXinUser.OpenID); //第三方登录
            //    TxtLogServices.WriteTxtLogEx("ParkingPayment", "用户自动登录，id:{0},Status:{1} ", WeiXinUser.OpenID, verify.Status);
            //    if (verify.Status == 1)
            //    {
            //        sToken = verify.Result;
            //        AppUserToken = sToken;
            //    }
            //    else if (verify.Status == 2)
            //    {
            //        AppUserToken = "";
            //        return RedirectToAction("LicensePlatePayment", "ParkingPayment", new { menuEnter = menuEnter });
            //    }
            //    else
            //    {
            //        AppUserToken = "";
            //        return RedirectToAction("Index", "ErrorPrompt", new { message = "用户自动登录失败" });
            //    }
            //}

            //CarManage carMessage = wxApi.getCarManage(sToken);
            ////if (carMessage == null)
            ////{
            ////    TxtLogServices.WriteTxtLogEx("ParkingPayment", "用户自动登录:{0}, ", "carMessage == null");
            ////    AppUserToken = "";
            ////    return RedirectToAction("Index", "ParkingPayment", new { menuEnter = menuEnter });
            ////}
            //if (carMessage == null || carMessage.Status == 40001)
            //{
            //    AppUserToken = "";
            //    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
            //}
            //else if (carMessage.Status == 1)
            //{
            //    if (carMessage.Result.Count > 0)
            //    {
            //        string number = carMessage.Result[0].LicensePlate.ToString().Trim();
            //        return RedirectToAction("ComputeNewParkingFee", "ParkingPayment", new { licensePlate = number });
            //    }
            //    else
            //    {
            //        return RedirectToAction("EmptyPage", "ParkingPayment");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户自动登录失败" });
            //}

        }

        /// <summary>
        /// 根据车牌缴费
        /// </summary>
        /// <returns></returns>
        public ActionResult LicensePlatePayment(string parkingId, bool menuEnter=false)
        {
            //List<string> myLicensePlates = CarService.GetTempCarInfoIn(WeiXinUser.AccountID);
            ViewBag.ParkingId = parkingId;
            string plateNumber = ""; // OnlineOrderServices.QueryLastPaymentPlateNumber(PaymentChannel.WeiXinPay, WeiXinUser.OpenID);
            if (menuEnter && !string.IsNullOrWhiteSpace(plateNumber))
            {
                ParkIORecord model = ParkIORecordServices.QueryLastExitIORecordByPlateNumber(plateNumber);
                if (model != null)
                {
                    return RedirectToAction("ComputeParkingFee", "ParkingPayment", new { licensePlate = plateNumber, parkingId = model.ParkingID });
                }
            }

            //string defaultPlateNumber = plateNumber;
            //if (!string.IsNullOrWhiteSpace(plateNumber))
            //{
            //    if (myLicensePlates.Contains(plateNumber))
            //    {
            //        myLicensePlates.Remove(plateNumber);
            //    }
            //}
            //else if (myLicensePlates.Count > 0)
            //{
            //    defaultPlateNumber = myLicensePlates.First();
            //    myLicensePlates.Remove(defaultPlateNumber);
            //}
            ViewBag.PlateNumber = plateNumber;
            return View();
        }
        /// <summary>
        /// 计算停车费
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ComputeParkingFee(string licensePlate,string parkingId)
        {
            string htmlurl = string.Empty;
            licensePlate = licensePlate.ToPlateNo();
            if (!string.IsNullOrWhiteSpace(licensePlate) && licensePlate.Length>2) {
                string firstPlate = HttpUtility.UrlEncode(licensePlate.Substring(0, 2), Encoding.GetEncoding("UTF-8"));
                Response.Cookies.Add(new HttpCookie("SmartSystem_WeiXinUser_DefaultPlate", firstPlate));
                //licensePlate = "浙B81X83";
                //string errMsg = "";
                ////string aa = string.Empty;
                ////aa=WXotherServices.GETURL(licensePlate).ToString();
                //ParkIORecord record =  ParkIORecordServices.GetNoExitIORecordByPlateNumber(parkingId, licensePlate, out errMsg);

                //if (record != null && record.EntranceImage.IsEmpty() == false)
                //{
                //    htmlurl = "http://www.yft166.net/Pic/" + record.EntranceImage;
                //}
                //else {
                //    htmlurl = "/Content/mobile/images/images.png";
                //}
                //
                //ViewBag.meas = parkingId;
                
            }
            

            bool IsShowPlateNumber = true;
            try
            {
                OnlineOrder model = new OnlineOrder();
                model.OrderTime = DateTime.Now;

                List<WX_CarInfo> myPlates = CarService.GetCarInfoByAccountID(WeiXinUser.AccountID);
                if (myPlates.Count(p => p.PlateNo.ToPlateNo() == licensePlate) == 0)
                {
                    IsShowPlateNumber = false;
                }
                TempParkingFeeResult result = RechargeService.WXTempParkingFee(licensePlate, parkingId, WeiXinUser.AccountID, model.OrderTime);
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
                    //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo);

                    int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo, result.ParkingID, result.Pkorder.TagID);
                    if (interfaceOrderState != 1)
                    {
                        string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                        return PageAlert("LicensePlatePayment", "ParkingPayment", new { RemindUserContent = msg });
                    }
                }
                model.ParkCardNo = result.CardNo;
                model.PKID = result.ParkingID;
                model.PKName = result.ParkName;
                model.InOutID = result.Pkorder.TagID;
                model.TagID = result.Pkorder.TagID;
                model.PlateNo = result.PlateNumber;
                model.EntranceTime = result.EntranceDate;
                model.ExitTime = model.OrderTime.AddMinutes(result.OutTime);
                model.Amount = result.Pkorder.Amount;
                model.PayDetailID = result.Pkorder.OrderNo;
                model.DiscountAmount = result.Pkorder.DiscountAmount;
                model.OrderSource = result.OrderSource;
                model.ExternalPKID = result.ExternalPKID;

                ParkIORecord record = ParkIORecordServices.QueryLastExitIORecordByPlateNumber(licensePlate);

                if (record != null && record.EntranceImage.IsEmpty() == false)
                {
                    htmlurl = "http://www.yft166.net/Pic/" + record.EntranceImage.Replace('\\','/');
                }
                else
                {
                    htmlurl = "/Content/mobile/images/images.png";
                }

                ViewBag.url = htmlurl;

                ViewBag.Result = result.Result;
                ViewBag.PayAmount = result.Pkorder.PayAmount;
                ViewBag.IsShowPlateNumber = IsShowPlateNumber;
                return View(model);

            }
            catch (MyException ex)
            {
                return PageAlert("LicensePlatePayment", "ParkingPayment", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "计算缴费金额失败", ex, LogFrom.WeiXin);
                return PageAlert("LicensePlatePayment", "ParkingPayment", new { RemindUserContent = "计算缴费金额失败" });
            }
        }

        public ActionResult ComputeNewParkingFee(string licensePlate, string parkingId)
        {
            string auth = AppUserToken;

            if (auth.IsEmpty())
            {
                //没有登录
                //
                return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });

            }
            else if (auth == "-1")
            {
                return RedirectToAction("Register", "ParkingPayment");
            }

            PresenceOrderList list = ParkingApi.GetPresenceOrder(auth);
            return View(list);

        }
        
        /// <summary>
        /// 不需要缴费提醒页面
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="type">提醒类型0-不需要缴费 1-缴过费了，目前还不需要再次缴费</param>
        /// <returns></returns>
        public ActionResult NotNeedPayment(string licensePlate, int type, int surplusMinutes, DateTime entranceTime)
        {
            ViewBag.LicensePlate = licensePlate.ToPlateNo();
            ViewBag.BackUrl = "/ParkingPayment/LicensePlatePayment";
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
                int time = WXOtherConfigServices.GetTempParkingWeiXinPayTimeOut(model.CompanyID);
                if (model.OrderTime.AddMinutes(time - 1) < DateTime.Now)
                {
                    throw new MyException("页面等待超时，请重新点击“立即结算”按钮");
                }
                if (model.OrderSource == PayOrderSource.Platform)
                {
                    //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(model.PayDetailID);
                    int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(model.PayDetailID,model.PKID, model.InOutID);
                    if (interfaceOrderState != 1)
                    {
                        string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                        return PageAlert("LicensePlatePayment", "ParkingPayment", new { RemindUserContent = msg });
                    }
                }

                BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
                if (company == null) throw new MyException("获取单位信息失败");

                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                if (config == null) {
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
                if (model.OrderSource == PayOrderSource.Platform && (CurrLoginWeiXinApiConfig == null || config.CompanyID != CurrLoginWeiXinApiConfig.CompanyID))
                {
                    string loginCompanyId = CurrLoginWeiXinApiConfig != null ? CurrLoginWeiXinApiConfig.CompanyID : string.Empty;
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "车场所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                    if (CurrLoginWeiXinApiConfig == null){
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信授权信息失败，请重试！" });
                    }
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "车场所属公众号和当前公众号不匹配，不能支付！" });
                }

                model.OrderID = IdGenerator.Instance.GetId();
                model.Status = OnlineOrderStatus.WaitPay;
                model.PayAccount = WeiXinUser.OpenID;
                model.Payer = WeiXinUser.OpenID;
                model.PayeeUser = config.SystemName;
                model.PayeeAccount = config.PartnerId;
                model.OrderType = OnlineOrderType.ParkFee;
                model.AccountID = WeiXinUser.AccountID;
                model.Amount = model.Amount;
                model.CardId = WeiXinUser.AccountID;
                model.CompanyID = config.CompanyID;
                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("生成待缴费订单失败");
                DateTime maxWaitTime = model.OrderTime.AddMinutes(time);
                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("ParkCarPayment", "WeiXinPayment", new { orderId = model.OrderID, maxWaitTime = maxWaitTime });
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return PageAlert("ComputeParkingFee", "ParkingPayment", new { licensePlate = model.PlateNo, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "保存临停缴费订单失败", ex, LogFrom.WeiXin);
                return PageAlert("ComputeParkingFee", "ParkingPayment", new { licensePlate = model.PlateNo, RemindUserContent = "提交支付失败" });
            }
        }

        
        public ActionResult Register() {
            return View();
        }

        public ActionResult EmptyPage() {
            return View();
        }

      

        
        /// <summary>
        ///  手机短信
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       [HttpPost]
        public JsonResult PhoneCode(string ID) {
            VerifyCode verify = wxApi.getPhone(ID);
            JsonResult res = new JsonResult();
            var Phone = new { Status = verify.Status, Result = verify.Result };
            res.Data = Phone;//返回单个对象； 
            return res;
        }

        /// <summary>
        /// 微信绑定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WeixinBingPhone(string phone, string code)
        {
            JsonResult res = new JsonResult();
            VerifyCode verify = wxApi.getBingding(phone, code, WeiXinUser.OpenID, WeiXinUser.OpenID);
            string sUrl = "";

            if (verify.Status == 1)
            {
                sUrl = "/l/ParkingPayment_ComputeNewParkingFee_moduleid=3%5Ecid=" + CurrLoginWeiXinApiConfig.CompanyID;

                //绑定成功了 再平台上也添加一个用户
                bool result = WeiXinAccountService.WXBindingMobilePhone(WeiXinUser.AccountID, phone);
                WeiXinUser.MobilePhone = phone;
                if (!result)
                {
                    TxtLogServices.WriteTxtLogEx("WXBindError", "用户绑定手机失败:{0}", result);
                }

            }

            var bangding = new { Status = verify.Status, Result = verify.Result, Url = sUrl };
            res.Data = bangding;
            return res;
        }
       

    }
}
