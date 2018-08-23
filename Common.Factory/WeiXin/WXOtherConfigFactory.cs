using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXOtherConfigFactory
    {
        private static IWXOtherConfig factory = null;
        public static IWXOtherConfig GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXOtherConfigDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXOtherConfig)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
