using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkCarType
    {
        bool Add(ParkCarType model);

        bool Add(List<ParkCarType> models, DbOperator dbOperator);

        bool Update(ParkCarType model);

        bool Delete(string recordId);

        ParkCarType QueryParkCarTypeByRecordId(string recordId);

        List<ParkCarType> QueryParkCarTypeByParkingId(string parkingId);

        List<ParkCarType> QueryParkCarTypeByParkingIds(List<string> parkingIds);

        List<ParkCarType> QueryCardTypesByBaseCardType(string parkingId, BaseCarType baseCarType);

        ParkCarType QueryCarTypesByCarTypeName(string parkingId, string carTypeName);

        List<ParkCarType> QueryParkCarTypeByRecordIds(List<string> recordIds);
        bool QueryGrant(string recordId);
    }
}
