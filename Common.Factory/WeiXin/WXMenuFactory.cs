using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXMenuFactory
    {
        private static IWXMenu factory = null;
        public static IWXMenu GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXMenuDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXMenu)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
