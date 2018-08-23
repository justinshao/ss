using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Statistics;
namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    public class InComeController : Controller
    {
        /// <summary>
        /// 车场收入
        /// </summary>
        /// <returns></returns>
         [CheckPurview(Roles = "PK010505")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询车场收入数据
        /// </summary>
        /// <returns></returns>
        public JsonResult Search_InCome()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            json.Data = StatisticsServices.Analysis_InCome(parkingid);
            return json; 
        }
    }
}
