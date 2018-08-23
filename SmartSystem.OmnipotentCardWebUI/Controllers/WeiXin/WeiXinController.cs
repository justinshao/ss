using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using SmartSystem.WXInerface;
using Common.Services.WeiXin;
using Common.Entities.Enum;
using SmartSystem.WeiXinInerface;
using Common.Entities;
using Common.Core;
using Common.Services;
using Common.Entities.AliPay;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class WeiXinController : Controller
    {
        public RequestSourceClient SourceClient
        {
            get
            {
                string user_agent = Request.Headers["user-agent"];
                if (user_agent != null && user_agent.ToLower().Contains("alipayclient"))
                {
                    return RequestSourceClient.AliPay;
                }
                if (user_agent != null && Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    return RequestSourceClient.WeiXin;
                }
                return RequestSourceClient.Other;
            }
        }
        /// <summary>
        /// 前台微信用户user信息。
        /// </summary>
        public WX_Info WeiXinUser
        {
            get
            {
                if (HttpContext.Session["SmartSystem_WX_Info"] == null)
                {
                    var cookie = HttpContext.Request.Cookies["SmartSystem_WeiXinOpenId"];
                    if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                    {
                        WX_Info user = WeiXinAccountService.QueryWXByOpenId(cookie.Value);
                        HttpContext.Session["SmartSystem_WX_Info"] = user;
                        return user;
                    }
                }
                return (WX_Info)HttpContext.Session["SmartSystem_WX_Info"];
            }
        }

        public string WeiXinOpenId
        {
            get
            {
                var cookie = Request.Cookies["SmartSystem_WeiXinOpenId"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return cookie.Value;
                }
                if (WeiXinUser != null)
                {
                    return WeiXinUser.OpenID;
                }
                return string.Empty;
            }
        }
        public string AliPayUserId
        {
            get
            {
                var cookie = Request.Cookies["SmartSystem_AliPay_UserId"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return cookie.Value;
                }
                return string.Empty;
            }
        }
        public WX_ApiConfig CurrLoginWeiXinApiConfig
        {
            get
            {
                if (Session["CurrLoginWeiXinApiConfig"] != null)
                {
                    return Session["CurrLoginWeiXinApiConfig"] as WX_ApiConfig;
                }
                return null;
            }
        }
        public AliPayApiConfig CurrLoginAliPayApiConfig
        {
            get
            {
                if (Session["CurrLoginAliPayApiConfig"] != null)
                {
                    return Session["CurrLoginAliPayApiConfig"] as AliPayApiConfig;
                }
                return null;
            }
        }
        public SysUser AdminLoginUser
        {
            get
            {
                if (HttpContext.Session["SmartSystem_SystemLoginUser"] == null)
                {
                    var userAccountCookie = HttpContext.Request.Cookies["SmartSystem_Current_Login_UserAccount"];
                    var passwordCookie = HttpContext.Request.Cookies["SmartSystem_Current_Login_Password"];
                    if (userAccountCookie != null && passwordCookie != null)
                    {
                        string account = userAccountCookie.Value;
                        string pwd = DES.DESDeCode(passwordCookie.Value, "Password");
                        SysUser sysUser = SysUserServices.QuerySysUserByUserAccount(account);
                        if (sysUser != null && sysUser.Password.Equals(MD5.Encrypt(pwd)))
                        {
                            HttpContext.Session["SmartSystem_SystemLoginUser"] = sysUser;
                            return sysUser;
                        }
                    }
                }
                return (SysUser)HttpContext.Session["SmartSystem_SystemLoginUser"];
            }
        }
        /// <summary>
        /// 当前登录用户所属单位
        /// </summary>
        public BaseCompany GetLoginUserCompany
        {
            get
            {
                if (Session["SmartSystem_LoginUserCompany"] != null)
                {
                    return Session["SmartSystem_LoginUserCompany"] as BaseCompany;
                }
                BaseCompany company = CompanyServices.QueryCompanyByRecordId(AdminLoginUser.CPID);
                Session["SmartSystem_LoginUserCompany"] = company;
                return company;
            }
        }
        /// <summary>
        /// 当前登录用户拥有的小区
        /// </summary>
        public List<BaseVillage> GetLoginUserVillages
        {
            get
            {
                if (Session["SmartSystem_LoginUser_ValidVillage"] != null)
                {
                    return Session["SmartSystem_LoginUser_ValidVillage"] as List<BaseVillage>;
                }
                return new List<BaseVillage>();
            }
        }
        /// <summary>
        /// 当前登录用户拥有的单位
        /// </summary>
        public List<BaseCompany> GetLoginUserRoleCompany
        {
            get
            {
                if (Session["SmartSystem_LoginUser_ValidCompany"] != null)
                {
                    return Session["SmartSystem_LoginUser_ValidCompany"] as List<BaseCompany>;
                }
                return new List<BaseCompany>();
            }
        }
        /// <summary>
        /// 当前登录用户的模块权限
        /// </summary>
        public List<SysRoleAuthorize> GetAdminLoginUserRoleAuthorize
        {
            get
            {
                if (Session["SmartSystem_LoginUser_SysRoleAuthorize"] != null)
                {
                    return Session["SmartSystem_LoginUser_SysRoleAuthorize"] as List<SysRoleAuthorize>;
                }
                return new List<SysRoleAuthorize>();
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
        /// <summary>
        /// 重定向关注页面
        /// </summary>
        /// <param name="msg">提醒消息</param>
        /// <returns></returns>
        public ContentResult RedirectAttentionPage(string companyId, string msg)
        {
            string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.PromptAttentionPage);

            if (!string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(value))
            {
                string script = Alert(msg, value);
                return Content(script, "text/html");
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                string script = "<script>location.href='" + value + "'</script>";
                return Content(script, "text/html");
            }

            return Alert("请先关注微信公众账号", "Index", "ErrorPrompt");
        }

        public string AppUserToken
        {
            get
            {
                var token = Convert.ToString(Session["SmartSystem_APP_UserToken"]);
                if (token.IsEmpty())
                {
                    return "";
                }
                return token;
            }
            set
            {
                Session["SmartSystem_APP_UserToken"] = value;
            }
        }
    }
}
