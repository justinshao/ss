using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class ExceptionsFactory
    {
        private static IExceptions factory = null;
        public static IExceptions GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ExceptionsDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IExceptions)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
