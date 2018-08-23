
using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class EmployeePlateFactory
    {
        private static IEmployeePlate factory = null;
        public static IEmployeePlate GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.EmployeePlateDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IEmployeePlate)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
