using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Core;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Admin
{
    /// <summary>
    /// 管理员微信登录端
    /// </summary>
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class AdminLoginController : WeiXinController
    {
        public ActionResult Index()
        {
            UserLoginModel model = GetUserLoginModelCache();
            return View(model);
        }
        private UserLoginModel GetUserLoginModelCache()
        {
            UserLoginModel login = new UserLoginModel();
            var rememberPassWordCookie = Request.Cookies["SmartSystem_Current_Login_RememberPassWord"];
            if (rememberPassWordCookie == null || rememberPassWordCookie.Value == "0")
            {
                return login;
            }

            var nameCookie = Request.Cookies["SmartSystem_Current_Login_UserAccount"];
            if (nameCookie != null)
            {
                login.UserAccount = nameCookie.Value;
            }
            var passwordCookie = Request.Cookies["SmartSystem_Current_Login_Password"];
            if (passwordCookie != null)
            {
                login.RememberPassword = true;
                login.Password = DES.DESDeCode(passwordCookie.Value, "Password");
            }

            return login;
        }
        [HttpPost]
        public ActionResult Index(string userAccount, string password, bool rememberPassWord = false)
        {

            UserLoginModel model = new UserLoginModel();
            model.UserAccount = userAccount;
            model.Password = password;
            model.RememberPassword = rememberPassWord;

            try
            {
                SysUser user = SysUserServices.QuerySysUserByUserAccount(userAccount);
                if (user == null)
                {
                    model.ErrorMessage = "用户名或密码错误";
                    return View(model);
                }
                if (!user.Password.Equals(MD5.Encrypt(model.Password)))
                {
                    model.ErrorMessage = "用户名或密码错误";
                    SysUserServices.LoginError(user.RecordID);
                    return View(model);
                }

                if (user.DataStatus == DataStatus.Delete)
                {
                    model.ErrorMessage = "用户不存在";
                    return View(model);
                }
                if (user.DataStatus != DataStatus.Normal)
                {
                    model.ErrorMessage = "账号不是有效状态，请联系系统管理员";
                    SysUserServices.LoginError(user.RecordID);
                    return View(model);
                }
                LoginSuccess(model, user);
                CacheData.CacheUserLoginData(user);
                Session["SmartSystem_OperatorUserAccount"] = user.UserAccount;
                Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                return RedirectToAction("Index", "AdminHome");
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "登录异常";
                ExceptionsServices.AddExceptions(ex, "登录异常");
                return View(model);
            }
        }


        private void LoginSuccess(UserLoginModel model, SysUser user)
        {
            SetLoginCookie(model);
            SaveLoginLog(user);
            SysUserServices.LoginSuccess(user.RecordID);
        }
        private void SetLoginCookie(UserLoginModel model)
        {
            Response.Cookies.Add(new HttpCookie("SmartSystem_Current_Login_UserAccount") { Value = HttpUtility.UrlEncode(model.UserAccount), Expires = DateTime.Now.AddDays(7) });
            Response.Cookies.Add(new HttpCookie("SmartSystem_Current_Login_Password") { Value = DES.DESEnCode((model.Password), "Password"), Expires = DateTime.Now.AddDays(7) });
            if (!model.RememberPassword)
            {
                var rememberPassWordCookie = Request.Cookies["SmartSystem_Current_Login_RememberPassWord"];
                if (rememberPassWordCookie != null)
                {
                    rememberPassWordCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.AppendCookie(rememberPassWordCookie);
                }
                return;
            }
            Response.Cookies.Add(new HttpCookie("SmartSystem_Current_Login_RememberPassWord") { Value = model.RememberPassword ? "1" : "0", Expires = DateTime.Now.AddDays(7) });

        }
        private void SaveLoginLog(SysUser user)
        {
            try
            {
                LoginLog model = new LoginLog();
                model.UserAccount = user.UserAccount;
                model.LoginTime = DateTime.Now;
                model.LoginIP = Common.Utilities.ClientIpHelper.GetClientIp();
                model.Remark = string.Format("账号：{0}，成功登录智慧停车平台", user.UserAccount);
                model.LogFrom = LogFrom.OmnipotentCard;
                LoginLogServies.Add(model);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "添加登录日志失败");
            }
        }

        public ActionResult Logout()
        {
            Session["SmartSystem_SystemLoginUser"] = null;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public int CheckLoginStatus()
        {
            if (Session["SmartSystem_SystemLoginUser"] == null)
            {
                var userAccountCookie = Request.Cookies["SmartSystem_Current_Login_UserAccount"];
                var passwordCookie = Request.Cookies["SmartSystem_Current_Login_Password"];
                if (userAccountCookie == null || passwordCookie == null)
                {
                    return 0;
                }
                string account = userAccountCookie.Value;
                string pwd = DES.DESDeCode(passwordCookie.Value, "Password");
                SysUser sysUser = SysUserServices.QuerySysUserByUserAccount(account);
                if (sysUser == null || !sysUser.Password.Equals(MD5.Encrypt(pwd)))
                {
                    return 0;
                }
            }
            return 1;
        }
        public ActionResult NotPurview() {
            return View();
        }
    }
}
