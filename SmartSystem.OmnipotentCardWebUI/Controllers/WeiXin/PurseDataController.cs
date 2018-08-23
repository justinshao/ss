using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
using ClassLibrary1.PurseData;
using ClassLibrary1.Authorization;
using SmartSystem.WeiXinServices.Payment;
using Common.Services;
using Common.Entities;
using Common.Entities.WX;
using Common.Services.WeiXin;
using Common.Entities.Order;
using Common.Utilities;
using Common.Entities.Enum;
using SmartSystem.WeiXinServices;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class PurseDataController : WeiXinController
    {
        //
        // GET: /PurseData/

        //我的钱包主页
        public ActionResult Index()
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

            //VerifyCode a = UnifiedClass.getUnified(token, "B20180623124743858", 0);
            //var aa = a.Result;
            //var bb = a.Status;
            ////token end 
            //var coupon = CouponClass.getCoupon(token, 100);
            //var balance = BalanceOrderClass.getBalanceOrder(token, 100, "e906234d-1875-e811-856b-ab69e9a7d233");

            UserInfo user = Purses.getUserInfo(auth);
            if (user != null && user.Status == 1)
            {
                ViewBag.Balance = user.Result.Balance;
                ViewBag.CouponCount = user.Result.CouponCount;
            }
            else
            {
                return RedirectToAction("index", "errorprompt", new { message = "用户登录失败" });
            }
            return View();
        }


        //钱包
        public ActionResult PurseIndex(string Balance)
        {
            ViewBag.balance = Balance;
            return View();
        }
        
        //优惠券
        public ActionResult MyCouponIndex(string a) {
            int b = Convert.ToInt32(a);

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


            MyCoupon coupons = MyCouponClass.getMyCoupon(auth, b);
            //判断有优惠券
            return View(coupons);
        }

        //详情
        public ActionResult CouponMore(string Cut, string timeStr, string timeEnd, string Brife, string ActivityName, string CouponType)
        {
            ViewBag.cut = Cut;
            ViewBag.timestr = timeStr;
            ViewBag.timeend = timeEnd;
            ViewBag.brife = Brife;
            ViewBag.activityname = ActivityName;
            ViewBag.coupontype = CouponType;
            return View();
        }

        //没有优惠券
        public ActionResult NotCouponMore() {
            return View();
        }

        /// <summary>
        /// 获得当前可以优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSellers(string price)
        {
            string auth = AppUserToken;


            JsonResult json = new JsonResult();
            if (auth.IsEmpty())
            {
                //没有登录
                //
                return json;

            }
            else if (auth == "-1")
            {
                return json;
            }

            
            try
            {
                  Coupon coupon = CouponClass.getCoupon(auth, Convert.ToInt32(price));
                  json.Data = coupon.Result.List.ToList();
            }  
            catch { }
            return json;
        }

        public ActionResult Czmx(decimal paymoney) {


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

            //生成一个订单

            try
            {
                
                TxtLogServices.WriteTxtLog("4");
                BaseCompany company = CompanyServices.QueryCompanyByRecordId(CurrLoginWeiXinApiConfig.CompanyID);
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
                string sPhone = "";
                //获取绑定的手机
                WX_Account account = WeiXinAccountService.GetAccountByID(WeiXinUser.AccountID);
                if (account != null && !account.MobilePhone.IsEmpty())
                {
                    sPhone = account.MobilePhone;
                }

                if (sPhone.IsEmpty())
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "您没有绑定APP账号，不能支付！" });
                }


                OnlineOrder model = new OnlineOrder();
                model.OrderID = IdGenerator.Instance.GetId();
                model.CardId = "";
                model.PKID = "";
                model.PKName = "";
                model.EntranceTime = DateTime.MinValue;
                model.ExitTime = DateTime.MinValue;
                model.MonthNum = 0;
                model.Amount = paymoney;
                model.Status = OnlineOrderStatus.WaitPay;
                model.PaymentChannel = PaymentChannel.WeiXinPay;
                model.Payer = WeiXinUser.OpenID;
                model.PayAccount = WeiXinUser.OpenID;
                model.OrderTime = DateTime.Now;
                model.PayeeChannel = PaymentChannel.WeiXinPay;
                model.AccountID = WeiXinUser.AccountID;
                model.OrderType = OnlineOrderType.APPRecharge;
                model.PlateNo = sPhone;
                model.PayeeUser = config.SystemName;
                model.PayeeAccount = config.PartnerId;
                model.CompanyID = config.CompanyID;
                model.Remark = "APP余额充值";
                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("余额失败[保存订单失败]");

                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("BalancePayment", "WeiXinPayment", new { orderId = model.OrderID });
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return  RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/PurseData/Index" }); ;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, returnUrl = "/PurseData/Index" });
            }
        }


        public ActionResult PaymentSuccess(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            return View(order);
        }



    }
}
