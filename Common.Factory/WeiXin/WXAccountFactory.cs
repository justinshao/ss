using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;
namespace Common.Factory.WeiXin
{
    public class WXAccountFactory
    {
        private static IWXAccount factory = null;
        public static IWXAccount GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXAccountDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXAccount)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
