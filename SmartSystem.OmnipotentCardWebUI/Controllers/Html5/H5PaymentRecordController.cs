using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinInerface;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    [H5LoginPurview(Roles = "Login")]
    public class H5PaymentRecordController : H5WeiXinController
    {
        public ActionResult Index()
        {
            List<PkOrderTemp> tempOrders = RecordQueryService.GetPkOrderTemp(UserAccount.AccountID);
            List<PkOrderMonth> monthOrders = RecordQueryService.GetPkOrderMonth(UserAccount.AccountID);
            ViewBag.TempOrders = tempOrders;
            ViewBag.MonthOrders = monthOrders;

            int defaultShowItem = 0;
            int type = 0;
            if (!string.IsNullOrWhiteSpace(Request["ShowItem"]) && int.TryParse(Request["ShowItem"].ToString(), out type))
            {
                defaultShowItem = type;
            }
            ViewBag.DefaultShowItem = defaultShowItem;

            return View();
        }
    }
}
