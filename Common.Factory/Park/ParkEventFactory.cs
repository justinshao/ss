using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkEventFactory
    {
        private static IParkEvent factory = null;
        public static IParkEvent GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkEventDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkEvent)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
