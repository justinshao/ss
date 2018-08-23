using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;

namespace Common.Services.WeiXin
{
    public class WXOpinionFeedbackServices
    {
        public static bool Create(WX_OpinionFeedback model)
        {
            IOpinionFeedback factory = WXOpinionFeedbackFactory.GetFactory();
            return factory.Create(model);
        }
        public static List<WX_OpinionFeedback> QueryPage(string companyId,DateTime start,DateTime end, int pagesize, int pageindex, out int total)
        {
            IOpinionFeedback factory = WXOpinionFeedbackFactory.GetFactory();
            return factory.QueryPage(companyId,start, end, pagesize, pageindex, out total);
        }
    }
}
