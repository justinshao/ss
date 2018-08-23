using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Order;
using Common.Entities.Order;
using Common.DataAccess;
using Common.Entities.Enum;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Condition;

namespace Common.SqlRepository
{
    public class OnlineOrderDAL : IOnlineOrder
    {
        public bool Create(OnlineOrder model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO OnlineOrder(OrderID,PKID,PKName,InOutID,PlateNo,EntranceTime,ExitTime,Amount,PaymentChannel,PayBank");
                strSql.Append(",PayAccount,Payer,PayeeChannel,PayeeBank,PayeeAccount,PayeeUser,PayDetailID,SerialNumber,PrepayId,MonthNum,AccountID");
                strSql.Append(",CardId,SyncResultTimes,LastSyncResultTime,ParkCardNo,Balance,RefundOrderId,Remark,OrderType,Status,OrderTime,RealPayTime");
                strSql.Append(",BookingStartTime,BookingEndTime,BookingAreaID,BookingBitNo,CompanyID,OrderSource,ExternalPKID,TagID)");
                strSql.Append(" values(@OrderID,@PKID,@PKName,@InOutID,@PlateNo,@EntranceTime,@ExitTime,@Amount,@PaymentChannel,@PayBank");
                strSql.Append(",@PayAccount,@Payer,@PayeeChannel,@PayeeBank,@PayeeAccount,@PayeeUser,@PayDetailID,@SerialNumber,@PrepayId,@MonthNum,@AccountID");
                strSql.Append(",@CardId,@SyncResultTimes,@LastSyncResultTime,@ParkCardNo,@Balance,@RefundOrderId,@Remark,@OrderType,@Status,@OrderTime,@RealPayTime");
                strSql.Append(",@BookingStartTime,@BookingEndTime,@BookingAreaID,@BookingBitNo,@CompanyID,@OrderSource,@ExternalPKID,@TagID)");
                dbOperator.AddParameter("OrderID", model.OrderID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PKName", model.PKName);
                dbOperator.AddParameter("InOutID", model.InOutID);
                dbOperator.AddParameter("PlateNo", model.PlateNo);
                dbOperator.AddParameter("EntranceTime", model.EntranceTime);
                dbOperator.AddParameter("ExitTime", model.ExitTime);
                dbOperator.AddParameter("Amount", model.Amount);
                dbOperator.AddParameter("PaymentChannel", (int)model.PaymentChannel);
                dbOperator.AddParameter("PayBank", model.PayBank);
                dbOperator.AddParameter("PayAccount", model.PayAccount);
                dbOperator.AddParameter("Payer", model.Payer);
                dbOperator.AddParameter("PayeeChannel", (int)model.PayeeChannel);
                dbOperator.AddParameter("PayeeBank", model.PayeeBank);
                dbOperator.AddParameter("PayeeAccount", model.PayeeAccount);
                dbOperator.AddParameter("PayeeUser", model.PayeeUser);
                dbOperator.AddParameter("PayDetailID", model.PayDetailID);
                dbOperator.AddParameter("SerialNumber", model.SerialNumber);
                dbOperator.AddParameter("PrepayId", model.PrepayId);
                dbOperator.AddParameter("MonthNum", model.MonthNum);
                dbOperator.AddParameter("AccountID", model.AccountID);
                dbOperator.AddParameter("CardId", model.CardId);
                dbOperator.AddParameter("SyncResultTimes", model.SyncResultTimes);
                dbOperator.AddParameter("LastSyncResultTime", model.LastSyncResultTime);
                dbOperator.AddParameter("ParkCardNo", model.ParkCardNo);
                dbOperator.AddParameter("Balance", model.Balance);
                dbOperator.AddParameter("RefundOrderId", model.RefundOrderId);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("OrderType", model.OrderType);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("OrderTime", model.OrderTime);
                dbOperator.AddParameter("RealPayTime", model.RealPayTime);
                dbOperator.AddParameter("BookingStartTime", model.BookingStartTime);
                dbOperator.AddParameter("BookingEndTime", model.BookingEndTime);
                dbOperator.AddParameter("BookingAreaID", model.BookingAreaID);
                dbOperator.AddParameter("BookingBitNo", model.BookingBitNo);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("OrderSource", model.OrderSource);
                dbOperator.AddParameter("ExternalPKID", model.ExternalPKID);
                dbOperator.AddParameter("TagID", model.TagID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public OnlineOrder QueryByOrderId(decimal orderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from OnlineOrder where OrderId=@OrderId");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("OrderId", orderId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<OnlineOrder>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool UpdatePrepayIdById(string prepayId, decimal orderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set PrepayId=@PrepayId where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("PrepayId", prepayId);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public bool UpdatePrepayIdById(string prepayId, string mWebUrl, decimal orderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set PrepayId=@PrepayId,MWebUrl=@MWebUrl where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("PrepayId", prepayId);
                dbOperator.AddParameter("MWebUrl", mWebUrl);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public bool UpdateSFMCode(string code, decimal orderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set InOutID=@InOutID where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("InOutID", code);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return PaySuccess(orderId, serialNumber, payTime, dbOperator);
            }
        }
        public bool PaySuccess(decimal orderId, string serialNumber, string payAccount, DateTime payTime)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set RealPayTime=@RealPayTime,SerialNumber=@SerialNumber,Status=@Status,PayAccount=@PayAccount,Payer=@Payer where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.PaySuccess);
                dbOperator.AddParameter("SerialNumber", serialNumber);
                dbOperator.AddParameter("PayAccount", payAccount);
                dbOperator.AddParameter("Payer", payAccount);
                dbOperator.AddParameter("RealPayTime", payTime);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime, DbOperator dbOperator)
        {
            string strSql = "update OnlineOrder set RealPayTime=@RealPayTime,SerialNumber=@SerialNumber,Status=@Status where OrderId=@OrderId";
            dbOperator.AddParameter("OrderId", orderId);
            dbOperator.AddParameter("Status", (int)OnlineOrderStatus.PaySuccess);
            dbOperator.AddParameter("SerialNumber", serialNumber);
            dbOperator.AddParameter("RealPayTime", payTime);
            return dbOperator.ExecuteNonQuery(strSql) > 0;
        }

        public bool UpdateOrderSerialNumber(decimal orderId, string serialNumber)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set SerialNumber=@SerialNumber where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("SerialNumber", serialNumber);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public bool UpdateOrderStatusByOrderId(decimal orderId, OnlineOrderStatus status)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set Status=@Status where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("Status", (int)status);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public bool RefundFail(decimal orderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set Status=@Status where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.RefundFail);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public bool SyncPayResultSuccess(decimal orderId, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            string strSql = "update OnlineOrder set Status=@Status where OrderId=@OrderId";
            dbOperator.AddParameter("OrderId", orderId);
            dbOperator.AddParameter("Status", (int)OnlineOrderStatus.SyncPayResultSuccess);
            int result = dbOperator.ExecuteNonQuery(strSql);
            return result > 0;
        }

        public bool UpdatePayDetailID(decimal orderId, string payDetailId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                string strSql = "update OnlineOrder set PayDetailID=@PayDetailID where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("PayDetailID", payDetailId);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public bool RefundSuccess(decimal orderId, string refundOrderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return RefundSuccess(orderId, refundOrderId, dbOperator);
            }
        }

        public bool RefundSuccess(decimal orderId, string refundOrderId, DbOperator dbOperator)
        {
            string strSql = "update OnlineOrder set Status=@Status,RefundOrderId=@RefundOrderId where OrderId=@OrderId";
            dbOperator.AddParameter("OrderId", orderId);
            dbOperator.AddParameter("RefundOrderId", refundOrderId);
            dbOperator.AddParameter("Status", (int)OnlineOrderStatus.RefundSuccess);
            return dbOperator.ExecuteNonQuery(strSql) > 0;
        }

        public bool SyncPayResultFail(decimal orderId, string remark)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set Status=@Status,SyncResultTimes=SyncResultTimes+1,LastSyncResultTime=@LastSyncResultTime,Remark=@Remark where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.SyncPayResultFail);
                dbOperator.AddParameter("LastSyncResultTime", DateTime.Now);
                dbOperator.AddParameter("Remark", remark);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public bool UpdatePayAccount(decimal orderId, string payAccount, string payer)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set PayAccount=@PayAccount,Payer=@Payer where OrderId=@OrderId";
                dbOperator.AddParameter("PayAccount", payAccount);
                dbOperator.AddParameter("Payer", payer);
                dbOperator.AddParameter("OrderId", orderId);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public bool UpdateSyncResultTimes(decimal orderId, string payDetailId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set SyncResultTimes=SyncResultTimes+1,LastSyncResultTime=@LastSyncResultTime";
                if (!string.IsNullOrWhiteSpace(payDetailId))
                {
                    strSql += ",PayDetailID=@PayDetailID";
                    dbOperator.AddParameter("PayDetailID", payDetailId);
                }
                strSql += " where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.SyncPayResultFail);
                dbOperator.AddParameter("LastSyncResultTime", DateTime.Now);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }

