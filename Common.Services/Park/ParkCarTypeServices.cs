using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.DataAccess;
using Common.Utilities;
using Common.Services.Park;

namespace Common.Services
{
    public class ParkCarTypeServices
    {
        public static bool Add(ParkCarType model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.CarTypeName = model.CarTypeName.Trim();
            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            ParkCarType carType = factory.QueryCarTypesByCarTypeName(model.PKID, model.CarTypeName);
            if (carType != null) throw new MyException("车类名称不能重复");

            if(model.BaseTypeID == BaseCarType.TempCar){
              List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(model.PKID);
              if (carTypes.FirstOrDefault(p => p.BaseTypeID == BaseCarType.TempCar) != null) {
                  throw new MyException("基础类型为临时车的已经存在，不能重复添加");
              }
            }
          
            model.CarTypeID = GuidGenerator.GetGuidString();
           
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarType>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefault(string parkingId, DbOperator dbOperator)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            List<ParkCarType> listcartype=GetDefaultParkCarTypes(parkingId);
            foreach (var obj in listcartype)
            {
                ParkCarTypeSingleServices.AddDefault(obj.CarTypeID, dbOperator);
            }
            return factory.Add(listcartype, dbOperator);

        }
        private static List<ParkCarType> GetDefaultParkCarTypes(string parkingId)
        {
            List<ParkCarType> models = new List<ParkCarType>();
            models.Add(new ParkCarType()
             {
                 CarTypeID = GuidGenerator.GetGuidString(),
                 CarTypeName = "贵宾卡",
                 PKID = parkingId,
                 BaseTypeID = BaseCarType.VIPCar,
                 RepeatIn = YesOrNo.Yes,
                 RepeatOut = YesOrNo.Yes,
                 AffirmIn = YesOrNo.No,
                 AffirmOut = YesOrNo.No,
                 MaxUseMoney = 0,
                 AllowLose = YesOrNo.No,
                 LpDistinguish = LpDistinguish.Plate,
                 InOutEditCar = YesOrNo.Yes,
                 InOutTime = 0,
                 CarNoLike = YesOrNo.No,
                 IsAllowOnlIne= YesOrNo.No,
                 AffirmBegin="00:00",
                 AffirmEnd="23:59",
                 IsNeedCapturePaper=false,
                 IsNeedAuthentication = false

             });
            models.Add(new ParkCarType()
            {
                CarTypeID = GuidGenerator.GetGuidString(),
                CarTypeName = "储值卡",
                PKID = parkingId,
                BaseTypeID = BaseCarType.StoredValueCar,
                RepeatIn = YesOrNo.Yes,
                RepeatOut = YesOrNo.Yes,
                AffirmIn = YesOrNo.No,
                AffirmOut = YesOrNo.No,
                MaxUseMoney = 0,
                MaxValue = 2000,
                AllowLose = YesOrNo.No,
                LpDistinguish = LpDistinguish.Plate,
                InOutEditCar = YesOrNo.Yes,
                InOutTime = 0,
                CarNoLike = YesOrNo.No,
                IsAllowOnlIne = YesOrNo.Yes,
                AffirmBegin = "00:00",
                AffirmEnd = "23:59",
                IsNeedCapturePaper=false,
                IsNeedAuthentication = false
            });
            models.Add(new ParkCarType()
            {
                CarTypeID = GuidGenerator.GetGuidString(),
                CarTypeName = "月卡",
                PKID = parkingId,
                BaseTypeID = BaseCarType.MonthlyRent,
                RepeatIn = YesOrNo.Yes,
                RepeatOut = YesOrNo.Yes,
                AffirmIn = YesOrNo.No,
                AffirmOut = YesOrNo.No,
                MaxUseMoney = 0,
                AllowLose = YesOrNo.No,
                LpDistinguish = LpDistinguish.Plate,
                InOutEditCar = YesOrNo.Yes,
                Amount = 300,
                MaxMonth = 12,
                InOutTime = 0,
                CarNoLike = YesOrNo.No,
                IsAllowOnlIne = YesOrNo.Yes,
                AffirmBegin = "00:00",
                AffirmEnd = "23:59",
                IsNeedCapturePaper=false,
                IsNeedAuthentication = false
            });

            models.Add(new ParkCarType()
            {
                CarTypeID = GuidGenerator.GetGuidString(),
                CarTypeName = "临时卡",
                PKID = parkingId,
                BaseTypeID = BaseCarType.TempCar,
                RepeatIn = YesOrNo.Yes,
                RepeatOut = YesOrNo.No,
                AffirmIn = YesOrNo.No,
                AffirmOut = YesOrNo.No,
                MaxUseMoney = 0,
                AllowLose = YesOrNo.No,
                LpDistinguish = LpDistinguish.Plate,
                InOutEditCar = YesOrNo.Yes,
                InOutTime = 0,
                CarNoLike = YesOrNo.No,
                IsAllowOnlIne = YesOrNo.Yes,
                AffirmBegin = "00:00",
                AffirmEnd = "23:59",
                IsNeedCapturePaper=false,
                IsNeedAuthentication=false
            });
            //models.Add(new ParkCarType()
            //{
            //    CarTypeID = GuidGenerator.GetGuidString(),
            //    CarTypeName = "工作卡",
            //    PKID = parkingId,
            //    BaseTypeID = BaseCarType.WorkCar,
            //    RepeatIn = YesOrNo.Yes,
            //    RepeatOut = YesOrNo.Yes,
            //    AffirmIn = YesOrNo.Yes,
            //    AffirmOut = YesOrNo.Yes,
            //    MaxUseMoney = 0,
            //    AllowLose = YesOrNo.No,
            //    LpDistinguish = LpDistinguish.Plate,
            //    InOutEditCar = YesOrNo.Yes,
            //    InOutTime = 30,
            //    CarNoLike = YesOrNo.No,
            //    IsAllowOnlIne = YesOrNo.No,
            //    AffirmBegin = "00:00",
            //    AffirmEnd = "23:59"
            //});
            return models;
        }

