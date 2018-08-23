using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository.WeiXin;

namespace Common.Factory.WeiXin
{
    public class WXInteractionInfoFactory
    {
        private static IWXInteractionInfo factory = null;
        public static IWXInteractionInfo GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXInteractionInfoDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXInteractionInfo)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
