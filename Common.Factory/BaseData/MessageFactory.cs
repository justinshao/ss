using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class MessageFactory
    {
        private static IMessage factory = null;
        public static IMessage GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.MessageDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IMessage)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
