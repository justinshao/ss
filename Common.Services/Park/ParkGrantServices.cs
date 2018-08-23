using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Utilities;
using Common.Entities.Parking;
using Common.Entities.Condition;
using Common.IRepository.BaseData;
using Common.Factory.BaseData;
using Common.DataAccess;
using Common.IRepository;
using Common.Services.BaseData;
using Common.Factory;
using Common.Entities.WX;
using Common.Entities.DAAPI;

namespace Common.Services.Park
{
    public class ParkGrantServices
    {
        private static object parkGrant_lock = new object();

        public static ParkGrant GetCardgrant(string plateNumberID, out string ErrorMessage)
        {
            if (plateNumberID.IsEmpty()) throw new ArgumentNullException("plateNumberID");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.GetCardgrant(plateNumberID, out ErrorMessage);
        }
        public static List<ParkGrant> GetParkGrantByPlateNumberID(string parkingID, string plateNumberID, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (plateNumberID.IsEmpty()) throw new ArgumentNullException("plateNumberID");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.GetParkGrantByPlateNumberID(parkingID, plateNumberID, out ErrorMessage);
        }
        public static List<ParkGrant> GetCardgrantsByLot(string parkingID, string lot, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (lot.IsEmpty()) throw new ArgumentNullException("lot");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.GetCardgrantsByLot(parkingID, lot, out ErrorMessage);
        }
        public static bool Add(ParkGrant model)
        {
            if (model == null) throw new ArgumentNullException("model");
            model.GID = GuidGenerator.GetGuidString();
            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkGrant>(model, OperateType.Add);
            }
            return result;
        }
        public static bool Add(List<ParkGrant> models)
        {
            if (models == null || models.Count == 0) throw new ArgumentNullException("models");
            models.ForEach(p => p.GID = GuidGenerator.GetGuidString());
            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.Add(models);
            if (result)
            {
                foreach (var item in models)
                {
                    OperateLogServices.AddOperateLog<ParkGrant>(item, OperateType.Add);
                }

            }
            return result;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="grantlist"></param>
        /// <param name="Amout"></param>
        /// <param name="EndTime"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool RefundCardAmout(ParkGrant grantmodel, DateTime EndTime, ParkOrder model)
        {
            if (grantmodel == null || model == null) throw new ArgumentNullException("models");
            model.RecordID = GuidGenerator.GetGuidString();
            IParkGrant factory = ParkGrantFactory.GetFactory();
            List<ParkGrant> list = new List<ParkGrant>();
            if (grantmodel.PKLot == null || grantmodel.PKLot.Trim() == "")
            {
                list.Add(grantmodel);
            }
            else
            {
                string mesg = "";
                list = factory.GetCardgrantsByLot(grantmodel.PKID, grantmodel.PKLot, out mesg);
            }

            bool result = factory.RefundCardAmout(list, EndTime, model);
            if (result)
            {
                foreach (var item in list)
                {
                    OperateLogServices.AddOperateLog<ParkGrant>(item, OperateType.Update);
                }
                OperateLogServices.AddOperateLog<ParkOrder>(model, OperateType.Add);
            }
            return result;
        }
        public static bool Add(BaseEmployee employee, EmployeePlate plate, BaseCard card, ParkGrant parkGrant)
        {

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = Add(employee, plate,card, parkGrant, dbOperator);
                    if (!result) throw new MyException("保存车辆信息失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Add(BaseEmployee employee, EmployeePlate plate, BaseCard card, ParkGrant parkGrant, DbOperator dbOperator)
        {
            if (employee == null) throw new ArgumentNullException("employee");
            if (plate == null) throw new ArgumentNullException("plate");
            if (card == null) throw new ArgumentNullException("card");
            if (parkGrant == null) throw new ArgumentNullException("parkGrant");

            bool result = BaseEmployeeServices.AddOrUpdateBaseEmployee(employee, dbOperator);
            if (!result) throw new MyException("保存人员信息失败");

            result = BaseCardServices.AddOrUpdateCard(card, dbOperator);
            if (!result) throw new MyException("保存卡信息失败");

            result = EmployeePlateServices.AddOrUpdateEmployeePlate(plate, dbOperator);
            if (!result) throw new MyException("保存车牌信息失败");

            parkGrant.PlateID = plate.PlateID;
            parkGrant.CardID = card.CardID;
            result = AddOrderUpdateParkGrant(parkGrant, dbOperator);
            if (!result) throw new MyException("保存授权失败");
            if (result)
            {
                OperateLogServices.AddOperateLog<BaseEmployee>(employee, OperateType.Add);
                OperateLogServices.AddOperateLog<EmployeePlate>(plate, OperateType.Add);
                OperateLogServices.AddOperateLog<BaseCard>(card, OperateType.Add);
                OperateLogServices.AddOperateLog<ParkGrant>(parkGrant, OperateType.Add);
            }
            return result;
        }
        public static bool DAAdd(List<DAAddMonth> listmonth)
        {
            if (listmonth.Count == 0) throw new ArgumentNullException("employee");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.BeginTransaction();
                try
                {
                    foreach (var obj in listmonth)
                    {
                        bool result = BaseEmployeeServices.AddOrUpdateBaseEmployee(obj.employye, dbOperator);
                        if (!result) throw new MyException("保存人员信息失败");

                        result = BaseCardServices.AddOrUpdateCard(obj.card, dbOperator);
                        if (!result) throw new MyException("保存卡信息失败");

                        result = EmployeePlateServices.AddOrUpdateEmployeePlate(obj.plate, dbOperator);
                        if (!result) throw new MyException("保存车牌信息失败");

                        obj.grant.PlateID = obj.plate.PlateID;
                        obj.grant.CardID = obj.card.CardID;
                        result = AddOrderUpdateParkGrant(obj.grant, dbOperator);
                        if (!result) throw new MyException("保存授权失败");
                        dbOperator.CommitTransaction();
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

        public static bool AddOrderUpdateParkGrant(ParkGrant parkGrant, DbOperator dbOperator)
        {
            IParkGrant grantFactory = ParkGrantFactory.GetFactory();
            ParkGrant oldGrant = grantFactory.QueryByCardIdAndParkingId(parkGrant.CardID, parkGrant.PKID);
            if (oldGrant != null)
            {
                parkGrant.GID = oldGrant.GID;

                parkGrant.BeginDate = oldGrant.BeginDate;

                parkGrant.EndDate = oldGrant.EndDate;
            }
            else
            {
                parkGrant.GID = GuidGenerator.GetGuidString();
            }
            //检查车位号是否有效
            if (!string.IsNullOrWhiteSpace(parkGrant.PKLot))
            {
                List<ParkGrant> oldGrants = grantFactory.QueryByParkingAndLotAndCarType(parkGrant.PKID, parkGrant.PKLot, BaseCarType.MonthlyRent, parkGrant.GID);
                foreach (var item in oldGrants)
                {
                    int oldLotLen = item.PKLot.Split(',').Length;
                    int newLotLen = parkGrant.PKLot.Split(',').Length;
                    if (oldLotLen != newLotLen)
                    {
                        throw new MyException(string.Format("车位号无效：车位号[{0}]与已存在的车位号[{1}]不完全一致", parkGrant.PKLot, item.PKLot));
                    }
                    parkGrant.BeginDate = item.BeginDate;
                    parkGrant.EndDate = item.EndDate;
                }
            }
            //临停转月卡 并且车位号存在完全一致的情况 给开始日期和结束日期赋值
            if (oldGrant != null && !string.IsNullOrWhiteSpace(parkGrant.PKLot))
            {
                ParkCarType newCarType = ParkCarTypeServices.QueryParkCarTypeByRecordId(parkGrant.CarTypeID);
                ParkCarType oldCarType = ParkCarTypeServices.QueryParkCarTypeByRecordId(oldGrant.CarTypeID);
                if (newCarType == null || oldCarType == null) throw new MyException("车类不存在");

                if (newCarType.BaseTypeID == BaseCarType.MonthlyRent && oldCarType.BaseTypeID == BaseCarType.TempCar)
                {
                    List<ParkGrant> oldGrants = grantFactory.QueryByParkingAndLotAndCarType(parkGrant.PKID, parkGrant.PKLot, BaseCarType.MonthlyRent, parkGrant.GID);
                    foreach (var item in oldGrants)
                    {
                        int oldLotLen = item.PKLot.Split(',').Length;
                        int newLotLen = parkGrant.PKLot.Split(',').Length;
                        if (oldLotLen == newLotLen)
                        {
                            parkGrant.BeginDate = item.BeginDate;
                            parkGrant.EndDate = item.EndDate;
                            break;
                        }
                    }
                }
            }
            if (oldGrant != null)
            {
                return grantFactory.Update(parkGrant, dbOperator);
            }
            else
            {
                return grantFactory.Add(parkGrant, dbOperator);
            }
        }

        public static bool Update(ParkGrant model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (model.GID.IsEmpty()) throw new ArgumentNullException("GID");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkGrant>(model, OperateType.Update);
            }
            return result;
        }

        //public static bool Update(ParkGrant model, DbOperator dbOperator);

        public static ParkGrant QueryByPlateNumber(string plateNumber)
        {
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByPlateNumber(plateNumber);
        }

        public static ParkGrant QueryByGrantId(string grantId)
        {
            if (grantId.IsEmpty()) throw new ArgumentNullException("grantId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByGrantId(grantId);
        }
        public static bool Delete(string grantId)
        {
            if (grantId.IsEmpty()) throw new ArgumentNullException("grantId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.Delete(grantId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", grantId));
            }
            return result;
        }

        public static bool Delete(string cardId, string parkingId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.Delete(cardId, parkingId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("cardId:{0},parkingId:{1}", cardId, parkingId));
            }
            return result;
        }

        public static bool DeleteByCardId(string cardId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            bool result = factory.DeleteByCardId(cardId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("cardId:{0}", cardId));
            }
            return result;
        }
        public static bool CancelParkGrant(string grantId)
        {
            if (grantId.IsEmpty()) throw new ArgumentNullException("grantId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            IBaseCard cardFactory = BaseCardFactory.GetFactory();
            IEmployeePlate plateFactory = EmployeePlateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    ParkGrant grant = factory.QueryByGrantId(grantId);

                    dbOperator.BeginTransaction();
                    bool result = plateFactory.Delete(grant.PlateID, dbOperator);
                    if (!result) throw new MyException("删除车牌失败");

                    result = cardFactory.Delete(grant.CardID, dbOperator);
                    if (!result) throw new MyException("删除卡失败");

                    result = factory.Delete(grantId, dbOperator);
                    if (!result) throw new MyException("删除授权失败");
                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("grantId:{0}", grantId));
                    }
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="grantId">以逗号隔开的id串</param>
        /// <returns></returns>
        public static bool CancelAllParkGrant(string grantId)
        {
            if (grantId.IsEmpty()) throw new ArgumentNullException("grantId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            IBaseCard cardFactory = BaseCardFactory.GetFactory();
            IEmployeePlate plateFactory = EmployeePlateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    string[] grantIds = grantId.Split(',');
                    bool success = true;
                    for (int i = 0; i < grantIds.Length; i++)
                    {
                        ParkGrant grant = factory.QueryByGrantId(grantIds[i]);

                        dbOperator.BeginTransaction();
                        bool result = plateFactory.Delete(grant.PlateID, dbOperator);
                        if (!result)
                        {
                            success = false;
                            throw new MyException("删除" + grantIds[i] + "车牌失败");
                        }

                        result = cardFactory.Delete(grant.CardID, dbOperator);
                        if (!result)
                        {
                            success = false;
                            throw new MyException("删除" + grant.CardID + "卡失败");
                        }

                        result = factory.Delete(grantIds[i], dbOperator);
                        if (!result)
                        {
                            success = false;
                            throw new MyException("删除" + grantIds[i] + "授权失败");
                        }
                        dbOperator.CommitTransaction();
                        if (result)
                        {
                            OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("grantId:{0}", grantIds[i]));
                        }
                    }

                    return success;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        public static List<ParkGrant> QueryNormalParkGrant(string parkingId)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryNormalParkGrant(parkingId);
        }

        public static List<ParkGrant> QueryByParkingId(string parkingId)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }

        public static List<ParkGrant> QueryByParkingIds(List<string> parkingIds)
        {
            if (parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByParkingIds(parkingIds);
        }

        public static List<ParkGrant> QueryByCardIds(List<string> cardIds)
        {
            if (cardIds.Count == 0) throw new ArgumentNullException("cardIds");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByCardIds(cardIds);
        }

        public static List<ParkGrant> QueryByCardIdAndParkingIds(string cardId, List<string> parkingIds)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");
            if (parkingIds.Count == 0) throw new ArgumentNullException("cardIds");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByCardIdAndParkingIds(cardId, parkingIds);
        }

        public static ParkGrant QueryByCardIdAndParkingId(string cardId, string parkingId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByCardIdAndParkingId(cardId, parkingId);
        }

        public static List<ParkGrant> QueryByCardId(string cardId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByCardId(cardId);
        }

        public static List<ParkGrant> QueryByParkingAndPlateId(string parkingId, string plateId)
        {
            if (plateId.IsEmpty()) throw new ArgumentNullException("plateId");
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByParkingAndPlateId(parkingId, plateId);
        }

        public static List<ParkGrant> QueryByPlateId(string plateId)
        {
            if (plateId.IsEmpty()) throw new ArgumentNullException("plateId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByPlateId(plateId);
        }


        public static ParkGrant QueryByParkingAndPlateNumber(string parkingId, string plateNumber)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByParkingAndPlateNumber(parkingId, plateNumber);
        }

        public static List<ParkGrant> QueryByParkingAndLotAndCarType(string parkingId, string lots, BaseCarType carType, string excludeGrantId)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (lots.IsEmpty()) throw new ArgumentNullException("lots");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryByParkingAndLotAndCarType(parkingId, lots, carType, excludeGrantId);
        }

