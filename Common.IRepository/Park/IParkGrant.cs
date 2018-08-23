using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities.Condition;
using Common.Entities.Parking;

namespace Common.IRepository.Park
{
    public interface IParkGrant
    {
        ParkGrant GetCardgrant(string plateNumberID, out string ErrorMessage);

        List<ParkGrant> GetParkGrantByPlateNumberID(string parkingID, string plateNumberID, out string ErrorMessage);

        List<ParkGrant> GetCardgrantsByLot(string parkingID, string likelot, out string ErrorMessage);

        bool Add(ParkGrant model);

        bool Add(ParkGrant model, DbOperator dbOperator);

        bool Add(List<ParkGrant> models);

        bool Add(List<ParkGrant> models, DbOperator dbOperator);

        bool Update(ParkGrant model);

        bool Update(ParkGrant model, DbOperator dbOperator);

        ParkGrant QueryByPlateNumber(string plateNumber);

        ParkGrant QueryByGrantId(string grantId);

        bool Update(string grantId, ParkGrantState state, DbOperator dbOperator, DateTime NewEndDate);

        bool Delete(string grantId);

        bool Delete(string cardId, string parkingId);

        bool DeleteByCardId(string cardId);

        bool DeleteByCardId(string cardId, DbOperator dbOperator);

        bool Delete(List<string> cardIds, DbOperator dbOperator);

        bool Delete(string grantId, DbOperator dbOperator);

        List<ParkGrant> QueryNormalParkGrant(string parkingId);

        List<ParkGrant> QueryByParkingId(string parkingId);

        List<ParkGrant> QueryByParkingIds(List<string> parkingIds);

        List<ParkGrant> QueryByCardIds(List<string> cardIds);

        List<ParkGrant> QueryByCardIdAndParkingIds(string cardId, List<string> parkingIds);

        ParkGrant QueryByCardIdAndParkingId(string cardId, string parkingId);

        List<ParkGrant> QueryByCardId(string cardId);

        List<ParkGrant> QueryByParkingAndPlateId(string parkingId, string plateId);

        List<ParkGrant> QueryByPlateId(string plateId);

        ParkGrant QueryByParkingAndPlateNumber(string parkingId, string plateNumber);

        List<ParkGrant> QueryByParkingAndLotAndCarType(string parkingId, string lots, BaseCarType carType, string excludeGrantId);

        List<ParkGrantView> QueryPage(ParkGrantCondition condition, int pagesize, int pageindex, out int total);

        List<ParkGrantView> QueryPage1(ParkGrantCondition condition);

        bool Renewals(string grantId, DateTime beginDate, DateTime endDate, DbOperator dbOperator);

        bool RestoreUse(string grantId, DateTime beginDate, DateTime NewRestoreDate, DbOperator dbOperator);

        List<ParkGrant> Query(List<string> parkingIds, string plateNumber, BaseCarType carType);

        List<ParkGrant> QueryHasLotByParkingId(string parkingId, BaseCarType carType);

        List<ParkGrant> GetCardgrantByParkingID(string grantId, out string errorMsg);

        List<ParkGrant> GetParkGrantByPlateNo(string plateNo);

        /// <summary>
        /// 退款操作
        /// </summary>
        /// <param name="GID"></param>
        /// <param name="Amout"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        bool RefundCardAmout(List<ParkGrant> grantlist, DateTime EndTime, ParkOrder model);

        List<ParkCarBitGroup> QueryCarBitGroupByParkingId(string parkingid);

        bool AddParkCarBitGroup(ParkCarBitGroup model, DbOperator dbOperator);

        bool UpdateParkGrantPKLot(string newPkLot, string oldPkLot, string parkingId, DbOperator dbOperator);

        ParkCarBitGroup GetParkCarBitGroup(string parkingId, string carBitName);

        bool UpdateParkCarBitGroup(string recordId, string newPkLot, int carBitNum, DbOperator dbOperator);

        ParkCarBitGroup GetParkCarBitGroupByRecordID(string recordId);

    }
}
