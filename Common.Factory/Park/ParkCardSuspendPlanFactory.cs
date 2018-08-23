using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;

namespace Common.Factory
{
    public class ParkCardSuspendPlanFactory
    {
        private static IParkCardSuspendPlan factory = null;
        public static IParkCardSuspendPlan GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCardSuspendPlanDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCardSuspendPlan)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
