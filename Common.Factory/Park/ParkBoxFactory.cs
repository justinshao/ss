using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkBoxFactory
    {
        private static IParkBox factory = null;
        public static IParkBox GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkBoxDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkBox)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
