using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using Common.Utilities;
using System.Data.Common;
using Common.Core;

namespace Common.SqlRepository
{
    public class ParkOrderDAL : BaseDAL, IParkOrder
    {
        public ParkOrder AddOrder(ParkOrder model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    return Add(model, dbOperator);
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }
        public ParkOrder Add(ParkOrder model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                return Add(model, dbOperator);
            }
        }
        public ParkOrder Add(ParkOrder model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            model.RecordID = GuidGenerator.GetGuidString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"insert into ParkOrder(RecordID,Amount,CarderateID,CashMoney,CashTime,DataStatus,DiscountAmount,HaveUpdate,LastUpdateTime,NewMoney,NewUsefulDate,
                                    NewUserBegin,OldMoney,OldUserBegin,OldUserulDate,OnlineOrderNo,OnlineUserID,OrderNo,OrderSource,OrderTime,OrderType,PayAmount,
                                    PayTime,PayWay,PKID,Remark,Status,TagID,UnPayAmount,UserID,FeeRuleID) ");
            strSql.Append(@" values(@RecordID,@Amount,@CarderateID,@CashMoney,@CashTime,@DataStatus,@DiscountAmount,@HaveUpdate,@LastUpdateTime,@NewMoney,@NewUsefulDate,
                                    @NewUserBegin,@OldMoney,@OldUserBegin,@OldUserulDate,@OnlineOrderNo,@OnlineUserID,@OrderNo,@OrderSource,@OrderTime,@OrderType,@PayAmount,
                                    @PayTime,@PayWay,@PKID,@Remark,@Status,@TagID,@UnPayAmount,@UserID,@FeeRuleID);");
            strSql.Append(" select * from ParkOrder where RecordID=@RecordID ");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("Amount", model.Amount);
            dbOperator.AddParameter("CarderateID", model.CarderateID);
            dbOperator.AddParameter("CashMoney", model.CashMoney);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("DiscountAmount", model.DiscountAmount);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("NewMoney", model.NewMoney);
            if (model.CashTime == null)
            {
                dbOperator.AddParameter("CashTime", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("CashTime", model.CashTime);
            }
            if (model.NewUsefulDate == null)
            {
                dbOperator.AddParameter("NewUsefulDate", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("NewUsefulDate", model.NewUsefulDate);
            }
            if (model.OldUserulDate == null)
            {
                dbOperator.AddParameter("OldUserulDate", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("OldUserulDate", model.OldUserulDate);
            }
            if (model.OldUserBegin == null)
            {
                dbOperator.AddParameter("OldUserBegin", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("OldUserBegin", model.OldUserBegin);
            }
            if (model.NewUserBegin == null)
            {
                dbOperator.AddParameter("NewUserBegin", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("NewUserBegin", model.NewUserBegin);
            }
            dbOperator.AddParameter("OldMoney", model.OldMoney);
            dbOperator.AddParameter("OnlineOrderNo", model.OnlineOrderNo);
            dbOperator.AddParameter("OnlineUserID", model.OnlineUserID);
            dbOperator.AddParameter("OrderNo", model.OrderNo);
            dbOperator.AddParameter("OrderSource", model.OrderSource);
            dbOperator.AddParameter("OrderTime", model.OrderTime);
            dbOperator.AddParameter("OrderType", model.OrderType);
            dbOperator.AddParameter("PayAmount", model.PayAmount);
            dbOperator.AddParameter("PayTime", model.PayTime);
            dbOperator.AddParameter("PayWay", model.PayWay);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
            dbOperator.AddParameter("Remark", model.Remark);
            dbOperator.AddParameter("Status", model.Status);
            dbOperator.AddParameter("TagID", model.TagID);
            dbOperator.AddParameter("UnPayAmount", model.UnPayAmount);
            dbOperator.AddParameter("UserID", model.UserID);
            using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
            {
                if (reader.Read())
                {
                    return DataReaderToModel<ParkOrder>.ToModel(reader);
                }
                return null;
            }
        }
        public ParkOrder GetCashMoneyCountByPlateNumber(string parkingID, OrderType orderType, string plateNumber, DateTime startTime, DateTime endtime, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" SELECT * from ParkOrder where id = (SELECT max(O.id) FROM ParkOrder O left outer join ParkIOrecord I on O.TagID = I.RecordID
                                WHERE O.status = 1 and O.CashTime >= @StartTime and  O.CashTime <= @EndTime and O.PKID = @PKID and O.OrderType =@OrderType
                and O.DataStatus != @DataStatus and I.PlateNumber =@PlateNumber) ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("EndTime", endtime);
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("OrderType", (int)orderType);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkOrder>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public List<ParkOrder> GetOrderByIORecordID(string iorecordID, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * from ParkOrder where DataStatus != @DataStatus and TagID =@TagID and Status=1 and (OrderType=1 or OrderType=6)");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TagID", iorecordID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public List<ParkOrder> GetOrderByTagID(string tagID, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * from ParkOrder where DataStatus != @DataStatus and TagID =@TagID and Status=1 ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TagID", tagID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public List<ParkOrder> GetOrderByIORecordIDByStatus(string iorecordID, int status, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from ParkOrder  where DataStatus!=@DataStatus and Status=@Status and TagID=@TagID and (OrderType=1 or OrderType=6) ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TagID", iorecordID);
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public List<ParkOrder> GetOrderByTimeseriesID(string timeseriesID, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * from ParkOrder where DataStatus != @DataStatus and TagID =@TagID  and Status=1 and (OrderType=7 or OrderType=8) ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TagID", timeseriesID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public List<ParkOrder> GetOrderByTimeseriesIDByStatus(string timeseriesID, int status, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from ParkOrder  where DataStatus!=@DataStatus and Status=@Status and TagID=@TagID and (OrderType=7 or OrderType=8) ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TagID", timeseriesID);
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public bool ModifyOrderStatus(string recordID, int status, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkOrder set Status=@Status,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public bool ModifyOrderStatusAndAmount(string recordID, decimal payAmount, decimal unPayamount, int status, int Payway, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkOrder set Status=@Status,HaveUpdate=@HaveUpdate,PayAmount=@PayAmount,UnPayamount=@UnPayamount,LastUpdateTime=@LastUpdateTime,PayWay=@PayWay where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("PayAmount", payAmount);
                    dbOperator.AddParameter("UnPayamount", unPayamount);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("PayWay", Payway);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }


        public bool ModifyOrderStatusAndAmount(string recordID, decimal amount, decimal payAmount, decimal unPayamount, int status, decimal Discountamount, string Carderateid, int payWay, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    string strSql = "";
                    strSql = @"update ParkOrder set  Carderateid=@Carderateid,Discountamount=@Discountamount,Amount=@Amount,Status=@Status,HaveUpdate=@HaveUpdate,PayAmount=@PayAmount,UnPayamount=@UnPayamount,LastUpdateTime=@LastUpdateTime,PayWay=@PayWay where RecordID=@RecordID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("Discountamount", Discountamount);
                    dbOperator.AddParameter("Carderateid", Carderateid);
                    dbOperator.AddParameter("Amount", amount);
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("PayAmount", payAmount);
                    dbOperator.AddParameter("UnPayamount", unPayamount);
                    dbOperator.AddParameter("PayWay", payWay);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public ParkOrder GetTimeseriesOrderChareFeeCount(string userID, OrderType OrderType, DateTime dt, int releaseType, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  ISNULL(sum(O.Amount),0) as Amount,ISNULL(sum(O.PayAmount),0) as PayAmount ,ISNULL(sum(O.UnPayAmount),0) as UnPayAmount,ISNULL(sum(O.DiscountAmount),0) as DiscountAmount   FROM  ParkOrder  O  
                                        left  outer JOIN ParkTimeseries I on O.TagID=I.TimeseriesID
                                        WHERE O.OrderTime>@OrderTime and O.UserID=@UserID and O.OrderType=@OrderType and O.DataStatus!=@DataStatus and I.ReleaseType=@ReleaseType  and Status=1 ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("UserID", userID);
                    dbOperator.AddParameter("OrderType", (int)OrderType);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("OrderTime", dt);
                    dbOperator.AddParameter("ReleaseType", releaseType);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkOrder>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, int status, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from ParkOrder  where  Status=@Status and DataStatus!=@DataStatus and (OrderType=1 or OrderType=6 or OrderType=7 or OrderType=8) and OrderTime>@StartTime and OrderTime<@EndTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("Status", status);
                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("EndTime", endtime);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public List<ParkOrder> GetOrderByStatus(DateTime startTime, DateTime endtime, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.ReleaseType from ParkOrder a left join ParkIORecord b on a.TagID=b.RecordID  where  (Status=1 or Status=2) and a.DataStatus!=@DataStatus  and OrderTime>@StartTime and OrderTime<@EndTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();

                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("EndTime", endtime);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        public ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType orderType, DateTime dt, int releaseType, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                string tableName = "ParkIORecord";
                string tarID = "RecordID";
                if (orderType == OrderType.AreaTempCardPayment
                    || orderType == OrderType.AreaValueCardPayment)
                {
                    tableName = "ParkTimeseries";
                    tarID = "TimeseriesID";
                }
                string strSql = string.Format(@"SELECT ISNULL(sum(O.Amount),0) as Amount,ISNULL(sum(O.PayAmount),0) as PayAmount,ISNULL(sum(O.DiscountAmount),0) as DiscountAmount  FROM  ParkOrder  O  
                                        left  outer JOIN {0} I on O.TagID=I.{1}
                                        WHERE O.OrderTime>@OrderTime and O.UserID=@UserID and O.OrderType=@OrderType and O.DataStatus!=@DataStatus and I.ReleaseType=@ReleaseType  and Status=1 ", tableName, tarID);


                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("UserID", userID);
                    dbOperator.AddParameter("OrderType", (int)orderType);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("OrderTime", dt);
                    dbOperator.AddParameter("ReleaseType", releaseType);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkOrder>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public ParkOrder GetIORecordOrderChareFeeCount(string userID, OrderType orderType, DateTime dt, int releaseType, OrderSource orderSource, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                string tableName = "ParkIORecord";
                string tarID = "RecordID";
                if (orderType == OrderType.AreaTempCardPayment
                    || orderType == OrderType.AreaValueCardPayment)
                {
                    tableName = "ParkTimeseries";
                    tarID = "TimeseriesID";
                }
                string strSql = string.Format(@"SELECT ISNULL(sum(O.Amount),0) as Amount, ISNULL(sum(O.PayAmount),0) as PayAmount, ISNULL(sum(O.DiscountAmount),0) as DiscountAmount  FROM  ParkOrder  O
                                        left  outer JOIN {0} I on O.TagID = I.{1}
                                        WHERE O.OrderTime >@OrderTime and O.UserID =@UserID and O.OrderType=@OrderType
                and O.DataStatus != @DataStatus and I.ReleaseType =@ReleaseType
                and O.OrderSource =@OrderSource
                and Status= 1 ", tableName, tarID);

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("UserID", userID);
                    dbOperator.AddParameter("OrderType", (int)orderType);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("OrderTime", dt);
                    dbOperator.AddParameter("ReleaseType", releaseType);
                    dbOperator.AddParameter("OrderSource", orderSource);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkOrder>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }


        public decimal QueryMonthExpiredNotPayAmount(DateTime start, DateTime end, string parkingId, List<string> plateNumbers)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isnull(sum(o.PayAmount),0) PayAmount from  ParkIORecord p inner join ParkOrder o on p.RecordID=o.TagID");
            strSql.Append("  where p.IsExit=1 and p.EnterType=1 and o.Status=4");
            strSql.Append("  and o.OrderTime>=@StartDate and o.OrderTime<=@EndDate and p.DataStatus!=@DataStatus and o.DataStatus!=@DataStatus");
            strSql.AppendFormat("  and p.PlateNumber in('{0}') and p.ParkingID=@ParkingID", string.Join("','", plateNumbers));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("StartDate", start.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("EndDate", end.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader.GetDecimalDefaultZero(0);
                    }
                    return 0;
                }
            }
        }

        public List<ParkOrder> QueryByIORecordIds(List<string> recordIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkOrder where TagID in('{0}') and DataStatus=@DataStatus", string.Join("','", recordIds));

            List<ParkOrder> models = new List<ParkOrder>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public bool UpdateOrderStatusByIORecordIds(List<string> recordIds, int status, DbOperator dbOperator)
        {
            string strSql = string.Format(@"update ParkOrder set Status=@Status,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where TagID in('{0}');", string.Join("','", recordIds));
            dbOperator.ClearParameters();
            dbOperator.AddParameter("Status", status);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="boxid">岗亭编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<ParkOrder> GetOrdersByParkingID(string parkingid, DateTime starttime, DateTime endtime)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select p.PayWay,p.Amount,p.UnPayAmount,p.PayAmount,p.DiscountAmount,p.OrderType OrderType 
                                            from parkorder p where p.status=1 and p.pkid=@parkingid and p.DataStatus!=2 
                                            and p.ordertime>=@starttime and p.ordertime<=@endtime");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                }
            }
            return orderlist;
        }
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="boxid">岗亭编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<ParkOrder> GetOrdersByGateID(string parkingid, string gateid, DateTime starttime, DateTime endtime)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select p.PayWay,p.Amount,p.UnPayAmount,p.PayAmount,p.DiscountAmount,p.OrderType OrderType 
                                            from parkorder p 
                                            left join parkiorecord i on i.recordid=p.tagid 
                                            where p.status=1 and i.exitgateid=@gateid and p.pkid=@parkingid and p.DataStatus!=2 
                                            and p.ordertime>=@starttime and p.ordertime<=@endtime");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("gateid", gateid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                }
            }
            return orderlist;
        }
        /// <summary>
        /// 通过通道获取订单
        /// </summary>
        /// <param name="parkingid"></param>
        /// <param name="boxid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<ParkOrder> GetOrdersByBoxID(string parkingid, string boxid, DateTime starttime, DateTime endtime)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select p.PayWay,p.Amount,p.UnPayAmount,p.PayAmount,p.OrderType OrderType 
                                            from parkorder p where p.status=1 and 
                                            tagid in (select recordid from parkiorecord where parkingid=@parkingid and DataStatus!=2 and 
                                            exitgateid in (select GateID from parkgate where IoState=2 and boxid=@boxid)) and p.DataStatus!=2 
                                            and p.ordertime>=@starttime and p.ordertime<=@endtime");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("boxid", boxid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                }
            }
            return orderlist;
        }


        public bool ModifyOrderTagIDAndOrderType(string recordID, string tagid, int orderType, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkOrder set TagID=@TagID,OrderType=@OrderType,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("OrderType", orderType);
                    dbOperator.AddParameter("TagID", tagid);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        /// <summary>
        /// 获取当班差异订单
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endtime"></param>
        /// <param name="userid"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public List<ParkOrder> GetDifferenceOrder(DateTime startTime, DateTime endtime, string userid, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from ParkOrder  where  UserID=@UserID and amount!=payamount and DataStatus!=@DataStatus and (OrderType=1 or OrderType=6 or OrderType=7 or OrderType=8) and OrderTime>@StartTime and OrderTime<@EndTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("UserID", userid);
                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("EndTime", endtime);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }

        /// <summary>
        /// 审核异常订单
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="PayAmount"></param>
        /// <returns></returns>
        public bool AuditingDiffOrder(string recordID, decimal Amount, decimal PayAmount, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkOrder set Amount=@Amount, PayAmount=@PayAmount,UnPayAmount=(@Amount-DiscountAmount-@PayAmount), HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("Amount", Amount);
                    dbOperator.AddParameter("PayAmount", PayAmount);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }

            return false;
        }


        public List<ParkOrder> GetOrderByCarDerateID(DateTime startTime, string carDerateID, out string ErrorMessage)
        {
            List<ParkOrder> ParkOrders = new List<ParkOrder>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * from ParkOrder where DataStatus != @DataStatus and CarderateID =@CarderateID and Status=1 and OrderTime>=@StartTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("CarderateID", carDerateID);
                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrders.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkOrders;
        }
        public List<ParkOrder> GetSellerRechargeOrder(string sellerId,int orderSource,DateTime? start,DateTime? end, int pageIndex, int pageSize, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from ParkOrder where OrderType=10 and DataStatus != @DataStatus and Status=1 ");
           
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(sellerId))
                {
                    strSql.Append(" and TagID=@TagID");
                    dbOperator.AddParameter("TagID", sellerId);
                }
                if (orderSource!=-1)
                {
                    strSql.Append(" and OrderSource=@OrderSource");
                    dbOperator.AddParameter("OrderSource", orderSource);
                }
                if (start.HasValue) {
                    strSql.AppendFormat(" and OrderTime>='{0}'",start.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (end.HasValue)
                {
                    strSql.AppendFormat(" and OrderTime<='{0}'", end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                List<ParkOrder> models = new List<ParkOrder>();
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " ID DESC", pageIndex, pageSize, out total))
                {
                   

                    while (reader.Read())
                    {

                        models.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }

                }
                return models;

            }
        }
        /// <summary>
        /// 根据车场编号获取所有订单信息
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<ParkOrder> GetOrdersByPKID(string PKID, DateTime startTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from ParkOrder where DataStatus != @DataStatus and Status=1 ");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                strSql.Append(" and PKID=@PKID");
                strSql.AppendFormat(" and OrderTime>='{0}'", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strSql.AppendFormat(" and OrderTime<='{0}'", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                List<ParkOrder> models = new List<ParkOrder>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                }
                return models;
            }
        }
    }
}
