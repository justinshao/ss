using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository.Park;
namespace Common.Factory.Park
{
    public class ParkSettlementApprovalFlowFactory
    {
        private static IParkSettlementApprovalFlow factory = null;
        public static IParkSettlementApprovalFlow GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Park.ParkSettlementApprovalFlowDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkSettlementApprovalFlow)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
