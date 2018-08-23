using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkAreaFactory
    {
        private static IParkArea factory = null;
        public static IParkArea GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkAreaDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkArea)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
