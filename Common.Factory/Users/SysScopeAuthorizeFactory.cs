using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysScopeAuthorizeFactory
    {
        private static ISysScopeAuthorize factory = null;
        public static ISysScopeAuthorize GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysScopeAuthorizeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysScopeAuthorize)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
