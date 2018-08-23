using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Core;
using Common.Services;
using Common.Services.Park;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class CheckSellerPurviewAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                ParkSeller user = null;
                if (filterContext.HttpContext.Session["SmartSystem_SellerLoginUser"] == null)
                {
                    var userAccountCookie = filterContext.HttpContext.Request.Cookies["SmartSystem_Seller_Login_UserAccount"];
                    var passwordCookie = filterContext.HttpContext.Request.Cookies["SmartSystem_Seller_Login_Password"];
                    if (userAccountCookie != null && passwordCookie != null)
                    {
                        string account = userAccountCookie.Value;
                        string pwd = DES.DESDeCode(passwordCookie.Value, "Password");
                        ParkSeller sysUser = ParkSellerDerateServices.WXGetSellerInfo(account, pwd);
                        if (sysUser != null)
                        {
                            user = sysUser;
                            filterContext.HttpContext.Session["SmartSystem_SellerLoginUser"] = user;
                        }
                    }

                    if (user == null)
                    {
                        string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=登录超时，请重新登录&returnUrl=/XFJMLogin/Index';</script>";
                        filterContext.HttpContext.Response.Write(response_js);
                        filterContext.HttpContext.Response.End();
                        return;
                    }

                }
                HttpContext.Current.Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                user = (ParkSeller)filterContext.HttpContext.Session["SmartSystem_SellerLoginUser"];
                if (user == null)
                {
                    string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=登录超时，请重新登录&returnUrl=/XFJMLogin/Index';</script>";
                    filterContext.HttpContext.Response.Write(response_js);
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "验证商户是否登录失败");
                string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=验证商户是否登录失败&returnUrl=/XFJMLogin/Index';</script>";
                filterContext.HttpContext.Response.Write(response_js);
                return;
            }
        }
    }
}