using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Entities.WX;
using Common.Services.WeiXin;
using Common.Utilities.Helpers;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    [CheckPurview(Roles = "PK010909")]
    public class WXOpinionFeedbackController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.DefaultStartDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.DefaultEndDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }
        public string GetOpinionFeedbackData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["CompanyID"]))
                {
                    return strData.ToString();
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

                DateTime start = DateTime.Now.AddDays(-1);
                DateTime sTime;
                if (!string.IsNullOrWhiteSpace(Request.Params["StartTime"]) && DateTime.TryParse(Request.Params["StartTime"], out sTime))
                {
                    start = sTime;
                }
                DateTime end = DateTime.Now;
                DateTime eTime;
                if (!string.IsNullOrWhiteSpace(Request.Params["EndTime"]) && DateTime.TryParse(Request.Params["EndTime"], out eTime))
                {
                    end = eTime;
                }
               
                string companyId = Request.Params["CompanyID"].ToString();
                int total = 0;
                List<WX_OpinionFeedback> result = WXOpinionFeedbackServices.QueryPage(companyId,start, end, page, rows, out total);
                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(result) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询微信意见反馈失败");
            }

            return strData.ToString();
        }

    }
}
