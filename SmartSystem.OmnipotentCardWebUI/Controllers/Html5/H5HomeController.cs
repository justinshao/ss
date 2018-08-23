using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    /// <summary>
    /// 首页
    /// </summary>
    public class H5HomeController : H5WeiXinController
    {
        public ActionResult Index()
        {
            ViewBag.HeadImage = "/Content/images/weixin/defaultweixinhead.png";
            string loginAccount = "未登录";
            bool loginStatus = false;
            if (UserAccount != null) {
                loginAccount = UserAccount.MobilePhone;
                loginStatus = true;
            }
            ViewBag.LoginAccount = loginAccount;
            ViewBag.LoginStatus = loginStatus;
            return View();
        }

    }
}
