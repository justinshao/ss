using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkGateFactory
    {
        private static IParkGate factory = null;
        public static IParkGate GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkGateDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkGate)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
