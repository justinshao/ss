using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WX_LockCarFactory
    {
        private static IWX_LockCar factory = null;
        public static IWX_LockCar GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WX_LockCarDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWX_LockCar)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
