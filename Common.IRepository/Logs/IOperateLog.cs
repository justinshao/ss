using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface IOperateLog
    {
        void Add(OperateLog model);

        Paging<OperateLog> QueryPage(OperateLogCondition condition, int pagesize, int pageindex, out int total);
    }
}
