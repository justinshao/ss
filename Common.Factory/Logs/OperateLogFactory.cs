using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;

namespace Common.Factory.Logs
{
    public class OperateLogFactory
    {
        private static IOperateLog factory = null;
        public static IOperateLog GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.OperateLogDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IOperateLog)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
