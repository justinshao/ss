using Common.Entities;
using Common.IRepository.PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.PG
{
    public class PGAreaFactory
    {
        private static IPGArea factory = null;
        public static IPGArea GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.PGAreaDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IPGArea)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
