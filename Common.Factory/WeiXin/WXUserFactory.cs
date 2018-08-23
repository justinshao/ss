using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;
namespace Common.Factory.WeiXin
{
    public class WXUserFactory
    {
        private static IWXUser factory = null;
        public static IWXUser GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXUserDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXUser)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
