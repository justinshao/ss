using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinBase;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class QRCodeController : Controller
    {
        public ActionResult Index(string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                {
                    return RedirectToAction("Index", "BrowseError", new { errorMsg = "请打开扫一扫" });
                }
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var timeStamp = DateTimeHelper.TransferUnixDateTime(DateTime.Now).ToString();
                var nonceStr = StringHelper.GetRndString(16);
                var url = Request.Url.ToString();

                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, false);
                var ticket = WxAdvApi.GetTicket(accessToken);
                string signature = WxService.GetJsApiSignature(nonceStr, ticket.ticket, timeStamp, url);
                ViewBag.Signature = signature;
                ViewBag.AppId = config.AppId;
                ViewBag.Timestamp = timeStamp;
                ViewBag.NonceStr = nonceStr;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "调用扫码方法异常", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "BrowseError", new { errorMsg = "调用扫码方法异常" });
            }
        }

    }
}
