using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXMenuAccessRecordFactory
    {
        private static IWXMenuAccessRecord factory = null;
        public static IWXMenuAccessRecord GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXMenuAccessRecordDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXMenuAccessRecord)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
