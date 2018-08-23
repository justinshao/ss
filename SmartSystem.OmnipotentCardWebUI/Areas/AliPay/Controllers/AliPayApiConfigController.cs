using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Entities;
using Common.Services;
using SmartSystem.AliPayServices;

namespace SmartSystem.OmnipotentCardWebUI.Areas.AliPay.Controllers
{
    [CheckPurview(Roles = "PK011001")]
    public class AliPayApiConfigController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAlipayApiConfig(string companyId)
        {
            try
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryByCompanyID(companyId);
                return Json(MyResult.Success(string.Empty, config));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取支付宝配置信息失败");
                return Json(MyResult.Error("获取支付宝配置信息失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK011001")]
        public JsonResult AddOrUpdate(AliPayApiConfig config)
        {
            try
            {
                config.SystemDomain = config.SystemDomain.TrimEnd('/');
                bool result = AliPayApiConfigServices.AddOrUpdate(config);
                if (!result) throw new MyException("保存失败");

                return Json(MyResult.Success("",config.RecordId));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存支付宝配置信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
    }
}
