using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;

namespace Common.Factory
{
    public class CompanyFactory
    {
        private static ICompany factory = null;
        public static ICompany GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.CompanyDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (ICompany)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
