using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkSellerFactory
    {
        private static IParkSeller factory = null;
        public static IParkSeller GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkSellerDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkSeller)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
