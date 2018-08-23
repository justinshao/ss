using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysRoles
    {
        bool AddSysRole(SysRoles model);

        bool AddSysRole(SysRoles model, DbOperator dbOperator);

        bool UpdateRole(SysRoles model);

        bool DeleteRoleByRecordId(string recordId);

        SysRoles QuerySysRolesByRecordId(string recordId);

        List<SysRoles> QuerySysRolesByCompanyId(string companyId);

        List<SysRoles> QuerySysRolesByUserId(string userId);
    }
}
