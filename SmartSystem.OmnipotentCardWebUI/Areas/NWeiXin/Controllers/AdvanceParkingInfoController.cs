using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Services.WeiXin;
using Common.Utilities.Helpers;
using Common.Entities.WX;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    public class AdvanceParkingInfoController : BaseController
    {
       
        public ActionResult Index()
        {
            return View();
        }
        public string GetAdvanceParkingInfoData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                string plateNo = string.Empty;
                if (!string.IsNullOrWhiteSpace(Request.Params["PlateNo"])) {
                    plateNo = Request.Params["PlateNo"].ToString();
                }
                DateTime start = DateTime.Now.AddDays(-1);
                DateTime? sTime=null;
                if (!string.IsNullOrWhiteSpace(Request.Params["StartTime"]) && DateTime.TryParse(Request.Params["StartTime"], out start))
                {
                    sTime = start;
                }
                DateTime end = DateTime.Now;
                DateTime? eTime=null;
                if (!string.IsNullOrWhiteSpace(Request.Params["EndTime"]) && DateTime.TryParse(Request.Params["EndTime"], out end))
                {
                    eTime = end;
                }
                if (string.IsNullOrWhiteSpace(Request.Params["CompanyId"]))
                {
                    return strData.ToString();
                }
                string companyId = Request.Params["CompanyId"].ToString();
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

                int total = 0;
                List<AdvanceParking> result = AdvanceParkingServices.QueryPage(companyId,plateNo, sTime, eTime, page, rows, out total);
                var models = from p in result select new { 
                    ID=p.ID,
                    OrderId = p.OrderId.ToString(),
                    PlateNo = p.PlateNo,
                    StartTime = p.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = p.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Amount = p.Amount,
                    PayTime = p.PayTime != DateTime.MinValue ? p.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                };
                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(models) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取预停车信息失败");
            }

            return strData.ToString();
        }
    }
}
