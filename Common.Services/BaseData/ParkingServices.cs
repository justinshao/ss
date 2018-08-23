using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.DataAccess;
using Common.Utilities;

namespace Common.Services
{
    public class ParkingServices
    {
        public static bool Add(BaseParkinfo model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.PKNo)) throw new MyException("车场编号不能为空");

            IParking factory = ParkingFactory.GetFactory();
            BaseParkinfo park = factory.QueryParkingByParkingNo(model.PKNo);
            if (park != null) throw new MyException("车场编号不能重复");

            model.PKID = GuidGenerator.GetGuidString();

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Add(model, dbOperator);
                    if (!result) throw new MyException("添加车场信息失败");

                    result = ParkCarModelServices.AddDefault(model.PKID, dbOperator);
                    if (!result) throw new MyException("添加车型失败");

                    result = ParkCarTypeServices.AddDefault(model.PKID, dbOperator);
                    if (!result) throw new MyException("添加车类型失败");
                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog<BaseParkinfo>(model, OperateType.Add);
                    }
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// 创建默认车场
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static bool AddParkinfoDefault(BaseParkinfo model, DbOperator dbOperator)
        {
            IParking factory = ParkingFactory.GetFactory();
            try
            {
                dbOperator.BeginTransaction();
                bool result = factory.Add(model, dbOperator);
                if (!result) throw new MyException("添加车场信息失败");

                result = ParkCarModelServices.AddDefault(model.PKID, dbOperator);
                if (!result) throw new MyException("添加车型失败");

                result = ParkCarTypeServices.AddDefault(model.PKID, dbOperator);
                if (!result) throw new MyException("添加车类型失败");
                dbOperator.CommitTransaction();
                if (result)
                {
                    OperateLogServices.AddOperateLog<BaseParkinfo>(model, OperateType.Add);
                }
                return true;
            }
            catch
            {
                dbOperator.RollbackTransaction();
                throw;
            }

        }

        public static bool Update(BaseParkinfo model, bool addlog = true)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.PKNo)) throw new MyException("车场编号不能为空");

            IParking factory = ParkingFactory.GetFactory();
            BaseParkinfo park = factory.QueryParkingByParkingNo(model.PKNo);
            if (park != null && park.PKID != model.PKID) throw new MyException("车场编号不能重复");

            bool result = factory.Update(model);
            if (result&& addlog)
            {
                OperateLogServices.AddOperateLog<BaseParkinfo>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(recordId);
            if (areas.Count != 0) throw new MyException("请先删除改车场下的所有区域");

            IParking factory = ParkingFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static BaseParkinfo QueryParkingByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByRecordId(recordId);
        }

        public static List<BaseParkinfo> QueryParkingByRecordIds(List<string> recordIds)
        {
            if (recordIds == null || recordIds.Count == 0) throw new ArgumentNullException("recordIds");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByRecordIds(recordIds);
        }

        public static List<BaseParkinfo> QueryParkingByVillageId(string villageId)
        {
            if (string.IsNullOrWhiteSpace(villageId)) throw new ArgumentNullException("villageId");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByVillageId(villageId);
        }
        public static List<BaseParkinfo> QueryParkingByCompanyIds(List<string> companyIds) {
            if (companyIds == null || companyIds.Count == 0) throw new ArgumentNullException("companyIds");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByCompanyIds(companyIds);
        }
        public static List<BaseParkinfo> QueryParkingByVillageIds(List<string> villageIds)
        {
            if (villageIds == null || villageIds.Count == 0) throw new ArgumentNullException("villageIds");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByVillageIds(villageIds);
        }
        public static List<BaseParkinfo> QueryAllParking()
        {
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryAllParking();
        }
        public static BaseParkinfo QueryParkingByParkingID(string ParkingID)
        {
            if (string.IsNullOrEmpty(ParkingID)) throw new ArgumentNullException("villageIds");
            IParking factory = ParkingFactory.GetFactory();
            return factory.QueryParkingByParkingID(ParkingID);
        }
        public static List<BaseParkinfo> QueryPage(string villageId, int pageIndex, int pageSize, out int totalCount)
        {
            IParking factory = ParkingFactory.GetFactory();
            List<BaseParkinfo> parkings = factory.QueryPage(villageId, pageIndex, pageSize, out totalCount);
            IParkArea factoryArea = ParkAreaFactory.GetFactory();
            foreach (var item in parkings)
            {
                item.CarBitNum = factoryArea.GetCarBitNumByParkingId(item.PKID);
            }
            return parkings;
        }

        public static int UpdateCarBit(string PKID)
        {
            IParking factory = ParkingFactory.GetFactory();
            int result = factory.UpdateCarBit(PKID);
            if (result>0)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("PKID:{0}", PKID));
            }
            return result;
        }
        public static DateTime GetServerTime(out string error)
        {
            IParking factory = ParkingFactory.GetFactory();
            return factory.GetServerTime(out error);  
        }
        /// <summary>
        /// 获取支持退款的所有车场
        /// </summary>
        /// <returns></returns>
        public static List<BaseParkinfo> GetParkingBySupportAutoRefund() {
            IParking factory = ParkingFactory.GetFactory();
            return factory.GetParkingBySupportAutoRefund();  
        }
        /// <summary>
        /// 修改车场结算配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateParkSettleConfig(BaseParkinfo model) {
            IParking factory = ParkingFactory.GetFactory();
            bool result = factory.UpdateParkSettleConfig(model);
            if (result) {
                OperateLogServices.AddOperateLog<BaseParkinfo>(model, OperateType.Update);
            }
            return result;
        }
    }
}
