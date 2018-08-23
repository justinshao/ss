using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository.Statistics;
namespace Common.Factory.Statistics
{
    public class StatisticsFactory
    {
        private static IStatistics factory = null;
        public static IStatistics GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.StatisticsDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IStatistics)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
