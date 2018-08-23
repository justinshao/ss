using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.AliPay;
using Common.Factory.AliPay;
using Common.IRepository.AliPay;
using Common.Utilities;
using Common.Entities;

namespace Common.Services.AliPay
{

    public class AliPayApiConfigServices
    {
        static List<AliPayApiConfig> staticApiConfigs = new List<AliPayApiConfig>();
        static AliPayApiConfigServices()
        {
            IAliPayApiConfig factory = AliPayApiConfigFactory.GetFactory();
            staticApiConfigs = factory.QueryAll();
        }
        public static bool AddOrUpdate(AliPayApiConfig model)
        {
            if (string.IsNullOrWhiteSpace(model.CompanyID)) throw new MyException("获取单位编号失败");

            IAliPayApiConfig factory = AliPayApiConfigFactory.GetFactory();
            if (factory.QueryByCompanyID(model.CompanyID) != null)
            {
                bool result = factory.Update(model);
                if (result)
                {
                    RefreshCache();
                }
                return result;
            }
            model.RecordId = GuidGenerator.GetGuidString();
            bool addResult = factory.Add(model);
            if (addResult)
            {
                RefreshCache();
            }
            return addResult;
        }
        private static void RefreshCache()
        {
            IAliPayApiConfig factory = AliPayApiConfigFactory.GetFactory();
            staticApiConfigs = factory.QueryAll();
        }
        /// <summary>
        /// 当前单位的支付宝配置
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static AliPayApiConfig QueryByCompanyID(string companyId)
        {
            IAliPayApiConfig factory = AliPayApiConfigFactory.GetFactory();
            return factory.QueryByCompanyID(companyId);
        }
        /// <summary>
        /// 当前单位或上级的支付宝配置
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static AliPayApiConfig QueryAliPayConfig(string companyId)
        {
            try
            {
                AliPayApiConfig config = staticApiConfigs.FirstOrDefault(p => p.CompanyID == companyId);
                if (config != null && config.Status)
                {
                    return config;
                }
                bool result = true;
                while (result)
                {
                    if (string.IsNullOrWhiteSpace(companyId))
                    {
                        result = false;
                        break;
                    }
                    BaseCompany company = CompanyServices.QueryCompanyByRecordId(companyId);
                    if (company == null)
                    {
                        result = false;
                        break;
                    }
                    config = staticApiConfigs.FirstOrDefault(p => p.CompanyID == companyId);
                    if (config == null)
                    {
                        companyId = company.MasterID;
                        continue;
                    }
                    if (!config.Status && config.SupportSuperiorPay)
                    {
                        companyId = company.MasterID;
                        continue;
                    }
                    break;
                }
                return config;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("WXApiConfigServices", "当前单位或上级的支付宝配置失败", ex);
                return null;
            }
        }
    }
}
