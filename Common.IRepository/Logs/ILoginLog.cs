using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface ILoginLog
    {
        void Add(LoginLog model);

        void UpdateLogoutTime(string loginAccount);

        Paging<LoginLog> QueryPage(LoginLogCondition condition, int pagesize, int pageindex, out int total);

    }
}
