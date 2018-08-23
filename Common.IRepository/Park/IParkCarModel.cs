using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkCarModel
    {
        bool Add(ParkCarModel model);

        bool Add(List<ParkCarModel> models, DbOperator dbOperator);

        bool Update(ParkCarModel model);

        bool Delete(string recordId);

        ParkCarModel QueryByRecordId(string recordId);

        List<ParkCarModel> QueryByParkingId(string parkingId);

        List<ParkCarModel> QueryByParkingIds(List<string> parkingIds);

        ParkCarModel GetDefaultCarModel(string parkingid, out string errorMsg);

    }
}
