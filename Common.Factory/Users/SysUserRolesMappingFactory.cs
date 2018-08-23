using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;

namespace Common.Factory
{
    public class SysUserRolesMappingFactory
    {
        private static ISysUserRolesMapping factory = null;
        public static ISysUserRolesMapping GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysUserRolesMappingDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysUserRolesMapping)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
