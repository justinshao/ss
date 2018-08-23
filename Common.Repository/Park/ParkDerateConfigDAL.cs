using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository;

namespace Common.SqlRepository
{
    public class ParkDerateConfigDAL : BaseDAL, IParkDerateConfig
    {
        public bool Add(ParkDerateConfig model) {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkDerateConfig(RecordID,PKID,ConsumeStartAmount,ConsumeEndAmount,DerateType,DerateValue,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@RecordID,@PKID,@ConsumeStartAmount,@ConsumeEndAmount,@DerateType,@DerateValue,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("ConsumeStartAmount", model.ConsumeStartAmount);
                dbOperator.AddParameter("ConsumeEndAmount", model.ConsumeEndAmount);
                dbOperator.AddParameter("DerateType", model.DerateType);
                dbOperator.AddParameter("DerateValue",model.DerateValue);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool Update(ParkDerateConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkDerateConfig set ConsumeStartAmount=@ConsumeStartAmount,ConsumeEndAmount=@ConsumeEndAmount,DerateType=@DerateType,DerateValue=@DerateValue,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
                strSql.Append(" where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("ConsumeStartAmount", model.ConsumeStartAmount);
                dbOperator.AddParameter("ConsumeEndAmount", model.ConsumeEndAmount);
                dbOperator.AddParameter("DerateType", model.DerateType);
                dbOperator.AddParameter("DerateValue", model.DerateValue);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool Delete(string recordId) {
            return CommonDelete("ParkDerateConfig", "RecordID", recordId);
        }
        public ParkDerateConfig QueryByRecordId(string recordId){

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerateConfig where RecordID=@RecordID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkDerateConfig>.ToModel(reader);
                    }
                }
            }
            return null;
        }
        public List<ParkDerateConfig> QueryByParkingId(string parkingId)
        {
            List<ParkDerateConfig> models = new List<ParkDerateConfig>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerateConfig where PKID=@PKID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkDerateConfig>.ToModel(reader));
                    }
                }
            }
            return models;
        }
        public ParkDerateConfig QueryByParkingIdAndAmount(string parkingId, decimal amount) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerateConfig where PKID=@PKID and ConsumeStartAmount<=@Amount and ConsumeEndAmount>=@Amount and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Amount", amount);
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<ParkDerateConfig>.ToModel(reader);
                    }
                }
            }
            return null;
        }
    }
}
