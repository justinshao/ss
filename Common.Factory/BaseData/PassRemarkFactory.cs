using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class PassRemarkFactory
    {
        private static IPassRemark factory = null;
        public static IPassRemark GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.PassRemarkDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IPassRemark)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
