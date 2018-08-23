using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;
using Common.Entities;

namespace Common.Services.WeiXin
{
    public class WXInteractionInfoServices
    {
        public static bool Add(WX_InteractionInfo model) {
            if (model == null) throw new ArgumentNullException("model");

            IWXInteractionInfo factory = WXInteractionInfoFactory.GetFactory();
            return factory.Add(model);
           
        }
        public static int QueryMaxIdByOpenId(string openId) {
            if (string.IsNullOrWhiteSpace(openId)) throw new ArgumentNullException("openId");

            IWXInteractionInfo factory = WXInteractionInfoFactory.GetFactory();
            return factory.QueryMaxIdByOpenId(openId);
        }
    }
}
