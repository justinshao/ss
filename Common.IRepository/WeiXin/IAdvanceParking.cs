using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IAdvanceParking
    {
        bool Add(AdvanceParking model);

        AdvanceParking QueryByOrderId(decimal orderId);

        bool UpdatePrepayIdById(string prepayId, decimal orderId);

        bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime);

        bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime, DbOperator dbOperator);

        List<AdvanceParking> QueryPage(string companyId,string plateNo, DateTime? start, DateTime? end, int pageIndex, int pageSize, out int recordTotalCount);
    }
}
