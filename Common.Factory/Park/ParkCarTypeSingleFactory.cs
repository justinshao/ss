using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory.Park
{
    public class ParkCarTypeSingleFactory
    {
        private static IParkCarTypeSingle factory = null;
        public static IParkCarTypeSingle GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkCarTypeSingleDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkCarTypeSingle)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
