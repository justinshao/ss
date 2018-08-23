using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using System.Data.Common;
using Common.Entities.WX;
using Common.IRepository.WeiXin;
using Common.Utilities;

namespace Common.SqlRepository.WeiXin
{
    public class AdvanceParkingDAL : BaseDAL, IAdvanceParking
    {
        public bool Add(AdvanceParking model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO AdvanceParking(OrderId,PlateNo,StartTime,EndTime,Amount,WxOpenId,OrderState,CreateTime,CompanyID) VALUES (@OrderId,@PlateNo,@StartTime,@EndTime,@Amount,@WxOpenId,@OrderState,@CreateTime,@CompanyID);");
                dbOperator.AddParameter("OrderId", model.OrderId);
                dbOperator.AddParameter("PlateNo", model.PlateNo);
                dbOperator.AddParameter("StartTime", model.StartTime);
                dbOperator.AddParameter("EndTime", model.EndTime);
                dbOperator.AddParameter("Amount", model.Amount);
                dbOperator.AddParameter("WxOpenId", model.WxOpenId);
                dbOperator.AddParameter("OrderState", 0);
                dbOperator.AddParameter("CreateTime", model.CreateTime);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public AdvanceParking QueryByOrderId(decimal orderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select ID,OrderId,PlateNo,StartTime,EndTime,Amount,WxOpenId,CreateTime,OrderState,PayTime,PrepayId,SerialNumber,CompanyID FROM AdvanceParking where OrderId=@OrderId");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("OrderId", orderId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<AdvanceParking>.ToModel(reader);

                    }
                    return null;
                }
            }
        }

        public bool UpdatePrepayIdById(string prepayId, decimal orderId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update AdvanceParking set PrepayId=@PrepayId where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("PrepayId", prepayId);
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

        public bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime, DbOperator dbOperator)
        {
            string strSql = "update AdvanceParking set PayTime=@PayTime,SerialNumber=@SerialNumber,OrderState=@OrderState where OrderId=@OrderId";
            dbOperator.AddParameter("OrderId", orderId);
            dbOperator.AddParameter("OrderState", 1);
            dbOperator.AddParameter("SerialNumber", serialNumber);
            dbOperator.AddParameter("PayTime", payTime);
            return dbOperator.ExecuteNonQuery(strSql) > 0;
        }

        public bool UpdateOrderSerialNumber(decimal orderId, string serialNumber)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "update AdvanceParking set SerialNumber=@SerialNumber where OrderId=@OrderId";
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.AddParameter("SerialNumber", serialNumber);
                return dbOperator.ExecuteNonQuery(strSql) > 0;
            }
        }
        public List<AdvanceParking> QueryPage(string companyId, string plateNo, DateTime? start, DateTime? end, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID,OrderId,PlateNo,StartTime,EndTime,Amount,WxOpenId,CreateTime,OrderState,PayTime,PrepayId,SerialNumber,CompanyID FROM AdvanceParking WHERE  OrderState=1");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                if (!string.IsNullOrEmpty(plateNo))
                {
                    sql.Append(" AND PlateNo like @PlateNo");
                    dbOperator.AddParameter("PlateNo", "%" + plateNo + "%");
                }
                if (start.HasValue)
                {
                    sql.AppendFormat(" AND CreateTime >= '{0}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (end.HasValue)
                {
                    sql.AppendFormat(" AND CreateTime <= '{0}'", end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (!string.IsNullOrEmpty(companyId))
                {
                    sql.Append(" AND CompanyID =@CompanyID");
                    dbOperator.AddParameter("CompanyID", companyId);
                }
                List<AdvanceParking> models = new List<AdvanceParking>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), " ID DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<AdvanceParking>.ToModel(reader));
                    }

                }
                return models;
            }
        }
    }
}