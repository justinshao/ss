using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;
using Common.Factory;
using Common.DataAccess;
using Common.Utilities;
using Common.Core;
using System.Xml.Linq;

namespace Common.Services
{
    public class SysUserServices
    {
        public static SysUser QuerySysUserByUserAccount(string userAccount) {
            if (string.IsNullOrWhiteSpace(userAccount)) throw new ArgumentNullException("userAccount");

            //if (DateTime.Now > DateTime.Parse("2017-09-30"))
            //{
            //    return null;
            //}
            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserByUserAccount(userAccount);
        }

        public static SysUser QuerySysUserByRecordId(string recordId) {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserByRecordId(recordId);
        }

        public static bool Update(SysUser model, List<SysUserRolesMapping> useRoleMappings, List<SysUserScopeMapping> useScopeMappings)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (useRoleMappings == null || useRoleMappings.Count==0) throw new ArgumentNullException("useRoleMappings");
            if (useScopeMappings == null || useScopeMappings.Count == 0) throw new ArgumentNullException("useScopeMappings");

            ISysUser factory = SysUserFactory.GetFactory();
            ISysUserRolesMapping roleFactory = SysUserRolesMappingFactory.GetFactory();
            ISysUserScopeMapping scopeFactory = SysUserScopeMappingFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Update(model, dbOperator);
                    if (!result) throw new MyException("修改用户信息失败");

                    roleFactory.DeleteByUserId(model.RecordID, dbOperator);
                    result = roleFactory.Add(useRoleMappings, dbOperator);
                    if (!result) throw new MyException("修改模块授权信息失败");

