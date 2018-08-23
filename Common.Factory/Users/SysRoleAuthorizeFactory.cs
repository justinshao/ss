using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysRoleAuthorizeFactory
    {
        private static ISysRoleAuthorize factory = null;
        public static ISysRoleAuthorize GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysRoleAuthorizeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysRoleAuthorize)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
