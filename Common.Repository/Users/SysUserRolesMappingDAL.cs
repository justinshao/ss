using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class SysUserRolesMappingDAL : BaseDAL, ISysUserRolesMapping
    {
        public List<SysUserRolesMapping> GetSysUserRolesMappingByUserId(string userId)
        {
            string sql = "select ID,RecordID,UserRecordID,RoleID,LastUpdateTime,HaveUpdate,DataStatus from SysUserRolesMapping where UserRecordID=@UserRecordID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("UserRecordID", userId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysUserRolesMapping(reader);
                }
            }
        }
        private List<SysUserRolesMapping> GetSysUserRolesMapping(DbDataReader reader)
        {
            List<SysUserRolesMapping> models = new List<SysUserRolesMapping>();
            while (reader.Read())
            {
                models.Add(new SysUserRolesMapping()
                {
                    ID = reader.GetInt32DefaultZero(0),
                    RecordID = reader.GetStringDefaultEmpty(1),
                    UserRecordID = reader.GetStringDefaultEmpty(2),
                    RoleID = reader.GetStringDefaultEmpty(3),
                    LastUpdateTime = reader.GetDateTimeDefaultMin(4),
                    HaveUpdate = reader.GetInt32DefaultZero(5),
                    DataStatus = (DataStatus)reader.GetInt32DefaultZero(6)
                });
            }
            return models;
        }
        public bool Add(List<SysUserRolesMapping> models, DbOperator dbOperator)
        {
            foreach (var item in models) {
                item.DataStatus = DataStatus.Normal;
                item.LastUpdateTime = DateTime.Now;
                item.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SysUserRolesMapping(RecordID,UserRecordID,RoleID,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@RecordID,@UserRecordID,@RoleID,@LastUpdateTime,@HaveUpdate,@DataStatus)");

                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", item.RecordID);
                dbOperator.AddParameter("UserRecordID", item.UserRecordID);
                dbOperator.AddParameter("RoleID", item.RoleID);
                dbOperator.AddParameter("LastUpdateTime", item.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", item.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)item.DataStatus);
                bool result = dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                if (!result)return false;
            }
            return true;
        }

        public bool DeleteByUserId(string userId, DbOperator dbOperator)
        {
            return CommonDelete("SysUserRolesMapping", "UserRecordID", userId,dbOperator);
        }
    }
}
