using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkDerateDAL : BaseDAL, IParkDerate
    {
        public bool Add(ParkDerate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = Add(model, dbOperator);
                    if (!result) throw new MyException("保存优免信息失败");

                    result = AddDerateIntervar(model.DerateIntervar, dbOperator);
                    if (!result) throw new MyException("保存优免时段信息失败");

                    dbOperator.CommitTransaction();
                    return result;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        private bool Add(ParkDerate model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkDerate(DerateID,SellerID,[Name],DerateSwparate,DerateType,DerateMoney,FreeTime,StartTime,EndTime,FeeRuleID,LastUpdateTime,HaveUpdate,DataStatus,MaxTimesCycle,MaxTimes,ValidityTime)");
            strSql.Append(" values(@DerateID,@SellerID,@Name,@DerateSwparate,@DerateType,@DerateMoney,@FreeTime,@StartTime,@EndTime,@FeeRuleID,@LastUpdateTime,@HaveUpdate,@DataStatus,@MaxTimesCycle,@MaxTimes,@ValidityTime)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("DerateID", model.DerateID);
            dbOperator.AddParameter("SellerID", model.SellerID);
            dbOperator.AddParameter("Name", model.Name);
            dbOperator.AddParameter("DerateSwparate", (int)model.DerateSwparate);
            dbOperator.AddParameter("DerateType", (int)model.DerateType);
            dbOperator.AddParameter("DerateMoney", model.DerateMoney);
            dbOperator.AddParameter("FreeTime", model.FreeTime);
            dbOperator.AddParameter("StartTime", model.StartTime);
            dbOperator.AddParameter("EndTime", model.EndTime);
            dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("MaxTimesCycle", model.MaxTimesCycle);
            dbOperator.AddParameter("MaxTimes", model.MaxTimes);
            dbOperator.AddParameter("ValidityTime", model.ValidityTime);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        private bool AddDerateIntervar(List<ParkDerateIntervar> models, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            if (models == null || models.Count == 0) return true;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ParkDerateIntervar(DerateIntervarID,DerateID,FreeTime,Monetry,LastUpdateTime,HaveUpdate,DataStatus)");
            bool hasData = false;
            int index = 1;
            foreach (var p in models)
            {
                p.DataStatus = DataStatus.Normal;
                p.LastUpdateTime = DateTime.Now;
                p.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                strSql.AppendFormat(" SELECT @DerateIntervarID{0},@DerateID{0},@FreeTime{0},@Monetry{0},@LastUpdateTime{0},@HaveUpdate{0},@DataStatus{0}  UNION ALL", index);

                dbOperator.AddParameter("DerateIntervarID"+index, p.DerateIntervarID);
                dbOperator.AddParameter("DerateID" + index, p.DerateID);
                dbOperator.AddParameter("FreeTime" + index, p.FreeTime);
                dbOperator.AddParameter("Monetry" + index, p.Monetry);
                dbOperator.AddParameter("LastUpdateTime" + index, p.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate" + index, p.HaveUpdate);
                dbOperator.AddParameter("DataStatus" + index, (int)p.DataStatus);
                hasData = true;
                index++;
            }
            if (hasData)
            {
                return dbOperator.ExecuteNonQuery(strSql.Remove(strSql.Length - 10, 10).ToString()) > 0;
            }
            return false;
        }
        public bool Update(ParkDerate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = Update(model, dbOperator);
                    if (!result) throw new MyException("修改优免信息失败");

                    CommonDelete("ParkDerateIntervar", "DerateID", model.DerateID, dbOperator);

                    result = AddDerateIntervar(model.DerateIntervar, dbOperator);
                    if (!result) throw new MyException("添加优免时段信息失败");

                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
          
        }

        private bool Update(ParkDerate model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkDerate set SellerID=@SellerID,[Name]=@Name,DerateSwparate=@DerateSwparate,DerateType=@DerateType,DerateMoney=@DerateMoney,FreeTime=@FreeTime,StartTime=@StartTime");
            strSql.Append(" ,EndTime=@EndTime,FeeRuleID=@FeeRuleID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,MaxTimesCycle=@MaxTimesCycle,MaxTimes=@MaxTimes,ValidityTime=@ValidityTime");
            strSql.Append(" where DerateID=@DerateID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("DerateID", model.DerateID);
            dbOperator.AddParameter("SellerID", model.SellerID);
            dbOperator.AddParameter("Name", model.Name);
            dbOperator.AddParameter("DerateSwparate", (int)model.DerateSwparate);
            dbOperator.AddParameter("DerateType", (int)model.DerateType);
            dbOperator.AddParameter("DerateMoney", model.DerateMoney);
            dbOperator.AddParameter("FreeTime", model.FreeTime);
            dbOperator.AddParameter("StartTime", model.StartTime);
            dbOperator.AddParameter("EndTime", model.EndTime);
            dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("MaxTimesCycle", model.MaxTimesCycle);
            dbOperator.AddParameter("MaxTimes", model.MaxTimes);
            dbOperator.AddParameter("ValidityTime", model.ValidityTime);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string derateId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = CommonDelete("ParkDerate", "DerateID", derateId, dbOperator);
                    if (!result) throw new MyException("移除优免信息失败");

                    CommonDelete("ParkDerateIntervar", "DerateID", derateId, dbOperator);
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public bool DeleteBySellerID(string sellerId)
        {
            List<ParkDerate> models = QueryBySellerID(sellerId);
            if (models.Count == 0) return true;

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = CommonDelete("ParkDerate", "SellerID", sellerId, dbOperator);
                    if (!result) throw new MyException("删除优免信息失败");

                    foreach (var item in models) {
                        CommonDelete("ParkDerateIntervar", "DerateID", item.DerateID, dbOperator);
                    }
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public List<ParkDerate> QueryBySellerID(string sellerId)
        {
            List<ParkDerate> models = new List<ParkDerate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerate  where SellerID= @SellerID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkDerate>.ToModel(reader));
                    }
                }
            }
            foreach (var item in models) {
                item.DerateIntervar = QueryParkDerateIntervar(item.DerateID);
            }
            return models;
        }

        public ParkDerate Query(string derateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerate  where DerateID= @DerateID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DerateID", derateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        ParkDerate model = DataReaderToModel<ParkDerate>.ToModel(reader);
                        model.DerateIntervar = QueryParkDerateIntervar(model.DerateID);
                        return model;
                    }
                    return null;
                }
            }
        }
        public  List<ParkDerateIntervar> QueryParkDerateIntervar(string derateId)
        {
            List<ParkDerateIntervar> models = new List<ParkDerateIntervar>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDerateIntervar where DerateID= @DerateID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DerateID", derateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkDerateIntervar>.ToModel(reader));
                    }
                }
            }
            return models;
        }
    }
}
