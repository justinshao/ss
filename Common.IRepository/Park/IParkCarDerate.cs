using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IParkCarDerate
    {
        bool Add(ParkCarDerate model);

        bool Add(ParkCarDerate model,DbOperator dbOperator);

        bool Update(ParkCarDerate model);

        bool QRCodeDiscount(string carDerateid,string parkingId,string ioRecordId,string plateNumber);

        bool UpdateStatus(string carDerateID, CarDerateStatus status);

        List<ParkCarDerate> QueryByDerateId(string derateId);

        List<ParkCarDerate> QueryByPlateNumber(string plateNumber);

        List<ParkCarDerate> QueryByCardNo(string cardNo);

        List<ParkCarDerate> QueryByIORecordID(string ioRecordId);

        bool DeleteByExpiryTime(string derateId, DateTime expiredTime);

        bool DeleteNotUseByDerateQRCodeID(string derateQRCodeId, DbOperator dbOperator);

        ParkCarDerate GetNotUseParkCarDerate(string derateId, DateTime lessThanTime);

        bool UpdateCarderateCreateTime(string carDerateID);

        ParkCarDerate QueryByCarDerateID(string carDerateId);

        ParkCarDerate QueryBySellerIdAndIORecordId(string sellerId,string ioReocdId);

        decimal GetTotalFreeMoney(string sellerId);

        Dictionary<string, int> QuerySettlementdCarDerate(List<string> derateQRCodeIds);

        List<ParkCarDerate> ParkCarDeratePage(string sellerId, string plateNumber, int? state, int? derateType, DateTime? start, DateTime? end, int pageSize, int pageIndex, ref int totalCount);
    }
}
