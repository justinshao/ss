using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Core.Expands;
using Common.Utilities;
namespace Common.SqlRepository
{
    public class ParkFeeRuleDAL : BaseDAL, IParkFeeRule
    {
        public bool Add(ParkFeeRule model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = AddFeeRule(model, dbOperator);
                    if (!result) throw new MyException("添加收费规则失败");
                    result = AddFeeRuleDetails(model.ParkFeeRuleDetails, dbOperator);
                    if (!result) throw new MyException("添加收费规则明细失败");
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        private bool AddFeeRule(ParkFeeRule model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkFeeRule(FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,LastUpdateTime,HaveUpdate,DataStatus,RuleText,IsOffline)");
            strSql.Append(" values(@FeeRuleID,@RuleName,@FeeType,@CarTypeID,@CarModelID,@AreaID,@LastUpdateTime,@HaveUpdate,@DataStatus,@RuleText,@IsOffline)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
            dbOperator.AddParameter("RuleName", model.RuleName);
            dbOperator.AddParameter("FeeType", (int)model.FeeType);
            dbOperator.AddParameter("CarTypeID", model.CarTypeID);
            dbOperator.AddParameter("CarModelID", model.CarModelID);
            dbOperator.AddParameter("AreaID", model.AreaID);
            dbOperator.AddParameter("RuleText", model.RuleText);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("IsOffline", model.IsOffline);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        private bool AddFeeRuleDetails(List<ParkFeeRuleDetail> parkFeeRuleDetails, DbOperator dbOperator)
        {
            if (parkFeeRuleDetails == null || parkFeeRuleDetails.Count == 0) throw new ArgumentNullException("parkFeeRuleDetails");

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkFeeRuleDetail(RuleDetailID,RuleID,StartTime,EndTime,Supplement,LoopType,Limit");
            strSql.Append(",FreeTime,FirstTime,FirstFee,Loop1PerTime,Loop1PerFee,Loop2Start,Loop2PerTime,Loop2PerFee,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@RuleDetailID,@RuleID,@StartTime,@EndTime,@Supplement,@LoopType,@Limit");
            strSql.Append(",@FreeTime,@FirstTime,@FirstFee,@Loop1PerTime,@Loop1PerFee,@Loop2Start,@Loop2PerTime,@Loop2PerFee,@LastUpdateTime,@HaveUpdate,@DataStatus)");

            foreach (var model in parkFeeRuleDetails)
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                dbOperator.ClearParameters();
                dbOperator.AddParameter("RuleDetailID", model.RuleDetailID);
                dbOperator.AddParameter("RuleID", model.RuleID);
                dbOperator.AddParameter("StartTime", model.StartTime);
                dbOperator.AddParameter("EndTime", model.EndTime);
                dbOperator.AddParameter("Supplement", model.Supplement);
                dbOperator.AddParameter("LoopType", (int)model.LoopType);
                dbOperator.AddParameter("Limit", model.Limit);
                dbOperator.AddParameter("FreeTime", model.FreeTime);
                dbOperator.AddParameter("FirstTime", model.FirstTime);
                dbOperator.AddParameter("FirstFee", model.FirstFee);
                dbOperator.AddParameter("Loop1PerTime", model.Loop1PerTime);
                dbOperator.AddParameter("Loop1PerFee", model.Loop1PerFee);
                dbOperator.AddParameter("Loop2Start", model.Loop2Start);
                dbOperator.AddParameter("Loop2PerTime", model.Loop2PerTime);
                dbOperator.AddParameter("Loop2PerFee", model.Loop2PerFee);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                bool result = dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                if (!result) return false;
            }
            return true;
        }
        public bool Update(ParkFeeRule model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = UpdateFeeRule(model, dbOperator);
                    if (!result) throw new MyException("修改收费规则失败");

                    result = CommonDelete("ParkFeeRuleDetail", "RuleID", model.FeeRuleID, dbOperator);
                    if (!result) throw new MyException("删除收费规则明细失败");

                    result = AddFeeRuleDetails(model.ParkFeeRuleDetails, dbOperator);
                    if (!result) throw new MyException("添加收费规则明细失败");

                    dbOperator.CommitTransaction();
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        private bool UpdateFeeRule(ParkFeeRule model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkFeeRule set RuleName=@RuleName,FeeType=@FeeType,CarTypeID=@CarTypeID,CarModelID=@CarModelID,AreaID=@AreaID");
            strSql.Append(" ,RuleText=@RuleText,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsOffline=@IsOffline where FeeRuleID=@FeeRuleID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
            dbOperator.AddParameter("RuleName", model.RuleName);
            dbOperator.AddParameter("FeeType", (int)model.FeeType);
            dbOperator.AddParameter("CarTypeID", model.CarTypeID);
            dbOperator.AddParameter("CarModelID", model.CarModelID);
            dbOperator.AddParameter("AreaID", model.AreaID);
            dbOperator.AddParameter("RuleText", model.RuleText);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("IsOffline", model.IsOffline);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Delete(string feeRuleId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = CommonDelete("ParkFeeRule", "FeeRuleID", feeRuleId, dbOperator);
                    if (!result) throw new MyException("删除收费规则失败");
                    result = CommonDelete("ParkFeeRuleDetail", "RuleID", feeRuleId, dbOperator);
                    if (!result) throw new MyException("删除收费规则明细失败");
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }

        }

        public List<ParkFeeRule> QueryFeeRuleByCarModelAndCarType(string areaId, string carModelId, string carTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,RuleText,IsOffline,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.Append(" from ParkFeeRule where AreaID=@AreaID and CarTypeID=@CarTypeID and CarModelID=@CarModelID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", areaId);
                dbOperator.AddParameter("CarTypeID", carTypeId);
                dbOperator.AddParameter("CarModelID", carModelId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRules(reader);
                }
            }
        }
        private List<ParkFeeRule> GetParkFeeRules(DbDataReader reader)
        {
            List<ParkFeeRule> models = new List<ParkFeeRule>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<ParkFeeRule>.ToModel(reader));
            }
            if (models.Count > 0)
            {
                List<ParkFeeRuleDetail> details = QueryParkFeeRuleDetails(models.Select(p => p.FeeRuleID).ToList());
                foreach (var item in models)
                {
                    item.ParkFeeRuleDetails = details.Where(p => p.RuleID == item.FeeRuleID).ToList();
                }
            }
            return models;
        }
        private List<ParkFeeRuleDetail> QueryParkFeeRuleDetails(List<string> ruleIds)
        {
            List<ParkFeeRuleDetail> details = new List<ParkFeeRuleDetail>();
            if (ruleIds.Count == 0) return details;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RuleDetailID,RuleID,StartTime,EndTime,Supplement,LoopType,Limit");
            strSql.Append(",FreeTime,FirstTime,FirstFee,Loop1PerTime,Loop1PerFee,Loop2Start,Loop2PerTime,Loop2PerFee,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.AppendFormat(" from ParkFeeRuleDetail where RuleID in('{0}') and DataStatus!=@DataStatus order by id", string.Join("','", ruleIds));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRuleDetails(reader);
                }
            }
        }
        private List<ParkFeeRuleDetail> GetParkFeeRuleDetails(DbDataReader reader)
        {
            List<ParkFeeRuleDetail> models = new List<ParkFeeRuleDetail>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<ParkFeeRuleDetail>.ToModel(reader)); 
            }
            return models;
        }
        public ParkFeeRule QueryParkFeeRuleByFeeRuleId(string feeRuleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,RuleText,IsOffline,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.Append(" from ParkFeeRule where FeeRuleID=@FeeRuleID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("FeeRuleID", feeRuleId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRules(reader).FirstOrDefault();
                }
            }
        }

        public ParkFeeRule QueryParkFeeRuleByFeeIsOffline(string PKID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.* ");
            strSql.Append(" from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID where IsOffline=1 and a.DataStatus!=@DataStatus and b.PKID=@PKID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkFeeRule>.ToModel(reader);
                    }

                }
            }
            return null;
        }

        public List<ParkFeeRule> QueryParkFeeRuleByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,RuleText,IsOffline,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.Append(" from ParkFeeRule where AreaID in(select AreaID from ParkArea where PKID =@PKID) and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRules(reader);
                }
            }
        }

        public List<ParkFeeRule> QueryFeeRules(string parkingId, string carTypeId, string carModelId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,RuleText,IsOffline,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.Append(" from ParkFeeRule where AreaID in(select AreaID from ParkArea where PKID =@PKID) and CarTypeID=@CarTypeID and CarModelID=@CarModelID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("CarTypeID", carTypeId);
                dbOperator.AddParameter("CarModelID", carModelId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRules(reader);
                }
            }
        }

        public List<ParkFeeRuleDetail> QueryFeeRuleDetailByFeeRuleId(string feeRuleId)
        {
            List<ParkFeeRuleDetail> details = new List<ParkFeeRuleDetail>();
            if (feeRuleId.IsEmpty()) return details;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RuleDetailID,RuleID,StartTime,EndTime,Supplement,LoopType,Limit");
            strSql.Append(",FreeTime,FirstTime,FirstFee,Loop1PerTime,Loop1PerFee,Loop2Start,Loop2PerTime,Loop2PerFee,LastUpdateTime,HaveUpdate,DataStatus");
            strSql.AppendFormat(" from ParkFeeRuleDetail where RuleID in('{0}') and DataStatus!=@DataStatus order by id", feeRuleId);

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkFeeRuleDetails(reader);
                }
            }
        }
    }
}
