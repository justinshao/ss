using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkOrderFactory
    {
        private static IParkOrder factory = null;
        public static IParkOrder GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkOrderDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkOrder)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
