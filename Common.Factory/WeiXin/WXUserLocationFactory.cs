using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository.WeiXin;

namespace Common.Factory.WeiXin
{
    public class WXUserLocationFactory
    {
        private static IWXUserLocation factory = null;
        public static IWXUserLocation GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXUserLocationDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXUserLocation)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
