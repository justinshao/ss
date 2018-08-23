using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Park
{
    public interface IParkReserveBit
    {
        /// <summary>
        /// 获取可用的车位预约信息
        /// </summary>
        /// <param name="parkID"></param>
        /// <param name="plateNumber"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        ParkReserveBit GetCanUseParkReserveBit(string parkID, string plateNumber, out string errorMsg);

        /// <summary>
        /// 修改预约状态
        /// </summary>
        /// <param name="ReserveBitID"></param>
        /// <param name="status"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool ModifyReserveBit(string ReserveBitID, int status, out string errorMsg);
    }
}
