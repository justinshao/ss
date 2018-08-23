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
    public class SysUserScopeMappingDAL : BaseDAL, ISysUserScopeMapping
    {

        public bool DeleteByUserID(string userId, DbOperator dbOperator)
        {
            return CommonDelete("SysUserScopeMapping", "UserRecordID", userId, dbOperator);
        }

        public List<SysUserScopeMapping> QuerySysUserScopeMappingByUserId(string userId)
        {
            string sql = "select ID,RecordID,UserRecordID,ASID,LastUpdateTime,HaveUpdate,DataStatus from SysUserScopeMapping where UserRecordID=@UserRecordID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("UserRecordID", userId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysUserScopeMapping(reader);
                }
            }
        }
        private List<SysUserScopeMapping> GetSysUserScopeMapping(DbDataReader reader)
        {
            List<SysUserScopeMapping> models = new List<SysUserScopeMapping>();
            while (reader.Read())
            {
                models.Add(new SysUserScopeMapping()
                {
                    ID = reader.GetInt32DefaultZero(0),
                    RecordID = reader.GetStringDefaultEmpty(1),
                    UserRecordID = reader.GetStringDefaultEmpty(2),
                    ASID = reader.GetStringDefaultEmpty(3),
                    LastUpdateTime = reader.GetDateTimeDefaultMin(4),
                    HaveUpdate = reader.GetInt32DefaultZero(5),
                    DataStatus = (DataStatus)reader.GetInt32DefaultZero(6)
                });
            }
            return models;
        }
        public bool Add(List<SysUserScopeMapping> models, DbOperator dbOperator)
        {
            foreach (var item in models)
            {
                item.DataStatus = DataStatus.Normal;
                item.LastUpdateTime = DateTime.Now;
                item.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SysUserScopeMapping(RecordID,UserRecordID,ASID,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@RecordID,@UserRecordID,@ASID,@LastUpdateTime,@HaveUpdate,@DataStatus)");

                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", item.RecordID);
                dbOperator.AddParameter("UserRecordID", item.UserRecordID);
                dbOperator.AddParameter("ASID", item.ASID);
                dbOperator.AddParameter("LastUpdateTime", item.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", item.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)item.DataStatus);
                bool result = dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                if (!result) return false;
            }
            return true;
        }
    }
}
