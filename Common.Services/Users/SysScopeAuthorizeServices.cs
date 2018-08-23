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
    public class SysScopeAuthorizeServices
    {
        public static List<SysScopeAuthorize> QuerySysScopeAuthorizeByScopeId(string scopeId) {
            if (string.IsNullOrWhiteSpace(scopeId)) throw new ArgumentNullException("scopeId");

            ISysScopeAuthorize factory = SysScopeAuthorizeFactory.GetFactory();
            return factory.QuerySysScopeAuthorizeByScopeId(scopeId);
        }

        public static bool Add(List<SysScopeAuthorize> models) {
            if (models == null || models.Count == 0) throw new ArgumentNullException("models");

            ISysScopeAuthorize factory = SysScopeAuthorizeFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.DeleteByScopeId(models.First().ASID, dbOperator);
                    result = factory.Add(models, dbOperator);
                    if (!result) throw new MyException("保存失败");
                    dbOperator.CommitTransaction();
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
