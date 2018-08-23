using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.IRepository.WeiXin;
using Common.Factory.WeiXin;

namespace Common.Services.WeiXin
{
    public class WXUserLocationServices
    {
        public static bool AddOrUpdate(WX_UserLocation model) {
            if (model == null) throw new ArgumentNullException("model");

            IWXUserLocation factory = WXUserLocationFactory.GetFactory();
            WX_UserLocation old = factory.QueryByOpenId(model.CompanyID,model.OpenId);
            if (old != null) {
                return factory.Update(model);
            }
            return factory.Create(model);
        }


        public static WX_UserLocation QueryByOpenId(string companyId,string openId) {
            if (string.IsNullOrWhiteSpace(openId)) throw new ArgumentNullException("openId");

            IWXUserLocation factory = WXUserLocationFactory.GetFactory();
            return factory.QueryByOpenId(companyId,openId);
        }
        public static WX_UserLocation QueryByOpenId(string openId) {
            if (string.IsNullOrWhiteSpace(openId)) throw new ArgumentNullException("openId");

            IWXUserLocation factory = WXUserLocationFactory.GetFactory();
            return factory.QueryByOpenId(openId);
        }
    }
}
