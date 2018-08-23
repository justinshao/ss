using Common.Factory;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.Park
{
    public class ParkDeviceDetectionServices
    {
        public static bool Update(string deviceID,string parkingid,bool connectState)
        {
            if (deviceID.IsEmpty()) throw new ArgumentNullException("model");

            IParkDeviceDetection factory = ParkDeviceDetectionFactory.GetFactory();
            return factory.Update(deviceID, parkingid, connectState);
        }
    }
}
