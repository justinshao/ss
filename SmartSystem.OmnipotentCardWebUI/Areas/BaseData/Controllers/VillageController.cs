using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Entities;
using Common.Utilities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.BaseData.Controllers
{
    [CheckPurview(Roles = "PK010102")]
    public class VillageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取小区信息
        /// </summary>
        /// <param name="Companyid"></param>
        /// <returns></returns>
        public string GetVillageData()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int pageIndex = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int pageSize = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                string sort = Request.Params["sort"];
                if (string.IsNullOrWhiteSpace(Request.Params["companyId"]))
                    return string.Empty;

                if (GetLoginUserVillages.Count == 0) return string.Empty;

               string companyId=Request.Params["companyId"];
                int totalRecord = 0;
                List<BaseVillage> list = VillageServices.QueryPage(GetLoginUserVillages.Select(p=>p.VID).ToList(),companyId, pageIndex, pageSize, out totalRecord);

                sb.Append("{");
                sb.Append("\"total\":" + totalRecord + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(list) + ",");
                sb.Append("\"index\":" + pageIndex);
                sb.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取小区信息失败");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 删除小区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01010203")]
        public JsonResult Delete(string villageId)
        {
            try
            {
                bool result = VillageServices.Delete(villageId);
                if (!result) throw new MyException("删除小区失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除小区失败");
                return Json(MyResult.Error("删除小区失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010201,PK01010202")]
        public JsonResult EditVillage(BaseVillage model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.VID))
                {
                    model.ProxyNo = GuidGenerator.GetGuidString();
                    model.VID = GuidGenerator.GetGuidString();
                    result = VillageServices.Add(model);
                    if (!result) throw new MyException("添加小区信息失败");
                    UpdateLoginUserVillageCache(model);
                    return Json(MyResult.Success("添加小区信息成功【如需查看或修改该小区信息，请对当前登录账号所在的作用域中勾选该小区】"));
                }
                else
                {
                    result = VillageServices.Update(model);
                    if (!result) throw new MyException("修改小区信息失败");
                    UpdateLoginUserVillageCache(model);

                    return Json(MyResult.Success("修改小区信息成功"));
                }
             
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存小区失败");
                return Json(MyResult.Error("保存小区失败"));
            }
        }
        private void UpdateLoginUserVillageCache(BaseVillage model)
        {
            List<BaseVillage> villages = GetLoginUserVillages;
            if (villages != null && villages.Count(p => p.VID == model.VID) > 0)
            {
                villages.Remove(villages.First(p => p.VID == model.VID));
                villages.Add(model);
                Session["SmartSystem_SystemLoginUserRole_Valid_Village"] = villages;
            }
        }
        /// <summary>
        /// 获取小区操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVillageOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010102").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010201":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010202":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010203":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }


            }

            SystemOperatePurview soption = new SystemOperatePurview();
            soption.text = "刷新";
            soption.handler = "Refresh";
            soption.sort = 4;
            options.Add(soption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
