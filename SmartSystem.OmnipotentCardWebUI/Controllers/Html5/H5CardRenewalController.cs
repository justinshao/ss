using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface;
using Common.Entities;
using Common.Services;
using Common.Entities.Enum;
using Common.Services.WeiXin;
using Common.Entities.Order;
using Common.Utilities;
using SmartSystem.WeiXinServices;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Services.BaseData;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5CardRenewalController : H5WeiXinController
    {
        public ActionResult Index()
        {
            var nameCookie = Request.Cookies["SmartSystem_MonthCardPayment_Month"];
            if (nameCookie != null)
            {
                nameCookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(nameCookie);

            }
            var nameCookieRecharge = Request.Cookies["SmartSystem_GiftCardRecharge_PayMoney"];
            if (nameCookieRecharge != null)
            {
                nameCookieRecharge.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(nameCookieRecharge);

            }
            int defaultShowItem = 0;
            int type = 0;
            if (!string.IsNullOrWhiteSpace(Request["ShowItem"]) && int.TryParse(Request["ShowItem"].ToString(), out type))
            {
                defaultShowItem = type;
            }
            ViewBag.DefaultShowItem = defaultShowItem;

            List<ParkUserCarInfo> carInfos = new List<ParkUserCarInfo>();
            if (UserAccount != null) {
                carInfos =RechargeService.GetMonthCarInfoByAccountID(UserAccount.AccountID);
            }
            return View(carInfos);
        }
        [HttpPost]
        public ActionResult CheckBindMobile()
        {
            try
            {
                if (UserAccount!=null)
                {
                    return Json(MyResult.Success(UserAccount.MobilePhone));
                }
                return Json(MyResult.Error("未绑定"));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "检查是否绑了手机好失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error(""));
            }

        }
        /// <summary>
        /// 根据车牌号月卡续期
        /// </summary>
        /// <returns></returns>
        public ActionResult PlateNoRenewal(string plateNo)
        {
            try
            {
                List<ParkUserCarInfo> carInfos = RechargeService.GetMonthCarInfoByPlateNumber(plateNo);
                if (carInfos == null || carInfos.Count == 0)
                {
                    return PageAlert("Index", "H5CardRenewal", new { RemindUserContent = "找不到该车辆信息，请检查车牌输入是否正确" });
                }
                if (carInfos.Count == 1)
                {
                    if (!carInfos.First().IsAllowOnlIne)
                    {
                        return PageAlert("Index", "H5CardRenewal", new { RemindUserContent = "该车辆不支持线上缴费" });
                    }
                    return RedirectToAction("MonthCardRenewal", "H5CardRenewal", new { plateNo = plateNo, cardId = carInfos.First().CardID, source = 1 });
                }
                return View(carInfos);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", string.Format("获取车牌信息失败，车牌号：" + plateNo), ex, LogFrom.WeiXin);
                return PageAlert("Index", "H5CardRenewal", new { RemindUserContent = "获取车牌信息失败" });
            }
        }
        /// <summary>
        /// 月卡续期
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthCardRenewal(string cardId, string plateNo = "", int source = 0)
        {
            try
            {
                string selectMonth = string.Empty;
                string selectPayType = string.Empty;
                var selectParam = HttpContext.Request.Cookies["SmartSystem_MonthCardPayment_Month"];
                if (selectParam != null && !string.IsNullOrWhiteSpace(selectParam.Value))
                {
                    if (selectParam.Value.Contains(","))
                    {
                        string[] strs = selectParam.Value.Split(',');
                        selectMonth = strs[0];
                        selectPayType = strs[1];
                    }
                }
                ViewBag.SelectMonth = selectMonth;
                ViewBag.SelectPayType = selectPayType;
                ViewBag.SystemDate = DateTime.Now.Date;

                List<ParkUserCarInfo> carInfos = new List<ParkUserCarInfo>();
                if (source == 1)
                {
                    carInfos =RechargeService.GetMonthCarInfoByPlateNumber(plateNo);
                }
                else {
                    if (UserAccount != null) {
                        carInfos = RechargeService.GetMonthCarInfoByAccountID(UserAccount.AccountID);
                    }
                }
                ParkUserCarInfo card = carInfos.FirstOrDefault(p => p.CardID == cardId);
                if (card == null) throw new MyException("获月卡信息失败");
                ViewBag.Source = source;

                bool canConnection = CarService.WXTestClientProxyConnectionByVID(card.VID);
                if (!canConnection)
                {
                    return PageAlert("Index", "H5CardRenewal", new { RemindUserContent = "车场网络异常，暂无法续期，请稍后再试！" });
                }
                ViewBag.CardRenewalMonthDic = GetCardRenewalMonth(card.OnlineUnit);
                return View(card);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", string.Format("获取续费信息失败，编号：" + cardId), ex, LogFrom.WeiXin);
                return PageAlert("Index", "H5CardRenewal", new { RemindUserContent = "获取卡信息失败" });
            }
        }
        private Dictionary<int, string> GetCardRenewalMonth(int onlineUnit)
        {
            Dictionary<int, string> models = new Dictionary<int, string>();
            if (onlineUnit <= 0)
            {
                onlineUnit = 1;
            }
            for (var i = 1; i <= 12; i++)
            {
                if (i % onlineUnit != 0)
                {
                    continue;
                }
                string text = string.Format("{0}个月", i);
                models.Add(i, text);
            }
            return models;
        }
        [HttpPost]
        public ActionResult SubmitMonthRenewals(string cardId, int month, double paymoney, PaymentChannel paytype, DateTime afterdate, string plateno, int source)
        {
            try
            {
                List<ParkUserCarInfo> carInfos = new List<ParkUserCarInfo>();
                if (source == 1)
                {
                    carInfos = RechargeService.GetMonthCarInfoByPlateNumber(plateno);
                }
                else
                {
                    if (UserAccount != null)
                    {
                        carInfos = RechargeService.GetMonthCarInfoByAccountID(UserAccount.AccountID);
                    }
                }
                ParkUserCarInfo card = carInfos.FirstOrDefault(p => p.CardID == cardId);
                if (card == null) throw new MyException("获月卡信息失败");
                CheckMonthCardOrder(card, month, paymoney, paytype, afterdate);

                BaseCompany company = CompanyServices.QueryByParkingId(card.PKID);
                if (company == null) throw new MyException("获取单位信息失败");

                OnlineOrder model = new OnlineOrder();
                if (paytype == PaymentChannel.AliPay)
                {
                    AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "获取支付宝配置信息失败[0001]", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "该支付宝暂停使用", "单位编号：" + config.CompanyID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用支付宝支付！" });
                    }
                    AliPayApiConfig requestConfig = AliPayApiConfigServices.QueryAliPayConfig(GetRequestCompanyId);
                    if (requestConfig == null) throw new MyException("获取请求单位微信配置失败");

                    if (config.CompanyID != requestConfig.CompanyID)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "支付的支付宝配置和请求的支付配置不匹配，不能支付", string.Format("支付单位：{0},请求单位：{1}", config.CompanyID, requestConfig.CompanyID), LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "支付的支付宝配置和请求的支付配置不匹配，不能支付！" });
                    }
                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PayeeAccount;
                    model.PayeeChannel = PaymentChannel.AliPay;
                    model.PaymentChannel = PaymentChannel.AliPay;
                    model.CompanyID = config.CompanyID;
                }
                else
                {
                    WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });

                    }
                    WX_ApiConfig requestConfig = WXApiConfigServices.QueryWXApiConfig(GetRequestCompanyId);
                    if (requestConfig == null) throw new MyException("获取请求单位微信配置失败");

                    if (config.CompanyID != requestConfig.CompanyID)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", "微信支付配置和当前请求微信支付配置不匹配，不能支付", string.Format("支付单位：{0},请求单位：{1}", config.CompanyID, requestConfig.CompanyID), LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "微信支付配置和当前请求微信支付配置不匹配，不能支付！" });
                    }
                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PartnerId;
                    model.PayeeChannel = PaymentChannel.WeiXinPay;
                    model.PaymentChannel = PaymentChannel.WeiXinPay;
                    model.CompanyID = config.CompanyID;
                }
                               
                model.OrderID = IdGenerator.Instance.GetId();
                model.CardId = card.CardID;
                model.PKID = card.PKID;
                model.PKName = card.PKName;
                model.EntranceTime = card.EndDate;
                model.ExitTime = afterdate;
                model.MonthNum = month;
                model.Amount = (decimal)paymoney;
                model.Status = OnlineOrderStatus.WaitPay;
            
                model.OrderTime = DateTime.Now;
                model.AccountID = LoginAccountID;
                model.OrderType = OnlineOrderType.MonthCardRecharge;
                model.PlateNo = card.PlateNumber;
           
                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("续期失败[保存订单失败]");

                Response.Cookies.Add(new HttpCookie("SmartSystem_MonthCardPayment_Month", string.Format("{0},{1}", month, (int)paytype)));

                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("MonthCardPayment", "H5WeiXinPayment", new { orderId = model.OrderID });
                        }
                    case PaymentChannel.AliPay:
                        {
                            return RedirectToAction("AliPayRequest", "H5Order", new { orderId = model.OrderID, requestSource=2 });
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return PageAlert("MonthCardRenewal", "H5CardRenewal", new { cardId = cardId, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CardRenewalError", string.Format("获取续费信息失败，编号：" + cardId), ex, LogFrom.WeiXin);
                return PageAlert("MonthCardRenewal", "CardRenewal", new { cardId = cardId, RemindUserContent = "提交支付失败" });
            }
        }
        private void CheckMonthCardOrder(ParkUserCarInfo card, int month, double paymoney, PaymentChannel paytype, DateTime afterdate)
        {
            if (card == null) throw new MyException("获月卡信息失败");

            if (!card.IsAllowOnlIne) throw new MyException("该车场不支持手机续期");

            if (month < 1) throw new MyException("请选择续期月数");

            if (card.MaxMonth != 0 && card.MaxMonth < month)
            {
                throw new MyException(string.Format("续期月数必须小于等于{0}个月", card.MaxMonth));
            }
            if (card.Amount * month != (decimal)paymoney)
            {
                throw new MyException("支付金额不正确");
            }
            DateTime start = DateTime.Now;
            if (card.BeginDate != DateTime.MinValue)
            {
                start = card.BeginDate;
            }
            if (BaseCardServices.CalculateNewEndDate(start, card.EndDate, month) != afterdate.Date)
            {
                throw new MyException("计算月卡结束时间错误");
            }
        }
        [HttpPost]
        public JsonResult CalculateNewEndDate(DateTime? beginDate, DateTime? endDate, int month)
        {
            DateTime start = beginDate.HasValue ? beginDate.Value : DateTime.Now;
            JsonResult json = new JsonResult();
            json.Data = BaseCardServices.CalculateNewEndDate(start, endDate, month).ToString("yyyy-MM-dd");
            return json;
        }
    }
}
