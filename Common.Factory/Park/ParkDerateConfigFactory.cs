using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;

namespace Common.Factory.Park
{
    public class ParkDerateConfigFactory
    {
        private static IParkDerateConfig factory = null;
        public static IParkDerateConfig GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkDerateConfigDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkDerateConfig)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
