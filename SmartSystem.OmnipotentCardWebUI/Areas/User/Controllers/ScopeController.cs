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

namespace SmartSystem.OmnipotentCardWebUI.Areas.User.Controllers
{
    [CheckPurview(Roles = "PK010602")]
    public class ScopeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.AddScope = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060201") != null;
            ViewBag.UpdateScope = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060202") != null;
            ViewBag.DeleteScope = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060203") != null;
            ViewBag.ScopeAuthorizeVillage = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060204") != null;
            return View();
        }
        public string GetScopeTreeData()
        {
            StringBuilder strTree = new StringBuilder();

            try
            {
                strTree.Append("[");
                var roles = SysScopeServices.QuerySysScopeByCompanyId(GetCurrentUserCompanyId);
                int i = 1;
                foreach (var obj in roles)
                {

                    strTree.Append("{\"id\":\"" + obj.ASID + "\",");
                    strTree.Append("\"attributes\":{\"type\":1,\"isdefault\":\"" + (int)obj.IsDefaultScope + "\"},");
                    if (obj.IsDefaultScope == Common.Entities.YesOrNo.Yes)
                    {
                        strTree.Append("\"text\":\"" + obj.ASName + "[系统默认]" + "\"");
                    }
                    else
                    {
                        strTree.Append("\"text\":\"" + obj.ASName + "\"");
                    }

                    strTree.Append("}");
                    if (i != roles.Count())
                    {
                        strTree.Append(",");
                    }
                    i++;
                }
                strTree.Append("]");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "作用域管理 构建作用域树失败");
            }
            return strTree.ToString();
        }
        [CheckPurview(Roles = "PK01060201,PK01060202")]
        public JsonResult SaveScope(SysScope model)
        {
            try
            {
                string errorMsg = string.Empty;
                if (string.IsNullOrWhiteSpace(model.ASID))
                {
                    model.CPID = GetCurrentUserCompanyId;
                    model.ASID = GuidGenerator.GetGuid().ToString();
                    bool result = SysScopeServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                }
                else
                {
                    bool result = SysScopeServices.Update(model);
                    if (!result) throw new MyException("修改失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存作用域失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [CheckPurview(Roles = "PK01060203")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = SysScopeServices.DeleteByRecordId(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除失败");
                return Json(MyResult.Error("删除失败"));
            }
        }

        public string GetSysScopeAuthorize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["scopeId"]))
                    return string.Empty;

                string scopeId = Request.Params["scopeId"];

                SysScope scope = SysScopeServices.QuerySysScopeByRecordId(scopeId);
                if (scope == null)
                    return string.Empty;

                StringBuilder strTree = new StringBuilder();
                strTree.Append("[{\"id\":\"" + scope.ASID + "\",");
                strTree.Append("\"attributes\":{\"type\":0},");
                strTree.Append("\"text\":\"" + scope.ASName + "[作用域]\"");

                List<SysScopeAuthorize> scopeAuthorizes = SysScopeAuthorizeServices.QuerySysScopeAuthorizeByScopeId(scope.ASID)
                                                            .Where(p => p.ASType == ASType.Village).ToList();

                List<BaseCompany> compamys = CompanyServices.QueryCompanyAndSubordinateCompany(GetCurrentUserCompanyId);
                if (compamys.Count == 0) return string.Empty;

                var list = VillageServices.QueryVillageByCompanyIds(compamys.Select(p => p.CPID).ToList());
                if (list.Count > 0)
                {
                    strTree.Append(",\"children\":[");
                }

                int i = 1;
                foreach (var item in list)
                {
                    string villageName = item.VName;
                    BaseCompany company = compamys.FirstOrDefault(p => p.CPID == item.CPID);
                    if (company != null)
                    {
                        villageName = string.Format("{0}【{1}】", item.VName, company.CPName);
                    }
                    strTree.Append("{\"id\":\"" + scope.ASID + "_" + item.VID + "\",");
                    strTree.Append("\"attributes\":{\"type\":1},");
                    strTree.Append("\"text\":\"" + villageName + "\"");
                    if (scopeAuthorizes != null && scopeAuthorizes.Exists(p => p.TagID == item.VID))
                    {
                        strTree.Append(",\"checked\":true");
                    }

                    strTree.Append("}");
                    if (i != list.Count())
                    {
                        strTree.Append(",");
                    }
                    i++;
                }
                if (list.Count > 0)
                {
                    strTree.Append("]");
                }

                strTree.Append("}]");
                return strTree.ToString();
            }
            catch (MyException ex)
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "作用域授权时获取小区信息失败");
                return string.Empty;
            }
        }
        [CheckPurview(Roles = "PK01060204")]
        public JsonResult SaveSysScopeAuthorize(string selectVillageIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectVillageIds)) throw new MyException("获取选择的小区失败");

                List<SysScopeAuthorize> scopeAuthorizes = new List<SysScopeAuthorize>();
                string[] strVillageIds = selectVillageIds.Split(',');
                for (int i = 0; i < strVillageIds.Length; i++)
                {
                    string[] s = strVillageIds[i].Split('_');
                    if (s.Length != 2)
                    {
                        continue;
                    }
                    SysScopeAuthorize model = new SysScopeAuthorize();
                    model.ASID = s[0];
                    model.ASDID = GuidGenerator.GetGuid().ToString();
                    model.TagID = s[1];
                    model.CPID = GetCurrentUserCompanyId;
                    model.ASType = ASType.Village;
                    scopeAuthorizes.Add(model);
                }
                bool result = SysScopeAuthorizeServices.Add(scopeAuthorizes);
                if (!result) throw new MyException("授权失败");

                CacheData.UpdateCacheUserLoginData(GetLoginUser.RecordID);
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "作用域授权失败");
                return Json(MyResult.Error("授权失败"));
            }
        }
    }
}
