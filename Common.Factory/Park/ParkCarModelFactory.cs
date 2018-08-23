using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkCarModelFactory
    {
        private static IParkCarModel factory = null;
        public static IParkCarModel GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCarModelDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCarModel)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
