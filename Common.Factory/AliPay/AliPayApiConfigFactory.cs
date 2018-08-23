using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.AliPay;
using Common.Entities;

namespace Common.Factory.AliPay
{
    public class AliPayApiConfigFactory
    {
        private static IAliPayApiConfig factory = null;
        public static IAliPayApiConfig GetFactory()
        {
            if (factory == null)
            {
                Type type = Type.GetType("Common." + SystemDefaultConfig.DatabaseProvider + "Repository.AliPay.AliPayApiConfigDAL,Common." + SystemDefaultConfig.DatabaseProvider + "Repository", true);
                factory = (IAliPayApiConfig)Activator.CreateInstance(type);
            }
            return factory;
        }
    }
}
