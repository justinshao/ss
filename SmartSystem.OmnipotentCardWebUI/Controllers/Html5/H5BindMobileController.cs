﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Common.Entities;
using Common.Services.WeiXin;
using Common.Entities.Enum;
using Common.ExternalInteractions.Sms.JuHe;
using Common.Services;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    public class H5BindMobileController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            returnUrl = returnUrl.Replace("_", "?");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult GetMobileCode(string mobile)
        {
            try
            {
                var companyCookie = Request.Cookies["SmartSystem_H5_CompanyID"];
                if (companyCookie == null || string.IsNullOrWhiteSpace(companyCookie.Value))
                {
                    throw new MyException("获取单位信息失败，请重新进入页面");
                }
                if (string.IsNullOrWhiteSpace(mobile) || !Regex.Match(mobile, @"^1[0-9]{10}$").Success)
                {
                    throw new MyException("手机号码格式不正确");
                }
                var code_cookie = Request.Cookies["SmartSystem_BindTradePassword_Code"];
                var code_time_cookie = Request.Cookies["SmartSystem_BindTradePassword_Code_GetTime"];
                var code_moblie_cookie = Request.Cookies["SmartSystem_BindTradePassword_Mobile"];
                if (code_cookie != null && code_time_cookie != null && code_moblie_cookie != null && code_moblie_cookie.Value == mobile && DateTime.Parse(code_time_cookie.Value).AddMinutes(1) > DateTime.Now)
                {
                    return Json(MyResult.Error("两次获取验证码的时间不能小于60秒"));
                }
                string code = new Random().Next(100000, 999999).ToString();

                string appkey = WXOtherConfigServices.GetConfigValue(companyCookie.Value, ConfigType.JuHeAppKey);
                string modeId = WXOtherConfigServices.GetConfigValue(companyCookie.Value, ConfigType.JuHeSmsTemplateId);
                JuHeSmsResult result = JuHeCodeSmsService.SendCodeSms(appkey, modeId, code, mobile);
                if (result.SendResult)
                {
                    //验证码30分钟内有效
                    var cookie_code = new HttpCookie("SmartSystem_BindTradePassword_Code", code);
                    cookie_code.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Add(cookie_code);

                    Response.Cookies.Add(new HttpCookie("SmartSystem_BindTradePassword_Mobile", mobile));
                    Response.Cookies.Add(new HttpCookie("SmartSystem_BindTradePassword_Code_GetTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    return Json(MyResult.Success("获取成功"));
                }
                return Json(MyResult.Error("获取失败"));
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5BindMobileError", ex.Message, ex, LogFrom.WeiXin);
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5BindMobileError", "获取验证码失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取验证码失败"));
            }
        }
        [HttpPost]
        public ActionResult GetNextSurplusTime(int seconds)
        {
            if (seconds != -1)
            {
                int surSeconds = --seconds;
                if (surSeconds < 1)
                {
                    return Json(MyResult.Success("能再次获取"));

                }
                return Json(MyResult.Error("不能获取", surSeconds));
            }
            var code_time_cookie = Request.Cookies["SmartSystem_BindTradePassword_Code_GetTime"];
            DateTime nowTime = DateTime.Now;
            if (code_time_cookie == null || DateTime.Parse(code_time_cookie.Value).AddMinutes(1) <= nowTime)
            {
                return Json(MyResult.Success("能再次获取"));

            }


            TimeSpan countdownSpan = DateTime.Parse(code_time_cookie.Value).AddMinutes(1) - nowTime;
            int surplusSeconds = (int)Math.Round(countdownSpan.TotalSeconds, 0);

            if (surplusSeconds > 0)
            {
                return Json(MyResult.Error("不能获取", surplusSeconds));

            }
            return Json(MyResult.Success("能再次获取"));
        }
        [HttpPost]
        public ActionResult SaveBindMobile(string phone, string code)
        {
            try
            {
                var companyCookie = Request.Cookies["SmartSystem_H5_CompanyID"];
                if (companyCookie == null || string.IsNullOrWhiteSpace(companyCookie.Value))
                {
                    throw new MyException("获取单位信息失败，请重新进入页面");
                }
                if (string.IsNullOrWhiteSpace(phone) || !new Regex("^1[0-9]{10}$").Match(phone).Success)
                {
                    throw new MyException("手机号码格式错误");
                }
                CheckBindTradePasswordCode(code, phone);


                WX_Info user = new WX_Info();
                user.OpenID = string.Empty;
                user.UserType = 0;
                user.FollowState = (int)WxUserState.UnAttention;
                user.CompanyID = companyCookie.Value;
                user.City = string.Empty;
                user.Country = string.Empty;
                user.Headimgurl = string.Empty;
                user.Language = string.Empty;
                user.NickName = string.Empty;
                user.Province = string.Empty;
                user.Sex = "M";
                user.MobilePhone = phone;
                user.LastSubscribeDate = DateTime.Now;
                WX_Info result = WXAccountServices.AddOrGetWXInfo(user);
                if (result==null) throw new MyException("绑定失败");

                RemoveTradePasswordCooike();
                Session["SmartSystem_H5_WX_Info"] = result;
                Response.Cookies.Add(new HttpCookie("SmartSystem_H5_MobilePhone", phone));
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5BindMobileError", "绑定手机号失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("绑定失败"));
            }
        }
        private void CheckBindTradePasswordCode(string code, string phone)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new MyException("请输入验证码");

            var cookie_code = string.Empty;
            var cookie = Request.Cookies["SmartSystem_BindTradePassword_Code"];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie_code = cookie.Value;
            }
            if (cookie_code != code)
            {
                throw new MyException("验证码输入不正确");
            }

            var cookie_mobile = string.Empty;
            var code_mobile_cookie = Request.Cookies["SmartSystem_BindTradePassword_Mobile"];
            if (code_mobile_cookie != null && !string.IsNullOrWhiteSpace(code_mobile_cookie.Value))
            {
                cookie_mobile = code_mobile_cookie.Value;
            }
            if (cookie_mobile != phone)
            {
                throw new MyException("手机号码与验证的手机号不一致");
            }
        }
        private void RemoveTradePasswordCooike()
        {
            var cookie = Request.Cookies["SmartSystem_BindTradePassword_Code"];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(cookie);

            }
            Request.Cookies.Remove("SmartSystem_BindTradePassword_Code");

            var code_mobile_cookie = Request.Cookies["SmartSystem_BindTradePassword_Mobile"];
            if (code_mobile_cookie != null && !string.IsNullOrWhiteSpace(code_mobile_cookie.Value))
            {
                code_mobile_cookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(code_mobile_cookie);

            }
            Request.Cookies.Remove("SmartSystem_BindTradePassword_Mobile");

            var code_time_cookie = Request.Cookies["SmartSystem_BindTradePassword_Code_GetTime"];
            if (code_time_cookie != null && !string.IsNullOrWhiteSpace(code_time_cookie.Value))
            {
                code_time_cookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(code_time_cookie);

            }
            Request.Cookies.Remove("SmartSystem_BindTradePassword_Code_GetTime");


        }
    }
}
