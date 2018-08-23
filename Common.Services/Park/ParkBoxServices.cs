using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utilities;
using Common.Factory;
using Common.IRepository;

namespace Common.Services
{
    public class ParkBoxServices
    {
        public static bool Add(ParkBox model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.BoxID = GuidGenerator.GetGuid().ToString();
            IParkBox factory = ParkBoxFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkBox>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefault(ParkBox model)
        {
            if (model == null) throw new ArgumentNullException("model");

           
            IParkBox factory = ParkBoxFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkBox>(model, OperateType.Add);
            }
            return result;
        }

       public static bool Update(ParkBox model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkBox factory = ParkBoxFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkBox>(model, OperateType.Update);
            }
            return result;
        }

       public static bool Delete(string recordId)
       {
           if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
           List<ParkGate> gates = ParkGateServices.QueryByParkBoxRecordId(recordId);
           if (gates.Count != 0) throw new MyException("请先删除该岗亭下面的所有通道");

           IParkBox factory = ParkBoxFactory.GetFactory();
           bool result = factory.Delete(recordId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
           }
           return result;
       }

       public static ParkBox QueryByRecordId(string recordId)
       {
           if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

           IParkBox factory = ParkBoxFactory.GetFactory();
           return factory.QueryByRecordId(recordId);
       }

       public static List<ParkBox> QueryByComputerIp(string ip)
       {
           if (string.IsNullOrWhiteSpace(ip)) throw new ArgumentNullException("ip");

           IParkBox factory = ParkBoxFactory.GetFactory();
           return factory.QueryByComputerIps(ip);
       }

       public static List<ParkBox> QueryByParkAreaId(string areaId)
       {
           if (string.IsNullOrWhiteSpace(areaId)) throw new ArgumentNullException("areaId");

           IParkBox factory = ParkBoxFactory.GetFactory();
           return factory.QueryByParkAreaId(areaId);
       }

       public static List<ParkBox> QueryByParkingID(string parkingid)
       {
           if (string.IsNullOrWhiteSpace(parkingid)) throw new ArgumentNullException("parkingid");
           IParkBox factory = ParkBoxFactory.GetFactory();
           return factory.QueryByParkingID(parkingid);
       }

       public static List<ParkBox> QueryByParkAreaIds(List<string> areaIds)
       {
           if (areaIds.Count == 0) throw new ArgumentNullException("areaIds");

           IParkBox factory = ParkBoxFactory.GetFactory();
           return factory.QueryByParkAreaIds(areaIds);
       }
    }
}
