using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXApiConfigFactory
    {
        private static IWXApiConfig factory = null;
        public static IWXApiConfig GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXApiConfigDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXApiConfig)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
