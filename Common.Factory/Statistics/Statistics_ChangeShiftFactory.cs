using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Statistics;
using Common.Entities;
namespace Common.Factory.Statistics
{
    public class Statistics_ChangeShiftFactory
    {
        private static IStatistics_ChangeShift factory = null;
        public static IStatistics_ChangeShift GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.Statistics_ChangeShiftDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IStatistics_ChangeShift)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
