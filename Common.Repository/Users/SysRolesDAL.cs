using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class SysRolesDAL : BaseDAL, ISysRoles
    {

        public bool AddSysRole(SysRoles model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return AddSysRole(model, dbOperator);
            }
        }

        public bool AddSysRole(SysRoles model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SysRoles(RecordID,RoleName,CPID,IsDefaultRole,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@RecordID,@RoleName,@CPID,@IsDefaultRole,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("RoleName", model.RoleName);
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("IsDefaultRole", (int)model.IsDefaultRole);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool UpdateRole(SysRoles model)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysRoles set RoleName=@RoleName,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where RecordID=@RecordID");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("RoleName", model.RoleName);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool DeleteRoleByRecordId(string recordId)
        {
            return CommonDelete("SysRoles", "RecordID", recordId);
        }

        public SysRoles QuerySysRolesByRecordId(string recordId)
        {
            string sql = "select ID,RecordID,RoleName,CPID,IsDefaultRole,LastUpdateTime,HaveUpdate,DataStatus from SysRoles where RecordID=@RecordID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysRoles> roles = GetSysRoles(reader);
                    return roles.FirstOrDefault();
                }
            }
        }
        private List<SysRoles> GetSysRoles(DbDataReader reader)
        {
            List<SysRoles> roles = new List<SysRoles>();
            while (reader.Read())
            {
                roles.Add(new SysRoles()
                {
                    ID = reader.GetInt32DefaultZero(0),
                    RecordID = reader.GetStringDefaultEmpty(1),
                    RoleName = reader.GetStringDefaultEmpty(2),
                    CPID = reader.GetStringDefaultEmpty(3),
                    IsDefaultRole = (YesOrNo)reader.GetInt32DefaultZero(4),
                    LastUpdateTime = reader.GetDateTimeDefaultMin(5),
                    HaveUpdate = reader.GetInt32DefaultZero(6),
                    DataStatus = (DataStatus)reader.GetInt32DefaultZero(7)
                });
            }
            return roles;
        }
        public List<SysRoles> QuerySysRolesByCompanyId(string companyId)
        {
            string sql = "select ID,RecordID,RoleName,CPID,IsDefaultRole,LastUpdateTime,HaveUpdate,DataStatus from SysRoles where CPID=@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPID", companyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysRoles(reader);
                }
            }
        }


        public List<SysRoles> QuerySysRolesByUserId(string userId)
        {
            string sql = "select distinct r.ID,r.RecordID,r.RoleName,r.CPID,r.IsDefaultRole,r.LastUpdateTime,r.HaveUpdate,r.DataStatus from SysRoles r inner join SysUserRolesMapping m on r.RecordID=m.RoleID where m.UserRecordID =@UserRecordID and m.DataStatus!=@DataStatus and r.DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("UserRecordID", userId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetSysRoles(reader);
                }
            }
        }
    }
}
