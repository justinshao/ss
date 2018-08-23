using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;

namespace Common.Factory.Logs
{
    public class LoginLogFactory
    {
        private static ILoginLog factory = null;
        public static ILoginLog GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.LoginLogDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ILoginLog)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
