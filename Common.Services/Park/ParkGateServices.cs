using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utilities;
using Common.Factory;
using Common.IRepository;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.Services
{
    public class ParkGateServices
    {
        public static bool Add(ParkGate model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.GateID = GuidGenerator.GetGuid().ToString();
            IParkGate factory = ParkGateFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkGate>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefault(ParkGate model)
        {
            if (model == null) throw new ArgumentNullException("model");

           
            IParkGate factory = ParkGateFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkGate>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(ParkGate model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkGate factory = ParkGateFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkGate>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            List<ParkDevice> devices = ParkDeviceServices.QueryParkDeviceByGateRecordId(recordId);
            if (devices.Count != 0) throw new MyException("请先删除该通道下所有设备信息");

            IParkGate factory = ParkGateFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }
        public static ParkGate QueryByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }

        public static List<ParkGate> QueryByParkBoxRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryByParkBoxRecordId(recordId);
        }

        public static List<ParkGate> QueryByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }

        public static List<ParkGate> QueryByParkAreaRecordIds(List<string> areaIds)
        {
            if (areaIds.Count == 0) throw new ArgumentNullException("areaIds");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryByParkAreaRecordIds(areaIds);
        }

        public static List<ParkGate> QueryByParkingIdAndIoState(string parkingId, IoState ioState)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryByParkingIdAndIoState(parkingId, ioState);
        }
        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <param name="villageId"></param>
        /// <param name="parkingId"></param>
        /// <param name="areaId"></param>
        /// <param name="boxId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordTotalCount"></param>
        /// <returns></returns>
        public static List<RemotelyOpenGateView> QueryRemotelyOpenGate(List<string> parkingIds, string areaId, string boxId, int pageIndex, int pageSize, out int recordTotalCount)
        {
            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryRemotelyOpenGate(parkingIds, areaId, boxId, pageIndex, pageSize, out recordTotalCount);
        }

        /// <summary>
        /// 根据通道获取进出规则
        /// </summary>
        /// <param name="gateId"></param>
        /// <returns></returns>
        public static List<ParkGateIOTime> QueryGateIOTime(string gateId)
        {
            if (string.IsNullOrWhiteSpace(gateId)) throw new ArgumentNullException("gateId");

            IParkGate factory = ParkGateFactory.GetFactory();
            return factory.QueryGateIOTime(gateId);
        }

        /// <summary>
        /// 增加通道规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddIOTime(List<ParkGateIOTime> list)
        {
            if (list == null || list.Count==0) throw new ArgumentNullException("list");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    foreach (var model in list)
                    {
                        model.RecordID = GuidGenerator.GetGuid().ToString();
                        IParkGate factory = ParkGateFactory.GetFactory();
                        bool result = factory.AddIOTime(model, dbOperator);
                        if (!result)throw new MyException("增加通道规则失败");
                    }
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            OperateLogServices.AddOperateLog<List<ParkGateIOTime>>(list, OperateType.Add);
            return true;
        }

        /// <summary>
        /// 修改通道规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateIOTime(List<ParkGateIOTime> list)
        {
            if (list == null || list.Count == 0) throw new ArgumentNullException("list");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    foreach (var model in list)
                    {
                        IParkGate factory = ParkGateFactory.GetFactory();
                        bool result = factory.UpdateIOTime(model, dbOperator);
                        if (!result) throw new MyException("修改通道规则失败");
                    }
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            OperateLogServices.AddOperateLog<List<ParkGateIOTime>>(list, OperateType.Update);
            return true;

        }
        public static bool AddOrUpdateIOTime(List<ParkGateIOTime> list){
            if (list == null || list.Count==0) throw new ArgumentNullException("list");

             IParkGate factory = ParkGateFactory.GetFactory();
             List<ParkGateIOTime> models = factory.QueryGateIOTime(list.First().GateID);
             List<ParkGateIOTime> updateModels = new List<ParkGateIOTime>();
             List<ParkGateIOTime> addModels = new List<ParkGateIOTime>();
             using (DbOperator dbOperator = ConnectionManager.CreateConnection())
             {
                 
                  try
                  {
                      dbOperator.BeginTransaction();
                      foreach (var item in list)
                      {
                          ParkGateIOTime model = models.FirstOrDefault(p => p.RuleType == 0 && p.WeekIndex == item.WeekIndex);
                          if (model != null)
                          {
                              model.StartTime = item.StartTime;
                              model.EndTime = item.EndTime;
                              model.InOutState = item.InOutState;
                              bool result = factory.UpdateIOTime(model, dbOperator);
                              if (!result) throw new MyException("修改星期规则失败");
                              updateModels.Add(model);
                          }
                          else {
                              item.RecordID = GuidGenerator.GetGuid().ToString();
                              bool result = factory.AddIOTime(item, dbOperator);
                              if (!result) throw new MyException("添加星期规则失败");
                              addModels.Add(item);
                          }
                      }
                      dbOperator.CommitTransaction();
                  }
                  catch {
                      dbOperator.RollbackTransaction();
                      throw;
                  }
                 
             }
             if(updateModels.Count > 0) {
                 OperateLogServices.AddOperateLog<List<ParkGateIOTime>>(updateModels, OperateType.Update);
             }
             if (addModels.Count > 0)
             {
                 OperateLogServices.AddOperateLog<List<ParkGateIOTime>>(addModels, OperateType.Add);
             }
             return true;
        }
        /// <summary>
        /// 删除特殊规则
        /// </summary>
        /// <param name="ruleid"></param>
        /// <returns></returns>
        public static bool DelIOTime(string ruleid)
        {
            if (string.IsNullOrWhiteSpace(ruleid)) throw new ArgumentNullException("ruleid");
          
            IParkGate factory = ParkGateFactory.GetFactory();
            bool result = factory.DelIOTime(ruleid);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", ruleid));
            }
            return result;
        }
    }
}
