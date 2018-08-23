using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;

namespace Common.Services.WeiXin
{
    public class WX_LockCarServices
    {
        public static WX_LockCar GetLockCar(string parkingID, string plateNumber, out string ErrorMessage)
        {
            IWX_LockCar factory = WX_LockCarFactory.GetFactory();
            return factory.GetLockCar(parkingID, plateNumber, out ErrorMessage);
        }
        public static bool ReleaseLockCar(string parkingID, string plateNumber, out string ErrorMessage)
        {
            IWX_LockCar factory = WX_LockCarFactory.GetFactory();
            return factory.ReleaseLockCar(parkingID, plateNumber, out ErrorMessage);
        }
    }
}
