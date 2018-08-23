using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkCarTypeFactory
    {
        private static IParkCarType factory = null;
        public static IParkCarType GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCarTypeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCarType)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