                    scopeFactory.DeleteByUserID(model.RecordID, dbOperator);
                    result = scopeFactory.Add(useScopeMappings, dbOperator);
                    if (!result) throw new MyException("修改作用域授权信息失败");

                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog<SysUser>(model, OperateType.Update);
                        OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("UserRolesMapping,UserId:{0}", model.RecordID));
                        OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("UserScopeMapping,UserId:{0}", model.RecordID));

                        foreach (var item in useRoleMappings)
                        {
                            OperateLogServices.AddOperateLog<SysUserRolesMapping>(item, OperateType.Add);
                        }
                        foreach (var item in useScopeMappings)
                        {
                            OperateLogServices.AddOperateLog<SysUserScopeMapping>(item, OperateType.Add);
                        }
                    }
                    return result;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Add(SysUser model, List<SysUserRolesMapping> useRoleMappings, List<SysUserScopeMapping> useScopeMappings)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (useRoleMappings == null || useRoleMappings.Count == 0) throw new ArgumentNullException("useRoleMappings");
            if (useScopeMappings == null || useScopeMappings.Count == 0) throw new ArgumentNullException("useScopeMappings");

            ISysUser factory = SysUserFactory.GetFactory();
            ISysUserRolesMapping roleFactory = SysUserRolesMappingFactory.GetFactory();
            ISysUserScopeMapping scopeFactory = SysUserScopeMappingFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Add(model, dbOperator);
                    if (!result) throw new MyException("添加用户信息失败");

                    result = roleFactory.Add(useRoleMappings, dbOperator);
                    if (!result) throw new MyException("添加模块授权信息失败");

                    result = scopeFactory.Add(useScopeMappings, dbOperator);
                    if (!result) throw new MyException("添加作用域授权信息失败");

                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog<SysUser>(model, OperateType.Add);
                        foreach (var item in useRoleMappings) {
                            OperateLogServices.AddOperateLog<SysUserRolesMapping>(item, OperateType.Add);
                        }
                        foreach (var item in useScopeMappings)
                        {
                            OperateLogServices.AddOperateLog<SysUserScopeMapping>(item, OperateType.Add);
                        }
                    }
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Add(SysUser model) {
            if (model == null) throw new ArgumentNullException("model");

            ISysUser factory = SysUserFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<SysUser>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(SysUser model)
        {
            if (model == null) throw new ArgumentNullException("model");

            ISysUser factory = SysUserFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<SysUser>(model, OperateType.Update);
            }
            return result;
        }

        public static bool LoginError(string recordId) {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysUser factory = SysUserFactory.GetFactory();
            return factory.LoginError(recordId);
        }

        public static bool LoginErrorByUserId(string userAccount)
        {
            if (string.IsNullOrWhiteSpace(userAccount)) throw new ArgumentNullException("userAccount");

            ISysUser factory = SysUserFactory.GetFactory();
            return factory.LoginErrorByUserId(userAccount);
        }

        public static bool LoginSuccess(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysUser factory = SysUserFactory.GetFactory();
            return factory.LoginSuccess(recordId);
        }

        public static List<SysUser> QuerySysUserByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserByParkingId(parkingId);
        }

        public static List<SysUser> QuerySysUserAll()
        {
            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserAll();
        }

        public static List<SysUser> QuerySysUserByCompanys(List<string> companys)
        {
            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserByCompanys(companys);
 
        }

        public static List<SysUser> QuerySysUserPage(string companyId, string userAccount, int pagesize, int pageindex, out int total)
        {
            ISysUser factory = SysUserFactory.GetFactory();
            return factory.QuerySysUserPage(companyId, userAccount,pagesize,pageindex,out total);
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysUser factory = SysUserFactory.GetFactory();

            SysUser model = factory.QuerySysUserByRecordId(recordId);
            if (model == null) throw new MyException("待删除的用户不存在");

            if (model.IsDefaultUser == YesOrNo.Yes) throw new MyException("该用户为系统默认用户不能删除");

            ISysUserRolesMapping roleFactory = SysUserRolesMappingFactory.GetFactory();
            ISysUserScopeMapping scopeFactory = SysUserScopeMappingFactory.GetFactory();
          
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Delete(recordId, dbOperator);
                    if (!result) throw new MyException("添加用户信息失败");

                    roleFactory.DeleteByUserId(recordId, dbOperator);
                    scopeFactory.DeleteByUserID(recordId, dbOperator);

                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("RecordId:{0}", recordId));
                    }
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool ResetPassword(string userAccount, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userAccount)) throw new ArgumentNullException("userAccount");
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword");

            ISysUser factory = SysUserFactory.GetFactory();
            bool result = factory.ResetPassword(userAccount,newPassword);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("userAccount:{0},修改密码为：{1}", userAccount, newPassword));
            }
            return result;
        }

        internal static bool AddCompanyDefaultUser(BaseCompany company, DbOperator dbOperator)
        {
            ISysUser factory = SysUserFactory.GetFactory();

            if (factory.QuerySysUserByUserAccount(company.UserAccount) != null) throw new MyException("用户名已存在");

            SysUser user = GetDefaultUserModel(company.CPID, company.UserAccount, company.UserPassword);
            bool result = factory.Add(user, dbOperator);
            if (!result) throw new MyException("添加用户失败");

            //添加默认角色
            SysRoles role = GetDefaultSysRolesModel(company.CPID);
            ISysRoles roleFactory = SysRolesFactory.GetFactory();
            result = roleFactory.AddSysRole(role, dbOperator);
            if (!result) throw new MyException("添加单位默认角色失败");


            //添加默认角色模块授权失败
            List<SysRoleAuthorize> roleAuthorizes = GetCompanyDefaultSysRoleAuthorize(role.RecordID);
            ISysRoleAuthorize roleAuthorizesFactory = SysRoleAuthorizeFactory.GetFactory();
            result = roleAuthorizesFactory.Add(roleAuthorizes, dbOperator);
            if (!result) throw new MyException("添加单位默认角色失败");

            //添加作用域
            SysScope scope = GetSystemDefaultSysScope(company.CPID);
            ISysScope scopeFactory = SysScopeFactory.GetFactory();
            result = scopeFactory.Add(scope, dbOperator);
            if (!result) throw new MyException("添加系统默认作用域失败");


            ISysUserRolesMapping roleMappingFactory = SysUserRolesMappingFactory.GetFactory();
            List<SysUserRolesMapping> roleMapping = GetSysUserRolesMapping(user.RecordID, role.RecordID);
            result = roleMappingFactory.Add(roleMapping, dbOperator);
            if (!result) throw new MyException("添加默认用户 用户授权角色失败");


            ISysUserScopeMapping userMappingFactory = SysUserScopeMappingFactory.GetFactory();
            List<SysUserScopeMapping> scopeMapping = GetSysUserScopeMapping(user.RecordID, scope.ASID);
            result = userMappingFactory.Add(scopeMapping, dbOperator);
            if (!result) throw new MyException("添加默认用户 用户授权作用域失败");
            return result;
        }

        internal static bool AddCompanyDefaultUserCS(BaseCompany company,BaseVillage village, DbOperator dbOperator,string systemmodelpath)
        {
            ISysUser factory = SysUserFactory.GetFactory();

            if (factory.QuerySysUserByUserAccount(company.UserAccount) != null) throw new MyException("用户名已存在");

            SysUser user = GetDefaultUserModel(company.CPID, company.UserAccount, company.UserPassword);
            bool result = factory.Add(user, dbOperator);
            if (!result) throw new MyException("添加用户失败");

            //添加默认角色
            SysRoles role = GetDefaultSysRolesModel(company.CPID);
            ISysRoles roleFactory = SysRolesFactory.GetFactory();
            result = roleFactory.AddSysRole(role, dbOperator);
            if (!result) throw new MyException("添加单位默认角色失败");

            //添加收费员角色
            SysRoles role2 = GetDefaultBaRolesModel(company.CPID);
            ISysRoles roleFactory2 = SysRolesFactory.GetFactory();
            result = roleFactory2.AddSysRole(role2, dbOperator);
            if (!result) throw new MyException("添加单位收费角色失败");

             //添加默认角色模块授权失败
            List<SysRoleAuthorize> roleAuthorizes2 = GetCompanyDefaultSFYRoleAuthorizeCS(role2.RecordID, systemmodelpath);
            ISysRoleAuthorize roleAuthorizesFactory2 = SysRoleAuthorizeFactory.GetFactory();
            result = roleAuthorizesFactory2.Add(roleAuthorizes2, dbOperator);
            if (!result) throw new MyException("添加单位收费角色失败");

            //添加默认角色模块授权失败
            List<SysRoleAuthorize> roleAuthorizes = GetCompanyDefaultSysRoleAuthorizeCS(role.RecordID, systemmodelpath);
            ISysRoleAuthorize roleAuthorizesFactory = SysRoleAuthorizeFactory.GetFactory();
            result = roleAuthorizesFactory.Add(roleAuthorizes, dbOperator);
            if (!result) throw new MyException("添加单位默认角色失败");

            //添加作用域
            SysScope scope = GetSystemDefaultSysScope(company.CPID);
            
            ISysScope scopeFactory = SysScopeFactory.GetFactory();
            result = scopeFactory.Add(scope, dbOperator);
            if (!result) throw new MyException("添加系统默认作用域失败");

            SysScopeAuthorize model=new SysScopeAuthorize();
            model.ASID=scope.ASID;
            model.ASType=ASType.Village;
            model.CPID=company.CPID;
            model.DataStatus=0;
            model.HaveUpdate=3;
            model.LastUpdateTime=DateTime.Now;
            model.TagID=village.VID;
            model.ASDID=GuidGenerator.GetGuidString();
            List<SysScopeAuthorize> list=new List<SysScopeAuthorize>();
            list.Add(model);

            ISysScopeAuthorize scopeauthorize = SysScopeAuthorizeFactory.GetFactory();
            result = scopeauthorize.Add(list, dbOperator);
            if (!result) throw new MyException("添加默认用户 用户作用域失败");

            ISysUserRolesMapping roleMappingFactory = SysUserRolesMappingFactory.GetFactory();
            List<SysUserRolesMapping> roleMapping = GetSysUserRolesMapping(user.RecordID, role.RecordID);
            result = roleMappingFactory.Add(roleMapping, dbOperator);
            if (!result) throw new MyException("添加默认用户 用户授权角色失败");

            ISysUserScopeMapping userMappingFactory = SysUserScopeMappingFactory.GetFactory();
            List<SysUserScopeMapping> scopeMapping = GetSysUserScopeMapping(user.RecordID, scope.ASID);
            result = userMappingFactory.Add(scopeMapping, dbOperator);
            if (!result) throw new MyException("添加默认用户 用户授权作用域失败");
            return result;
        }

        private static List<SysUserRolesMapping> GetSysUserRolesMapping(string userId,string roleId){
            List<SysUserRolesMapping> models = new List<SysUserRolesMapping>();
            SysUserRolesMapping model = new SysUserRolesMapping();
            model.RecordID = GuidGenerator.GetGuidString();
            model.RoleID = roleId;
            model.UserRecordID = userId;
            models.Add(model);
            return models;
        }
        private static List<SysUserScopeMapping> GetSysUserScopeMapping(string userId, string scopeId)
        {
            List<SysUserScopeMapping> models = new List<SysUserScopeMapping>();
            SysUserScopeMapping model = new SysUserScopeMapping();
            model.RecordID = GuidGenerator.GetGuidString();
            model.ASID = scopeId;
            model.UserRecordID = userId;
            models.Add(model);
            return models;
        }
        private static SysUser GetDefaultUserModel(string companyId,string userAccount,string userPassword) {
            SysUser user = new SysUser();
            user.RecordID = GuidGenerator.GetGuidString();
            user.UserAccount = userAccount;
            user.UserName = "管理员";
            user.Password = MD5.Encrypt(userPassword);
            user.IsDefaultUser = YesOrNo.Yes;
            user.CPID = companyId;
            return user;
        }
        private static SysRoles GetDefaultSysRolesModel(string companyId)
        {
            SysRoles role = new SysRoles();
            role.RecordID = GuidGenerator.GetGuidString();
            role.CPID = companyId;
            role.IsDefaultRole =  YesOrNo.Yes;
            role.RoleName = "系统管理员";
            return role;
        }
        private static SysScope GetSystemDefaultSysScope(string companyId)
        {
            SysScope model = new SysScope();
            model.ASID = GuidGenerator.GetGuidString();
            model.ASName = "默认作用域";
            model.CPID = companyId;
            model.IsDefaultScope = YesOrNo.Yes;
            return model;
        }

        private static SysRoles GetDefaultBaRolesModel(string companyId)
        {
            SysRoles role = new SysRoles();
            role.RecordID = GuidGenerator.GetGuidString();
            role.CPID = companyId;
            role.IsDefaultRole = YesOrNo.No;
            role.RoleName = "收费员";
            return role;
        }

        private static List<SysRoleAuthorize> GetCompanyDefaultSysRoleAuthorize(string roleId)
        {
            List<SysRoleAuthorize> rights = new List<SysRoleAuthorize>();
            XDocument xdoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/SystemModule.xml"));
         
            foreach (XElement element in xdoc.Root.Elements())
            {
                string defaultModule = element.Attribute("SystemDefault").Value;
                if (defaultModule == "true")
                {
                    SysRoleAuthorize right = new SysRoleAuthorize();
                    string moduleId = element.Attribute("ModuleID").Value;
                    string moduleName = element.Attribute("ModuleName").Value;
                    string parentId = element.Attribute("ParentID").Value;
                    right.RecordID = GuidGenerator.GetGuidString();
                    right.RoleID = roleId;
                    right.ModuleID = moduleId;
                    right.ModuleName = moduleName;
                    right.ParentID = parentId;
                    rights.Add(right);
                }
            }
            return rights;
        }

        private static List<SysRoleAuthorize> GetCompanyDefaultSysRoleAuthorize(string roleId,string systemmodelepath)
        {
            List<SysRoleAuthorize> rights = new List<SysRoleAuthorize>();
            XDocument xdoc = XDocument.Load(systemmodelepath);

            foreach (XElement element in xdoc.Root.Elements())
            {
                string defaultModule = element.Attribute("SystemDefault").Value;

                    SysRoleAuthorize right = new SysRoleAuthorize();
                    string moduleId = element.Attribute("ModuleID").Value;
                    string moduleName = element.Attribute("ModuleName").Value;
                    string parentId = element.Attribute("ParentID").Value;
                    right.RecordID = GuidGenerator.GetGuidString();
                    right.RoleID = roleId;
                    right.ModuleID = moduleId;
                    right.ModuleName = moduleName;
                    right.ParentID = parentId;
                    rights.Add(right);
                
            }
            return rights;
        }

        private static List<SysRoleAuthorize> GetCompanyDefaultSysRoleAuthorizeCS(string roleId, string systemmodelepath)
        {
            List<SysRoleAuthorize> rights = new List<SysRoleAuthorize>();
            XDocument xdoc = XDocument.Load(systemmodelepath);

            foreach (XElement element in xdoc.Root.Elements())
            {
                string defaultModule = element.Attribute("SystemDefault").Value;

                SysRoleAuthorize right = new SysRoleAuthorize();
                string moduleId = element.Attribute("ModuleID").Value;
                string moduleName = element.Attribute("ModuleName").Value;
                string parentId = element.Attribute("ParentID").Value;
                right.RecordID = GuidGenerator.GetGuidString();
                right.RoleID = roleId;
                right.ModuleID = moduleId;
                right.ModuleName = moduleName;
                right.ParentID = parentId;
                rights.Add(right);

            }
            return rights;
        }

        private static List<SysRoleAuthorize> GetCompanyDefaultSFYRoleAuthorizeCS(string roleId, string systemmodelepath)
        {
            List<SysRoleAuthorize> rights = new List<SysRoleAuthorize>();
            XDocument xdoc = XDocument.Load(systemmodelepath);

            foreach (XElement element in xdoc.Root.Elements())
            {
                if (element.Attribute("ModuleID").Value == "PK0107" || element.Attribute("ParentID").Value == "PK0107")
                {
                    //string defaultModule = element.Attribute("SystemDefault").Value;
                    SysRoleAuthorize right = new SysRoleAuthorize();
                    string moduleId = element.Attribute("ModuleID").Value;
                    string moduleName = element.Attribute("ModuleName").Value;
                    string parentId = element.Attribute("ParentID").Value;
                    right.RecordID = GuidGenerator.GetGuidString();
                    right.RoleID = roleId;
                    right.ModuleID = moduleId;
                    right.ModuleName = moduleName;
                    right.ParentID = parentId;
                    rights.Add(right);
                }

            }
            return rights;
        }

        public static List<SysUserScopeMapping> QuerySysUserScopeMappingByUserId(string userId) { 
            ISysUserScopeMapping factory = SysUserScopeMappingFactory.GetFactory();
            return factory.QuerySysUserScopeMappingByUserId(userId);
        }
        public static List<SysUserRolesMapping> QuerySysUserRolesMappingByUserId(string userId)
        {
            ISysUserRolesMapping factory = SysUserRolesMappingFactory.GetFactory();
            return factory.GetSysUserRolesMappingByUserId(userId);
        }
    }
}
