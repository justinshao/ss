using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using Common.Entities.Statistics;
namespace Common.IRepository.Statistics
{
    public interface IStatistics_ChangeShift
    {
        bool Delete(string changeshiftid, DbOperator db);
        bool Insert(Statistics_ChangeShift changeshift, DbOperator db);
    }
}
