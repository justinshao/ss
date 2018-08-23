using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysUserScopeMappingFactory
    {
        private static ISysUserScopeMapping factory = null;
        public static ISysUserScopeMapping GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysUserScopeMappingDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysUserScopeMapping)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
