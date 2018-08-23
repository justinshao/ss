using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using Common.IRepository;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class SysScopeDAL : BaseDAL, ISysScope
    {
        public bool Add(SysScope model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(SysScope model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SysScope(ASID,ASName,CPID,IsDefaultScope,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@ASID,@ASName,@CPID,@IsDefaultScope,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("ASID", model.ASID);
            dbOperator.AddParameter("ASName", model.ASName);
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("IsDefaultScope", (int)model.IsDefaultScope);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(SysScope model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update SysScope set ASName=@ASName,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where ASID=@ASID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ASID", model.ASID);
                dbOperator.AddParameter("ASName", model.ASName);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool DeleteByRecordId(string recordId)
        {
            return CommonDelete("SysScope", "ASID", recordId);
        }

        public SysScope QuerySysScopeByRecordId(string recordId)
        {
            string sql = "select ID,ASID,ASName,CPID,IsDefaultScope,LastUpdateTime,HaveUpdate,DataStatus from SysScope where ASID=@ASID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ASID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysScope> models = GetSysScope(reader);
                    return models.FirstOrDefault();
                }
            }
        }
        private List<SysScope> GetSysScope(DbDataReader reader)
        {
            List<SysScope> models = new List<SysScope>();
            while (reader.Read())
            {
                models.Add(new SysScope()
                {
                    ID = reader.GetInt32DefaultZero(0),
                    ASID = reader.GetStringDefaultEmpty(1),
                    ASName = reader.GetStringDefaultEmpty(2),
                    CPID = reader.GetStringDefaultEmpty(3),
                    IsDefaultScope = (YesOrNo)reader.GetInt32DefaultZero(4),
                    LastUpdateTime = reader.GetDateTimeDefaultMin(5),
                    HaveUpdate = reader.GetInt32DefaultZero(6),
                    DataStatus = (DataStatus)reader.GetInt32DefaultZero(7)
                });
            }
            return models;
        }
        public List<SysScope> QuerySysScopeByCompanyId(string companyId)
        {
            string sql = "select ID,ASID,ASName,CPID,IsDefaultScope,LastUpdateTime,HaveUpdate,DataStatus from SysScope where CPID=@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPID", companyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysScope(reader);
                }
            }
        }


        public List<SysScope> QuerySysScopeByUserId(string userId)
        {
            StringBuilder strSql = new StringBuilder();
           using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                strSql.Append("select distinct s.ID,s.ASID,s.ASName,s.CPID,s.IsDefaultScope,s.LastUpdateTime,s.HaveUpdate,s.DataStatus from SysScope s inner join SysUserScopeMapping t on s.ASID=t.ASID where s.DataStatus!=@DataStatus and t.DataStatus!=@DataStatus and t.UserRecordID =@UserRecordID");
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("UserRecordID", userId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetSysScope(reader);
                }
            }
        }
    }
}
