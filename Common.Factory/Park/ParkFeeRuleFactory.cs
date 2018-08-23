using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkFeeRuleFactory
    {
        private static IParkFeeRule factory = null;
        public static IParkFeeRule GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkFeeRuleDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkFeeRule)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
