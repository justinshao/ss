using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkReserveBitFactory
    {
        private static IParkReserveBit factory = null;
        public static IParkReserveBit GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Park.ParkReserveBitDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkReserveBit)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
