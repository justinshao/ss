using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Statistics;
using Common.Entities;

namespace Common.Factory.Statistics
{
   public class StatisticstgPerson
   {
       private static ItgPerson factory = null;
       public static ItgPerson GetFactory() {
           if (factory == null) {
               Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.Statistics.tgPersonDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
               factory = (ItgPerson)Activator.CreateInstance(type);
           }
           return factory;
       }
   }
}
