using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IParkDeviceDetection
    {
        bool Add(ParkDeviceDetection model, DbOperator dbOperator);

        bool Update(string deviceID,string parkingid,bool connectionState);

        bool Delete(string deviceId, DbOperator dbOperator);

        ParkDeviceDetection QueryByDeviceID(string deviceId);
    }
}
