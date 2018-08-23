using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Factory.WeiXin
{
    public class WXOpinionFeedbackFactory
    {
        private static IOpinionFeedback factory = null;
        public static IOpinionFeedback GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.WeiXin.WXOpinionFeedbackDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IOpinionFeedback)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
