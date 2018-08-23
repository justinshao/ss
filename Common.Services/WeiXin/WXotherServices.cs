using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.IRepository.WeiXin;
using Common.Factory.WeiXin;

namespace Common.Services.WeiXin
{
    public class WXotherServices
    {
        public static WX_Info GetWXInfo(string openid)
        {
            IWXohters factory = WXothers.GetFactory();
            return factory.QueryByOpenId(openid);
        }

        public static string GETURL(string plateno) {
            IWXohters factory = WXothers.GetFactory();
            return factory.GetUrl(plateno);
        }


    }
}
