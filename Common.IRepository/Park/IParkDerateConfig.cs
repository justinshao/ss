using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;

namespace Common.IRepository
{
    public interface IParkDerateConfig
    {
        bool Add(ParkDerateConfig model);

        bool Update(ParkDerateConfig model);

        bool Delete(string recordId);

        ParkDerateConfig QueryByRecordId(string recordId);

        List<ParkDerateConfig> QueryByParkingId(string parkingId);

        ParkDerateConfig QueryByParkingIdAndAmount(string parkingId,decimal amount);
    }
}
