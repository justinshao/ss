using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkDeviceFactory
    {
        private static IParkDevice factory = null;
        public static IParkDevice GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkDeviceDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkDevice)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
