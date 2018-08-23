using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysScopeAuthorize
    {
        List<SysScopeAuthorize> QuerySysScopeAuthorizeByScopeId(string scopeId);

        bool Add(List<SysScopeAuthorize> models, DbOperator dbOperator);

        bool DeleteByScopeId(string scopeId, DbOperator dbOperator);

    }
}
