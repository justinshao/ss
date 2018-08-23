using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using Common.Core;
using Common.Services;
using SmartSystem.WeiXinInerface;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.XFJM
{
    [CheckWeiXinPurview(Roles = "Login")]
    public class XFJMLoginController : Controller
    {
        public ActionResult Index()
        {
            UserLoginModel model = GetUserLoginModelCache();
            return View(model);
        }
        private UserLoginModel GetUserLoginModelCache()
        {
            UserLoginModel login = new UserLoginModel();
            var rememberPassWordCookie = Request.Cookies["SmartSystem_XFJM_Login_RememberPassWord"];
            if (rememberPassWordCookie == null || rememberPassWordCookie.Value == "0")
            {
                return login;
            }

            var nameCookie = Request.Cookies["SmartSystem_XFJM_Login_UserAccount"];
            if (nameCookie != null)
            {
                login.UserAccount = nameCookie.Value;
            }
            var passwordCookie = Request.Cookies["SmartSystem_XFJM_Login_Password"];
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
                ParkSeller seller = ParkSellerDerateServices.WXGetSellerInfo(userAccount, MD5.Encrypt(password));
                if (seller == null)
                {
                    model.ErrorMessage = "用户名或密码错误";
                    return View(model);
                }
                if (seller.DataStatus == DataStatus.Delete)
                {
                    model.ErrorMessage = "商家不存在";
                    return View(model);
                }
                if (seller.DataStatus != DataStatus.Normal)
                {
                    model.ErrorMessage = "账号不是有效状态，请联系系统管理员";
                    return View(model);
                }
                Session["SmartSystem_SellerLoginUser"] = seller;
                SetLoginCookie(model);
                Session["SmartSystem_Seller_Login_UserAccount"] = seller.SellerNo;
                Session["SmartSystem_Seller_Login_Password"] = seller.PWD;
                
                Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                return RedirectToAction("Index", "XFJMMain");
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "登录异常";
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "商家登录异常", ex, LogFrom.WeiXin);
                return View(model);
            }
        }
        private void SetLoginCookie(UserLoginModel model)
        {
            Response.Cookies.Add(new HttpCookie("SmartSystem_XFJM_Login_UserAccount") { Value = HttpUtility.UrlEncode(model.UserAccount), Expires = DateTime.Now.AddDays(7) });
            Response.Cookies.Add(new HttpCookie("SmartSystem_XFJM_Login_Password") { Value = DES.DESEnCode((model.Password), "Password"), Expires = DateTime.Now.AddDays(7) });
            if (!model.RememberPassword)
            {
                var rememberPassWordCookie = Request.Cookies["SmartSystem_XFJM_Login_RememberPassWord"];
                if (rememberPassWordCookie != null)
                {
                    rememberPassWordCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.AppendCookie(rememberPassWordCookie);
                }
                return;
            }
            Response.Cookies.Add(new HttpCookie("SmartSystem_XFJM_Login_RememberPassWord") { Value = model.RememberPassword ? "1" : "0", Expires = DateTime.Now.AddDays(7) });

        }
    }
}
