using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using System.Web.Routing;
using Common.Services;
using Common.Core;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI
{
    public class CheckPurviewAttribute:AuthorizeAttribute
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
                        string response_js = "<script>window.parent.location.href='/RedirectPage/LoginTimeOut';</script>";
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
                HttpContext.Current.Session["SmartSystem_LogFrom"] = LogFrom.OmnipotentCard;
                if (user != null) {
                    filterContext.HttpContext.Session["SmartSystem_OperatorUserAccount"] = user.UserAccount;
                }
                if (filterContext.HttpContext.Session["SmartSystem_LoginUser_SysRoleAuthorize"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "RedirectPage", action = "NotPurview" }));
                    return;
                }
                List<SysRoleAuthorize> roleAuthorizes = (List<SysRoleAuthorize>)filterContext.HttpContext.Session["SmartSystem_LoginUser_SysRoleAuthorize"];
                if (!string.IsNullOrWhiteSpace(Roles))
                {

                    List<string> strRoles = Roles.Split(',').ToList();
                    if (!roleAuthorizes.Exists(p => strRoles.Contains(p.ModuleID)))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "RedirectPage", action = "NotPurview" }));
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex,"验证访问权限异常");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "RedirectPage", action = "Error" }));
                return;
            }
        }
    }
}