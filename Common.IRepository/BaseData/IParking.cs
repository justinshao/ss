using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParking
    {
        bool Add(BaseParkinfo model);

        bool Add(BaseParkinfo model, DbOperator dbOperator);

        bool Update(BaseParkinfo model);

        bool Delete(string recordId);

        BaseParkinfo QueryParkingByRecordId(string recordId);

        List<BaseParkinfo> QueryParkingByRecordIds(List<string> recordIds);

        List<BaseParkinfo> QueryParkingByVillageId(string villageId);

        List<BaseParkinfo> QueryParkingByCompanyIds(List<string> companyIds);

        List<BaseParkinfo> QueryParkingByVillageIds(List<string> villageIds);

        List<BaseParkinfo> QueryAllParking();

        BaseParkinfo QueryParkingByParkingID(string ParkingID);

        BaseParkinfo QueryParkingByParkingNo(string parkingNo);

        List<BaseParkinfo> QueryPage(string villageId, int pageIndex, int pageSize, out int totalCount);

        List<BaseParkinfo> QueryParkingAll();

        int UpdateCarBit(string PKID);

        DateTime GetServerTime(out string error);

        List<BaseParkinfo> GetParkingBySupportAutoRefund();

        bool UpdateParkSettleConfig(BaseParkinfo model);
    }
}
