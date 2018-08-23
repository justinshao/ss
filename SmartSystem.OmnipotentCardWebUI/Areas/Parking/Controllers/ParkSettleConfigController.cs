using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Entities;
using Common.Services;
using Common.Utilities.Helpers;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
     [CheckPurview(Roles = "PK011103")]
    public class ParkSettleConfigController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetParkingData()
        {
            try
            {
                StringBuilder str = new StringBuilder();
                if (string.IsNullOrWhiteSpace(Request.Params["villageId"]))
                {
                    return str.ToString();
                }

                int pageIndex = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int pageSize = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

                int totalCount = 0;
                List<BaseParkinfo> parkData = ParkingServices.QueryPage(Request.Params["villageId"].ToString(), pageIndex, pageSize, out totalCount);

                str.Append("{");
                str.Append("\"total\":" + totalCount + ",");
                str.Append("\"rows\":" + JsonHelper.GetJsonString(parkData) + ",");
                str.Append("\"index\":" + pageIndex);
                str.Append("}");
                return str.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "结算获取车场信息失败");
                return string.Empty;
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01110301")]
        public JsonResult SaveConfig(BaseParkinfo model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.PKID)){
                    throw new MyException("获取配置编号失败");
                }

                bool result = ParkingServices.UpdateParkSettleConfig(model);
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存结算信息失败");
                return Json(MyResult.Error("保存失败"));
            }

        }
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSettleOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK011103").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01110301":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 7;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}