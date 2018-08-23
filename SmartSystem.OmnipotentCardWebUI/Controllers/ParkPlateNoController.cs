using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class ParkPlateNoController : Controller
    {
        public ActionResult Index(string selectControlId = "defaultProvinceId", string txtControlId = "defaultNumberId", string txtWidth = "100px", bool needShowHead = true,bool needShowInput=true)
        {
            ViewBag.SelectControlId = selectControlId;
            ViewBag.TxtControlId = txtControlId;
            ViewBag.Width = txtWidth;
            ViewBag.NeedShowHead = needShowHead;
            ViewBag.NeedShowInput = needShowInput;
            return PartialView();
        }
    }
}
