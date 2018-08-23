using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkGrantFactory
    {
        private static IParkGrant factory = null;
        public static IParkGrant GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkGrantDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkGrant)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
