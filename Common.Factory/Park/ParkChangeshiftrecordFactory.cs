using Common.Entities;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Factory.Park
{
    public class ParkChangeshiftrecordFactory
    {
        private static IParkChangeshiftrecord factory = null;
        public static IParkChangeshiftrecord GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.ParkChangeshiftrecordDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IParkChangeshiftrecord)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
