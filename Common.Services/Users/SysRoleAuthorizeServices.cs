using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.DataAccess;

namespace Common.Services
{
    public class SysRoleAuthorizeServices
    {
        public static List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleId(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId)) throw new ArgumentNullException("roleId");

            ISysRoleAuthorize factory = SysRoleAuthorizeFactory.GetFactory();
            return factory.QuerySysRoleAuthorizeByRoleId(roleId);
        }

        public static List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleIds(List<string> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0) throw new ArgumentNullException("roleIds");

            ISysRoleAuthorize factory = SysRoleAuthorizeFactory.GetFactory();
            return factory.QuerySysRoleAuthorizeByRoleIds(roleIds);
        }

        public static bool CheckUserAuthorize(string userid, string modelid)
        {
            if (userid.IsEmpty()) throw new ArgumentNullException("userid");

            ISysRoleAuthorize factory = SysRoleAuthorizeFactory.GetFactory();
            return factory.CheckUserAuthorize(userid,modelid);
        }

        public static bool Add(List<SysRoleAuthorize> models)
        {
            if (models == null || models.Count == 0) throw new ArgumentNullException("models");

            ISysRoleAuthorize factory = SysRoleAuthorizeFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.DeleteByRoleId(models.First().RoleID, dbOperator);
                     result = factory.Add(models, dbOperator);
                    if (!result) throw new MyException("保存失败");
                    dbOperator.CommitTransaction();
                    OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("RoleId:{0}", models.First().RoleID));
                    OperateLogServices.AddOperateLog(OperateType.Add, string.Format("RecordIds:{0}", string.Join(",", models.Select(p => p.RecordID))));
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
