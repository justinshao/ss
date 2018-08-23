using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class SysUserFactory
    {
        private static ISysUser factory = null;
        public static ISysUser GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.SysUserDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ISysUser)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
