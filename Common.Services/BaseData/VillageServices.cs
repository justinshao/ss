using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.Utilities;
using Common.DataAccess;

namespace Common.Services
{
    public class VillageServices
    {
        /// <summary>
        /// 创建默认小区
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static bool AddVillageDefaultUser(BaseVillage model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            IVillage factory = VillageFactory.GetFactory();
            bool result = factory.Add(model, dbOperator);
            if (result)
            {
                OperateLogServices.AddOperateLog<BaseVillage>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Add(BaseVillage model) {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.VNo)) throw new ArgumentNullException("小区编号不能为空");

            model.VID = GuidGenerator.GetGuid().ToString();
            IVillage factory = VillageFactory.GetFactory();
            BaseVillage dbModel = factory.QueryVillageByVillageNo(model.VNo, model.CPID);
            if (dbModel != null) throw new MyException("小区编号已经存在");

            dbModel = factory.QueryVillageByProxyNo(model.ProxyNo);
            if (dbModel != null) throw new MyException("代理编号已存在");

            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<BaseVillage>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(BaseVillage model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.VNo)) throw new ArgumentNullException("小区编号不能为空");

            IVillage factory = VillageFactory.GetFactory();

            BaseVillage dbModel = factory.QueryVillageByVillageNo(model.VNo,model.CPID);
            if (dbModel != null && dbModel.VID!=model.VID) throw new MyException("小区编号已经存在");

            dbModel = factory.QueryVillageByProxyNo(model.ProxyNo);
            if (dbModel != null && dbModel.VID != model.VID) throw new MyException("代理编号已存在");

            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<BaseVillage>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            List<BaseParkinfo> parks = ParkingServices.QueryParkingByVillageId(recordId);
            if (parks.Count != 0) throw new MyException("请先删除小区下面所有的车场");

            IVillage factory = VillageFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result) {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static BaseVillage QueryVillageByProxyNo(string proxyNo)
        {
            if (string.IsNullOrWhiteSpace(proxyNo)) throw new ArgumentNullException("proxyNo");

            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByProxyNo(proxyNo);
        }

        public static List<BaseVillage> QueryVillageByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");

            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByUserId(userId);
        }

        public static List<BaseVillage> QueryVillageAll()
        {
            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageAll();
        }

        public static List<BaseVillage> QueryPage(string companyId, int pageIndex, int pageSize, out int totalRecord)
        {
            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryPage(companyId,pageIndex,pageSize,out totalRecord);
        }
        public static List<BaseVillage> QueryPage(List<string> villageIds,string companyId, int pageIndex, int pageSize, out int totalRecord)
        {
            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryPage(villageIds,companyId, pageIndex, pageSize, out totalRecord);
        }
        public static List<BaseVillage> QueryVillageByCompanyId(string companyId)
        {
            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByCompanyId(companyId);
        }

        public static List<BaseVillage> QueryVillageByCompanyIds(List<string> companyIds)
        {
            if (companyIds == null || companyIds.Count == 0) throw new ArgumentNullException("companyIds");

            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByCompanyIds(companyIds);
        }

        public static BaseVillage QueryVillageByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByRecordId(recordId);
        }
        public static List<BaseVillage> QueryVillageByEmployeeMobilePhone(string mobilePhone) {
            if (string.IsNullOrWhiteSpace(mobilePhone)) throw new ArgumentNullException("mobilePhone");

            IVillage factory = VillageFactory.GetFactory();
            return factory.QueryVillageByEmployeeMobilePhone(mobilePhone);
        }
    }
}
