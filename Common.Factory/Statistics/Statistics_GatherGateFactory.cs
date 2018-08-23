using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Statistics;
using Common.Entities;
namespace Common.Factory.Statistics
{
    public class Statistics_GatherGateFactory
    {
        private static IStatistics_GatherGate factory = null;
        public static IStatistics_GatherGate GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.Statistics_GatherGateDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IStatistics_GatherGate)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
