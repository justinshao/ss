using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory.BaseData
{
    public class BaseEmployeeFactory
    {
        private static IBaseEmployee factory = null;
        public static IBaseEmployee GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.BaseEmployeeDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IBaseEmployee)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
