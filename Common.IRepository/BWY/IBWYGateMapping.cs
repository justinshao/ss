using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BWY;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IBWYGateMapping
    {
        bool Add(BWYGateMapping model);

        bool Add(BWYGateMapping model, DbOperator dbOperator);

        bool Update(BWYGateMapping model);

        bool Update(BWYGateMapping model, DbOperator dbOperator);

        bool UpdateParkNo(string recordId, string parkNo);

        bool Delete(string recordId);

        List<BWYGateMapping> QueryAll();

        BWYGateMapping QueryByRecordId(string recordId);

        List<BWYGateMapping> QueryByParkingID(string parkingId);

        List<BWYGateMapping> QueryByDataSource(int dataSurce);

        BWYGateMapping QueryByGateID(int dataSurce, string gateId);
        BWYGateMapping QueryByGateID(int dataSuorce, string parkNo, string gateID);

        List<BWYGateMapping> QueryPage(string parkName, string gateName, int? dataSource, int pageIndex, int pageSize, out int recordTotalCount);

    }
}
