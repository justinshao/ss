using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Park
{
    public interface IParkBlacklist
    {
        ParkBlacklist Query(string recordId);

        bool Add(ParkBlacklist model);

        bool Update(ParkBlacklist model);

        ParkBlacklist Query(string parkingid, string plateNo);

        bool Delete(string recordId);

        List<ParkBlacklist> QueryByParkingId(string parkingId);

        List<ParkBlacklist> QueryPage(string parkingId, string plateNo, int pagesize, int pageindex, out int total);

        ParkBlacklist GetBlacklist(string parkingID, string plateNumber, out string ErrorMessage);
    }
}
