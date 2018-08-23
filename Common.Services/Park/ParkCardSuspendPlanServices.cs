using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Factory;
using Common.IRepository;

namespace Common.Services.Park
{
    public class ParkCardSuspendPlanServices
    {
        public static  List<ParkCardSuspendPlan> QueryByGrantIds(List<string> grantIds)
        {
            if (grantIds.Count == 0) return new List<ParkCardSuspendPlan>();

            IParkCardSuspendPlan factory = ParkCardSuspendPlanFactory.GetFactory();
            return factory.QueryByGrantIds(grantIds);
        }
    }
}
