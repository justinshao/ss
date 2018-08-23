using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.IRepository.WeiXin;
using Common.Factory.WeiXin;
using Common.Entities.Enum;
using Common.Entities;

namespace Common.Services.WeiXin
{
    public class WXOtherConfigServices
    {
        public static bool AddOrUpdate(WX_OtherConfig model) {
            
            IWXOtherConfig factory = WXOtherConfigFactory.GetFactory();
            WX_OtherConfig dbConfig = factory.QueryByConfigType(model.CompanyID,model.ConfigType);
            if (dbConfig != null) {
                bool result = factory.Update(model);
                if (result)
                {
                    OperateLogServices.AddOperateLog<WX_OtherConfig>(model, OperateType.Update);
                }
                return result;
            }
            bool addResult = factory.Create(model);
            if (addResult)
            {
                OperateLogServices.AddOperateLog<WX_OtherConfig>(model, OperateType.Add);
            }
            return addResult;
        }
       public static WX_OtherConfig QueryByConfigType(string companyId,ConfigType type)
       {
           IWXOtherConfig factory = WXOtherConfigFactory.GetFactory();
           return factory.QueryByConfigType(companyId,type);
       }

       public static List<WX_OtherConfig> QueryAll(string companyId)
       {
           IWXOtherConfig factory = WXOtherConfigFactory.GetFactory();
           return factory.QueryAll(companyId);
       }
       public static string GetConfigValue(string companyId,ConfigType type)
       {
           IWXOtherConfig factory = WXOtherConfigFactory.GetFactory();
           WX_OtherConfig config = factory.QueryByConfigType(companyId,type);
           if (config == null || string.IsNullOrWhiteSpace(config.ConfigValue)) return string.Empty;

           return config.ConfigValue;
       }
       public static int GetTempParkingWeiXinPayTimeOut(string companyId) {
           IWXOtherConfig factory = WXOtherConfigFactory.GetFactory();
           WX_OtherConfig config = factory.QueryByConfigType(companyId,ConfigType.TempParkingWeiXinPayTimeOut);
           if (config == null || string.IsNullOrWhiteSpace(config.ConfigValue)) {
               return int.Parse(ConfigType.TempParkingWeiXinPayTimeOut.GetEnumDefaultValue());
           }
           int time;
           if (int.TryParse(config.ConfigValue, out time)) {
               return time;
           }
           return int.Parse(ConfigType.TempParkingWeiXinPayTimeOut.GetEnumDefaultValue());
       }
    }
}
