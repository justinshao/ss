using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Core;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkBlacklistDAL : BaseDAL, IParkBlacklist
    {
        public ParkBlacklist GetBlacklist(string pkid, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkBlacklist where PKID=@PKID and PlateNumber=@PlateNumber and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", pkid);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkBlacklist>.ToModel(reader);
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

        public ParkBlacklist Query(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkBlacklist where RecordID=@RecordID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkBlacklist>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool Add(ParkBlacklist model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkBlacklist(RecordID,PKID,PlateNumber,Remark,Status,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@RecordID,@PKID,@PlateNumber,@Remark,@Status,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("Status", (int)model.Status);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkBlacklist model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkBlacklist set PKID=@PKID,PlateNumber=@PlateNumber,Remark=@Remark,Status=@Status");
                strSql.Append(",LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("Status", (int)model.Status);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public ParkBlacklist Query(string parkingid, string plateNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkBlacklist where PKID=@PKID and PlateNumber=@PlateNumber and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingid);
                dbOperator.AddParameter("PlateNumber", plateNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkBlacklist>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("ParkBlacklist", "RecordID", recordId);
        }

        public List<ParkBlacklist> QueryByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkBlacklist where PKID=@PKID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkBlacklist> models = new List<ParkBlacklist>();
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkBlacklist>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkBlacklist> QueryPage(string parkingId, string plateNo, int pagesize, int pageindex, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkBlacklist where PKID=@PKID  and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(plateNo)) {
                    strSql.Append(" and PlateNumber like @PlateNumber");
                    dbOperator.AddParameter("PlateNumber", "%" + plateNo + "%");
                }
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), "ID DESC", pageindex, pagesize, out total))
                {
                    List<ParkBlacklist> models = new List<ParkBlacklist>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkBlacklist>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
