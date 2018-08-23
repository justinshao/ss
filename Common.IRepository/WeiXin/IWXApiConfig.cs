using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXApiConfig
    {
        bool Create(WX_ApiConfig model);

        bool Update(WX_ApiConfig model);

        bool UpdatePayConfig(WX_ApiConfig model);

        WX_ApiConfig QueryByCompanyID(string companyId);

        WX_ApiConfig QueryByToKen(string token);

        List<WX_ApiConfig> QueryAll();
    }
}
