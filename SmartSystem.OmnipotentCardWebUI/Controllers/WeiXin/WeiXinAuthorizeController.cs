using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.WeiXin;
using Common.Services;
using SmartSystem.WeiXinBase;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface;
using Common.Entities.Enum;
using System.Web.Routing;
using Common.Entities;
using Common.Entities.Order;
using SmartSystem.WeiXinServices;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class WeiXinAuthorizeController : WeiXinController
    {
        /// <summary>
        /// 微信单独授权 type  0-支付  1-岗亭扫码进入
        /// </summary>
        /// <param name="id">ControllerName_actionName_ispayauthorize=0^orderId=123</param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Index(string id,string code, string state)
        {
            try
            {
                if (SourceClient != RequestSourceClient.WeiXin)
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "请在微信中打开" });
                }
                ClearQRCodeCookie();
                WX_ApiConfig config = null;
                Dictionary<string, string> dicParams = GetRequestParams(id);
                if (string.IsNullOrWhiteSpace(dicParams["COMPANYID"]))
                {
                    throw new MyException("获取单位信息失败");
                }
                config = WXApiConfigServices.QueryWXApiConfig(dicParams["COMPANYID"]);
                if (config == null) {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置失败" });
                }
                if (!config.Status) {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "该公众号暂停使用，稍后再试！" });
                }
                Session["CurrLoginWeiXinApiConfig"] = config;
                if (string.IsNullOrEmpty(state))
                {

                    string redirectUri = config.Domain.IndexOf("http://", StringComparison.Ordinal) < 0 ? string.Format("http://{0}", config.Domain) : config.Domain;
                    redirectUri = string.Format("{0}/WeiXinAuthorize/{1}", redirectUri, id);

                    TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "获取微信OpenId请求redirectUri：{0}", redirectUri);
                    string url = WxAdvApi.GetAuthorizeUrl(config.AppId, redirectUri, "1", OAuthScope.snsapi_base);
                    TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "获取微信OpenId请求url：{0}", url);
                    return Redirect(url);
                }
                TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "state不为空进入，id：{0}, code：{1}, state：{2}", id, code, state);
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "微信获取授权失败，请重新进入或请联系管理员" });
                }

                var accessToken = WxAdvApi.GetAccessToken(config.AppId, config.AppSecret, code);
                TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "调用微信的AccessToken接口：openid:{0}, access_token：{1}", accessToken.openid, accessToken.access_token);
                if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.openid))
                {
                    throw new MyException("获取微信用户信息失败");
                }
                
                //添加登陆
                Response.Cookies.Add(new HttpCookie("SmartSystem_WeiXinOpenId", accessToken.openid));
                TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "获取OpenId成功：openid:{0},cookie openid:{1}", accessToken.openid,WeiXinOpenId);
                return Redir(id, accessToken.openid);
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinAuthorize", "WeiXinAuthorize方法处理异常", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "微信获取授权失败，请重新进入或请联系管理员" });
            }
        }
        private void ClearQRCodeCookie()
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
            Session["CurrLoginWeiXinApiConfig"] = null;
            Session["CurrLoginAliPayApiConfig"] = null;
        }
        private Dictionary<string, string> GetRequestParams(string id) {
            Dictionary<string, string> dicParams = new Dictionary<string, string>();
            var separator = new[] { '|', '_' };
            var param = new[] { '^' };//^参数分隔符
            var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var parameters = ids[2].Split(param, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in parameters)
            {
                var parame = item.Split(new[] { '=' });
                if (parame.Length < 2)
                {
                    continue;
                }
                if (!dicParams.ContainsKey(parame[0].ToUpper()))
                {
                    dicParams.Add(parame[0].ToUpper(), parame[1]);
                }
               
            }
            return dicParams;
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
                TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "Redir方法：id：{0}，openId：{1}", id, openId);
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
                        TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "Redir方法参数设置错误：param:{0}", ids[2]);

                        return RedirectToAction("Index", "ErrorPrompt", new { message = "参数出现错误，请联系管理员" });
                    }
                    values.Add(parame[0], parame[1]);
                }
                TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "Redir 跳转到的页面信息,controllerName：{0}, actionName：{1}，parameters数量：{2}", controllerName, actionName, parameters.Length);
                return RedirectToAction(actionName, controllerName, values);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinAuthorize", "Redir方法参数处理异常", ex, LogFrom.WeiXin);
                throw;
            }
        }
        //private WX_ApiConfig GetApiConfig(string id)
        //{
        //    TxtLogServices.WriteTxtLogEx("WeiXinAuthorize", "获取微信配置信息，id:{0}", id);
        //    var separator = new[] { '|', '_' };
        //    var param = new[] { '^' };//^参数分隔符
        //    var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        //    var parameters = ids[2].Split(param, StringSplitOptions.RemoveEmptyEntries);
        //    if (parameters.Length == 0) throw new MyException("获取单位失败");
        //    foreach (var item in parameters)
        //    {
        //        var parame = item.Split(new[] { '=' });
        //        if (parame.Length < 2)
        //        {
        //            if (parame[0].ToUpper() == "COMPANYID") {
        //                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(parame[1]);
        //                if (config == null) throw new MyException("获取微信配置失败，CompanyId:"+parame[1]);
        //                return config;
        //            }
        //            if (parame[0] == "PARKINGID") {
        //                BaseCompany company = CompanyServices.QueryByParkingId(parame[1]);
        //                if (company == null) throw new MyException("获取单位信息失败");

        //                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
        //                if (config == null) throw new MyException("获取微信配置失败，CompanyId:" + parame[1]);
        //                return config;
        //            }
        //        }
        //    }
        //    throw new MyException("获取微信配置失败，id:" + id);
        //}
    }
}
