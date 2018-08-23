using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utilities;
using Common.Factory.Park;
using Common.IRepository.Park;
using Common.DataAccess;

namespace Common.Services.Park
{
    public class ParkCarDerateServices
    {
        public static bool Add(ParkCarDerate model) {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.CarDerateID))
            {
                model.CarDerateID = GuidGenerator.GetGuidString();
            }
            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.Add(model);
        }
        public static bool Add(List<ParkCarDerate> models)
        {
            if (models == null || models.Count==0) throw new ArgumentNullException("models");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    foreach (var item in models) {
                       bool result = factory.Add(item, dbOperator);
                       if (!result) throw new MyException("保存失败");
                    }
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        public static bool Update(ParkCarDerate model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.CarDerateID)) throw new ArgumentNullException("CarDerateID");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.Update(model);
        }

        public static bool UpdateStatus(string carDerateID, CarDerateStatus status)
        {
            if (string.IsNullOrWhiteSpace(carDerateID)) throw new ArgumentNullException("CarDerateID");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.UpdateStatus(carDerateID,status);
        }

        public static List<ParkCarDerate> QueryByDerateId(string derateId)
        {
            if (string.IsNullOrWhiteSpace(derateId)) throw new ArgumentNullException("derateId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QueryByDerateId(derateId);
        }

        public static List<ParkCarDerate> QueryByPlateNumber(string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber)) throw new ArgumentNullException("plateNumber");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QueryByPlateNumber(plateNumber);
        }
        public static Dictionary<string, int> QuerySettlementdCarDerate(List<string> derateQRCodeIds)
        {
            if (derateQRCodeIds == null || derateQRCodeIds.Count == 0) throw new ArgumentNullException("derateQRCodeIds");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QuerySettlementdCarDerate(derateQRCodeIds);
        }
        public static List<ParkCarDerate> QueryByCardNo(string cardNo)
        {
            if (string.IsNullOrWhiteSpace(cardNo)) throw new ArgumentNullException("cardNo");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QueryByCardNo(cardNo);
        }

        public static List<ParkCarDerate> QueryByIORecordID(string ioRecordId)
        {
            if (string.IsNullOrWhiteSpace(ioRecordId)) throw new ArgumentNullException("ioRecordId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QueryByIORecordID(ioRecordId);
        }

        public static bool DeleteByExpiryTime(string derateId, DateTime expiredTime)
        {
            if (string.IsNullOrWhiteSpace(derateId)) throw new ArgumentNullException("derateId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.DeleteByExpiryTime(derateId,expiredTime);
        }
        public static ParkCarDerate GetNotUseParkCarDerate(string derateId, DateTime lessThanTime) {
            if (string.IsNullOrWhiteSpace(derateId)) throw new ArgumentNullException("derateId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.GetNotUseParkCarDerate(derateId, lessThanTime);
        }
        public static bool UpdateCarderateCreateTime(string carDerateID) {
            if (string.IsNullOrWhiteSpace(carDerateID)) throw new ArgumentNullException("carDerateID");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.UpdateCarderateCreateTime(carDerateID);
        }
        public static ParkCarDerate QueryByCarDerateID(string carDerateId) {
            if (string.IsNullOrWhiteSpace(carDerateId)) throw new ArgumentNullException("carDerateId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.QueryByCarDerateID(carDerateId);
        }
        public static List<ParkCarDerate> ParkCarDeratePage(string sellerId, string plateNumber, int? state, int? derateType, DateTime? start, DateTime? end, int pageSize, int pageIndex, ref int totalCount)
        {
            if (string.IsNullOrWhiteSpace(sellerId)) throw new ArgumentNullException("sellerId");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            return factory.ParkCarDeratePage(sellerId,plateNumber,state,derateType,start,end,pageSize,pageIndex,ref totalCount);
        }
        public static ParkCarDerate QueryBySellerIdAndIORecordId(string sellerId, string ioRecordId)
        {
            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            ParkCarDerate carDerate = factory.QueryBySellerIdAndIORecordId(sellerId, ioRecordId);
            return carDerate;
        }
        public static bool QRCodeDiscount(string sellerId,string derateId,string carDerateId, string parkingId, string ioRecordId, string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(sellerId)) throw new ArgumentNullException("sellerId");
            if (string.IsNullOrWhiteSpace(derateId)) throw new ArgumentNullException("derateId");
            if (string.IsNullOrWhiteSpace(carDerateId)) throw new ArgumentNullException("carDerateId");
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");
            if (string.IsNullOrWhiteSpace(plateNumber)) throw new ArgumentNullException("plateNumber");

            List<ParkCarDerate> carDerates = ParkCarDerateServices.QueryByDerateId(carDerateId);
            if (carDerates == null || carDerates.Count == 0) throw new MyException("该优免券不存在");
            if (!string.IsNullOrWhiteSpace(carDerates.First().PlateNumber)) throw new MyException("该优免券已使用过了[001]");
            if (carDerates.First().Status == CarDerateStatus.Settlementd) throw new MyException("该优免券已使用过了[002]");
            if (carDerates.First().Status != CarDerateStatus.Used) throw new MyException("该优免券不是有效状态");

            IParkCarDerate factory = ParkCarDerateFactory.GetFactory();
            ParkCarDerate carDerate = factory.QueryBySellerIdAndIORecordId(sellerId, ioRecordId);
            if (carDerate != null) throw new MyException("该车辆已优免了");

            ParkDerate derate = ParkDerateServices.Query(derateId);
            if (derate == null) throw new MyException("找不到优免规则");

            ParkSeller seller = ParkSellerServices.QueryBySellerId(derate.SellerID);
            if (seller == null) throw new MyException("找不到商家信息");

            return factory.QRCodeDiscount(carDerateId, parkingId, ioRecordId,plateNumber);
        }
    }
}
