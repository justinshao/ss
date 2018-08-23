using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkArea
    {
        bool Add(ParkArea model);

        bool Update(ParkArea model);

        bool UpdateCarbitNum(string recordId, int carNum);

        bool Delete(string recordId);

        ParkArea QueryByRecordId(string recordId);

        ParkArea GetParkAreaByParkBoxRecordId(string recordId);

        ParkArea GetParkAreaByParkGateRecordId(string recordId);

        List<ParkArea> GetParkAreaByParkBoxIps(List<string> ips);

        List<ParkArea> GetParkAreaByParkingId(string parkingId);

        List<ParkArea> GetParkAreaByMasterId(string masterId);

        List<ParkArea> GetParkAreaByParkingIds(List<string> parkingIds);

        List<ParkArea> GetTopParkAreaByParkingId(string parkingId);

        int GetCarBitNumByParkingId(string parkingId);

    }
}
