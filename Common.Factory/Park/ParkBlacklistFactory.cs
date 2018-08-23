using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkBlacklistFactory
    {
        private static IParkBlacklist factory = null;
        public static IParkBlacklist GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkBlacklistDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkBlacklist)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
