using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5WeiXinController : Controller
    {
        /// <summary>
        /// 前台微信用户user信息。
        /// </summary>
        public WX_Info UserAccount
        {
            get
            {
                if (HttpContext.Session["SmartSystem_H5_WX_Info"] == null)
                {
                    var cookie = HttpContext.Request.Cookies["SmartSystem_H5_MobilePhone"];
                    var cookieCompany = HttpContext.Request.Cookies["SmartSystem_H5_CompanyID"];
                    if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value) && cookieCompany!=null && !string.IsNullOrWhiteSpace(cookieCompany.Value))
                    {
                        WX_Info user = WXAccountServices.QueryWXInfoByMobilePhone(cookie.Value, cookieCompany.Value);
                        HttpContext.Session["SmartSystem_H5_WX_Info"] = user;
                        return user;
                    }
                }
                return (WX_Info)HttpContext.Session["SmartSystem_H5_WX_Info"];
            }
        }
        public string LoginAccountID {
            get {
                if (UserAccount != null) {
                    return UserAccount.AccountID;
                }
                return string.Empty;
            }
        }
        public string GetAliPayUserId
        {
            get { 
                var cookie = Request.Cookies["SmartSystem_AliPay_UserId"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return cookie.Value;
                }
                return string.Empty;
            }
        }
        public string GetRequestCompanyId 
        {
            get {
                var cookie = Request.Cookies["SmartSystem_H5_CompanyID"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return cookie.Value;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 格式化script字符串
        /// </summary>
        /// <param name="strMsg">提示的消息</param>
        /// <param name="strUrl">跳转的url</param>
        /// <returns></returns>
        public static string Alert(string strMsg, string strUrl)
        {
            string rtnStr = "<script>alert('" + strMsg + "');location.href='" + strUrl + "'</script>";
            return rtnStr;
        }
        public ContentResult Alert(string strMsg, string actionName, string controllerName, object routeValues)
        {
            var script = Alert(strMsg, Url.Action(actionName, controllerName, routeValues));
            return Content(script, "text/html");
        }
        public ContentResult Alert(string strMsg, string actionName, string controllerName)
        {
            var script = Alert(strMsg, Url.Action(actionName, controllerName));
            return Content(script, "text/html");
        }
        public ContentResult PageAlert(string actionName, string controllerName)
        {
            string script = "<script>location.href='" + Url.Action(actionName, controllerName) + "'</script>";
            return Content(script, "text/html");
        }
        public ContentResult PageAlert(string actionName, string controllerName, dynamic routeValues)
        {
            string script = "<script>location.href='" + Url.Action(actionName, controllerName, routeValues) + "'</script>";
            return Content(script, "text/html");
        }
    }
}
