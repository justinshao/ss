using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkIORecordFactory
    {
        private static IParkIORecord factory = null;
        public static IParkIORecord GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkIORecordDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkIORecord)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
