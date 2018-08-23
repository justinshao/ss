using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysScopeFactory
    {
        private static ISysScope factory = null;
        public static ISysScope GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysScopeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysScope)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
