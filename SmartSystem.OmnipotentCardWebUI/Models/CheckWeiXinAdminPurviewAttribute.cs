using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Core;
using Common.Services;
using System.Web.Routing;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class CheckWeiXinAdminPurviewAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                SysUser user = null;
                if (filterContext.HttpContext.Session["SmartSystem_SystemLoginUser"] == null)
                {
                    var userAccountCookie = filterContext.HttpContext.Request.Cookies["SmartSystem_Current_Login_UserAccount"];
                    var passwordCookie = filterContext.HttpContext.Request.Cookies["SmartSystem_Current_Login_Password"];
                    if (userAccountCookie != null && passwordCookie != null)
                    {
                        string account = userAccountCookie.Value;
                        string pwd = DES.DESDeCode(passwordCookie.Value, "Password");
                        SysUser sysUser = SysUserServices.QuerySysUserByUserAccount(account);
                        if (sysUser != null && sysUser.Password.Equals(MD5.Encrypt(pwd)))
                        {
                            user = sysUser;
                            filterContext.HttpContext.Session["SmartSystem_SystemLoginUser"] = user;
                        }
                    }

                    if (user == null)
                    {
                        string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=登录超时，请重新登录&returnUrl=/AdminLogin/Index';</script>";
                        filterContext.HttpContext.Response.Write(response_js);
                        return;
                    }

                }
                user = (SysUser)filterContext.HttpContext.Session["SmartSystem_SystemLoginUser"];
                if (user != null && (filterContext.HttpContext.Session["SmartSystem_LoginUser_ValidVillage"] == null
               || filterContext.HttpContext.Session["SmartSystem_LoginUser_ValidCompany"] == null
               || filterContext.HttpContext.Session["SmartSystem_SystemLoginUser_Role"] == null
               || filterContext.HttpContext.Session["SmartSystem_LoginUser_SysRoleAuthorize"] == null))
                {
                    CacheData.CacheUserLoginData(user);
                }
             
                HttpContext.Current.Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                if (user == null)
                {
                    string response_js = "<script>window.parent.location.href='/AdminLogin/Index';</script>";
                    filterContext.HttpContext.Response.Write(response_js);
                    return;
                }
              
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "验证微信管理员是否登录失败");
                string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=验证微信管理员是否登录失败&returnUrl=/AdminLogin/Index';</script>";
                filterContext.HttpContext.Response.Write(response_js);
                return;
            }
        }
    }
}