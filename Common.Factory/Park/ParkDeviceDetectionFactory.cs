using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;
using Common.IRepository.Park;

namespace Common.Factory
{
    public class ParkDeviceDetectionFactory
    {
        private static IParkDeviceDetection factory = null;
        public static IParkDeviceDetection GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkDeviceDetectionDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkDeviceDetection)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
