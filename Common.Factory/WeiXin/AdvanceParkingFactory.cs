using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository.WeiXin;

namespace Common.Factory.WeiXin
{
    public class AdvanceParkingFactory
    {
        private static IAdvanceParking factory = null;
        public static IAdvanceParking GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.AdvanceParkingDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IAdvanceParking)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
