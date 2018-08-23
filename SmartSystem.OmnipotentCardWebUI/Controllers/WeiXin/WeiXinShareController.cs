using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Common;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinBase;


namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 微信分享类型
    /// </summary>
    public class WeiXinShareController : Controller
    {
        public ActionResult Index(string shareaction = "l/ParkingPayment_Index_moduleid=2", string img = "", string title = "", string description = "")
        {
            try
            {
                //WX_ApiConfig config = WXApiConfigServices.QueryFirst();

                //title = string.IsNullOrWhiteSpace(title) ? config.SystemName : title;
                //description = string.IsNullOrWhiteSpace(description) ? config.SystemName : description;
                //img = string.IsNullOrWhiteSpace(img) ? config.SystemLogo : img;
                //if (Request.RawUrl.ToUpper().Contains("QRCODE/"))
                //{
                //    return Content("");
                //}
                //if (string.IsNullOrWhiteSpace(img)) {
                //    img = config.SystemLogo.TrimStart('/');
                //}
                //if (string.IsNullOrWhiteSpace(title)) {
                //    title = config.SystemName;
                //}
                //if (string.IsNullOrWhiteSpace(description))
                //{
                //    description = "智能停车系统，包括找停车场，移动支付停车费、月租车续期等，为车主提供更便捷的出行体验";
                //}
                //ViewBag.Url = config.Domain +"/"+ shareaction;
                //ViewBag.TLImg = config.Domain + "/" + img;
                //ViewBag.Title = title;
                //ViewBag.Description = description;

                //var timeStamp = DateTimeHelper.TransferUnixDateTime(DateTime.Now).ToString();
                //var nonceStr = StringHelper.GetRndString(16);
                //var url = Request.Url.ToString();
                //var signature = GetWeiXinSignature(config,nonceStr, url, timeStamp);
                //ViewBag.AppId = config.AppId;
                //ViewBag.Timestamp = timeStamp.ToString();
                //ViewBag.NonceStr = nonceStr;
                //ViewBag.Signature = signature;
                return PartialView();
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "分享页面错误", ex, LogFrom.WeiXin);
                return Content("");
            }
        }
        private string GetWeiXinSignature(WX_ApiConfig config,string noncestr, string url, string timestamp)
        {
            try
            {
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, false);
                var ticket = WxAdvApi.GetTicket(accessToken);
                return WxService.GetJsApiSignature(noncestr, ticket.ticket, timestamp, url);
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "分享页面获取签名错误", ex, LogFrom.WeiXin);
                return string.Empty;
            }

        }
    }
}
