using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 加载提示
    /// </summary>
    public class LoadPromptController : Controller
    {
        public ActionResult Index(string id)
        {
            TxtLogServices.WriteTxtLogEx("RedirectHandle", "LoadPrompt id：{0}", id);
            ViewBag.RequestId = id;
            return View();
        }

    }
}
