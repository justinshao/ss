using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using Common.Utilities;
using Common.Core;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class ParkInterimDAL : BaseDAL, IParkInterim
    {
        public ParkInterim AddInterim(ParkInterim model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
                    model.RecordID = GuidGenerator.GetGuidString();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ParkInterim(RecordID,IORecordID,StartInterimTime,EndInterimTime,LastUpdateTime,HaveUpdate,DataStatus,Remark)");
                    strSql.Append(" values(@RecordID,@IORecordID,@StartInterimTime,@EndInterimTime,@LastUpdateTime,@HaveUpdate,@DataStatus,@Remark);");
                    strSql.Append(" select * from ParkInterim where RecordID=@RecordID ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("StartInterimTime", model.StartInterimTime);
                    if (model.EndInterimTime == null)
                    {
                        dbOperator.AddParameter("EndInterimTime", DBNull.Value);
                    }
                    else
                    {
                        dbOperator.AddParameter("EndInterimTime", model.EndInterimTime);
                    }
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", (int)model.HaveUpdate);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("Remark", model.Remark);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkInterim>.ToModel(reader);
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

        public List<ParkInterim> GetInterimByIOrecord(string recordID, out string ErrorMessage)
        {
            List<ParkInterim> ParkInterims = new List<ParkInterim>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkInterim where IORecordID=@IORecordID  and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("IORecordID", recordID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkInterims.Add(DataReaderToModel<ParkInterim>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkInterims;
        }

        public bool ModifyInterim(ParkInterim model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkInterim set IORecordID=@IORecordID,StartInterimTime=@StartInterimTime,EndInterimTime=@EndInterimTime,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,DataStatus=@DataStatus,Remark=@Remark");
                    strSql.Append(" where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("StartInterimTime", model.StartInterimTime);
                    dbOperator.AddParameter("EndInterimTime", model.EndInterimTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("Remark", model.Remark);
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public bool RemoveByIORecordId(string recordid)
        {
            return CommonDelete("ParkInterim", "IORecordId", recordid);
        }
    }
}
