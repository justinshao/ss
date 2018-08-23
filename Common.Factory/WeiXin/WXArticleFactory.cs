using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXArticleFactory
    {
        private static IWXArticle factory = null;
        public static IWXArticle GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXArticleDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IWXArticle)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
