using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Utilities;
using Common.Core;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.User.Controllers
{
     [CheckPurview(Roles = "PK010603")]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetUserTreeData()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                string queryUserAccount = Request.Params["queryUserAccount"];

                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                int totalCount = 0;

                List<SysUser> users = SysUserServices.QuerySysUserPage(GetCurrentUserCompanyId, queryUserAccount, rows, page, out totalCount);
                var currObj = from p in users
                              select new
                              {
                                  ID = p.ID,
                                  RecordID = p.RecordID,
                                  UserAccount = p.UserAccount,
                                  UserName = p.UserName,
                                  IsDefaultUser = (int)p.IsDefaultUser,
                                  RoleDescription = GetUserRoleDescription(p),
                                  ScopeDescription = GetScopeDescription(p)
                              };

                sb.Append("{");
                sb.Append("\"total\":" + totalCount + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(currObj) + ",");
                sb.Append("\"index\":" + page);
                sb.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取用户信息失败[用户管理]");
            }

            return sb.ToString();
        }
        private string GetUserRoleDescription(SysUser user)
        {
            try
            {
                string errorMsg = string.Empty;
                List<SysRoles> smRoles = SysRolesServies.QuerySysRolesByUserId(user.RecordID);
                if (smRoles != null)
                {
                    return string.Join("<br>", smRoles.Select(p => p.RoleName));
                }
                return string.Empty;
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "获取角色描述[用户管理]");
                return string.Empty;
            }
        }
        private string GetScopeDescription(SysUser users)
        {
            try
            {
                StringBuilder strInfo = new StringBuilder();
                List<SysScope> scopes = SysScopeServices.QuerySysScopeByUserId(users.RecordID);
                if (scopes != null && scopes.Count > 0)
                {
                    foreach (var item in scopes)
                    {
                        strInfo.AppendFormat("{0}<br>", item.ASName);
                    }
                    if (!string.IsNullOrWhiteSpace(strInfo.ToString()))
                    {
                        return strInfo.ToString().Substring(0, strInfo.Length - 4);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "获取作用域描述失败[用户管理]");
                return string.Empty;
            }
        }
        public string GetRoleTreeData()
        {

            StringBuilder strTree = new StringBuilder();
            try
            {
                strTree.Append("[{\"id\":\"0\",");
                strTree.Append("\"attributes\":{\"type\":0},");
                strTree.Append("\"text\":\"系统角色\"");
                GetRoleChildTreeData(strTree);
                strTree.Append("}]");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取用户角色失败[用户管理]");
            }
            return strTree.ToString();
        }
        //构造一个树形结构的json
        private void GetRoleChildTreeData(StringBuilder childTree)
        {
            List<SysRoles> roles = SysRolesServies.QuerySysRolesByCompanyId(GetCurrentUserCompanyId);
            if (roles.Count() > 0)
            {
                roles = roles.OrderByDescending(p => p.IsDefaultRole).ToList();
                childTree.Append(",\"children\":[");
            }
            List<SysRoles> userRoles = new List<SysRoles>();
            if (!string.IsNullOrWhiteSpace(Request.Params["userId"]))
            {
                userRoles = SysRolesServies.QuerySysRolesByUserId(Request.Params["userId"]);
            }

            int i = 1;
            foreach (var obj in roles)
            {
                childTree.Append("{\"id\":\"" + obj.RecordID + "\",");
                childTree.Append("\"attributes\":{\"type\":1},");
                if (userRoles.FirstOrDefault(p => p.RecordID == obj.RecordID) != null)
                {
                    childTree.Append("\"checked\":true,");
                }
                if (obj.IsDefaultRole == YesOrNo.Yes)
                {
                    childTree.Append("\"text\":\"" + obj.RoleName + "[系统默认]" + "\"");
                }
                else
                {
                    childTree.Append("\"text\":\"" + obj.RoleName + "\"");
                }

                childTree.Append("}");
                if (i != roles.Count())
                {
                    childTree.Append(",");
                }
                i++;
            }

            if (roles.Count() > 0)
            {
                childTree.Append("]");
            }
        }
        public string GetScopeTreeData()
        {
            StringBuilder strTree = new StringBuilder();
            try
            {
                List<SysUserScopeMapping> scpoes = new List<SysUserScopeMapping>();
                if (!string.IsNullOrWhiteSpace(Request.Params["userId"]))
                {

                    scpoes = SysUserServices.QuerySysUserScopeMappingByUserId(Request.Params["userId"]);
                }

                strTree.Append("[{\"id\":\"0\",");
                strTree.Append("\"attributes\":{\"type\":0},");
                strTree.Append("\"text\":\"用户作用域\"");
                GetScopeChildTreeData(scpoes, strTree);
                strTree.Append("}]");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取用户作用域失败[用户管理]");
            }
            return strTree.ToString();
        }

        private void GetScopeChildTreeData(List<SysUserScopeMapping> mappings, StringBuilder childTree)
        {
            string ErrorMessage = string.Empty;
            List<SysScope> scopes = SysScopeServices.QuerySysScopeByCompanyId(GetCurrentUserCompanyId);
            if (scopes != null && scopes.Count > 0)
            {
                childTree.Append(",\"children\":[");
                int index = 1;
                foreach (var item in scopes)
                {
                    childTree.Append("{\"id\":\"" + item.ASID + "\",");
                    childTree.Append("\"attributes\":{\"type\":1},");
                    if (mappings.FirstOrDefault(p => p.ASID == item.ASID) != null)
                    {
                        childTree.Append("\"checked\":true,");
                    }
                    childTree.Append("\"text\":\"" + item.ASName + "\"");
                    childTree.Append("}");
                    if (index != scopes.Count)
                    {
                        childTree.Append(",");
                    }
                    index++;
                }
                childTree.Append("]");
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01060301,PK01060302")]
         public JsonResult SaveUser()
        {
            try
            {
                string userId = !string.IsNullOrWhiteSpace(Request.Params["RecordID"])?Request.Params["RecordID"].ToString():GuidGenerator.GetGuidString();
              
                SysUser smusers = GetSmUsers(userId);
                List<SysUserRolesMapping> rolesMappings = GetSysUserRolesMappings(userId);
                List<SysUserScopeMapping> scopeMappings = GetSysUserScopeMappings(userId);
                bool result = false;
                if (string.IsNullOrWhiteSpace(Request.Params["RecordID"]))
                {
                    result = SysUserServices.Add(smusers, rolesMappings, scopeMappings);
                }
                else
                {
                    result = SysUserServices.Update(smusers, rolesMappings, scopeMappings);
                }
                if (!result) throw new MyException("保存失败");

                CacheData.UpdateCacheUserLoginData(GetLoginUser.RecordID);
                return Json(MyResult.Success());

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存用户失败");
                return Json(MyResult.Error("保存失败"));
            }
        }

        private List<SysUserScopeMapping> GetSysUserScopeMappings(string userId)
        {
            string scopes = Request.Params["UserScopes"];
            if (string.IsNullOrWhiteSpace(scopes)) throw new MyException("获取小区信息失败");

            List<SysUserScopeMapping> scopeMappings = new List<SysUserScopeMapping>();
            string[] strActionScopes = scopes.Split(',');
            for (int i = 0; i < strActionScopes.Length; i++)
            {
                SysUserScopeMapping scopeMapping = new SysUserScopeMapping();
                scopeMapping.RecordID = GuidGenerator.GetGuidString();
                scopeMapping.UserRecordID = userId;
                scopeMapping.ASID = strActionScopes[i];
                scopeMappings.Add(scopeMapping);
            }
            return scopeMappings;
        }

        private List<SysUserRolesMapping> GetSysUserRolesMappings(string userId)
        {
            string roles = Request.Params["UserRoles"];
            if (string.IsNullOrWhiteSpace(roles)) throw new MyException("获取角色信息失败");
        
            List<SysUserRolesMapping> roleMappings = new List<SysUserRolesMapping>();
            string[] strRoles = roles.Split(',');
            for (int i = 0; i < strRoles.Length; i++)
            {
                SysUserRolesMapping role = new SysUserRolesMapping();
                role.RecordID = GuidGenerator.GetGuidString();
                role.UserRecordID = userId;
                role.RoleID = strRoles[i];
                roleMappings.Add(role);
            }
            return roleMappings;
        }
        private SysUser GetSmUsers(string recordId)
        {
            string userName = Request.Params["UserName"];
            string account = Request.Params["UserAccount"];
            string password = Request.Params["Password"];
            string isDefault = Request.Params["IsDefaultUser"];

            SysUser users = new SysUser();
            users.RecordID = recordId;
            users.UserAccount = account;
            users.UserName = userName;
            users.Password = string.IsNullOrWhiteSpace(password) ? string.Empty : MD5.Encrypt(password);
            users.CPID = GetCurrentUserCompanyId;
            users.IsDefaultUser = YesOrNo.No;
            return users;
        }
         [HttpPost]
         [CheckPurview(Roles = "PK01060303")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = SysUserServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除用户失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
        /// <summary>
        /// 获取系统用户操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSysUserOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleRights = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010603").ToList();

            foreach (var item in roleRights)
            {
                switch (item.ModuleID)
                {
                    case "PK01060301":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01060302":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01060303":
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
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 4;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
