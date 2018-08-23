using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class ErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            string excepitonInfo = filterContext.Exception.ToString();
            ExceptionsServices.AddExceptions(filterContext.Exception,"系统异常");
            filterContext.HttpContext.Response.Redirect("~/RedirectPage/SystemError");
        }
    }
}