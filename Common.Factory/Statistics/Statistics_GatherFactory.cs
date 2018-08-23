using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Statistics;
using Common.Entities;
namespace Common.Factory.Statistics
{
    public class Statistics_GatherFactory
    {
        private static IStatistics_Gather factory = null;
        public static IStatistics_Gather GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.Statistics_GatherDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IStatistics_Gather)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
