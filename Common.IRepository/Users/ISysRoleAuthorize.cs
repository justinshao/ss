using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysRoleAuthorize
    {
        List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleId(string roleId);

        List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleIds(List<string> roleIds);

        bool Add(List<SysRoleAuthorize> models,DbOperator dbOperator);

        bool DeleteByRoleId(string roleId, DbOperator dbOperator);

        bool  CheckUserAuthorize(string userid,string modelid);
    }
}