        public static List<ParkGrantView> QueryPage(ParkGrantCondition condition, int pagesize, int pageindex, out int total)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryPage(condition, pagesize, pageindex, out total);
        }

        public static List<ParkGrantView> QueryPage1(ParkGrantCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryPage1(condition);
        }

        public static List<ParkGrant> Query(List<string> parkingIds, string plateNumber, BaseCarType carType)
        {
            if (parkingIds.Count == 0) throw new ArgumentNullException("parkingIds");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.Query(parkingIds, plateNumber, carType);
        }

        public static List<ParkGrant> QueryHasLotByParkingId(string parkingId, BaseCarType carType)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryHasLotByParkingId(parkingId, carType);
        }
         public static ParkCardSuspendPlan QueryByGrantID(string grantid)
        {
            IParkCardSuspendPlan factory= ParkCardSuspendPlanFactory.GetFactory();
            ParkCardSuspendPlan plan = factory.QueryByGrantId(grantid);
            return plan;
        }
        /// <summary>
        /// 续期或续费
        /// </summary>
        /// <param name="grantId"></param>
        /// <param name="renewalMonth"></param>
        /// <param name="payTotalMoney"></param>
        /// <param name="operatorId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool RenewalsOrRecharge(string grantId, int renewalMonth, decimal payTotalMoney, string operatorId, DateTime startDate, DateTime endDate)
        {
            lock (parkGrant_lock)
            {
                ParkGrant grant = ParkGrantServices.QueryByGrantId(grantId);
                if (grant == null) throw new MyException("获取授权信息失败");

                if (endDate != null && endDate != DateTime.MinValue)
                {
                    endDate = endDate.Date.AddDays(1).AddSeconds(-1);
                }
                string errorMsg = string.Empty;
                BaseCard card = BaseCardServices.GetBaseCard(grant.CardID, out errorMsg);
                if (card == null || !string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取卡失败");

                ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(grant.CarTypeID);
                if (carType == null) throw new MyException("获取卡类失败");

                List<ParkGrant> operateTargets = new List<ParkGrant>();
                List<string> ioRecords = new List<string>();

                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    try
                    {
                        bool renewalsResult = false;
                        dbOperator.BeginTransaction();
                        switch (carType.BaseTypeID)
                        {
                            case BaseCarType.StoredValueCar:
                                {
                                    IBaseCard factory = BaseCardFactory.GetFactory();
                                    if (payTotalMoney > 0)
                                    {
                                        ParkOrder order = ParkOrderServices.AddStoredValueCarOrder(grant.PKID, payTotalMoney, card, operatorId, dbOperator);
                                        if (order == null) throw new MyException("创建充值订单失败");

                                        decimal cardtotalmoney = card.Balance + payTotalMoney;
                                        if (order.NewMoney != cardtotalmoney) throw new MyException("充值金额计算错误");
                                        renewalsResult = factory.Recharge(card.CardID, payTotalMoney, dbOperator);
                                        if (!renewalsResult) throw new MyException("卡充值失败【更改卡余额失败】");
                                    }
                                    renewalsResult = factory.SetEndDate(card.CardID, endDate, dbOperator);
                                    if (!renewalsResult) throw new MyException("卡充值失败【更改有效期失败】");
                                    break;
                                }
                            case BaseCarType.VIPCar:
                                {
                                    ParkOrder order = ParkOrderServices.AddVipCardOrder(grant.PKID, grantId, grant.BeginDate, grant.EndDate, startDate, endDate, renewalMonth, operatorId, dbOperator);
                                    if (order == null) throw new MyException("充值失败【创建充值订单失败】");

                                    IParkGrant factory = ParkGrantFactory.GetFactory();
                                    renewalsResult = factory.Renewals(grant.GID, startDate, endDate, dbOperator);
                                    if (!renewalsResult) throw new MyException("修改车辆有效期失败");
                                    break;
                                }
                            case BaseCarType.SeasonRent:
                            case BaseCarType.YearRent:
                            case BaseCarType.MonthlyRent:
                            case BaseCarType.CustomRent:
                                {

                                    int pkLotQuantity = 1;
                                    if (!string.IsNullOrWhiteSpace(grant.PKLot))
                                    {
                                        pkLotQuantity = grant.PKLot.TrimEnd(',').Split(',').Length;
                                    }
                                    decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(startDate, grantId);

                                    //if (payTotalMoney != ((carType.Amount * renewalMonth * pkLotQuantity) + amount))
                                    //    throw new MyException("凭证续期金额计算有误");



                                    List<ParkGrant> shareCardGrants = new List<ParkGrant>();
                                    if (!string.IsNullOrWhiteSpace(grant.PKLot))
                                    {
                                        shareCardGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.MonthlyRent, grant.GID);
                                    }
                                    IParkGrant factory = ParkGrantFactory.GetFactory();
                                    renewalsResult = factory.Renewals(grant.GID, startDate, endDate, dbOperator);
                                    if (!renewalsResult) throw new MyException("修改车辆有效期失败");


                                    //修改多车多位的有效期
                                    foreach (var item in shareCardGrants)
                                    {
                                        if (string.IsNullOrWhiteSpace(item.PKLot)) continue;

                                        int len1 = item.PKLot.TrimEnd(',').Split(',').Length;
                                        int len2 = grant.PKLot.TrimEnd(',').Split(',').Length;
                                        if (len1 != len2) continue;

                                        item.BeginDate = startDate;
                                        item.EndDate = endDate;
                                        factory.Update(item, dbOperator);
                                        operateTargets.Add(item);
                                    }
                                    //过期转临停订单处理
                                    if (grant.EndDate.Date < DateTime.Now.Date)
                                    {
                                        List<string> plateNos = new List<string>();
                                        EmployeePlate plate = EmployeePlateServices.Query(grant.PlateID);
                                        if (plate == null) throw new MyException("授权车牌信息不存在");

                                        plateNos.Add(plate.PlateNo);

                                        foreach (var item in shareCardGrants)
                                        {
                                            if (string.IsNullOrWhiteSpace(item.PKLot)) continue;

                                            int len1 = item.PKLot.TrimEnd(',').Split(',').Length;
                                            int len2 = grant.PKLot.TrimEnd(',').Split(',').Length;
                                            if (len1 != len2) continue;

                                            EmployeePlate otherplate = EmployeePlateServices.Query(item.PlateID);
                                            if (otherplate == null) throw new MyException("多车多位存在无效的车牌");
                                            plateNos.Add(otherplate.PlateNo);
                                        }

                                        if (plateNos.Count > 0 && grant.EndDate!=DateTime.MinValue)
                                        {
                                            IParkIORecord ioRecord = ParkIORecordFactory.GetFactory();
                                            ioRecords = ioRecord.QueryMonthExpiredNotPayAmountIORecordIds(grant.EndDate.AddDays(1).Date, startDate, grant.PKID, plateNos);
                                            if (ioRecords.Count > 0)
                                            {
                                                bool result = ioRecord.UpdateIORecordEnterType(ioRecords, 0, dbOperator);
                                                if (!result) throw new MyException("修改进出记录类型失败");
                                                result = ParkOrderServices.AddExpiredToProStopOrder(ioRecords, operatorId, dbOperator);
                                                if (!result) throw new MyException("创建月卡转临停订单失败");
                                            }
                                        }
                                    }
                                    ParkOrder order = ParkOrderServices.AddMonthlyRentOrderCS(grant.PKID, carType.CarTypeID, grant.GID, grant.BeginDate, grant.EndDate, startDate, endDate, renewalMonth, pkLotQuantity, operatorId, payTotalMoney, dbOperator);
                                    if (order == null) throw new MyException("续期失败【创建续期订单失败】");
                                    break;
                                }
                            //case BaseCarType.WorkCar:
                            //    {
                            //        IParkGrant factory = ParkGrantFactory.GetFactory();
                            //        renewalsResult = factory.Renewals(grant.GID, startDate, endDate, dbOperator);
                            //        if (!renewalsResult) throw new MyException("修改卡结束时间错误");
                            //        break;
                            //    }
                            default: throw new MyException("选择的车辆不能续期和充值");
                        }
                        dbOperator.CommitTransaction();

                        string remark = string.Format("grantId：{0}；renewalMonth：{1}；payTotalMoney：{2}；operatorId：{3}；startDate：{4}；endDate：{5}", grantId, renewalMonth, payTotalMoney, operatorId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                        OperateLogServices.AddOperateLog(OperateType.Update, remark);
                        if (ioRecords.Count > 0)
                        {
                            string updateIORecord = string.Format("修改IORecord表，RecordIDs:{0},EnterType:0", string.Join(",", ioRecords));
                            OperateLogServices.AddOperateLog(OperateType.Update, updateIORecord);
                        }
                        return renewalsResult;
                    }
                    catch (Exception ex)
                    {
                        dbOperator.RollbackTransaction();
                        throw ex;
                    }
                }

            }
        }

        /// <summary>
        /// 月租车暂停使用
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="grantId"></param>
        /// <returns></returns>
        public static bool CarSuspendUse(DateTime start, DateTime end, string grantId,DateTime NewEndDate)
        {
            IParkGrant factory = ParkGrantFactory.GetFactory();
            ParkGrant grant = factory.QueryByGrantId(grantId);
            if (grant == null) throw new MyException("获取授权信息失败");

            if (grant.State == ParkGrantState.Stop) throw new MyException("停止状态不能设置，如需设置请先启用");
            if (grant.State == ParkGrantState.Pause) throw new MyException("暂停状态不能设置，请先恢复使用后重新设置");

            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(grant.CarTypeID);
            if (carType == null) throw new MyException("获取车型类型失败");
            //if (carType.BaseTypeID != BaseCarType.MonthlyRent && carType.BaseTypeID != BaseCarType.WorkCar
            //   && carType.BaseTypeID != BaseCarType.VIPCar) throw new MyException("该车辆不能使用此功能");

            IParkCardSuspendPlan planFactory = ParkCardSuspendPlanFactory.GetFactory();

            ParkCardSuspendPlan plan = planFactory.QueryByGrantId(grantId);
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    if (plan != null)
                    {
                        if (start.Date < DateTime.Now.Date) throw new MyException("开始时间不能小于系统当前时间");

                        bool result = planFactory.Update(start, end, grantId, dbOperator);
                        if (!result) throw new MyException("保存暂停计划失败");

                        if (start.Date == DateTime.Now.Date)
                        {
                            result = factory.Update(grantId, ParkGrantState.Pause, dbOperator, NewEndDate);
                            if (!result) throw new MyException("修改使用状态失败");
                        }
                    }
                    else
                    {
                        if (start.Date < DateTime.Now.Date) throw new MyException("开始时间不能小于系统当前时间");

                        ParkCardSuspendPlan model = new ParkCardSuspendPlan();
                        model.RecordId = GuidGenerator.GetGuidString();
                        model.StartDate = start;
                        model.EndDate = end;
                        model.GrantID = grantId;
                        bool result = planFactory.Add(model, dbOperator);
                        if (!result) throw new MyException("保存暂停计划失败");
                        if (start.Date == DateTime.Now.Date||NewEndDate!=DateTime.MinValue)
                        {
                            result = factory.Update(grantId, ParkGrantState.Pause, dbOperator, NewEndDate);
                            if (!result) throw new MyException("修改使用状态失败");
                        }
                    }
                    dbOperator.CommitTransaction();
                    string endDate = end==DateTime.MinValue ? end.ToString("yyyy-MM-dd") : string.Empty;
                    OperateLogServices.AddOperateLog(OperateType.Update, string.Format("grantId:{0},startDate:{1},endDate:{2}", grantId, start.ToString("yyyy-MM-dd"), endDate));
                    return true;
                }
                catch (Exception ex)
                {
                    dbOperator.RollbackTransaction();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 月租车恢复使用
        /// </summary>
        /// <param name="cardGrantId"></param>
        /// <returns></returns>
        public static bool CarRestoreUse(string grantId,DateTime NewRestoreDate)
        {
            IParkGrant factory = ParkGrantFactory.GetFactory();
            ParkGrant grant = factory.QueryByGrantId(grantId);
            if (grant == null) throw new MyException("获取授权信息失败");
            if (grant.State == ParkGrantState.Normal) return true;

            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(grant.CarTypeID);
            if (carType == null) throw new MyException("获取车型类型失败");

            IParkCardSuspendPlan planFactory = ParkCardSuspendPlanFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    ParkCardSuspendPlan plan = planFactory.QueryByGrantId(grantId);
                    DateTime newEndDate = ComputeParkGrantNewEndDate(grant, plan);
                    grant.BeginDate = grant.BeginDate == null ? DateTime.Now.Date : grant.BeginDate;
                    bool result = factory.RestoreUse(grantId, grant.BeginDate, NewRestoreDate, dbOperator);
                    if (!result) throw new MyException("恢复暂停失败");

                    result = planFactory.Delete(grantId, dbOperator);
                    if (!result) throw new MyException("取消暂停计划失败");
                    dbOperator.CommitTransaction();

                    ParkGrant dbGrant = factory.QueryByGrantId(grantId);
                    if (dbGrant != null)
                    {
                        OperateLogServices.AddOperateLog<ParkGrant>(dbGrant, OperateType.Update);
                    }
                    OperateLogServices.AddOperateLog(OperateType.Update, string.Format("CarRestore.GrantID:{0}", grantId));
                    return result;
                }
                catch (Exception ex)
                {
                    dbOperator.RollbackTransaction();
                    throw ex;
                }
            }
        }
        private static DateTime ComputeParkGrantNewEndDate(ParkGrant grant, ParkCardSuspendPlan plan)
        {

            int suspendDays = (DateTime.Now - plan.StartDate).Days;
            if (suspendDays <= 0)
            {
                if (grant.EndDate == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return grant.EndDate;
                }
            }

            if (grant.EndDate.Date < DateTime.Now.Date)
            {
                return DateTime.Now.Date.AddDays(suspendDays);
            }
            return grant.EndDate.Date.AddDays(suspendDays);
        }

        /// <summary>
        /// 车辆重新启用
        /// </summary>
        /// <param name="grantId"></param>
        /// <returns></returns>
        public static bool CarAgainEnabledUse(string grantId,DateTime EndDate)
        {
            IParkGrant factory = ParkGrantFactory.GetFactory();
            ParkGrant grant = factory.QueryByGrantId(grantId);
            if (grant == null) throw new MyException("获取授权信息失败");

            if (grant.State != ParkGrantState.Stop) throw new MyException("状态不正确：只有停用状态才能重新启用");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                bool result = factory.Update(grantId, ParkGrantState.Normal, dbOperator, EndDate);
                if (result)
                {
                    OperateLogServices.AddOperateLog(OperateType.Update, string.Format("grantId：{0},State：{1}", grantId, (int)ParkGrantState.Normal));
                }
                return result;
            }

        }
        /// <summary>
        /// 车辆停用
        /// </summary>
        /// <param name="grantId"></param>
        /// <returns></returns>
        public static bool CarStopUse(string grantId,DateTime EndDate)
        {
            IParkGrant factory = ParkGrantFactory.GetFactory();
            ParkGrant grant = factory.QueryByGrantId(grantId);
            if (grant == null) throw new MyException("获取授权信息失败");

            if (grant.State != ParkGrantState.Normal) throw new MyException("状态不正确：只有正在使用状态才能停用");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                bool result = factory.Update(grantId, ParkGrantState.Stop, dbOperator, EndDate);
                if (result)
                {
                    OperateLogServices.AddOperateLog(OperateType.Update, string.Format("grantId：{0},State：{1}", grantId, (int)ParkGrantState.Stop));
                }
                return result;
            }
        }

        public static List<ParkGrant> GetCardgrantByParkingID(string parkingID, out string errorMsg)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.GetCardgrantByParkingID(parkingID, out errorMsg);
        }
        public static List<CarParkingResult> GetParkGrantByPlateNo(string plateNo)
        {
            if (plateNo.IsEmpty()) throw new ArgumentNullException("plateNo");

            List<CarParkingResult> models = new List<CarParkingResult>();
            IParkGrant factory = ParkGrantFactory.GetFactory();
            List<ParkGrant> grants = factory.GetParkGrantByPlateNo(plateNo);
            if (grants.Count > 0)
            {
                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByRecordIds(grants.Select(p => p.CarTypeID).ToList());
                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByRecordIds(grants.Select(p => p.PKID).ToList());
                List<BaseVillage> villages = new List<BaseVillage>();
                if (parkings.Count > 0)
                {
                    IVillage factoryVillage = VillageFactory.GetFactory();
                    villages = factoryVillage.QueryVillageByRecordIds(parkings.Select(p => p.VID).ToList());
                }
                foreach (var item in grants)
                {
                    CarParkingResult model = new CarParkingResult();
                    model.PlateNo = plateNo;
                    BaseParkinfo parking = parkings.FirstOrDefault(p => p.PKID == item.PKID);

                    if (parking != null)
                    {
                        model.ParkingName = parkings != null ? parking.PKName : string.Empty;
                        BaseVillage village = villages.FirstOrDefault(p => p.VID == parking.VID);
                        model.VillageName = village != null ? village.VName : string.Empty;
                    }
                    ParkCarType carType = carTypes.FirstOrDefault(p => p.CarTypeID == item.CarTypeID);
                    if (carType != null)
                    {
                        model.CarTypeName = carType.CarTypeName;
                    }
                    model.StartTime = item.BeginDate;
                    model.EndTime = item.EndDate;
                    models.Add(model);
                }
            }
            return models;
        }


        public static List<ParkCarBitGroup> QueryCarBitGroupByParkingId(string parkingid)
        {
            if (parkingid.IsEmpty()) throw new ArgumentNullException("parkingid");

            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.QueryCarBitGroupByParkingId(parkingid);
        }
        public static ParkCarBitGroup GetParkCarBitGroupByRecordID(string recordId) {
            IParkGrant factory = ParkGrantFactory.GetFactory();
            return factory.GetParkCarBitGroupByRecordID(recordId);
        }
        public static bool AddOrUpdateCarBitGroup(ParkCarBitGroup model,out bool isAdd) {
            isAdd = false;
            if (model==null) throw new ArgumentNullException("model");

             IParkGrant factory = ParkGrantFactory.GetFactory();
             ParkCarBitGroup oldGroup = factory.GetParkCarBitGroup(model.PKID, model.CarBitName);

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                if (oldGroup==null)
                {
                    model.RecordID= GuidGenerator.GetGuidString();
                    isAdd = true;
                    return factory.AddParkCarBitGroup(model,dbOperator);
                }
                else {
                    try
                    {
                        dbOperator.BeginTransaction();
                        //factory.UpdateParkGrantPKLot(model.CarBitName,oldGroup.CarBitName, oldGroup.PKID, dbOperator);
                        model.RecordID = oldGroup.RecordID;
                        bool result = factory.UpdateParkCarBitGroup(oldGroup.RecordID, model.CarBitName,model.CarBitNum,dbOperator);
                        if (!result) throw new MyException("修改车位组信息失败");
                        dbOperator.CommitTransaction();
                        return result;
                    }
                    catch {
                        dbOperator.RollbackTransaction();
                        throw;
                    }
                   
                }
            }
        }
    }
}
