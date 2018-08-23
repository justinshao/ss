using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Statistics;
using Common.Entities;
namespace Common.Factory.Statistics
{
    public class Statistics_GatherLongTimeFactory
    {
        private static IStatistics_GatherLongTime factory = null;
        public static IStatistics_GatherLongTime GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.Statistics_GatherLongTimeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IStatistics_GatherLongTime)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
