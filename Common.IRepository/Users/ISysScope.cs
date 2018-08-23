using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysScope
    {
        bool Add(SysScope model);

        bool Add(SysScope model, DbOperator dbOperator);

        bool Update(SysScope model);

        bool DeleteByRecordId(string recordId);

        SysScope QuerySysScopeByRecordId(string recordId);

        List<SysScope> QuerySysScopeByCompanyId(string companyId);

        List<SysScope> QuerySysScopeByUserId(string userId);
    }
}