        public List<OnlineOrder> QueryBySyncPayResultFail()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from OnlineOrder where Status=@Status and SyncResultTimes<3");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.SyncPayResultFail);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<OnlineOrder> models = new List<OnlineOrder>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<OnlineOrder>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<OnlineOrder> QueryWaitRefund(List<string> parkingIds, DateTime minRealPayTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from OnlineOrder where Status=@Status and SyncResultTimes>=3");
            strSql.AppendFormat(" and PKID in('{0}') and RealPayTime is not null and RealPayTime>='{1}'", string.Join("','", parkingIds), minRealPayTime.ToString("yyyy-MM-dd HH:mm:ss"));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Status", (int)OnlineOrderStatus.SyncPayResultFail);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<OnlineOrder> models = new List<OnlineOrder>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<OnlineOrder>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<OnlineOrder> QueryPage(OnlineOrderCondition condition, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT o.*,w.NickName as PayerNickName FROM OnlineOrder o left join WX_Info w on o.PayAccount=w.OpenID");
            sql.Append(" WHERE  o.OrderTime>=@STARTCREATETIME  AND o.OrderTime<=@ENDCREATETIME and o.PaymentChannel=@PaymentChannel and o.CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.End.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("PaymentChannel", (int)condition.PaymentChannel);
                dbOperator.AddParameter("CompanyID", condition.CompanyId);
                if (!string.IsNullOrEmpty(condition.OrderId))
                {
                    sql.Append(" AND o.OrderId=@OrderId");
                    dbOperator.AddParameter("OrderId", condition.OrderId);
                }
                if (!string.IsNullOrWhiteSpace(condition.ExternalPKID))
                {
                    sql.Append(" AND o.ExternalPKID=@ExternalPKID");
                    dbOperator.AddParameter("ExternalPKID", condition.ExternalPKID);
                }
                if (!string.IsNullOrEmpty(condition.PlateNo))
                {
                    sql.Append(" AND o.PlateNo LIKE @PlateNo");
                    dbOperator.AddParameter("PlateNo", "%" + condition.PlateNo + "%");
                }
                if (condition.Ordertype.HasValue)
                {
                    sql.Append(" AND o.OrderType=@OrderType");
                    dbOperator.AddParameter("OrderType", (int)condition.Ordertype);
                }
                if (condition.Status.HasValue)
                {
                    sql.Append(" AND o.Status=@Status");
                    dbOperator.AddParameter("Status", (int)condition.Status);
                }
                if (!string.IsNullOrEmpty(condition.ParkingId))
                {
                    sql.Append(" AND o.PKID=@PKID");
                    dbOperator.AddParameter("PKID", condition.ParkingId);
                }
                List<OnlineOrder> models = new List<OnlineOrder>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "OrderId DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<OnlineOrder>.ToModel(reader));
                    }

                }
                return models;

            }
        }
        public List<OnlineOrder> QueryAll(OnlineOrderCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT o.*,w.NickName as PayerNickName FROM OnlineOrder o left join WX_Info w on o.PayAccount=w.OpenID");
            sql.Append(" WHERE  o.OrderTime>=@STARTCREATETIME  AND o.OrderTime<=@ENDCREATETIME and o.PaymentChannel=@PaymentChannel and o.CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.End.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("PaymentChannel", (int)condition.PaymentChannel);
                dbOperator.AddParameter("CompanyID", condition.CompanyId);
                if (!string.IsNullOrEmpty(condition.OrderId))
                {
                    sql.Append(" AND o.OrderId=@OrderId");
                    dbOperator.AddParameter("OrderId", condition.OrderId);
                }
                if (!string.IsNullOrWhiteSpace(condition.ExternalPKID))
                {
                    sql.Append(" AND o.ExternalPKID=@ExternalPKID");
                    dbOperator.AddParameter("ExternalPKID", condition.ExternalPKID);
                }
                if (!string.IsNullOrEmpty(condition.PlateNo))
                {
                    sql.Append(" AND o.PlateNo LIKE @PlateNo");
                    dbOperator.AddParameter("PlateNo", "%" + condition.PlateNo + "%");
                }
                if (condition.Ordertype.HasValue)
                {
                    sql.Append(" AND o.OrderType=@OrderType");
                    dbOperator.AddParameter("OrderType", (int)condition.Ordertype);
                }
                if (condition.Status.HasValue)
                {
                    sql.Append(" AND o.Status=@Status");
                    dbOperator.AddParameter("Status", (int)condition.Status);
                }
                if (!string.IsNullOrEmpty(condition.ParkingId))
                {
                    sql.Append(" AND o.PKID=@PKID");
                    dbOperator.AddParameter("PKID", condition.ParkingId);
                }
                sql.Append(" order by OrderId DESC");
                List<OnlineOrder> models = new List<OnlineOrder>();
                using (DbDataReader dr = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (dr.Read())
                    {
                        models.Add(DataReaderToModel<OnlineOrder>.ToModel(dr));
                    }
                }
                return models;

            }
        }
        public List<OnlineOrder> ExportQueryPage(OnlineOrderCondition condition)
        {
            List<OnlineOrder> onlineOrder = new List<OnlineOrder>();
            string strSql = string.Format(@"SELECT o.*,w.NickName as PayerNickName FROM OnlineOrder o left join WX_Info w on o.PayAccount=w.OpenID
                                           WHERE  o.OrderTime>=@STARTCREATETIME  AND o.OrderTime<=@ENDCREATETIME and o.PaymentChannel=@PaymentChannel and o.CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.End.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("PaymentChannel", (int)condition.PaymentChannel);
                dbOperator.AddParameter("CompanyID", condition.CompanyId);
                if (!string.IsNullOrEmpty(condition.OrderId))
                {
                    strSql += " AND o.OrderId=@OrderId";
                    dbOperator.AddParameter("OrderId", condition.OrderId);
                }
                if (!string.IsNullOrWhiteSpace(condition.ExternalPKID))
                {
                    strSql += " AND o.ExternalPKID=@ExternalPKID";
                    dbOperator.AddParameter("ExternalPKID", condition.ExternalPKID);
                }
                if (!string.IsNullOrEmpty(condition.PlateNo))
                {
                    strSql += " AND o.PlateNo LIKE @PlateNo";
                    dbOperator.AddParameter("PlateNo", "%" + condition.PlateNo + "%");
                }
                if (condition.Ordertype.HasValue)
                {
                    strSql += " AND o.OrderType=@OrderType";
                    dbOperator.AddParameter("OrderType", (int)condition.Ordertype);
                }
                if (condition.Status.HasValue)
                {
                    strSql += " AND o.Status=@Status";
                    dbOperator.AddParameter("Status", (int)condition.Status);
                }
                if (!string.IsNullOrEmpty(condition.ParkingId))
                {
                    strSql += " AND o.PKID=@PKID";
                    dbOperator.AddParameter("PKID", condition.ParkingId);
                }
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        onlineOrder.Add(DataReaderToModel<OnlineOrder>.ToModel(reader));
                    }

                }
                return onlineOrder;

            }
        }

        public string QueryLastPaymentPlateNumber(PaymentChannel channel, string openId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select top 1 PlateNo FROM onlineorder where OrderType in({0},{1}) and paymentchannel=@PaymentChannel and payAccount=@PayAccount order by OrderID desc", (int)OnlineOrderType.ParkFee, (int)OnlineOrderType.PkBitBooking);

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PaymentChannel", (int)channel);
                dbOperator.AddParameter("PayAccount", openId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<OnlineOrder> models = new List<OnlineOrder>();
                    if (reader.Read())
                    {
                        return reader.GetStringDefaultEmpty(0);
                    }
                    return string.Empty;
                }
            }
        }

        public string QueryLastPaymentPlateNumber(string accountId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select top 1 PlateNo FROM onlineorder where OrderType in({0},{1}) and AccountID=@AccountID order by OrderID desc", (int)OnlineOrderType.ParkFee, (int)OnlineOrderType.PkBitBooking);

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AccountID", accountId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<OnlineOrder> models = new List<OnlineOrder>();
                    if (reader.Read())
                    {
                        return reader.GetStringDefaultEmpty(0);
                    }
                    return string.Empty;
                }
            }
        }


        public bool UpdatePayAccount(decimal orderId, string payAccount)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update OnlineOrder set PayAccount=@PayAccount,Payer=@PayAccount where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("PayAccount", payAccount);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
    }
}
