using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ParkingFactory
    {
        private static IParking factory = null;
        public static IParking GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkingDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParking)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
