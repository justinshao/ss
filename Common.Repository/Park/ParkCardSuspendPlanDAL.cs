using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkCardSuspendPlanDAL : BaseDAL, IParkCardSuspendPlan
    {

        public bool Add(ParkCardSuspendPlan model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            model.CreateTime = DateTime.Now;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkCardSuspendPlan(RecordId,GrantID,StartDate,EndDate,CreateTime,HaveUpdate,LastUpdateTime,DataStatus)");
            strSql.Append(" values(@RecordId,@GrantID,@StartDate,@EndDate,@CreateTime,@HaveUpdate,@LastUpdateTime,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordId", model.RecordId);
            dbOperator.AddParameter("GrantID", model.GrantID);
            dbOperator.AddParameter("StartDate", model.StartDate);
            dbOperator.AddParameter("EndDate", model.EndDate);
            dbOperator.AddParameter("CreateTime", model.CreateTime);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(DateTime start, DateTime? end, string grantId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCardSuspendPlan set StartDate=@StartDate,EndDate=@EndDate,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where GrantID=@GrantID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GrantID", grantId);
            dbOperator.AddParameter("StartDate", start);
            if (end.HasValue)
            {
                dbOperator.AddParameter("EndDate", end.Value);
            }
            else {
                dbOperator.AddParameter("EndDate",DBNull.Value);
            }
           
            dbOperator.AddParameter("LastUpdateTime",DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string grantId)
        {
            return CommonDelete("ParkCardSuspendPlan", "GrantID", grantId);
        }

        public bool Delete(string grantId, DbOperator dbOperator)
        {
            return CommonDelete("ParkCardSuspendPlan", "GrantID", grantId, dbOperator);
        }


        public ParkCardSuspendPlan QueryByGrantId(string grantId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCardSuspendPlan where GrantID=@GrantID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GrantID", grantId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<ParkCardSuspendPlan> QueryByGrantIds(List<string> grantIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkCardSuspendPlan where GrantID in('{0}') and DataStatus!=@DataStatus", string.Join("','", grantIds));

            List<ParkCardSuspendPlan> models = new List<ParkCardSuspendPlan>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
