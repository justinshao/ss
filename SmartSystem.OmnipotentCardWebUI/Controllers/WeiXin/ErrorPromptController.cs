using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinBase;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities.Enum;
using System.Configuration;
using Common.Services;


namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 错误提醒
    /// </summary>
    public class ErrorPromptController : WeiXinController
    {
        public ActionResult Index(string message, string returnUrl,bool ShowCustomerServicePhone=false)
        {
            try
            {
                ViewBag.ErrorMessage = string.IsNullOrWhiteSpace(message) ? "访问异常" : message;
                ViewBag.BackUrl = returnUrl;
                ViewBag.AppId = string.Empty;
                ViewBag.Timestamp = string.Empty;
                ViewBag.NonceStr = string.Empty;
                ViewBag.Signature = string.Empty;
                ViewBag.CustomerServicePhone = ConfigurationManager.AppSettings["CustomerServicePhone"] ?? "0";
                return View();
            }
            catch (Exception ex) {
                TxtLogServices.WriteTxtLogEx("ErrorPrompt", "message:" + ex.Message + ",StackTrace:" + ex.StackTrace);
                return RedirectToAction("Index", "Error", message="");
            }
        }
        public ActionResult Error(string message) {
            ViewBag.Message = string.IsNullOrWhiteSpace(message) ? " 系统异常，请重试！" : message;
            return View();
        }
        public ActionResult NotPage() {
            return View();
        }
    }
}
