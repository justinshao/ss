using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Admin
{
    /// <summary>
    /// 管理员车辆管理
    /// </summary>
    public class AdminCarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
