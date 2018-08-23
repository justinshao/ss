using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;
using Common.Entities;
using System.Web.Caching;
using System.Web;

namespace Common.Services.WeiXin
{
    public class WXApiConfigServices
    {
        static List<WX_ApiConfig> staticConfigs = new List<WX_ApiConfig>();
        static WXApiConfigServices() {
            IWXApiConfig factory = WXApiConfigFactory.GetFactory();
            staticConfigs = factory.QueryAll();
        }
        public static bool AddOrUpdate(WX_ApiConfig model)
        {
            if (string.IsNullOrWhiteSpace(model.CompanyID)) throw new MyException("获取单位编号失败");

            IWXApiConfig factory = WXApiConfigFactory.GetFactory();

            if (factory.QueryByCompanyID(model.CompanyID)==null)
            {
                WX_ApiConfig oldApiConfig = factory.QueryByToKen(model.Token);
                if (oldApiConfig != null) throw new MyException("Token重复");
                bool result = factory.Create(model);
                if (result)
                {
                    RefreshCache();
                    OperateLogServices.AddOperateLog<WX_ApiConfig>(model, OperateType.Add);
                }
                return result;
            }
            else {
                WX_ApiConfig oldApiConfig = factory.QueryByToKen(model.Token);
                if (oldApiConfig != null && oldApiConfig.CompanyID!=model.CompanyID) throw new MyException("Token重复");

                bool result = factory.Update(model);
                if (result)
                {
                    RefreshCache();
                    OperateLogServices.AddOperateLog<WX_ApiConfig>(model, OperateType.Update);
                }
                return result;
            }
        }

        public static bool UpdatePayConfig(WX_ApiConfig model)
        {
            IWXApiConfig factory = WXApiConfigFactory.GetFactory();
            bool result = factory.UpdatePayConfig(model);
            if (result)
            {
                RefreshCache();
                OperateLogServices.AddOperateLog<WX_ApiConfig>(model, OperateType.Update);
            }
            return result;
        }
        /// <summary>
        /// 获取当前单位的配置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static WX_ApiConfig QueryByCompanyId(string companyId)
        {
            IWXApiConfig factory = WXApiConfigFactory.GetFactory();
            return factory.QueryByCompanyID(companyId);
        }
        private static void RefreshCache()
        {
            IWXApiConfig factory = WXApiConfigFactory.GetFactory();
            staticConfigs = factory.QueryAll();
        }
        /// <summary>
        /// 获取当前单位或上级单位的配置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static WX_ApiConfig QueryWXApiConfig(string companyId)
        {
            try
            {
                WX_ApiConfig config = staticConfigs.FirstOrDefault(p => p.CompanyID == companyId);
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
                    config = staticConfigs.FirstOrDefault(p => p.CompanyID == companyId);
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
            catch (Exception ex) {
                TxtLogServices.WriteTxtLogEx("WXApiConfigServices", "获取当前单位或上级单位的配置信息", ex);
                return null;
            }
        }
        public static WX_ApiConfig QueryByToKen(string token){
           return staticConfigs.FirstOrDefault(p => p.Token == token);
        }
    }
}
