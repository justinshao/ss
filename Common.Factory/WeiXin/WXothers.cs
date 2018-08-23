using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Entities;
using Common.IRepository.WeiXin;

namespace Common.Factory.WeiXin
{
   public class WXothers
    {
       private static IWXohters factory = null;
       public static IWXohters GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXothersDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXohters)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
