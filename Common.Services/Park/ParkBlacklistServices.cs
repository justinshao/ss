using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Utilities;
using Common.Entities;

namespace Common.Services.Park
{
    public class ParkBlacklistServices
    {
        public static ParkBlacklist GetBlacklist(string parkingID, string plateNumber, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            return factory.GetBlacklist(parkingID, plateNumber, out ErrorMessage);
        }
        public static ParkBlacklist Query(string recordId) {
            if (recordId.IsEmpty()) throw new ArgumentNullException("recordId");
          
            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            return factory.Query(recordId);
        }

        public static bool Add(ParkBlacklist model)
        {
            if (model == null) throw new ArgumentNullException("model");
            model.RecordID = GuidGenerator.GetGuid().ToString();
            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            ParkBlacklist dbModel = factory.Query(model.PKID, model.PlateNumber);
            if (dbModel != null) throw new MyException("当前车场的黑名单中已存在该车牌了");
            bool result= factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkBlacklist>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(ParkBlacklist model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            ParkBlacklist dbModel = factory.Query(model.PKID, model.PlateNumber);
            if (dbModel != null && dbModel.RecordID!=model.RecordID) throw new MyException("当前车场的黑名单中已存在该车牌了");
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkBlacklist>(model, OperateType.Update);
            }
            return result;
        }

        public static ParkBlacklist Query(string parkingid, string plateNo)
        {
            if (parkingid.IsEmpty()) throw new ArgumentNullException("parkingid");
            if (plateNo.IsEmpty()) throw new ArgumentNullException("plateNo");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            return factory.Query(parkingid, plateNo);
        }

        public static bool Delete(string recordId)
        {
            if (recordId.IsEmpty()) throw new ArgumentNullException("recordId");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static List<ParkBlacklist> QueryByParkingId(string parkingId)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }

        public static List<ParkBlacklist> QueryPage(string parkingId, string plateNo, int pagesize, int pageindex, out int total)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkBlacklist factory = ParkBlacklistFactory.GetFactory();
            return factory.QueryPage(parkingId, plateNo,pagesize,pageindex,out total);
        }
    }
}
