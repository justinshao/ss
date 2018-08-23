using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Statistics;
namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    public class InOutController : Controller
    {
        /// <summary>
        /// 进出分析
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010506")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询车场进出数据
        /// </summary>
        /// <returns></returns>
        public JsonResult Search_InOut()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            json.Data = StatisticsServices.Analysis_InOut(parkingid); 
            return json;  
        }
    }
}
