using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.Utilities;

namespace Common.Services
{
    public class SysRolesServies
    {
        public static bool AddSysRole(SysRoles model) {
            if (model == null) throw new ArgumentNullException("model");

            model.RecordID = GuidGenerator.GetGuid().ToString();
            ISysRoles factory = SysRolesFactory.GetFactory();
            bool result = factory.AddSysRole(model);
            if (result) {
                OperateLogServices.AddOperateLog<SysRoles>(model, OperateType.Add);
            }
            return result;
        }

        public static bool UpdateRole(SysRoles model)
        {
            if (model == null) throw new ArgumentNullException("model");

            ISysRoles factory = SysRolesFactory.GetFactory();
            bool result = factory.UpdateRole(model);
            if (result)
            {
                model = factory.QuerySysRolesByRecordId(model.RecordID);
                if (model != null) {
                    OperateLogServices.AddOperateLog<SysRoles>(model, OperateType.Update);
                }
            }
            return result;
        }

        public static bool DeleteRoleByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysRoles factory = SysRolesFactory.GetFactory();
            SysRoles model = factory.QuerySysRolesByRecordId(recordId);
            if (model != null && model.IsDefaultRole == YesOrNo.Yes) 
                throw new MyException("系统默认角色不能删除");

            bool result = factory.DeleteRoleByRecordId(recordId);
            if (result){
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("RecordId:{0}",recordId));
            }
            return result;
        }

        public static SysRoles QuerySysRolesByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysRoles factory = SysRolesFactory.GetFactory();
            return factory.QuerySysRolesByRecordId(recordId);
        }

        public static List<SysRoles> QuerySysRolesByCompanyId(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId)) throw new ArgumentNullException("companyId");

            ISysRoles factory = SysRolesFactory.GetFactory();
            return factory.QuerySysRolesByCompanyId(companyId);
        }
        public static List<SysRoles> QuerySysRolesByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");

            ISysRoles factory = SysRolesFactory.GetFactory();
            return factory.QuerySysRolesByUserId(userId);
        }
    }
}
