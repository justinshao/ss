using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Park;
using Common.Entities;
namespace Common.Factory.Park
{
    public class ParkSettlementDetailsFactory
    {
        private static IParkSettlementDetails factory = null;
        public static IParkSettlementDetails GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkSettlementDetails,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkSettlementDetails)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
