using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkDevice
    {
        bool Add(ParkDevice model, DbOperator dbOperator);

        bool Update(ParkDevice model, DbOperator dbOperator);
        bool UpdateParam(string deviceId, string deviceNo, DbOperator dbOperator);
        bool Delete(string recordId, DbOperator dbOperator);
        bool DeleParam(string deviceId, DbOperator dbOperator);
        List<ParkDevice> QueryParkDeviceByGateRecordId(string gateRecordId);

        ParkDevice QueryParkDeviceByRecordId(string recordId);

        List<ParkDevice> QueryParkDeviceByIPAddress(string ipaddress);

        List<ParkDevice> QueryParkDeviceDetectionByParkingID(string parkingid);
        List<ParkDevice> QueryParkDeviceAll();

        /// <summary>
        /// 根据设备ID获取设备参数信息
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        ParkDeviceParam QueryParkDeviceParamByDID(string DeviceID);
        /// <summary>
        /// 根据设备ID获取设备参数信息
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        ParkDeviceParam QueryParkDeviceParamByDevID(int DevID);

        /// <summary>
        /// 增加设备参数
        /// </summary>
        /// <param name="model"></param>
        bool AddParam(ParkDeviceParam model, DbOperator dbOperator);

        /// <summary>
        /// 修改设备参数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        bool UpdateParam(ParkDeviceParam model, DbOperator dbOperator);
    }
}
