using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory.BWY
{
    public class BWYGateMappingFactory
    {
        private static IBWYGateMapping factory = null;
        public static IBWYGateMapping GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.BWYGateMappingDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IBWYGateMapping)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
