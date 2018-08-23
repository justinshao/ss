using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class VillageFactory
    {
        private static IVillage factory = null;
        public static IVillage GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.VillageDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IVillage)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
