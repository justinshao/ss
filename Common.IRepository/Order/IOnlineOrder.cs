using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Order;
using Common.DataAccess;
using Common.Entities.Enum;
using Common.Entities.Condition;

namespace Common.IRepository.Order
{
    public interface IOnlineOrder
    {
        bool Create(OnlineOrder model);

        OnlineOrder QueryByOrderId(decimal orderId);

        bool UpdatePrepayIdById(string prepayId, decimal orderId);

        bool UpdatePrepayIdById(string prepayId, string mWebUrl, decimal orderId);

        bool UpdateSFMCode(string code, decimal orderId);

        bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime);

        bool PaySuccess(decimal orderId, string serialNumber, string payAccount, DateTime payTime);

        bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime, DbOperator dbOperator);

        bool UpdateOrderSerialNumber(decimal orderId, string serialNumber);

        bool UpdateOrderStatusByOrderId(decimal orderId, OnlineOrderStatus status);

        bool RefundFail(decimal orderId);

        bool SyncPayResultSuccess(decimal orderId, DbOperator dbOperator);

        bool UpdatePayDetailID(decimal orderId, string payDetailId);

        bool RefundSuccess(decimal orderId, string refundOrderId);

        bool RefundSuccess(decimal orderId, string refundOrderId, DbOperator dbOperator);

        bool SyncPayResultFail(decimal orderId, string remark);

        bool UpdatePayAccount(decimal orderId, string payAccount, string payer);

        bool UpdateSyncResultTimes(decimal orderId, string payDetailId);

        List<OnlineOrder> QueryBySyncPayResultFail();

        List<OnlineOrder> QueryWaitRefund(List<string> parkingIds, DateTime minRealPayTime);

        List<OnlineOrder> QueryPage(OnlineOrderCondition condition, int pageIndex, int pageSize, out int recordTotalCount);

        List<OnlineOrder> QueryAll(OnlineOrderCondition condition);

        List<OnlineOrder> ExportQueryPage(OnlineOrderCondition condition);

        string QueryLastPaymentPlateNumber(PaymentChannel channel,string openId);

        string QueryLastPaymentPlateNumber(string accountId);

        bool UpdatePayAccount(decimal orderId, string payAccount);

    }
}
