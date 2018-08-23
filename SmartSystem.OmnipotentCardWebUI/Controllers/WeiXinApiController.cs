using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinBase;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class WeiXinApiController : Controller
    {
        //
        // GET: /WeiXinApi/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string TargetedValue() {
            string appId = string.Empty;
            string appSecret = string.Empty;
            SqlConnection con = new SqlConnection(SystemDefaultConfig.ReadConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("select top 1 * from WX_ApiConfig", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count>0)
            {
                appId = dt.Rows[0]["AppId"].ToString();
                appSecret = dt.Rows[0]["AppSecret"].ToString();
            }
            else {
                return "微信AppID未配置";
            }
            var accessToken = AccessTokenContainer.TryGetToken(appId,appSecret, false);
            var ticket = WxAdvApi.GetTicket(accessToken);
            StringBuilder strbui = new StringBuilder();
                strbui.Append("{");
                strbui.Append("\"accesstoken\":" + accessToken + ",");
                strbui.Append("\"ticket\":" + ticket.ticket + ",");
                strbui.Append("}");
                return strbui.ToString();
        }

    }


}
