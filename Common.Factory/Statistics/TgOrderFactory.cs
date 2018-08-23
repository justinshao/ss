using Common.Entities;
using Common.IRepository.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Statistics
{
    public class TgOrderFactory
    {
        private static ITgOrder factory = null;
        public static ITgOrder GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.TgOrderDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ITgOrder)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
