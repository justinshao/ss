using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Entities.Enum;

namespace Common.IRepository.WeiXin
{
    public interface IWXOtherConfig
    {
        bool Create(WX_OtherConfig model);

        bool Update(WX_OtherConfig model);

        WX_OtherConfig QueryByConfigType(string companyId, ConfigType type);

        List<WX_OtherConfig> QueryAll(string companyId);
    }
}
