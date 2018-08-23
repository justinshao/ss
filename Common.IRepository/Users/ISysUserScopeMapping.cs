using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;

namespace Common.IRepository
{
    public interface ISysUserScopeMapping
    {
        bool DeleteByUserID(string userId, DbOperator dbOperator);

        List<SysUserScopeMapping> QuerySysUserScopeMappingByUserId(string userId);

        bool Add(List<SysUserScopeMapping> models, DbOperator dbOperator);
    }
}
