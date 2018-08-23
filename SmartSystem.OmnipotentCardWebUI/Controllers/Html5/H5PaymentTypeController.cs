using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Services.WeiXin;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5PaymentTypeController : H5WeiXinController
    {
        public ActionResult Index(string preferentialMoney = "0", string payMoney = "0", bool showPreferentialMoney = true, string alreadyPayment = "0", bool showWaitPayMoney = true)
        {
            ViewBag.PreferentialMoney = preferentialMoney;
            ViewBag.PayMoney = payMoney;
            ViewBag.ShowWaitPayMoney = showWaitPayMoney;
            ViewBag.ShowPreferentialMoney = showPreferentialMoney;
            ViewBag.AlreadyPayment = alreadyPayment;
            bool SupportWeiXinPayment = false;
            bool SupportAliPayment = false;
            if (!string.IsNullOrWhiteSpace(GetRequestCompanyId))
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(GetRequestCompanyId);
                if (config != null) {
                    SupportAliPayment = true;
                }
                WX_ApiConfig wxConfig = WXApiConfigServices.QueryWXApiConfig(GetRequestCompanyId);
                if (wxConfig != null)
                {
                    SupportWeiXinPayment = true;
                }
            }
            ViewBag.SupportWeiXinPayment = SupportWeiXinPayment;
            ViewBag.SupportAliPayment = SupportAliPayment;
            return PartialView();
        }

    }
}
