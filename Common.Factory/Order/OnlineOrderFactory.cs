using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Order;
using Common.Entities;

namespace Common.Factory.Order
{
    public class OnlineOrderFactory
    {
        private static IOnlineOrder factory = null;
        public static IOnlineOrder GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.OnlineOrderDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IOnlineOrder)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
