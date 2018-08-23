using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Entities.WX;
using Common.Services;
using SmartSystem.WXInerface;
using Common.Entities.Enum;
using Common.Services.WeiXin;
using Common.Entities;
using SmartSystem.WeiXinInerface;
using ClassLibrary1;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class CheckWeiXinPurviewAttribute : AuthorizeAttribute
    {
        //public string AppUserToken
        //{
        //    get
        //    {
        //        var cookie = HttpContext.Current.Request.Cookies["SmartSystem_APP_UserToken"];
        //        if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
        //        {
        //            return cookie.Value;
        //        }
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        var cookie = HttpContext.Current.Request.Cookies["SmartSystem_APP_UserToken"];
        //        if (cookie == null)
        //        {
        //            cookie = new HttpCookie("SmartSystem_APP_UserToken");
        //            cookie.Expires = DateTime.Now.AddYears(1);
        //            cookie.Value = value;
        //            HttpContext.Current.Request.Cookies.Add(cookie);
        //        }
        //        else
        //        {
        //            cookie.Value = value;
        //            HttpContext.Current.Request.Cookies.Set(cookie);
        //        }

        //    }
        //}



        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var openId = string.Empty;
                WX_Info user = null;
                var cookie = filterContext.RequestContext.HttpContext.Request.Cookies["SmartSystem_WeiXinOpenId"];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    openId = cookie.Value;
                }
                var permission = Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (permission.Contains("Login"))
                {
                    if (string.IsNullOrWhiteSpace(openId))
                    {
#if DEBUG
                        //odvkywUwtjaKnj7yGN-df7XV6ru4,odvkywSnlKr8anm3ddoIcredwvN0，oaC2Qt5oZsvPH_hlz0MoEw0sK2yg
                        openId = "ohqkK00bNhbFKWniuJyMsSXivoXc";//"o-Xw8wzQE2QmB-x5zehYlVdxcs5M";
#endif
                    }

                    if (string.IsNullOrWhiteSpace(openId))
                    {
                        //如果获取不到cookie中的微信uid则跳转至appRedir
                        TxtLogServices.WriteTxtLogEx("CheckWeiXinPurview", "Request.Url.PathAndQuery:{0}", filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery);
                        var queryString = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery.TrimStart('/').Replace('/', '_').Replace('?', '_').Replace('&', '^');
                        TxtLogServices.WriteTxtLogEx("CheckWeiXinPurview", "请求获取微信信息 queryString:{0}", queryString);
                        filterContext.HttpContext.Response.Redirect(string.Format("~/L/Index?id={0}", queryString));
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new EmptyResult();
                        return;
                    }
                }
                if (filterContext.HttpContext.Session["SmartSystem_WX_Info"] != null)
                {
                    user = (WX_Info)filterContext.HttpContext.Session["SmartSystem_WX_Info"];
                }
                if (user == null)
                {
                      user = WXotherServices.GetWXInfo(openId);
                     //user = WeiXinAccountService.QueryWXByOpenId(openId);
                }
                if (user == null || (WxUserState)user.FollowState == WxUserState.UnAttention)
                {
                    string companyId = user == null ? string.Empty : user.CompanyID;
                    string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.PromptAttentionPage);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        value = "~/ErrorPrompt/Index?message=请关注SPS停车服务微信公众号";
                    }
                    //返回错误页面 （请求关注页面）
                    filterContext.HttpContext.Response.Redirect(value);
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                    return;
                }
                HttpContext.Current.Session["SmartSystem_LogFrom"] = LogFrom.WeiXin;
                HttpContext.Current.Session["SmartSystem_OperatorUserAccount"] = user.OpenID;
                filterContext.HttpContext.Session["SmartSystem_WX_Info"] = user;
                if (user == null)
                {
                    TxtLogServices.WriteTxtLogEx("CheckWeiXinPurview", "微信用户不存在，OPENID:{0}", openId);
                }
                //RegisterAccount
                if (permission.Contains("REGISTERACCOUNT"))
                {
                    WX_Account account = WeiXinAccountService.GetAccountByID(user.AccountID);
                    if (account == null || string.IsNullOrWhiteSpace(account.MobilePhone))
                    {
                        var queryString = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                        if (filterContext.RequestContext.HttpContext.Request["returnUrl"] != null)
                        {
                            queryString = filterContext.RequestContext.HttpContext.Request["returnUrl"];
                        }
                        filterContext.HttpContext.Response.Redirect(string.Format("~/BindMobile/Index?returnUrl={0}", queryString));
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new EmptyResult();
                        return;
                    }
                }

                //if (permission.Contains("APP"))
                //{
                //    //TradePassword
                //    string sToken = AppUserToken;
                //    if (string.IsNullOrEmpty(sToken))
                //    {
                //        TxtLogServices.WriteTxtLogEx("ParkingPayment", "TOKEN = {0} ", "null or ''");
                //    }
                //    else
                //    {
                //        TxtLogServices.WriteTxtLogEx("ParkingPayment", "TOKEN，id:{0},Status:{1} ", openId, sToken);

                //    }

                //    do
                //    {
                //        //APP
                //        if (sToken.IsEmpty())
                //        {
                //            VerifyCode verify = wxApi.getThirdLogin(openId, openId); //第三方登录
                //            TxtLogServices.WriteTxtLogEx("ParkingPayment", "用户自动登录，id:{0},Status:{1} ", openId, verify.Status);
                //            if (verify.Status == 1)
                //            {
                //                sToken = verify.Result;
                //                AppUserToken = sToken;
                //                return;
                //            }
                //            else if (verify.Status == 2)
                //            {
                //                //未绑定
                //                AppUserToken = "";
                //                sToken = "";
                //                filterContext.HttpContext.Response.Redirect("~/ParkingPayment/LicensePlatePayment");
                //                filterContext.HttpContext.Response.End();
                //                filterContext.Result = new EmptyResult();
                //                return;
                //            }
                //            else
                //            {
                //                //其他都是失败
                //                AppUserToken = "";
                //                sToken = "";
                //                filterContext.HttpContext.Response.Redirect("~/ErrorPrompt/Index?message=用户自动登录失败");
                //                filterContext.HttpContext.Response.End();
                //                filterContext.Result = new EmptyResult();
                //                return;
                //            }
                //        }

                //        CarManage carMessage = wxApi.getCarManage(sToken);
                //        if (carMessage == null || carMessage.Status == 40001)
                //        {
                //            //
                //            AppUserToken = "";
                //            sToken = "";
                //            continue;
                //        }
                //    } while (sToken.IsEmpty());
                //}
            }

            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "验证微信登陆信息失败", LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("CheckWeiXinPurview", ex);
                return;
            }
        }
    }
}