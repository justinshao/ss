using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class SysRoleAuthorizeDAL : BaseDAL, ISysRoleAuthorize
    {

        public List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleId(string roleId)
        {
            string sql = "select ID,RecordID,RoleID,ModuleID,ParentID,LastUpdateTime,HaveUpdate,DataStatus from SysRoleAuthorize where RoleID=@RoleID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RoleID", roleId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysRoles(reader);
                }
            }
        }
        private List<SysRoleAuthorize> GetSysRoles(DbDataReader reader)
        {
            List<SysRoleAuthorize> models = new List<SysRoleAuthorize>();
            while (reader.Read())
            {
                models.Add(new SysRoleAuthorize()
                {
                    ID = reader.GetInt32DefaultZero(0),
                    RecordID = reader.GetStringDefaultEmpty(1),
                    RoleID = reader.GetStringDefaultEmpty(2),
                    ModuleID = reader.GetStringDefaultEmpty(3),
                    ParentID = reader.GetStringDefaultEmpty(4),
                    LastUpdateTime = reader.GetDateTimeDefaultMin(5),
                    HaveUpdate = reader.GetInt32DefaultZero(6),
                    DataStatus = (DataStatus)reader.GetInt32DefaultZero(7)
                });
            }
            return models;
        }
        public List<SysRoleAuthorize> QuerySysRoleAuthorizeByRoleIds(List<string> roleIds)
        {
            string sql = string.Format("select ID,RecordID,RoleID,ModuleID,ParentID,LastUpdateTime,HaveUpdate,DataStatus from SysRoleAuthorize where RoleID in('{0}') and DataStatus!=@DataStatus;", string.Join("','", roleIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<SysRoleAuthorize> models = new List<SysRoleAuthorize>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysRoleAuthorize> roles =  GetSysRoles(reader);
                    foreach (var item in roles)
                    {
                        if (models.Count(p => p.ModuleID == item.ModuleID) == 0) {
                            models.Add(item);
                        }
                    }
                }
                return models;
            }
        }

        public bool CheckUserAuthorize(string userID, string modelID)
        {
            string sql = string.Format(@"SELECT * from SysRoleAuthorize r LEFT JOIN SysUserRolesMapping u  on r.RoleID=u.RoleID 
                    WHERE r.ModuleID=@ModuleID  and u.UserRecordID=@UserRecordID and r.DataStatus!=@DataStatus and  u.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("UserRecordID", userID);
                dbOperator.AddParameter("ModuleID", modelID);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Add(List<SysRoleAuthorize> models, DbOperator dbOperator)
        {
           
                foreach (var item in models)
                {
                    item.DataStatus = DataStatus.Normal;
                    item.LastUpdateTime = DateTime.Now;
                    item.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into SysRoleAuthorize(RecordID,RoleID,ModuleID,ParentID,LastUpdateTime,HaveUpdate,DataStatus)");
                    strSql.Append(" values(@RecordID,@RoleID,@ModuleID,@ParentID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", item.RecordID);
                    dbOperator.AddParameter("RoleID", item.RoleID);
                    dbOperator.AddParameter("ModuleID", item.ModuleID);
                    dbOperator.AddParameter("ParentID", item.ParentID);
                    dbOperator.AddParameter("LastUpdateTime", item.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", item.HaveUpdate);
                    dbOperator.AddParameter("DataStatus", (int)item.DataStatus);
                    bool result = dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                    if (!result) return false;
                }
            return true;
        }

        public bool DeleteByRoleId(string roleId, DbOperator dbOperator)
        {
            return CommonDelete("SysRoleAuthorize", "RoleID", roleId);
        }
    }
}
