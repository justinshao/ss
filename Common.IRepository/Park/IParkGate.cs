using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkGate
    {
        bool Add(ParkGate model);

        bool Update(ParkGate model);

        bool Delete(string recordId);

        ParkGate QueryByRecordId(string recordId);

        List<ParkGate> QueryByParkBoxRecordId(string recordId);

        List<ParkGate> QueryByParkingId(string parkingId);

        List<ParkGate> QueryByParkAreaRecordIds(List<string> areaIds);

        List<ParkGate> QueryByParkingIdAndIoState(string parkingId, IoState ioState);

        string QueryParkingIdByGateId(string gateId);

        List<RemotelyOpenGateView> QueryRemotelyOpenGate(List<string> parkingIds, string areaId, string boxId, int pageIndex, int pageSize, out int recordTotalCount);

        List<ParkGateIOTime> QueryGateIOTime(string gateId);
        bool AddIOTime(ParkGateIOTime model, DbOperator dbOperator);
        bool UpdateIOTime(ParkGateIOTime model, DbOperator dbOperator);
        /// <summary>
        /// 删除特殊规则
        /// </summary>
        /// <param name="ruleid"></param>
        /// <returns></returns>
        bool DelIOTime(string ruleid);
    }
}