        public static bool Update(ParkCarType model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.CarTypeName = model.CarTypeName.Trim();

            IParkCarType factory = ParkCarTypeFactory.GetFactory();

            ParkCarType carType = factory.QueryCarTypesByCarTypeName(model.PKID, model.CarTypeName);
            if (carType != null && model.CarTypeID!=carType.CarTypeID) throw new MyException("车类名称不能重复");

            if (model.BaseTypeID == BaseCarType.TempCar)
            {
                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(model.PKID);
                if (carTypes.FirstOrDefault(p => p.BaseTypeID == BaseCarType.TempCar && p.CarTypeID!=model.CarTypeID) != null)
                {
                    throw new MyException("基础类型为临时车的已经存在，不能重复添加");
                }
            }

            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarType>(model, OperateType.Update);
            }
            return result;
        }

        public static bool QueryGrant(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            bool result = factory.QueryGrant(recordId);
            return result;
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");


            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;

        }

        public static ParkCarType QueryParkCarTypeByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            return factory.QueryParkCarTypeByRecordId(recordId);
        }
        public static List<ParkCarType> QueryParkCarTypeByRecordIds(List<string> recordIds) {
            if (recordIds.Count == 0) return new List<ParkCarType>();

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            return factory.QueryParkCarTypeByRecordIds(recordIds);
        }
        public static List<ParkCarType> QueryParkCarTypeByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            return factory.QueryParkCarTypeByParkingId(parkingId);
        }

        public static List<ParkCarType> QueryParkCarTypeByParkingIds(List<string> parkingIds)
        {
            if (parkingIds == null || parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            return factory.QueryParkCarTypeByParkingIds(parkingIds);
        }

        public static List<ParkCarType> QueryCardTypesByBaseCardType(string parkingId, BaseCarType baseCarType)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkCarType factory = ParkCarTypeFactory.GetFactory();
            return factory.QueryCardTypesByBaseCardType(parkingId, baseCarType);
        }
    }
}
