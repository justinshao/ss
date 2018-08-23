using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;
using SmartSystem.AliPayServices;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using Common.Entities.Enum;
using System.Web.Routing;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Entities;
using SmartSystem.WeiXinServices;
using Common.Entities.Order;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.AliPay
{
    public class AliPayAuthorizeController : WeiXinController
    {
        public ActionResult Index(string id,string state)
        {
            try
            {
                if (SourceClient != RequestSourceClient.AliPay){
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "请用支付宝页面打开" });
                }
                ClearQRCodeCookie();
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", string.Format("state：{0}", state));
                if (!string.IsNullOrWhiteSpace(state) && string.IsNullOrWhiteSpace(id)) {
                    id = state;
                }
                Dictionary<string, string> dicParams = GetRequestParams(id);
                if (!dicParams.ContainsKey("COMPANYID")){
                    throw new MyException("获取单位信息失败");
                }
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(dicParams["COMPANYID"]);
                if (config == null){
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝配置失败" });
                }
                if (!config.Status){
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "已暂停支付宝支付，稍后再试！" });
                }
                Session["CurrLoginAliPayApiConfig"] = config;
                if (string.IsNullOrWhiteSpace(Request["auth_code"]))
                {
                    TxtLogServices.WriteTxtLogEx("AliPayAuthorize", string.Format("autState：{0}", id));

                    string publicAppAuthorizeUrl = AliPayApiServices.GetPublicAppAuthorizeUrl(id, config);
                    TxtLogServices.WriteTxtLogEx("AliPayAuthorize", string.Format("PublicAppAuthorizeUrl：{0}", publicAppAuthorizeUrl));
                    return Redirect(publicAppAuthorizeUrl);
                }
                string auth_code = Request["auth_code"];
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", string.Format("auth_code：{0}", auth_code));
                string userId = "";
                string aliAccessToken = AliPayApiServices.GetAccessToken(dicParams["COMPANYID"], auth_code, ref userId);
                if (string.IsNullOrWhiteSpace(aliAccessToken) || string.IsNullOrWhiteSpace(aliAccessToken) || string.IsNullOrWhiteSpace(userId))
                {
                    throw new MyException("获取支付宝用户授权信息失败");
                }
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", string.Format("userId：{0}", userId));
                Response.Cookies.Add(new HttpCookie("SmartSystem_AliPay_UserId", userId));
                return Redir(id, userId);
            }
            catch (MyException ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", ex);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message });
            }
            catch (Exception ex) {
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize",ex);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝授权失败" });
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
        private Dictionary<string, string> GetRequestParams(string id)
        {
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
        public ActionResult Redir(string id, string userId)
        {
            try
            {
                var actionName = "Index";
                var controllerName = "ParkingPayment"; //默认
                var separator = new[] { '|', '_' };
                var param = new[] { '^' };//^参数分隔符
                var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", "Redir方法：id：{0}，userId：{1}", id, userId);
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
                        TxtLogServices.WriteTxtLogEx("AliPayAuthorize", "Redir方法参数设置错误：param:{0}", ids[2]);

                        return RedirectToAction("Index", "ErrorPrompt", new { message = "参数出现错误，请联系管理员" });
                    }
                    values.Add(parame[0], parame[1]);
                }
                TxtLogServices.WriteTxtLogEx("AliPayAuthorize", "Redir 跳转到的页面信息,controllerName：{0}, actionName：{1}，parameters数量：{2}", controllerName, actionName, parameters.Length);
                return RedirectToAction(actionName, controllerName, values);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AliPayAuthorize", "Redir方法参数处理异常", ex, LogFrom.WeiXin);
                throw;
            }
        }
    }
}
