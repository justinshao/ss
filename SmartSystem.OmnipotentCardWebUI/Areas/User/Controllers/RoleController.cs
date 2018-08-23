using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Services;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using System.Xml.Linq;
using Common.Utilities;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Areas.User.Controllers
{
    [CheckPurview(Roles = "PK010601")]
    public class RoleController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.CanAdd = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060101") != null;
            ViewBag.CanUpdate = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060102") != null;
            ViewBag.CanDelete = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060103") != null;
            ViewBag.CanaAuthorize = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK01060104") != null;
            return View();
        }
        [CheckPurview(Roles = "PK01060101,PK01060102")]
        public JsonResult SaveRole(SysRoles role)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(role.RecordID))
                {
                    role.CPID = GetCurrentUserCompanyId;
                    result = SysRolesServies.AddSysRole(role);
                }
                else
                {
                    result = SysRolesServies.UpdateRole(role);
                }
                return Json(new MyResult
                {
                    result = result,
                    msg = result ? "保存成功" : "保存失败"
                });
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存角色失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [CheckPurview(Roles = "PK01060103")]
        public JsonResult DeleteRole(string recordId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recordId)) throw new MyException("获取编号失败");

                bool result = SysRolesServies.DeleteRoleByRecordId(recordId);
                return Json(new MyResult
                {
                    result = result,
                    msg = result ? "删除成功" : "删除失败"
                });
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除角色失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
        public string GetSystemRoles()
        {
            StringBuilder strTree = new StringBuilder();
            try
            {
                List<SysRoles> roles = SysRolesServies.QuerySysRolesByCompanyId(GetCurrentUserCompanyId);
                strTree.Append("[");
                int i = 1;
                foreach (var obj in roles)
                {
                    strTree.Append("{\"id\":\"" + obj.RecordID + "\",");
                    strTree.Append("\"iconCls\":\"my-role-icon\",");
                    strTree.Append("\"attributes\":{\"type\":0,\"isdefaultrole\":\"" + (int)obj.IsDefaultRole + "\"},");
                    if (obj.IsDefaultRole == YesOrNo.Yes)
                    {

                        strTree.Append("\"text\":\"" + obj.RoleName + "[系统默认]" + "\"");
                    }
                    else
                    {
                        strTree.Append("\"text\":\"" + obj.RoleName + "\"");
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
                ExceptionsServices.AddExceptions(ex, "用户角色管理 构建用户角色树失败");
            }
            return strTree.ToString();
        }
        public string GetSystemModule()
        {
            try
            {
                List<SysRoleAuthorize> roles = new List<SysRoleAuthorize>();

                XDocument xdoc = XDocument.Load(Server.MapPath("~/SystemModule.xml"));
                foreach (XElement element in xdoc.Root.Elements())
                {
                    string moduleId = element.Attribute("ModuleID").Value;
                    string moduleName = element.Attribute("ModuleName").Value;
                    string parentId = element.Attribute("ParentID").Value;
                    string isSystemDefault = element.Attribute("SystemDefault").Value;

                    SysRoleAuthorize role = new SysRoleAuthorize();
                    role.ModuleID = moduleId;
                    role.ModuleName = moduleName;
                    role.ParentID = parentId;
                    role.IsSystemDefault = isSystemDefault == "true";
                    roles.Add(role);
                }


                if (string.IsNullOrWhiteSpace(Request.Params["roleId"]))
                    return "[]";

                string roleId = Request.Params["roleId"].ToString();
                SysRoles sysRole = SysRolesServies.QuerySysRolesByRecordId(roleId);

                StringBuilder strTree = new StringBuilder();
                strTree.Append("[{\"id\":\"" + sysRole.RecordID + "\",");
                strTree.Append("\"attributes\":{\"type\":0},");
                strTree.Append("\"text\":\"" + sysRole.RoleName + "[授权]\"");


                List<SysRoleAuthorize> rootModules = roles.Where(p => p.ParentID == "").ToList();
                if (rootModules.Count > 0)
                {
                    strTree.Append(",\"children\":[");
                }
                List<SysRoleAuthorize> roleAuthorizes = SysRoleAuthorizeServices.QuerySysRoleAuthorizeByRoleId(sysRole.RecordID);
                int i = 1;
                foreach (var item in rootModules)
                {
                    strTree.Append("{\"id\":\"" + item.ModuleID + "_" + item.ParentID + "_" + roleId + "\",");
                    strTree.Append("\"text\":\"" + item.ModuleName + "\",");
                    bool isDefaultRole = false;
                    if (sysRole.IsDefaultRole == YesOrNo.Yes && item.IsSystemDefault)
                    {
                        isDefaultRole = true;
                        strTree.Append("\"attributes\":{\"type\":1,\"isdefault\":1},");
                        strTree.Append("\"checked\":true");
                    }
                    if (!isDefaultRole)
                    {
                        strTree.Append("\"attributes\":{\"type\":1,\"isdefault\":0}");
                    }
                    if (!isDefaultRole && roleAuthorizes != null && roleAuthorizes.Exists(p => p.ModuleID == item.ModuleID) && !roleAuthorizes.Exists(p => p.ParentID == item.ModuleID))
                    {
                        strTree.Append(",\"checked\":true");
                    }

                    GetChildRoles(sysRole, item, roles, strTree, roleAuthorizes, roleId);
                    strTree.Append("}");
                    if (i != rootModules.Count())
                    {
                        strTree.Append(",");
                    }
                    i++;
                }
                if (rootModules.Count > 0)
                {
                    strTree.Append("]");
                }

                strTree.Append("}]");
                return strTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取角色授权模块失败");
                return "[]";
            }

        }
        private void GetChildRoles(SysRoles role, SysRoleAuthorize rootModule, List<SysRoleAuthorize> roles, StringBuilder childTree, List<SysRoleAuthorize> dbRoles, string roleId)
        {
            List<SysRoleAuthorize> childRights = roles.Where(p => p.ParentID == rootModule.ModuleID).ToList();
            if (childRights.Count == 0) return;

            if (childRights.Count > 0)
            {
                childTree.Append(",\"state\":\"closed\",\"children\":[");
            }
            int i = 1;
            foreach (var obj in childRights)
            {
                childTree.Append("{\"id\":\"" + obj.ModuleID + "_" + obj.ParentID + "_" + roleId + "\",");
                //childTree.Append("\"attributes\":{\"type\":1},");
                childTree.Append("\"text\":\"" + obj.ModuleName + "\",");
                //if (dbRoles != null && dbRoles.Exists(p => p.ModuleID == obj.ModuleID) && !dbRoles.Exists(p => p.ParentID == obj.ModuleID))
                //{
                //    childTree.Append(",\"checked\":true");
                //}
                bool isDefaultRole = false;
                if (role.IsDefaultRole == YesOrNo.Yes && obj.IsSystemDefault)
                {
                    isDefaultRole = true;
                    childTree.Append("\"attributes\":{\"type\":1,\"isdefault\":1},");
                    childTree.Append("\"checked\":true");
                }
                if (!isDefaultRole)
                {
                    childTree.Append("\"attributes\":{\"type\":1,\"isdefault\":0}");
                }
                if (!isDefaultRole && dbRoles != null && dbRoles.Exists(p => p.ModuleID == obj.ModuleID) && !dbRoles.Exists(p => p.ParentID == obj.ModuleID))
                {
                    childTree.Append(",\"checked\":true");
                }
                GetChildRoles(role, obj, roles, childTree, dbRoles, roleId);

                childTree.Append("}");
                if (i != childRights.Count())
                {
                    childTree.Append(",");
                }
                i++;
            }
            if (childRights.Count > 0)
            {
                childTree.Append("]");
            }
        }
        [CheckPurview(Roles = "PK01060104")]
        public JsonResult SaveRoleModule()
        {

            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["roleId"])) throw new MyException("请选择角色");
              
                string roleId = Request.Params["roleId"].ToString();
                SysRoles role = SysRolesServies.QuerySysRolesByRecordId(roleId);
                if (role == null) throw new MyException("获取角色信息失败");

                if (role.IsDefaultRole == YesOrNo.No && string.IsNullOrWhiteSpace(Request.Params["selectModuleIds"]))
                    throw new MyException("请选择需要授权的模块");

                string roleModules = Request.Params["selectModuleIds"].ToString();
                List<SysRoleAuthorize> models = new List<SysRoleAuthorize>();
                if (!string.IsNullOrWhiteSpace(roleModules))
                {
                    string[] strRoles = roleModules.Split(',');
                    for (int i = 0; i < strRoles.Length; i++)
                    {
                        string[] d = strRoles[i].Split('_');
                        if (d.Length != 3) continue;

                        SysRoleAuthorize model = new SysRoleAuthorize();
                        model.RecordID = GuidGenerator.GetGuid().ToString();
                        model.RoleID = role.RecordID;
                        model.ModuleID = d[0];
                        model.ParentID = d[1];
                        models.Add(model);
                    }
                }
                FillDefaultModule(models, role);

                if (models.Count == 0) throw new MyException("获取模块信息失败");

                bool result = SysRoleAuthorizeServices.Add(models);
                if (!result) throw new MyException("保存角色模块失败");

                CacheData.UpdateCacheUserLoginData(GetLoginUser.RecordID);
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存角色模块授权失败");
                return Json(MyResult.Error("保存失败"));
            }

        }
        private void FillDefaultModule(List<SysRoleAuthorize> models, SysRoles role)
        {

            if (role.IsDefaultRole != YesOrNo.Yes) return;

            XDocument xdoc = XDocument.Load(Server.MapPath("~/SystemModule.xml"));
            foreach (XElement element in xdoc.Root.Elements())
            {
                string isSystemDefault = element.Attribute("SystemDefault").Value;
                if (isSystemDefault != "true") continue;

                string moduleId = element.Attribute("ModuleID").Value;
                if (models.Count(p => p.ModuleID == moduleId) > 0) continue;

                string moduleName = element.Attribute("ModuleName").Value;
                string parentId = element.Attribute("ParentID").Value;


                SysRoleAuthorize model = new SysRoleAuthorize();
                model.RecordID = GuidGenerator.GetGuid().ToString();
                model.RoleID = role.RecordID;
                model.ModuleID = moduleId;
                model.ParentID = parentId;
                model.ModuleName = moduleName;
                model.IsSystemDefault = true;
                models.Add(model);
            }
        }
    }
}
