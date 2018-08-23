using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class CityFactory
    {
        private static ICity factory = null;
        public static ICity GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.CityDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ICity)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
