using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using Common.Services.WeiXin;
using Common.Services;
using SmartSystem.WeiXinBase;
using Common.Entities;
using System.Web.Routing;
using Common.Entities.Enum;
using SmartSystem.AliPayServices;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 扫码入口
    /// </summary>
    public class QRCodeEntranceController : WeiXinController
    {
        public ActionResult Index(string id, string code, string state)
        {
             try
            {
                ClearSystemCache();

                var actionName = "Index";
                var controllerName = "ParkingPayment"; //默认
                var separator = new[] { '|', '_' };
                var param = new[] { '^' };//^参数分隔符
                var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);
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
                        TxtLogServices.WriteTxtLogEx("QRCodeEntrance", "Redir方法参数设置错误：param:{0}", ids[2]);

                        return RedirectToAction("Index", "ErrorPrompt", new { message = "参数出现错误，请联系管理员" });
                    }
                    values.Add(parame[0], parame[1]);
                }
                TxtLogServices.WriteTxtLogEx("QRCodeEntrance", "QRCodeEntrance 跳转到的页面信息,controllerName：{0}, actionName：{1}，parameters数量：{2}", controllerName, actionName, parameters.Length);
                return RedirectToAction(actionName, controllerName, values);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "QRCodeEntrance方法处理异常", ex, LogFrom.WeiXin);
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
            if (HttpContext.Session["SmartSystem_WX_Info"] != null) {
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
      
    }
}
