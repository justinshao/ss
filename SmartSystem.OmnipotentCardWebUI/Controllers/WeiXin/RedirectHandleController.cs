using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Common;
using System.Web.Routing;
using Common.Entities.WX;
using SmartSystem.WeiXinBase;
using Common.Services;
using SmartSystem.WeiXinInerface;
using Common.Entities.Enum;
using Common.Entities;
using Common.Services.WeiXin;
using ClassLibrary1;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 跳转页面处理
    /// </summary>
    public class RedirectHandleController : WeiXinController
    {
        public ActionResult Index(string id, string code, string state)
        {
            try
            {
                ClearSystemCache();
                WX_ApiConfig config = GetApiConfig(id);
                if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)
                    || string.IsNullOrWhiteSpace(config.Domain) || string.IsNullOrWhiteSpace(config.SystemName))
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信基础信息失败" });
                }
                if (!config.Status)
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "该公众账号暂停使用，请稍后再试！" });
                }
                Session["CurrLoginWeiXinApiConfig"] = config;
                if (string.IsNullOrEmpty(id))
                {
                    id = "ParkingPayment_Index";
                }
                if (Request.UserAgent != null && !Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "请在微信中打开" });
                }
                if (string.IsNullOrEmpty(state))
                {
                    var redirectUri = config.Domain;
                    if (string.IsNullOrWhiteSpace(redirectUri))
                    {
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取域名配置信息失败" });
                    }
                    if (redirectUri != null)
                    {
                        redirectUri = redirectUri.IndexOf("http://", StringComparison.Ordinal) < 0 ? string.Format("http://{0}", redirectUri) : redirectUri;
                        redirectUri = string.Format("{0}/r/{1}", redirectUri, id);
                    }
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "获取微信OpenId请求redirectUri：{0}", redirectUri);
                    string url = WxAdvApi.GetAuthorizeUrl(config.AppId, redirectUri, "1", OAuthScope.snsapi_base);
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "获取微信OpenId请求url：{0}", url);
                    return Redirect(url);
                }
                TxtLogServices.WriteTxtLogEx("RedirectHandle", "state不为空进入，id：{0}, code：{1}, state：{2}", id, code, state);
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "微信获取授权失败，请重新进入或请联系管理员" });
                }

                var accessToken = WxAdvApi.GetAccessToken(config.AppId, config.AppSecret, code);
                TxtLogServices.WriteTxtLogEx("RedirectHandle", "调用微信的AccessToken接口：openid:{0}, access_token：{1}", accessToken.openid, accessToken.access_token);
                var user = WeiXinAccountService.QueryWXByOpenId(accessToken.openid);
                string s = user == null ? "user is null" : "user is not null";
                TxtLogServices.WriteTxtLogEx("RedirectHandle", s);
                if (user != null)
                {
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "关注状态:{0}", ((WxUserState)user.FollowState).GetDescription());
                }
                if (user == null || (WxUserState)user.FollowState == WxUserState.UnAttention)
                {
                    return RedirectAttentionPage(config.CompanyID, "请先关注公众账号");
                }
                //添加登陆
                Response.Cookies.Add(new HttpCookie("SmartSystem_WeiXinOpenId", accessToken.openid));
                TxtLogServices.WriteTxtLogEx("RedirectHandle", "获取OpenId成功：openid:{0}", accessToken.openid);
                //登录APP
                string openId = accessToken.openid;
                string sToken = "";
                VerifyCode verify = wxApi.getThirdLogin(openId, openId); //第三方登录
                TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户自动登录，id:{0},Status:{1} ", openId, verify.Status);
                if (verify.Status == 1)
                {
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户自动登录成功，id:{0},Status:{1} ", openId, verify.Result);

                    sToken = verify.Result;
                    AppUserToken = sToken; //渠道TOKEN了
                    //    //获取用户的信息
                    //    ClassLibrary1.PurseData.UserInfo appUser = wxApi.getUserInfo(sToken);
                    //    if (appUser != null && appUser.Status == 1)
                    //    {

                    //        TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户，获取用户信息成功，id:{0},Status:{1} ", openId, appUser.Result.Phone);
                    //        AppUserPhone = appUser.Result.Phone;
                    //    }
                    //    else
                    //    {
                    //        TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户，获取用户信息失败，id:{0},Status:{1} ", openId, appUser.Status);
                    //    }
                }
                else if (verify.Status == 2)
                {
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户自动登录失败，未绑定，id:{0},Status:{1} ", openId, verify.Result);
                    //未绑定
                    AppUserToken = "-1";
                }
                else
                {
                    //其他都是失败
                    AppUserToken = "";
                    sToken = "";
                    TxtLogServices.WriteTxtLogEx("RedirectHandle", "APP用户自动登录失败，id:{0},Status:{1} ", openId, verify.Result);
                }

                return Redir(id, accessToken.openid);
            }
            catch (MyException ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "id:" + id, ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message =ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "RedirectHandle方法处理异常", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "跳转链接失败" });
            }
        }
        private void ClearSystemCache()
        {
            var aliPayUserIdCookie = Request.Cookies["SmartSystem_AliPay_UserId"];
            if (aliPayUserIdCookie != null)
            {
                aliPayUserIdCookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(aliPayUserIdCookie);
            }
            if (HttpContext.Session["SmartSystem_WX_Info"] != null)
            {
                HttpContext.Session["SmartSystem_WX_Info"] = null;
            }
            var weiXinOpenCookie = Request.Cookies["SmartSystem_WeiXinOpenId"];
            if (weiXinOpenCookie != null)
            {
                weiXinOpenCookie.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(weiXinOpenCookie);
            }
            AppUserToken = null;
            Session["CurrLoginWeiXinApiConfig"] = null;
            Session["CurrLoginAliPayApiConfig"] = null;
        }
        public ActionResult Redir(string id, string openId)
        {
            try
            {
                var actionName = "Index";
                var controllerName = "ParkingPayment"; //默认
                var separator = new[] { '|', '_' };
                var param = new[] { '^' };//^参数分隔符
                var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                TxtLogServices.WriteTxtLogEx("RedirectHandle", "Redir方法：id：{0}，openId：{1}", id, openId);
                switch (ids.Length)
                {
                    case 0:
                        return RedirectToAction(actionName, controllerName);
                    case 1:
                        return RedirectToAction(actionName, ids[0]);
                    case 2:
                        return RedirectToAction(ids[1], ids[0]);
                }
                var values = new RouteValueDictionary();
                controllerName = ids[0];
                actionName = ids[1];
                var parameters = ids[2].Split(param, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in parameters)
                {
                    var parame = item.Split(new[] { '=' });
                    if (parame.Length < 2)
                    {
                        TxtLogServices.WriteTxtLogEx("RedirectHandle", "Redir方法参数设置错误：param:{0}", ids[2]);

                        return RedirectToAction("Index", "ErrorPrompt", new { message = "参数出现错误，请联系管理员" });
                    }
                    values.Add(parame[0], parame[1]);
                }
                TxtLogServices.WriteTxtLogEx("RedirectHandle","RedirectHandle 跳转到的页面信息,controllerName：{0}, actionName：{1}，parameters数量：{2}", controllerName, actionName, parameters.Length);
                return RedirectToAction(actionName, controllerName, values);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "RedirectHandle方法参数处理异常", ex, LogFrom.WeiXin);
                throw;
            }
        }
        private WX_ApiConfig GetApiConfig(string id)
        {
            TxtLogServices.WriteTxtLogEx("RedirectHandle", "获取微信配置信息，id:{0}", id);
            var separator = new[] { '|', '_' };
            var param = new[] { '^' };//^参数分隔符
            var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            var parameters = ids[2].Split(param, StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length == 0) throw new MyException("获取单位失败");
            foreach (var item in parameters)
            {
                var parame = item.Split(new[] { '=' });
                if (parame.Length == 2)
                {
                    if (parame[0].ToUpper() == "CID")
                    {
                        WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(parame[1]);
                        if (config == null) throw new MyException("获取微信配置失败");
                        if (config.CompanyID != parame[1]) throw new MyException("该公众号暂停使用");
                        return config;
                    }
                    if (parame[0] == "PARKINGID" || parame[0] == "PID")
                    {
                        BaseCompany company = CompanyServices.QueryByParkingId(parame[1]);
                        if (company == null) throw new MyException("获取单位信息失败");

                        WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                        if (config == null) throw new MyException("获取微信配置失败");
                        if (config.CompanyID != parame[1]) throw new MyException("该公众号暂停使用");
                        return config;
                    }
                }
            }
            throw new MyException("获取微信配置失败，id:" + id);
        }
    }
}
