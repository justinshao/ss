using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;

namespace Common.IRepository.Park
{
    public interface IParkOrder
    {
        List<ParkOrder> GetOrderByTimeseriesID(string TimeseriesID, out string ErrorMsg);
        List<ParkOrder> GetOrderByIORecordID(string IORecordID, out string ErrorMsg);
        ParkOrder GetCashMoneyCountByPlateNumber(string parkingID, OrderType orderType, string plateNumber, DateTime startTime, DateTime endtime, out string errorMsg);
        ParkOrder AddOrder(ParkOrder mode, out string errorMsg);
        bool ModifyOrderStatusAndAmount(string recordID, decimal payAmount, decimal unPayamount, int status, int payway,out string errorMsg);
        bool ModifyOrderStatusAndAmount(string recordID, decimal amount, decimal payAmount, decimal unPayamount, int status, decimal Discountamount, string Carderateid, int payWay, out string ErrorMessage);
        bool ModifyOrderStatus(string recordID, int status, out string errorMsg);
        List<ParkOrder> GetOrderByIORecordIDByStatus(string iorecordID, int status, out string errorMsg);
        List<ParkOrder> GetOrderByTimeseriesIDByStatus(string timeseriesID, int status, out string errorMsg);
        ParkOrder GetTimeseriesOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, out string ErrorMessage);
        ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, OrderSource orderSource, out string ErrorMessage);
        ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, out string ErrorMessage);
        List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, int status, out string ErrorMessage);
        List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, out string ErrorMessage);
        ParkOrder Add(ParkOrder model, DbOperator dbOperator);
        ParkOrder Add(ParkOrder model);
        decimal QueryMonthExpiredNotPayAmount(DateTime start, DateTime end, string parkingId, List<string> plateNumbers);

        List<ParkOrder> QueryByIORecordIds(List<string> recordIds);

        bool UpdateOrderStatusByIORecordIds(List<string> recordIds, int status, DbOperator dbOperator);
        List<ParkOrder> GetOrdersByParkingID(string parkingid, DateTime starttime, DateTime endtime);
        List<ParkOrder> GetOrdersByGateID(string parkingid, string gateid, DateTime starttime, DateTime endtime);
        List<ParkOrder> GetOrdersByBoxID(string parkingid, string boxid, DateTime starttime, DateTime endtime);
        List<ParkOrder> GetOrderByTagID(string tagid, out string ErrorMsg);
        bool ModifyOrderTagIDAndOrderType(string recordID, string tagid, int orderType, out string ErrorMessage);
        List<ParkOrder> GetDifferenceOrder(DateTime startTime, DateTime endtime, string userid, out string ErrorMessage);
        bool AuditingDiffOrder(string recordID,decimal Amount, decimal PayAmount, out string ErrorMessage);
        List<ParkOrder> GetOrderByCarDerateID(DateTime startTime, string carDerateID, out string ErrorMsg);
        List<ParkOrder> GetSellerRechargeOrder(string sellerId, int orderSource, DateTime? start, DateTime? end, int pageIndex, int pageSize, out int total);
        List<ParkOrder> GetOrdersByPKID(string PKID,DateTime startTime, DateTime endTime);
    }
}
