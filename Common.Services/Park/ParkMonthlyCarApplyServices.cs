using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Factory;
using Common.IRepository.Park;
using Common.Entities;
using Common.DataAccess;
using Common.Utilities;
using Common.Services.BaseData;

namespace Common.Services.Park
{
    public class ParkMonthlyCarApplyServices
    {
        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(ParkMonthlyCarApply model) 
        {
            if (model == null) throw new ArgumentNullException("model");
            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            return factory.Add(model);
        }
        /// <summary>
        /// 重新提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AgainApply(ParkMonthlyCarApply model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.RecordID)) throw new ArgumentNullException("RecordID");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            return factory.AgainApply(model);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="recrdId"></param>
        /// <returns></returns>
        public static bool Cancel(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("RecordID");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            ParkMonthlyCarApply model = factory.QueryByRecordID(recordId);
            if (model == null) throw new MyException("需要取消的申请不存在");
            if (model.ApplyStatus != MonthlyCarApplyStatus.Applying) throw new MyException("只有申请中的状态才能取消");

            return factory.Cancel(recordId);
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="remark">拒绝原因</param>
        /// <returns></returns>
        public static bool Refused(string recordId, string remark)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("RecordID");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            ParkMonthlyCarApply model = factory.QueryByRecordID(recordId);
            if (model == null) throw new MyException("需要拒绝的申请不存在");
            if (model.ApplyStatus != MonthlyCarApplyStatus.Applying) throw new MyException("只有申请中的状态才能拒绝");

            return factory.Refused(recordId, remark);
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static bool Passed(string RecordID, string AuditRemark, string CarTypeID, string CarModelID, string AreaIDS, string GateID, string OperatorId)
        {

            if (string.IsNullOrWhiteSpace(CarTypeID)) throw new MyException("获取车类失败");
            if (string.IsNullOrWhiteSpace(CarModelID)) throw new MyException("获取车型失败");

            ParkMonthlyCarApply monthlyCarApply = ParkMonthlyCarApplyServices.QueryByRecordID(RecordID);
            if (monthlyCarApply == null) throw new MyException("该申请不存在");
            if (monthlyCarApply.ApplyStatus != MonthlyCarApplyStatus.Applying) throw new MyException("该申请是申请中状态");
            monthlyCarApply.CarModelID = CarModelID;
            monthlyCarApply.CarTypeID = CarTypeID;
            monthlyCarApply.AuditRemark = AuditRemark;

            BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(monthlyCarApply.PKID);
            if (parking == null) throw new MyException("车场信息不存在");

            BaseEmployee employee = GenerateBaseEmployeeModel(parking.VID, monthlyCarApply.ApplyName, monthlyCarApply.ApplyMoblie, monthlyCarApply.FamilyAddress);
            EmployeePlate plate = GenerateEmployeePlateModel(employee, parking.VID, monthlyCarApply.PlateNo);
            BaseCard card = GenerateCardModel(parking, employee, plate, OperatorId);
            ParkGrant parkGrant = GenerateParkGrantModel(parking, plate, card, monthlyCarApply.PKLot, monthlyCarApply.CarModelID, monthlyCarApply.CarTypeID, AreaIDS, GateID);

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = ParkGrantServices.Add(employee, plate, card, parkGrant, dbOperator);
                    if (!result) throw new MyException("保存车辆信息失败");

                    IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
                    result = factory.Passed(monthlyCarApply, dbOperator);
                    if (!result) throw new MyException("修改申请状态失败");

                    dbOperator.CommitTransaction();
                    monthlyCarApply = ParkMonthlyCarApplyServices.QueryByRecordID(monthlyCarApply.RecordID);
                    OperateLogServices.AddOperateLog<ParkMonthlyCarApply>(monthlyCarApply, OperateType.Update);
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
           
        }
        public static BaseEmployee GenerateBaseEmployeeModel(string villageId, string employeeName, string mobilePhone, string familyAddr)
        {
            BaseEmployee model = new BaseEmployee();
            model.EmployeeName = employeeName.Trim();
            model.RegTime = DateTime.Now;
            model.MobilePhone = mobilePhone;
            model.VID = villageId;
            model.EmployeeType = EmployeeType.Owner;
            model.EmployeeID = GuidGenerator.GetGuidString();
            model.FamilyAddr = familyAddr;
            return model;
        }
        public static EmployeePlate GenerateEmployeePlateModel(BaseEmployee employee, string villageId, string plateNo)
        {

            EmployeePlate model = new EmployeePlate();
            model.PlateNo = plateNo;

            string errorMsg = string.Empty;
            EmployeePlate dbPlate = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(villageId, model.PlateNo, out errorMsg);
            if (!string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取车牌失败");
            if (dbPlate != null)
            {
                dbPlate.EmployeeID = employee.EmployeeID;
                return dbPlate;
            }
            model.Color = PlateColor.Blue;
            model.PlateID = GuidGenerator.GetGuidString();
            model.EmployeeID = employee.EmployeeID;
            return model;
        }
        public static BaseCard GenerateCardModel(BaseParkinfo parking, BaseEmployee employee, EmployeePlate plate, string loginUserRecordId)
        {
            BaseCard model = new BaseCard();
            model.CardID = GuidGenerator.GetGuidString();
            model.CardNo = plate.PlateNo;
            model.CardNumb = model.CardNo;
            model.VID = parking.VID;

            BaseCard oldCard = BaseCardServices.QueryBaseCardByParkingId(parking.PKID, model.CardNo);
            if (oldCard != null)
            {
                model.CardID = oldCard.CardID;
                if (oldCard != null && oldCard.CardID != model.CardID) throw new MyException("车牌号已存在，不能重复添加");

                string errorMsg = string.Empty;
                model = BaseCardServices.GetBaseCard(model.CardID, out errorMsg);
                if (model == null || !string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取卡信息失败");

                model.CardNo = plate.PlateNo;
                model.CardNumb = model.CardNo;
                model.VID = parking.VID;
            }
            else
            {
                if (oldCard != null) throw new MyException("车牌号已存在，不能重复添加");
                model.RegisterTime = DateTime.Now;
            }

            model.OperatorID = loginUserRecordId;
            model.CardSystem = CardSystem.Park;
            model.EmployeeID = employee.EmployeeID;
            model.CardType = CardType.Plate;
            return model;
        }

        public static BaseCard GenerateCardModel(string VID, string PKID, BaseEmployee employee, EmployeePlate plate, string loginUserRecordId)
        {
            BaseCard model = new BaseCard();
            model.CardID = GuidGenerator.GetGuidString();
            model.CardNo = plate.PlateNo;
            model.CardNumb = model.CardNo;
            model.VID = VID;

            BaseCard oldCard = BaseCardServices.QueryBaseCardByParkingId(PKID, model.CardNo);
            if (oldCard != null)
            {
                model.CardID = oldCard.CardID;
                if (oldCard != null && oldCard.CardID != model.CardID) throw new MyException("车牌号已存在，不能重复添加");

                string errorMsg = string.Empty;
                model = BaseCardServices.GetBaseCard(model.CardID, out errorMsg);
                if (model == null || !string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取卡信息失败");

                model.CardNo = plate.PlateNo;
                model.CardNumb = model.CardNo;
                model.VID = VID;
            }
            else
            {
                if (oldCard != null) throw new MyException("车牌号已存在，不能重复添加");
                model.RegisterTime = DateTime.Now;
            }

            model.OperatorID = loginUserRecordId;
            model.CardSystem = CardSystem.Park;
            model.EmployeeID = employee.EmployeeID;
            model.CardType = CardType.Plate;
            return model;
        }

        public static ParkGrant GenerateParkGrantModel(BaseParkinfo parking, EmployeePlate plate, BaseCard card, string pkLot, string carModelId, string carTypeId, string AreaIDS, string GateID)
        {
            ParkGrant model = new ParkGrant();
            model.PKID = parking.PKID;
            model.CardID = card.CardID;
            model.PlateID = plate.PlateID;
            model.PlateNo = plate.PlateNo;
            model.PKLot = pkLot;
            model.PKLotNum = 0;

            ParkCarModel carModel = ParkCarModelServices.QueryByRecordId(carModelId);
            if (carModel == null) throw new MyException("选择的车型不存在");
            model.CarModelID = carModel.CarModelID;

            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) throw new MyException("选择的车类不存在");
            model.CarTypeID = carType.CarTypeID;

            if (string.IsNullOrWhiteSpace(AreaIDS)) throw new MyException("获取选择的车场区域失败");
            model.AreaIDS = AreaIDS == "-1" ? string.Empty : AreaIDS;

            if (string.IsNullOrWhiteSpace(GateID)) throw new MyException("获取选择的车场通道失败");
            model.GateID = GateID == "-1" ? string.Empty : GateID;
            return model;
        }

        public static ParkGrant GenerateParkGrantModel(string PKID, EmployeePlate plate, BaseCard card, string pkLot, string carModelId, string carTypeId, string AreaIDS, string GateID)
        {
            ParkGrant model = new ParkGrant();
            model.PKID = PKID;
            model.CardID = card.CardID;
            model.PlateID = plate.PlateID;
            model.PlateNo = plate.PlateNo;
            model.PKLot = pkLot;
            model.PKLotNum = 0;

            ParkCarModel carModel = ParkCarModelServices.QueryByRecordId(carModelId);
            if (carModel == null) throw new MyException("选择的车型不存在");
            model.CarModelID = carModel.CarModelID;

            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) throw new MyException("选择的车类不存在");
            model.CarTypeID = carType.CarTypeID;

            if (string.IsNullOrWhiteSpace(AreaIDS)) throw new MyException("获取选择的车场区域失败");
            model.AreaIDS = AreaIDS == "-1" ? string.Empty : AreaIDS;

            if (string.IsNullOrWhiteSpace(GateID)) throw new MyException("获取选择的车场通道失败");
            model.GateID = GateID == "-1" ? string.Empty : GateID;
            return model;
        }

        public static ParkMonthlyCarApply QueryByRecordID(string recordId) {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            return factory.QueryByRecordID(recordId);
        }
        public static List<ParkMonthlyCarApply> QueryByAccountID(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException("accountId");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            return factory.QueryByAccountID(accountId);
        }

        public static List<ParkMonthlyCarApply> QueryPage(List<string> parkingIds, string applyInfo, MonthlyCarApplyStatus? Status, DateTime start, DateTime end, int pagesize, int pageindex, out int total)
        {
            if (parkingIds == null || parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");

            IParkMonthlyCarApply factory = ParkMonthlyCarApplyFactory.GetFactory();
            return factory.QueryPage(parkingIds, applyInfo, Status, start, end, pagesize, pageindex, out total);
        }
    }
}
