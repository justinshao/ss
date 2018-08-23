using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.Utilities;

namespace Common.Services
{
    public class ParkAreaServices
    {
        public static bool Add(ParkArea model) {
            if (model == null) throw new ArgumentNullException("model");

            model.AreaID = GuidGenerator.GetGuid().ToString();
            IParkArea factory = ParkAreaFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkArea>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefualt(ParkArea model)
        {
            if (model == null) throw new ArgumentNullException("model");
            IParkArea factory = ParkAreaFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkArea>(model, OperateType.Add);
            }
            return result;
        }
        public static bool Update(ParkArea model) {
            if (model == null) throw new ArgumentNullException("model");

            IParkArea factory = ParkAreaFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkArea>(model, OperateType.Update);
            }
            return result;
        }

        public static bool UpdateCarbitNum(string recordId, int carNum)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            bool result = factory.UpdateCarbitNum(recordId, carNum);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("recordId:{0},CarbitNum:{1}", recordId,carNum));
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            List<ParkBox> boxs = ParkBoxServices.QueryByParkAreaId(recordId);
            if (boxs.Count != 0) throw new MyException("请先删除改区域下所有岗亭");

            IParkArea factory = ParkAreaFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static ParkArea QueryByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }

        public static ParkArea GetParkAreaByParkBoxRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByParkBoxRecordId(recordId);
        }

        public static ParkArea GetParkAreaByParkGateRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByParkGateRecordId(recordId);
        }

        public static List<ParkArea> GetParkAreaByParkBoxIps(List<string> ips)
        {
            if (ips.Count == 0) throw new ArgumentNullException("ips");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByParkBoxIps(ips);
        }

        public static List<ParkArea> GetParkAreaByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByParkingId(parkingId);
        }

        public static List<ParkArea> GetParkAreaByMasterId(string masterId)
        {
            if (string.IsNullOrWhiteSpace(masterId)) throw new ArgumentNullException("masterId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByMasterId(masterId);
        }

        public static List<ParkArea> GetParkAreaByParkingIds(List<string> parkingIds)
        {
            if (parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetParkAreaByParkingIds(parkingIds);
        }

        public static List<ParkArea> GetTopParkAreaByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkArea factory = ParkAreaFactory.GetFactory();
            return factory.GetTopParkAreaByParkingId(parkingId);
        }

    }
}
