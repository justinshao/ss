using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;
using Common.DataAccess;
using Common.Utilities;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class ParkCarDerateDAL : BaseDAL, IParkCarDerate
    {

        public bool Add(ParkCarDerate model)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }
        public bool Add(ParkCarDerate model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            string strsql = "insert into ParkCarDerate (CarDerateID,CarDerateNo,DerateID,FreeTime,PlateNumber,IORecordID,CardNo,ExpiryTime,CreateTime,Status,FreeMoney,AreaID,PKID,DerateQRCodeID,LastUpdateTime,HaveUpdate,DataStatus)";
            strsql += "values(@CarDerateID,@CarDerateNo,@DerateID,@FreeTime,@PlateNumber,@IORecordID,@CardNo,@ExpiryTime,@CreateTime,@Status,@FreeMoney,@AreaID,@PKID,@DerateQRCodeID,@LastUpdateTime,@HaveUpdate,@DataStatus)";
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CarDerateID", model.CarDerateID);
            dbOperator.AddParameter("CarDerateNo", model.CarDerateNo);
            dbOperator.AddParameter("DerateID", model.DerateID);
            dbOperator.AddParameter("FreeTime", model.FreeTime);
            dbOperator.AddParameter("PlateNumber", model.PlateNumber);
            dbOperator.AddParameter("IORecordID", model.IORecordID);
            dbOperator.AddParameter("CardNo", model.CardNo);
            dbOperator.AddParameter("ExpiryTime", model.ExpiryTime);
            dbOperator.AddParameter("CreateTime", model.CreateTime);
            dbOperator.AddParameter("Status", model.Status);
            dbOperator.AddParameter("FreeMoney", model.FreeMoney);
            dbOperator.AddParameter("AreaID", model.AreaID);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("DerateQRCodeID", model.DerateQRCodeID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", model.DataStatus);
            return dbOperator.ExecuteNonQuery(strsql) > 0;
        }
        public bool Update(ParkCarDerate model)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkCarDerate set CarDerateNo=@CarDerateNo,DerateID=@DerateID,FreeTime=@FreeTime,PlateNumber=@PlateNumber,IORecordID=@IORecordID,CardNo=@CardNo,ExpiryTime=@ExpiryTime,Status=@Status");
                strSql.Append(",FreeMoney=@FreeMoney,AreaID=@AreaID,PKID=@PKID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate  where CarDerateID=@CarDerateID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarDerateID", model.CarDerateID);
                dbOperator.AddParameter("CarDerateNo", model.CarDerateNo);
                dbOperator.AddParameter("DerateID", model.DerateID);
                dbOperator.AddParameter("FreeTime", model.FreeTime);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("IORecordID", model.IORecordID);
                dbOperator.AddParameter("CardNo", model.CardNo);
                dbOperator.AddParameter("ExpiryTime", model.ExpiryTime);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("FreeMoney", model.FreeMoney);
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool UpdateStatus(string carDerateID,CarDerateStatus status)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkCarDerate set Status=@Status,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate  where CarDerateID=@CarDerateID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarDerateID", carDerateID);
                dbOperator.AddParameter("Status", status);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool QRCodeDiscount(string carDerateId, string parkingId, string ioRecordId, string plateNumber)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkCarDerate set PlateNumber=@PlateNumber,PKID=@PKID,IORecordID=@IORecordID,Status=@Status,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate  where CarDerateID=@CarDerateID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarDerateID", carDerateId);
                dbOperator.AddParameter("PlateNumber", plateNumber);
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("IORecordID", ioRecordId);
                dbOperator.AddParameter("Status", CarDerateStatus.Used);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public List<ParkCarDerate> QueryByDerateId(string derateId)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarDerate where CarDerateID=@CarDerateID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarDerateID", derateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkCarDerate> QueryByPlateNumber(string plateNumber)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarDerate where PlateNumber=@PlateNumber and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateNumber", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkCarDerate> QueryByCardNo(string cardNo)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarDerate where CardNo=@CardNo and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CardNo", cardNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkCarDerate> QueryByIORecordID(string ioRecordId)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarDerate where IORecordID=@IORecordID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("IORecordID", ioRecordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public bool DeleteByExpiryTime(string derateId, DateTime expiredTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarDerate set DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.AppendFormat(" where DerateID=@DerateID and ExpiryTime<'{0}'", expiredTime.ToString("yyyy-MM-dd HH:mm:ss"));
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("DerateID", derateId);

                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool DeleteNotUseByDerateQRCodeID(string derateQRCodeId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarDerate set DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.Append(" where DerateQRCodeID=@DerateQRCodeID and Status=@Status");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("DerateQRCodeID", derateQRCodeId);
            dbOperator.AddParameter("Status", 0);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public ParkCarDerate GetNotUseParkCarDerate(string derateId, DateTime lessThanTime)
        {
            string strSql = "select top 1 * from ParkCarDerate where Status=@Status and DataStatus!=@DataStatus and DerateID=@DerateID and CreateTime<=@CreateTime order by CreateTime";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Status", CarDerateStatus.Normal);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CreateTime", lessThanTime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<ParkCarDerate>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public bool UpdateCarderateCreateTime(string carDerateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarDerate set CreateTime=@CreateTime,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.Append(" where DerateID=@DerateID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CreateTime", DateTime.Now);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("CarDerateID", carDerateID);

                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public ParkCarDerate QueryByCarDerateID(string carDerateId)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarDerate where CarDerateID=@CarDerateID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarDerateID", carDerateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkCarDerate>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public ParkCarDerate QueryBySellerIdAndIORecordId(string sellerId, string ioReocdId) {
          
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.*,b.SellerID from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID where IORecordID=@IORecordID and b.SellerID=@SellerID  and a.DataStatus!=@DataStatus  and Status!=@Status");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("IORecordID", ioReocdId);
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("Status", (int)CarDerateStatus.Obsolete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkCarDerate>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public decimal GetTotalFreeMoney(string sellerId) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(FreeMoney) as SumFreeMoney from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID  where a.DataStatus!=DataStatus and a.Status=@Status and b.SellerID=@SellerID ");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("Status", (int)CarDerateStatus.Used);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader["SumFreeMoney"].ToDecimal();
                    }
                    return 0;
                }
            }
        }
        public Dictionary<string, int> QuerySettlementdCarDerate(List<string> derateQRCodeIds)
        {
            StringBuilder strSql = new StringBuilder();
            Dictionary<string, int> result = new Dictionary<string, int>();
            strSql.AppendFormat("select DerateQRCodeID,count(id) Numbers from ParkCarDerate where Status=2 and DerateQRCodeID in('{0}')  and DataStatus!=@DataStatus group by DerateQRCodeID", string.Join("','", derateQRCodeIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        result.Add(reader["DerateQRCodeID"].ToString(), reader["Numbers"].ToInt());
                    }
                    return result;
                }
            }
        }
        public List<ParkCarDerate> ParkCarDeratePage(string sellerId, string plateNumber, int? state, int? derateType, DateTime? start, DateTime? end, int pageSize, int pageIndex, ref int totalCount)
        {
            List<ParkCarDerate> models = new List<ParkCarDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.*,d.Name DerateName,d.DerateType from parkcarderate c inner join parkderate d on c.DerateID=d.DerateID where  d.SellerID=@SellerID and c.Status!=@Status and c.DataStatus!=@DataStatus and d.DataStatus!=@DataStatus ");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("Status", CarDerateStatus.Normal);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                if (!string.IsNullOrWhiteSpace(plateNumber))
                {
                    strSql.Append(" and c.PlateNumber like @PlateNumber");
                }
                if (state.HasValue)
                {
                    strSql.Append(" and c.Status =@Status");
                }
                if (derateType.HasValue)
                {
                    strSql.Append(" and d.DerateType =@DerateType");
                }
                if (start.HasValue)
                {
                    strSql.AppendFormat(" and c.CreateTime >='{0}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (end.HasValue)
                {
                    strSql.AppendFormat(" and c.CreateTime <='{0}'", end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " CreateTime DESC", pageIndex, pageSize, out totalCount))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                    }
                }
            }
            return models;
        }
    }
}
