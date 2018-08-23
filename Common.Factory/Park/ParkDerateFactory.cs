using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory.Park
{
    public class ParkDerateFactory
    {
        private static IParkDerate factory = null;
        public static IParkDerate GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkDerateDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkDerate)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
