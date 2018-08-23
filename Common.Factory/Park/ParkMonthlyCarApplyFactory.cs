using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory
{
    public class ParkMonthlyCarApplyFactory
    {
        private static IParkMonthlyCarApply factory = null;
        public static IParkMonthlyCarApply GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkMonthlyCarApplyDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkMonthlyCarApply)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
