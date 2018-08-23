using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities.Enum;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信其他配置
    /// </summary>
    [CheckPurview(Roles = "PK010902")]
    public class WXOtherConfigController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetWxOtherConfigData(string companyId)
        {
            JsonResult json = new JsonResult();
            try
            {
                List<WX_OtherConfig> configs = WXOtherConfigServices.QueryAll(companyId);
                List<WX_OtherConfig> models = new List<WX_OtherConfig>();
                foreach (object o in Enum.GetValues(typeof(ConfigType)))
                {
                    ConfigType type = (ConfigType)o;
                    WX_OtherConfig model = new WX_OtherConfig();
                    model.ConfigType = type;
                    WX_OtherConfig config = configs.FirstOrDefault(p => p.ConfigType == type);
                    if (config != null)
                    {
                        model.ConfigValue = config.ConfigValue;
                    }
                    model.CompanyID = companyId;
                    model.Description = type.GetDescription();
                    model.DefaultDescription = type.GetEnumDefaultValue();
                    models.Add(model);
                }
                json.Data = models;

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取微信其他配置信息失败");
            }
            return json;
        }
        [HttpPost]
        public JsonResult AddOrUpdate(WX_OtherConfig config)
        {
            try
            {
                bool result = WXOtherConfigServices.AddOrUpdate(config);
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "修改微信其他配置信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        /// <summary>
        /// 获取微信其他配置操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWxOtherConfigOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK0109").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {

                    case "PK010902":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview option1 = new SystemOperatePurview();
            option1.id = "btnformat";
            option1.iconCls = "icon-wxconfig-format";
            option1.text = "对应格式";
            option1.handler = "Format";
            option1.sort = 2;
            options.Add(option1);

            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
