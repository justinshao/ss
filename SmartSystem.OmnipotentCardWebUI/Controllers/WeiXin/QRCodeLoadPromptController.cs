using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class QRCodeLoadPromptController : Controller
    {
        public ActionResult Index(string id)
        {
            TxtLogServices.WriteTxtLogEx("QRCodeEntrance", "LoadPrompt id：{0}", id);
            ViewBag.RequestId = id;
            return View();
        }
    }
}
