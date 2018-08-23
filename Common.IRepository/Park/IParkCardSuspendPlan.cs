using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkCardSuspendPlan
    {
        bool Add(ParkCardSuspendPlan model, DbOperator dbOperator);

        bool Update(DateTime start, DateTime? end, string grantId, DbOperator dbOperator);

        bool Delete(string grantId);

        bool Delete(string grantId, DbOperator dbOperator);

        ParkCardSuspendPlan QueryByGrantId(string grantId);

        List<ParkCardSuspendPlan> QueryByGrantIds(List<string> grantIds);
    }
}
