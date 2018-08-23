using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory.Park
{
    public class ParkLpPlanFactory
    {
        private static IParkLpPlan factory = null;
        public static IParkLpPlan GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkLpPlanDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkLpPlan)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
