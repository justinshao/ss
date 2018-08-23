using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWX_LockCar
    {
        WX_LockCar GetLockCar(string parkingID, string plateNumber, out string ErrorMessage);

        bool ReleaseLockCar(string parkingID, string plateNumber, out string ErrorMessage);
    }
}
