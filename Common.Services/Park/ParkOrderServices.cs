using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Entities;
using Common.DataAccess;
using Common.Utilities;

namespace Common.Services.Park
{
    public class ParkOrderServices
    {
        public static List<ParkOrder> GetOrderByTimeseriesID(string timeseriesID, out string errorMsg)
        {
            if (timeseriesID.IsEmpty()) throw new ArgumentNullException("timeseriesID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByTimeseriesID(timeseriesID, out errorMsg);
        }
        public static List<ParkOrder> GetOrderByIORecordID(string recordID, out string errorMsg)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByIORecordID(recordID, out errorMsg);
        }
        public static ParkOrder GetCashMoneyCountByPlateNumber(string parkingID, OrderType orderType, string plateNumber, DateTime startTime, DateTime endtime, out string errorMsg)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetCashMoneyCountByPlateNumber(parkingID, orderType, plateNumber, startTime, endtime, out errorMsg);
        }

        public static ParkOrder AddOrder(ParkOrder mode, out string errorMsg)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.AddOrder(mode, out errorMsg);
        }
        public static bool ModifyOrderStatusAndAmount(string recordID, decimal payAmount, decimal unPayamount, int status,int payWay, out string errorMsg)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.ModifyOrderStatusAndAmount(recordID, payAmount, unPayamount, status, payWay,out errorMsg);
        }

        public static bool ModifyOrderStatusAndAmount(string recordID, decimal amount, decimal payAmount, decimal unPayamount, int status, decimal Discountamount, string Carderateid, int payWay, out string ErrorMessage)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.ModifyOrderStatusAndAmount(recordID, amount, payAmount, unPayamount, status, Discountamount, Carderateid, payWay,out ErrorMessage);
        }
        public static bool ModifyOrderStatus(string recordID, int status, out string errorMsg)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.ModifyOrderStatus(recordID, status, out errorMsg);
        }
        public static List<ParkOrder> GetOrderByIORecordIDByStatus(string iorecordID, int status, out string errorMsg)
        {
            if (iorecordID.IsEmpty()) throw new ArgumentNullException("iorecordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByIORecordIDByStatus(iorecordID, status, out errorMsg);
        }
        public static List<ParkOrder> GetOrderByTimeseriesIDByStatus(string timeseriesID, int status, out string errorMsg)
        {
            if (timeseriesID.IsEmpty()) throw new ArgumentNullException("timeseriesID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByTimeseriesIDByStatus(timeseriesID, status, out errorMsg);
        }

        public static List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByStatus(startTime, endtime, out ErrorMessage);
        }
        public static ParkOrder GetTimeseriesOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetTimeseriesOrderChareFeeCount(userID, OrderType, dt, releaseType, out ErrorMessage);
        }
        public static ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, OrderSource orderSource, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetIORecordOrderChareFeeCount(userID, OrderType, dt, releaseType, orderSource, out ErrorMessage);
        }
        public static ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetIORecordOrderChareFeeCount(userID, OrderType, dt, releaseType, out ErrorMessage);
        }
        public static List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, int status, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByStatus(startTime, endtime, status, out ErrorMessage);
        }

        public static List<ParkOrder> QueryByIORecordIds(List<string> recordIds)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.QueryByIORecordIds(recordIds);
        }

        public static decimal CalculateMonthlyRentExpiredWaitPayAmount(DateTime start, string grantId)
        {
            ParkGrant grant = ParkGrantServices.QueryByGrantId(grantId);
            if (grant == null) throw new MyException("获取授权失败");
            if (grant.BeginDate==DateTime.MinValue || grant.EndDate == DateTime.MinValue || (grant.EndDate!= DateTime.MinValue && grant.EndDate.Date >= DateTime.Now.Date))
            {
                return 0;
            }
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(grant.CarTypeID);
            if (carType == null) throw new MyException("获取车类失败");

            if (carType.BaseTypeID != BaseCarType.MonthlyRent)
                return 0;

            DateTime startDate = grant.EndDate.AddDays(1).Date;
            DateTime endDate = start.Date;

            List<string> plateNos = new List<string>();
            EmployeePlate plate = EmployeePlateServices.Query(grant.PlateID);
            if (plate == null) throw new MyException("获取车牌号失败");

            plateNos.Add(plate.PlateNo);

            if (!string.IsNullOrWhiteSpace(grant.PKLot))
            {
                List<ParkGrant> sameGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.MonthlyRent, grant.GID);
                foreach (var item in sameGrants)
                {
                    int lot1 = grant.PKLot.Split(',').Length;
                    int lot2 = item.PKLot.Split(',').Length;
                    if (lot1 != lot2) continue;

                    EmployeePlate plate1 = EmployeePlateServices.Query(item.PlateID);
                    if (plate1 == null) throw new MyException("获取车牌号失败");
                    plateNos.Add(plate1.PlateNo);
                }
            }
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.QueryMonthExpiredNotPayAmount(startDate, endDate, grant.PKID, plateNos);
        }

        /// <summary>
        /// 生存商家充值订单
        /// </summary>
        /// <param name="seller">商家信息</param>
        /// <param name="chargeBalance">充值金额</param>
        /// <param name="operatorId">操作者编号</param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static ParkOrder MarkSellerChargeOrder(ParkSeller seller, decimal chargeBalance, string operatorId, OrderSource orderSource, OrderPayWay payWay, DbOperator dbOperator)
        {
            ParkOrder order = GetSellerChargeOrder(seller, chargeBalance, operatorId, orderSource, payWay);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }
        /// <summary>
        /// 构建商家充值充值订单
        /// </summary>
        /// <param name="seller">商家信息</param>
        /// <param name="chargeBalance">充值金额</param>
        /// <param name="systemId">系统编号</param>
        /// <param name="loginUserRecordId">登录用户编号</param>
        /// <returns></returns>
        private static ParkOrder GetSellerChargeOrder(ParkSeller seller, decimal chargeBalance, string operatorId, OrderSource orderSource,OrderPayWay payWay)
        {
            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.OrderSource = orderSource;
            order.Amount = chargeBalance;
            order.PayAmount = chargeBalance;
            order.Remark = "商家充值";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.BusinessRecharge;
            order.TagID = seller.SellerID;
            order.PayWay = payWay;
            order.UserID = operatorId;
            order.OldMoney = seller.Balance;
            order.NewMoney = chargeBalance + seller.Balance;
            order.PayTime = DateTime.Now;
            return order;
        }
        /// <summary>
        /// 生存储值卡续费订单
        /// </summary>
        /// <param name="rechargeMoney"></param>
        /// <param name="card"></param>
        /// <param name="operatorId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static ParkOrder AddStoredValueCarOrder(string parkingId, decimal rechargeMoney, BaseCard card, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GenerateStoredValueCarOrderModel(parkingId, rechargeMoney, card, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }
        /// <summary>
        /// 构建储值卡续费订单
        /// </summary>
        /// <param name="rechargeMoney">充值金额（分）</param>
        /// <param name="parkingId">车场编号</param>
        /// <param name="card">用户卡信息</param>
        /// <param name="operatorId">登录用户编号</param>
        /// <returns></returns>
        private static ParkOrder GenerateStoredValueCarOrderModel(string parkingId, decimal rechargeMoney, BaseCard card, string operatorId)
        {
            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.PKID = parkingId;
            order.OrderSource = OrderSource.ManageOffice;
            order.Amount = rechargeMoney;
            order.PayAmount = rechargeMoney;
            order.Remark = "储值卡续费";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.ValueCardRecharge;
            order.TagID = card.CardID;
            order.PayWay = OrderPayWay.Cash;
            order.UserID = operatorId;
            order.OldMoney = card.Balance;
            order.NewMoney = card.Balance + rechargeMoney;
            order.PayTime = DateTime.Now;
            return order;
        }
        /// <summary>
        /// 生存月卡续期订单
        /// </summary>
        /// <param name="carTypeId"></param>
        /// <param name="grantId"></param>
        /// <param name="originalStartDate"></param>
        /// <param name="originalEndDate"></param>
        /// <param name="newStartDate"></param>
        /// <param name="newEndDate"></param>
        /// <param name="renewalMonth"></param>
        /// <param name="pkLotQuantit"></param>
        /// <param name="operatorId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static ParkOrder AddMonthlyRentOrder(string parkingId, string carTypeId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, int pkLotQuantit, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GenerateMonthCardOrderModel(parkingId, carTypeId, grantId, originalEndDate, originalEndDate, newStartDate, newEndDate, renewalMonth, pkLotQuantit, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }

        public static ParkOrder AddMonthlyRentOrderCS(string parkingId, string carTypeId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, int pkLotQuantit, string operatorId, decimal Amount, DbOperator dbOperator)
        {
            ParkOrder order = GenerateMonthCardOrderModelCS(parkingId, carTypeId, grantId, originalEndDate, originalEndDate, newStartDate, newEndDate, renewalMonth, pkLotQuantit, Amount, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }
        /// <summary>
        /// 构建月卡续期订单
        /// </summary>
        /// <param name="carTypeId"></param>
        /// <param name="grantId"></param>
        /// <param name="originalEndDate"></param>
        /// <param name="newEndDate"></param>
        /// <param name="renewalMonth"></param>
        /// <param name="pkLotQuantit"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        private static ParkOrder GenerateMonthCardOrderModel(string parkingId, string carTypeId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, int pkLotQuantit, string operatorId)
        {
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) throw new MyException("获取卡类失败");

            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.Amount = carType.Amount * renewalMonth * pkLotQuantit;
            order.PayAmount = order.Amount;
            order.Remark = "月卡续费";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.MonthCardPayment;
            order.PKID = parkingId;
            order.TagID = grantId;
            order.PayWay = OrderPayWay.Cash;
            order.OrderSource = OrderSource.ManageOffice;
            order.UserID = operatorId;
            order.OldUserBegin = originalStartDate;
            order.OldUserulDate = originalEndDate;
            order.NewUserBegin = newStartDate;
            order.NewUsefulDate = newEndDate;
            order.OldMoney = 0;
            order.NewMoney = 0;
            order.PayTime = DateTime.Now;
            return order;
        }

        private static ParkOrder GenerateMonthCardOrderModelCS(string parkingId, string carTypeId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, int pkLotQuantit, decimal Amount, string operatorId)
        {
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) throw new MyException("获取卡类失败");

            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.Amount = Amount;
            order.PayAmount = order.Amount;
            order.Remark = "月卡续费";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.MonthCardPayment;
            order.PKID = parkingId;
            order.TagID = grantId;
            order.PayWay = OrderPayWay.Cash;
            order.OrderSource = OrderSource.ManageOffice;
            order.UserID = operatorId;
            order.OldUserBegin = originalStartDate;
            order.OldUserulDate = originalEndDate;
            order.NewUserBegin = newStartDate;
            order.NewUsefulDate = newEndDate;
            order.OldMoney = 0;
            order.NewMoney = 0;
            order.PayTime = DateTime.Now;
            return order;
        }

        /// <summary>
        /// 生存VIP卡续期订单
        /// </summary>
        /// <param name="grantId"></param>
        /// <param name="originalStartDate"></param>
        /// <param name="originalEndDate"></param>
        /// <param name="newStartDate"></param>
        /// <param name="newEndDate"></param>
        /// <param name="renewalMonth"></param>
        /// <param name="operatorId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static ParkOrder AddVipCardOrder(string parkingId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GenerateVipCardOrderModel(parkingId, grantId, originalStartDate, originalEndDate, newEndDate, newEndDate, renewalMonth, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }

        /// <summary>
        /// 构建VIP卡续期订单
        /// </summary>
        /// <param name="grantId"></param>
        /// <param name="originalLimitEnd"></param>
        /// <param name="newLimitEnd"></param>
        /// <param name="renewalMonth"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        private static ParkOrder GenerateVipCardOrderModel(string parkingId, string grantId, DateTime originalStartDate, DateTime originalEndDate, DateTime newStartDate, DateTime newEndDate, int renewalMonth, string operatorId)
        {
            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.Amount = 0;
            order.PayAmount = 0;
            order.PKID = parkingId;
            order.Remark = "VIP卡续期";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.VIPCardRenewal;
            order.TagID = grantId;
            order.PayWay = OrderPayWay.Cash;
            order.OrderSource = OrderSource.ManageOffice;
            order.UserID = operatorId;
            order.OldUserBegin = originalStartDate;
            order.OldUserulDate = originalEndDate;
            order.NewUserBegin = newStartDate;
            order.NewUsefulDate = newEndDate;
            order.OldMoney = 0;
            order.NewMoney = 0;
            order.PayTime = DateTime.Now;
            return order;
        }
        /// <summary>
        /// 生存发卡押金订单信息
        /// </summary>
        /// <param name="card"></param>
        /// <param name="operatorId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static ParkOrder MarkUserCardDepositPKOrder(BaseCard card, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GetDepositPKOrder(card, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }
        /// <summary>
        /// 构建发卡片押金订单
        /// </summary>
        /// <param name="card"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        private static ParkOrder GetDepositPKOrder(BaseCard card, string operatorId)
        {
            ParkOrder order = new ParkOrder();
            order.RecordID = GuidGenerator.GetGuidString();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.PayWay = OrderPayWay.Cash;
            order.OrderSource = OrderSource.ManageOffice;
            order.Amount = card.Deposit;
            order.PayAmount = card.Deposit;
            order.Remark = "卡片押金";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.CardDeposit;
            order.TagID = card.CardID;
            order.Amount = order.Amount;
            order.UserID = operatorId;
            order.OldMoney = 0;
            order.NewMoney = 0;
            order.PayTime = DateTime.Now;
            return order;
        }
        /// <summary>
        /// 生存返还卡片押金订单
        /// </summary>
        /// <param name="card"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public static ParkOrder MarkUserCardDepositRefundOrder(BaseCard card, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GetUserCardDepositRefundOrder(card, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }

        /// <summary>
        /// 构建返还卡片押金订单
        /// </summary>
        /// <param name="usercard"></param>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static ParkOrder GetUserCardDepositRefundOrder(BaseCard usercard, string operatorId)
        {
            ParkOrder order = new ParkOrder();
            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.Status = 1;
            order.PayWay = OrderPayWay.Cash;
            order.OrderSource = OrderSource.ManageOffice;
            order.Amount = -usercard.Deposit;
            order.PayAmount = -usercard.Deposit;
            order.Remark = "卡片押金-退还";
            order.OrderTime = DateTime.Now;
            order.OrderType = OrderType.CardDeposit;
            order.TagID = usercard.CardID;
            order.Amount = order.Amount;
            order.UserID = operatorId;
            order.OldMoney = 0;
            order.NewMoney = 0;
            order.PayTime = DateTime.Now;
            return order;
        }

        /// <summary>
        /// 过期转临停订单处理
        /// </summary>
        /// <param name="ioRecordIds"></param>
        /// <param name="operatorId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public static bool AddExpiredToProStopOrder(List<string> ioRecordIds, string operatorId, DbOperator dbOperator)
        {
            if (ioRecordIds.Count == 0) return false;

            IParkOrder factory = ParkOrderFactory.GetFactory();
            List<ParkOrder> orders = factory.QueryByIORecordIds(ioRecordIds);
            if (orders.Count == 0) return true;

            bool result = factory.UpdateOrderStatusByIORecordIds(ioRecordIds, -1, dbOperator);
            if (!result) return false;

            foreach (var item in orders)
            {
                item.OrderNo = IdGenerator.Instance.GetId().ToString();
                item.TagID = item.RecordID;
                item.Status = 1;
                item.OrderSource = OrderSource.ManageOffice;
                item.OrderTime = DateTime.Now;
                item.UserID = operatorId;
                item.Amount = item.Amount;
                item.PayAmount = item.PayAmount;
                item.OldMoney = item.OldMoney;
                item.NewMoney = item.NewMoney;
                item.PayWay = OrderPayWay.Cash;
                item.Remark = "过期转临停关联订单记录：" + item.RecordID;
                item.PayTime = DateTime.Now;
                item.CashTime = item.PayTime;
                item.CashMoney = item.CashMoney;
                item.PayTime = DateTime.Now;
                ParkOrder order = factory.Add(item, dbOperator);
                if (order == null) return false;
            }
            return true;
        }

        /// <summary>
        /// 生存中央缴费订单
        /// </summary>
        /// <param name="parkingId">车场编号</param>
        /// <param name="ioRecordId">记录编号</param>
        /// <param name="amount">订单金额</param>
        /// <param name="payAmount">已付金额</param>
        /// <param name="discountAmount">优惠金额</param>
        /// <param name="carderateIds">优免券编号</param>
        /// <param name="feeRuleId">收费规则编号</param>
        /// <param name="cashTime">结余时间</param>
        /// <param name="cashMoney">结余金额</param>
        /// <param name="operatorId">操作员编号</param>
        /// <returns></returns>
        public static ParkOrder AddCentralFeeOrder(string parkingId, string ioRecordId, decimal amount, decimal payAmount, decimal discountAmount,
            List<string> carderateIds, string feeRuleId, DateTime cashTime, decimal cashMoney, string operatorId, DbOperator dbOperator)
        {
            ParkOrder order = GenerateCentralFeeOrderModel(parkingId, ioRecordId, amount, payAmount, discountAmount, carderateIds, feeRuleId, cashTime, cashMoney, operatorId);
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.Add(order, dbOperator);
        }
        /// <summary>
        /// 构建中央缴费订单
        /// </summary>
        /// <param name="parkingId">车场编号</param>
        /// <param name="ioRecordId">记录编号</param>
        /// <param name="amount">订单金额</param>
        /// <param name="payAmount">已付金额</param>
        /// <param name="discountAmount">优惠金额</param>
        /// <param name="carderateIds">优免券编号</param>
        /// <param name="feeRuleId">收费规则编号</param>
        /// <param name="cashTime">结余时间</param>
        /// <param name="cashMoney">结余金额</param>
        /// <param name="operatorId">操作员编号</param>
        /// <returns></returns>
        private static ParkOrder GenerateCentralFeeOrderModel(string parkingId, string ioRecordId, decimal amount, decimal payAmount, decimal discountAmount,
            List<string> carderateIds, string feeRuleId, DateTime cashTime, decimal cashMoney, string operatorId)
        {

            ParkOrder order = new ParkOrder();

            order.OrderNo = IdGenerator.Instance.GetId().ToString();
            order.TagID = ioRecordId;
            order.OrderType = OrderType.TempCardPayment;
            order.PayWay = OrderPayWay.Cash;
            order.Amount = amount;
            order.UnPayAmount = 0;
            order.PayAmount = payAmount;
            order.DiscountAmount = discountAmount;
            if (carderateIds != null && carderateIds.Count > 0)
            {
                order.CarderateID = string.Join(",", carderateIds);
            }

            order.Status = 1;
            order.OrderSource = OrderSource.CenterCharge;
            DateTime now = DateTime.Now;
            order.OrderTime = now;
            order.PayTime = now;
            order.PKID = parkingId;
            order.UserID = operatorId;
            order.Remark = "中央收费";
            order.CashMoney = cashMoney;
            order.CashTime = cashTime;
            order.FeeRuleID = feeRuleId;
            return order;
        }


        public static List<ParkOrder> GetOrderByTagID(string tagID, out string errorMsg)
        {
            if (tagID.IsEmpty()) throw new ArgumentNullException("tagID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByTagID(tagID, out errorMsg);
        }


        public static bool ModifyOrderTagIDAndOrderType(string recordID, string tagid, int orderType, out string errorMsg)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.ModifyOrderTagIDAndOrderType(recordID, tagid, orderType, out errorMsg);
        }
        public static List<ParkOrder> GetDifferenceOrder(DateTime startTime, DateTime endtime, string userid, out string ErrorMessage)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetDifferenceOrder(startTime, endtime, userid, out ErrorMessage);
        }

        public static bool AuditingDiffOrder(ParkOrder model, decimal Amount, decimal PayAmount, out string ErrorMessage)
        {
            if (model.RecordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            bool result = factory.AuditingDiffOrder(model.RecordID, Amount, PayAmount, out ErrorMessage);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkOrder>(model, OperateType.Update);
            }
            return result;
        }


        public static List<ParkOrder> GetOrderByCarDerateID(DateTime startTime, string carDerateID, out string errorMsg)
        {
            if (carDerateID.IsEmpty()) throw new ArgumentNullException("CarDerateID");

            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrderByCarDerateID(startTime,carDerateID, out errorMsg);
        }
        public static List<ParkOrder> GetSellerRechargeOrder(string sellerId, int orderSource, DateTime? start, DateTime? end, int pageIndex, int pageSize, out int total)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetSellerRechargeOrder(sellerId,orderSource,start,end, pageIndex, pageSize, out total);
        }

        public static List<ParkOrder> GetOrdersByPKID(string PKID, DateTime startTime, DateTime endTime)
        {
            IParkOrder factory = ParkOrderFactory.GetFactory();
            return factory.GetOrdersByPKID(PKID, startTime, endTime);
        }
    }
}
