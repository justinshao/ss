using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysRolesFactory
    {
        private static ISysRoles factory = null;
        public static ISysRoles GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysRolesDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysRoles)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
