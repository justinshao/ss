using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkVisitorFactory
    {
        private static IParkVisitor factory = null;
        public static IParkVisitor GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkVisitorDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkVisitor)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
