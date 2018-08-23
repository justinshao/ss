using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using Common.Entities.Statistics;
namespace Common.IRepository.Statistics
{
    public interface IStatistics_GatherGate
    {
        bool IsExists(string gateid, DateTime gathertime);
        bool Delete(string parkingid, DateTime gathertime,DbOperator db);
        bool Insert(Statistics_GatherGate gategather, DbOperator db);
    }
}
