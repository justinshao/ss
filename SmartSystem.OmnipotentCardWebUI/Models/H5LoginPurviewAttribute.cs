using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Core;
using Common.Services;
using System.Web.Routing;
using Common.Entities.WX;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class H5LoginPurviewAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var mobilePhone = string.Empty;
                WX_Info user = null;
                var cookie = filterContext.RequestContext.HttpContext.Request.Cookies["SmartSystem_H5_MobilePhone"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    mobilePhone = cookie.Value;
                }
                var companyCookie = filterContext.RequestContext.HttpContext.Request.Cookies["SmartSystem_H5_CompanyID"];
                if (companyCookie == null || string.IsNullOrWhiteSpace(companyCookie.Value))
                {
                    filterContext.HttpContext.Response.Redirect(string.Format("~/ErrorPrompt/Error?message={0}", "获取单位信息失败，请重新进入页面"));
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                    return;
                }
               // string mobilePhone = string.Empty;
                var permission = Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (permission.Contains("Login"))
                {
                    if (string.IsNullOrWhiteSpace(mobilePhone))
                    {
#if DEBUG
                        mobilePhone = "18711015805";
#endif
                    }
                    if (string.IsNullOrWhiteSpace(mobilePhone))
                    {
                        var queryString = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                        if (filterContext.RequestContext.HttpContext.Request["returnUrl"] != null)
                        {
                            queryString = filterContext.RequestContext.HttpContext.Request["returnUrl"];
                        }
                        filterContext.HttpContext.Response.Redirect(string.Format("~/H5BindMobile/Index?returnUrl={0}", queryString));
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new EmptyResult();
                        return;
                    }
                }
                if (filterContext.HttpContext.Session["SmartSystem_H5_WX_Info"] != null)
                {
                    user = (WX_Info)filterContext.HttpContext.Session["SmartSystem_H5_WX_Info"];
                }
                if (user == null)
                {
                    user = WXAccountServices.QueryWXInfoByMobilePhone(mobilePhone, companyCookie.Value);
                }
                if (user == null)
                {
                    var queryString = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    if (filterContext.RequestContext.HttpContext.Request["returnUrl"] != null)
                    {
                        queryString = filterContext.RequestContext.HttpContext.Request["returnUrl"];
                    }
                    filterContext.HttpContext.Response.Redirect(string.Format("~/H5BindMobile/Index?returnUrl={0}", queryString));
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                    return;
                }
                HttpContext.Current.Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                filterContext.HttpContext.Session["SmartSystem_H5_WX_Info"] = user;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "验证H5登录信息失败", LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("H5LoginPurview", ex);
                return;
            }
        }
    }
}