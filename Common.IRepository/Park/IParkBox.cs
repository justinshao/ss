using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface IParkBox
    {
        bool Add(ParkBox model);

        bool Update(ParkBox model);

        bool Delete(string recordId);

        ParkBox QueryByRecordId(string recordId);

        List<ParkBox> QueryByComputerIps(string ip);

        List<ParkBox> QueryByParkAreaId(string areaId);

        List<ParkBox> QueryByParkAreaIds(List<string> areaIds);

        List<ParkBox> QueryByParkingID(string parkingid);
    }
}
