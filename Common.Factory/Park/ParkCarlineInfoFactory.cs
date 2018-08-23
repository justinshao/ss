using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkCarlineInfoFactory
    {
        private static IParkCarlineInfo factory = null;
        public static IParkCarlineInfo GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCarlineInfoDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCarlineInfo)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
