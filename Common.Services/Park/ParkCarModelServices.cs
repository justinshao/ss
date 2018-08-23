using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.DataAccess;
using Common.Utilities;
using Common.Core.Expands;

namespace Common.Services
{
    public class ParkCarModelServices
    {
        public static bool Add(ParkCarModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.CarModelName = model.CarModelName.Trim();
            model.CarModelID = GuidGenerator.GetGuidString();
            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            List<ParkCarModel> carModels = factory.QueryByParkingId(model.PKID);
            if (carModels.Count(p => p.CarModelName == model.CarModelName) != 0) {
                throw new MyException("车型名称已存在，请更改名称");
            }
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarModel>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefault(string parkingId, DbOperator dbOperator)
        {

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            return factory.Add(GetDefaultParkCarModel(parkingId), dbOperator);
        }
        private static List<ParkCarModel> GetDefaultParkCarModel(string parkingId)
        {
            List<ParkCarModel> models = new List<ParkCarModel>();

            models.Add(new ParkCarModel()
            {

                PKID = parkingId,
                CarModelID = GuidGenerator.GetGuidString(),
                CarModelName = "小车",
                PlateColor="无",
                IsDefault = YesOrNo.Yes
            });
            models.Add(new ParkCarModel()
            {

                PKID = parkingId,
                CarModelID = GuidGenerator.GetGuidString(),
                PlateColor = "无",
                CarModelName = "摩托车",
                IsDefault = YesOrNo.No
            });
            models.Add(new ParkCarModel()
            {

                PKID = parkingId,
                CarModelID = GuidGenerator.GetGuidString(),
                CarModelName = "大车",
                PlateColor = "无",
                IsDefault = YesOrNo.No
            });
            models.Add(new ParkCarModel()
            {

                PKID = parkingId,
                CarModelID = GuidGenerator.GetGuidString(),
                CarModelName = "超大车",
                PlateColor = "无",
                IsDefault = YesOrNo.No
            });
            return models;
        }
        public static bool Update(ParkCarModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.CarModelName = model.CarModelName.Trim();
            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            ParkCarModel carModel = factory.QueryByRecordId(model.CarModelID);
            //if (carModel.IsDefault == YesOrNo.Yes && model.IsDefault == YesOrNo.No)
            //    throw new MyException("必须设置一个默认类型");

            List<ParkCarModel> carModels = factory.QueryByParkingId(model.PKID);
            if (carModels.Count(p => p.CarModelName == model.CarModelName && p.CarModelID!=model.CarModelID) != 0)
            {
                throw new MyException("车型名称已存在，请更改名称");
            }

            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarModel>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static ParkCarModel QueryByRecordId(string recordId)
        { 
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }

        public static List<ParkCarModel> QueryByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }

        public static List<ParkCarModel> QueryByParkingIds(List<string> parkingIds)
        {
            if (parkingIds == null || parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            return factory.QueryByParkingIds(parkingIds);
        }
        public static ParkCarModel GetDefaultCarModel(string parkingid, out string errorMsg)
        {
            if (parkingid.IsEmpty()) throw new ArgumentNullException("parkingid");

            IParkCarModel factory = ParkCarModelFactory.GetFactory();
            return factory.GetDefaultCarModel(parkingid, out errorMsg);
        }
    }
}
