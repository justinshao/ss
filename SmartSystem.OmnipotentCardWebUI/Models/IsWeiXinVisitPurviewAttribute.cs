using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class IsWeiXinVisitPurviewAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Request.UserAgent != null && !filterContext.HttpContext.Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=请在微信中打开';</script>";
                    filterContext.HttpContext.Response.Write(response_js);
                    return;
                }

            }
            catch (Exception ex)
            {
                string response_js = "<script>window.parent.location.href='/ErrorPrompt/Index?message=验证是否为微信访问';</script>";
                filterContext.HttpContext.Response.Write(response_js);
                return;
            }
        }
    }
}