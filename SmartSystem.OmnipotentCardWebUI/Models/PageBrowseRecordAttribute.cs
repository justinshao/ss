using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Services;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities.Enum;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class PageBrowseRecordAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var moduleid = filterContext.RequestContext.HttpContext.Request["moduleid"];
                var openid = filterContext.RequestContext.HttpContext.Request["openId"];
                if (string.IsNullOrWhiteSpace(openid))
                {
                    var cookie = filterContext.RequestContext.HttpContext.Request.Cookies["SmartSystem_WeiXinOpenId"];
                    if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                    {
                        openid = cookie.Value;
                    }
                }
                if (!string.IsNullOrWhiteSpace(moduleid) && !string.IsNullOrWhiteSpace(openid))
                {
                    WX_MenuAccessRecord model = new WX_MenuAccessRecord();
                    model.OpenID = openid;
                    model.MenuName = ((WeiXinModule)int.Parse(moduleid)).GetDescription();
                    WXMenuAccessRecordServices.Create(model);
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "记录页面访问记录失败", LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("PageBrowseRecord", ex);
            }
        }
    }
}