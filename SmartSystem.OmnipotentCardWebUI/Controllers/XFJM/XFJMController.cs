using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface;
using Common.Entities;
using Common.Core;
using Common.Services.Park;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.XFJM
{
    /// <summary>
    /// 消费减免
    /// </summary>
    public class XFJMController : WeiXinController
    {
        public ParkSeller SellerLoginUser
        {
            get
            {
                if (HttpContext.Session["SmartSystem_SellerLoginUser"] == null)
                {
                    var userAccountCookie = HttpContext.Request.Cookies["SmartSystem_Seller_Login_UserAccount"];
                    var passwordCookie = HttpContext.Request.Cookies["SmartSystem_Seller_Login_Password"];
                    if (userAccountCookie != null && passwordCookie != null)
                    {
                        string account = userAccountCookie.Value;
                        string pwd = DES.DESDeCode(passwordCookie.Value, "Password");
                        ParkSeller sysUser = ParkSellerServices.QueryBySellerNo(account);
                        if (sysUser != null && sysUser.PWD.Equals(MD5.Encrypt(pwd)))
                        {
                            HttpContext.Session["SmartSystem_SellerLoginUser"] = sysUser;
                            return sysUser;
                        }
                    }
                }
                return (ParkSeller)HttpContext.Session["SmartSystem_SellerLoginUser"];
            }
        }
    }
}
