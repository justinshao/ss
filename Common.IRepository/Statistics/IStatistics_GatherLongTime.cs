using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Statistics
{
    public interface IStatistics_GatherLongTime
    {
        bool IsExists(string parkingid,DateTime gathertime);
        bool Insert(string strSql);
    }
}
