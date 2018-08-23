using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Utilities;
using Common.Factory.Park;
using Common.IRepository;
using Common.Entities;

namespace Common.Services.Park
{
    public class ParkDerateConfigServices
    {
        public static bool Add(ParkDerateConfig model) {
            if (model == null) throw new ArgumentNullException("model");

            model.RecordID = GuidGenerator.GetGuidString();
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
            List<ParkDerateConfig> configs = factory.QueryByParkingId(model.PKID);
            configs = configs.Where(p => (p.ConsumeStartAmount <= model.ConsumeStartAmount && p.ConsumeEndAmount >= model.ConsumeStartAmount)
                || (p.ConsumeStartAmount <= model.ConsumeEndAmount && p.ConsumeEndAmount >= model.ConsumeEndAmount)).ToList();
            if (configs.Count > 0)
            {
                throw new MyException("消费金额重复");
            }
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerateConfig>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(ParkDerateConfig model)
        {
            if (model == null) throw new ArgumentNullException("model");
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
             List<ParkDerateConfig> configs = factory.QueryByParkingId(model.PKID);
            configs = configs.Where(p=>p.RecordID!=model.RecordID && ((p.ConsumeStartAmount<=model.ConsumeStartAmount && p.ConsumeEndAmount>=model.ConsumeStartAmount)
                || (p.ConsumeStartAmount <= model.ConsumeEndAmount && p.ConsumeEndAmount >= model.ConsumeEndAmount))).ToList();
            if (configs.Count > 0) {
                throw new MyException("消费金额重复");
            }
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerateConfig>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
            return factory.Delete(recordId);
        }

        public static ParkDerateConfig QueryByRecordId(string recordId)
        {
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }

        public static List<ParkDerateConfig> QueryByParkingId(string parkingId)
        {
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }
        /// <summary>
        /// 根据车场编号和消费金额获优免配置信息
        /// </summary>
        /// <param name="parkingId">车场编号</param>
        /// <param name="amount">消费金额</param>
        /// <returns></returns>
        public static ParkDerateConfig QueryByParkingIdAndAmount(string parkingId, decimal amount) {
            IParkDerateConfig factory = ParkDerateConfigFactory.GetFactory();
            return factory.QueryByParkingIdAndAmount(parkingId,amount);
        }
    }
}
