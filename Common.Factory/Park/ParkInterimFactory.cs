using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkInterimFactory
    {
        private static IParkInterim factory = null;
        public static IParkInterim GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkInterimDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkInterim)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
