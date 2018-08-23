using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface IParkCarlineInfo
    {
        bool Add(ParkCarlineInfo model);

        bool Update(ParkCarlineInfo model);

        bool Delete(string gateId);
        ParkCarlineInfo QueryByGate(string gateId);
         
        /// <summary>
        /// 获取第一个排队有效的车辆信息
        /// </summary>
        /// <returns></returns>
        ParkCarlineInfo QueryMinTargetTimeInfo(string parkingID);
    }
}
