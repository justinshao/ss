using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.WeiXin;
using Common.Entities.Other;
using System.Text;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信账号
    /// </summary>
    [CheckPurview(Roles = "PK010906")]
    public class WXAccountController : BaseController
    {
        /// <summary>
        /// 微信帐号
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 微信帐户信息
        /// </summary>
        /// <returns></returns>
        public string Search_WXAccount()
        {
            int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
            int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
            string accountname = Request.Params["accountname"];
            string mobile = Request.Params["mobile"];
            DateTime starttime = DateTime.Parse(Request.Params["starttime"]);
            DateTime endtime = DateTime.Parse(Request.Params["endtime"]);
            if (string.IsNullOrWhiteSpace(Request.Params["CompanyID"]))
                return string.Empty;
            string companyId = Request.Params["CompanyID"].ToString();

            Pagination pagination = WXAccountServices.Search_WXAccount(companyId,accountname, mobile, starttime, endtime, page, rows);
            StringBuilder sb = new StringBuilder();
            string str = JsonHelper.GetJsonString(pagination.WXAccountList);
            sb.Append("{");
            sb.Append("\"total\":" + pagination.Total + ",");
            sb.Append("\"rows\":" + str + ",");
            sb.Append("\"index\":" + rows);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
