using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;
using System.IO;
using Common.Utilities.Helpers;
using SmartSystem.OmnipotentCardWebUI.Controllers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信支付配置
    /// </summary>
    [CheckPurview(Roles = "PK010908")]
    public class WXPaymentConfigController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetWXPaymentConfigData(string companyId)
        {
            try
            {
                WX_ApiConfig config = WXApiConfigServices.QueryByCompanyId(companyId);
                return Json(MyResult.Success(string.Empty, config));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取微信API配置信息失败");
                return Json(MyResult.Error("获取配置信息失败"));
            }
        }
        [HttpPost]
        public JsonResult AddOrUpdate(WX_ApiConfig config)
        {
            try
            {
                bool result = WXApiConfigServices.UpdatePayConfig(config);
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存微信API配置信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
    }
}
