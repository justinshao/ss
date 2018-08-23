using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXUserLocation
    {
        bool Create(WX_UserLocation model);

        bool Update(WX_UserLocation model);

        WX_UserLocation QueryByOpenId(string companyId, string openId);

        WX_UserLocation QueryByOpenId(string openId);
    }
}
