using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory.Park
{
    public class ParkCarDerateFactory
    {
        private static IParkCarDerate factory = null;
        public static IParkCarDerate GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCarDerateDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCarDerate)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
