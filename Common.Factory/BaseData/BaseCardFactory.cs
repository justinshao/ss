using Common.Entities;
using Common.IRepository.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.BaseData
{
    public class BaseCardFactory
    {
        private static IBaseCard factory = null;
        public static IBaseCard GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.BaseData.BaseCardDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IBaseCard)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
