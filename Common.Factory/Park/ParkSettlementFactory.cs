using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Park;
using Common.Entities;
namespace Common.Factory.Park
{
    public class ParkSettlementFactory
    {
        private static IParkSettlement factory = null;
        public static IParkSettlement GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Park.ParkSettlementDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkSettlement)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
