using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IOpinionFeedback
    {
        bool Create(WX_OpinionFeedback model);

        List<WX_OpinionFeedback> QueryPage(string companyId,DateTime start,DateTime end, int pageIndex, int pageSize, out int recordTotalCount);
    }
}
