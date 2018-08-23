using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;

namespace Common.Factory
{
    public class ParkDerateQRcodeFactory
    {
        private static IParkDerateQRcode factory = null;
        public static IParkDerateQRcode GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkDerateQRcodeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkDerateQRcode)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
