using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Common;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 缴费记录
    /// </summary>
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class PaymentRecordController : WeiXinController
    {
        public ActionResult Index()
        {
            List<PkOrderTemp> tempOrders = RecordQueryService.GetPkOrderTemp(WeiXinUser.AccountID);
            List<PkOrderMonth> monthOrders = RecordQueryService.GetPkOrderMonth(WeiXinUser.AccountID);
            ViewBag.TempOrders = tempOrders;
            ViewBag.MonthOrders = monthOrders;

            int defaultShowItem = 0;
            int type = 0;
            if (!string.IsNullOrWhiteSpace(Request["ShowItem"]) && int.TryParse(Request["ShowItem"].ToString(), out type))
            {
                defaultShowItem = type;
            }
            ViewBag.DefaultShowItem = defaultShowItem;

            return View(WeiXinUser);
        }
    }
}
