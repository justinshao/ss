using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using Common.Entities.Statistics;
namespace Common.IRepository.Statistics
{
    public interface IStatistics_Gather
    {
        bool IsExistsGather(string parkingid, DateTime gathertime);
        bool DeleteGather(string parkingid, DateTime gathertime,DbOperator dboperator);
        bool Insert(Statistics_Gather gather, DbOperator dboperator);
        bool DeleteGatherTime(string parkingid, DateTime start, DateTime end);
    }
}
