using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BaseData;
using Common.DataAccess;
using Common.IRepository;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class PassRemarkDAL : BaseDAL, IPassRemark
    {

        public List<BasePassRemark> QueryByParkingId(string parkingId, Entities.PassRemarkType? passType)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
               
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ID,RecordID,PassType,Remark,PKID,LastUpdateTime,HaveUpdate,DataStatus from BasePassRemark");
                strSql.Append(" where PKID=@PKID and DataStatus!=@DataStatus");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                if (passType.HasValue) {
                    strSql.Append(" and PassType=@PassType");
                    dbOperator.AddParameter("PassType", (int)passType.Value);
                }
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<BasePassRemark> models = new List<BasePassRemark>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BasePassRemark>.ToModel(reader)); 
                    }
                    return models;
                }
            }
        }

        public bool Add(BasePassRemark model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into BasePassRemark(RecordID,PassType,Remark,PKID,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@RecordID,@PassType,@Remark,@PKID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PassType", (int)model.PassType);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(BasePassRemark model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BasePassRemark set PassType=@PassType,Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
                strSql.Append(" where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PassType", (int)model.PassType);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
           return CommonDelete("BasePassRemark", "RecordID", recordId);
        }

        public BasePassRemark QueryByRecordId(string recordId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ID,RecordID,PassType,Remark,PKID,LastUpdateTime,HaveUpdate,DataStatus from BasePassRemark");
                strSql.Append(" where RecordID=@RecordID and DataStatus!=@DataStatus");
                dbOperator.ClearParameters();

                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                { 
                    if(reader.Read())
                    {
                        return DataReaderToModel<BasePassRemark>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
    }
}
