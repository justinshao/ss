using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXKeywordFactory
    {
        private static IWXKeyword factory = null;
        public static IWXKeyword GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXKeywordDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXKeyword)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
