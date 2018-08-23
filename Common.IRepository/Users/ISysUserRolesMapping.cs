using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysUserRolesMapping
    {
        List<SysUserRolesMapping> GetSysUserRolesMappingByUserId(string userId);

        bool Add(List<SysUserRolesMapping> models, DbOperator dbOperator);

        bool DeleteByUserId(string userId, DbOperator dbOperator);
    }
}
