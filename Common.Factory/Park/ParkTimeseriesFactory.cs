using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkTimeseriesFactory
    {
        private static IParkTimeseries factory = null;
        public static IParkTimeseries GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkTimeseriesDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkTimeseries)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
