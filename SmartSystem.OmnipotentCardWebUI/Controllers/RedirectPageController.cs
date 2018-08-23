using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class RedirectPageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginTimeOut() {
            return View();
        }
        public ActionResult NotPurview() {
            return View();
        }
        public ActionResult Error(string msg) {
            ViewBag.Error = string.IsNullOrWhiteSpace(msg) ? "访问出现异常，请联系系统管理员" : msg;
            return View();
        }
        public ActionResult SystemError() {
            return View();
        }
    }
}
