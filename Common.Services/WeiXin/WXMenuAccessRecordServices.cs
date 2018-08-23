using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;

namespace Common.Services.WeiXin
{
    public class WXMenuAccessRecordServices
    {
        public static bool Create(WX_MenuAccessRecord model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IWXMenuAccessRecord factory = WXMenuAccessRecordFactory.GetFactory();
            return factory.Create(model);
        }
    }
}
