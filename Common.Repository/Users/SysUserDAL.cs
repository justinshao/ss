using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class SysUserDAL : BaseDAL, ISysUser
    {

        public SysUser QuerySysUserByUserAccount(string userAccount)
        {
            string sql = "select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser where UserAccount=@UserAccount and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("UserAccount", userAccount);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysUser> users = GetSysUser(reader);
                    return users.FirstOrDefault();
                }
            }
        }


        private List<SysUser> GetSysUser(DbDataReader reader)
        {
            List<SysUser> users = new List<SysUser>();
            while (reader.Read())
            {
                users.Add(DataReaderToModel<SysUser>.ToModel(reader));
            }
            return users;
        }
        public SysUser QuerySysUserByRecordId(string recordId)
        {
            string sql = "select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser where RecordID=@RecordID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysUser> users = GetSysUser(reader);
                    return users.FirstOrDefault();
                }
            }
        }

        public bool Update(SysUser model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set UserName=@UserName,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                strSql.Append(",Password=@Password");
            }
            strSql.Append(" where RecordID=@RecordID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("UserName", model.UserName);
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                dbOperator.AddParameter("Password", model.Password);
            }
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("RecordID", model.RecordID);

            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Add(SysUser model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SysUser(RecordID,UserAccount,UserName,Password,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser)");
            strSql.Append(" values(@RecordID,@UserAccount,@UserName,@Password,@LastUpdateTime,@HaveUpdate,@CPID,@DataStatus,@IsDefaultUser)");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("UserAccount", model.UserAccount);
            dbOperator.AddParameter("UserName", model.UserName);
            dbOperator.AddParameter("Password", model.Password);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("IsDefaultUser", (int)model.IsDefaultUser);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Add(SysUser model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Update(SysUser model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Update(model, dbOperator);
            }
        }

        public bool LoginError(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set PwdErrorTime=@PwdErrorTime,PwdErrorCount=PwdErrorCount+1,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PwdErrorTime", DateTime.Now);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("RecordID", recordId);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool LoginErrorByUserId(string userAccount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set PwdErrorTime=@PwdErrorTime,PwdErrorCount=PwdErrorCount+1,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where UserAccount=@UserAccount");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PwdErrorTime", DateTime.Now);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("UserAccount", userAccount);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool LoginSuccess(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set PwdErrorCount=0,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("RecordID", recordId);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public List<SysUser> QuerySysUserByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser ");
            strSql.Append(" where CPID in(select CPID from BaseVillage where VID in (select VID from BaseParkinfo where PKID=@PKID)) and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                List<SysUser> models = new List<SysUser>();
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    models = GetSysUser(reader);
                }
                return models;
            }
        }

        public List<SysUser> QuerySysUserPage(string companyId, string username, int pagesize, int pageindex, out int totalCount)
        {
            string sql = "select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser where DataStatus!=@DataStatus";
            totalCount = 0;
            string sequence = " id desc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    sql += " and CPID=@CPID";
                    dbOperator.AddParameter("CPID", companyId);
                }
                if (!string.IsNullOrWhiteSpace(username))
                {
                    sql += " and UserAccount like @UserAccount";
                    dbOperator.AddParameter("UserAccount", "%" + username + "%");
                }

                using (DbDataReader reader = dbOperator.Paging(sql, sequence, pageindex, pagesize, out totalCount))
                {
                    return GetSysUser(reader);
                }
            }
        }

        public List<SysUser> QuerySysUserAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser ");
            strSql.Append(" where DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                List<SysUser> models = new List<SysUser>();
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    models = GetSysUser(reader);
                }
                return models;
            }
        }

        public List<SysUser> QuerySysUserByCompanys(List<string> companys)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser from SysUser ");
            strSql.Append(" where DataStatus!=@DataStatus and cpid in ('" + string.Join("','", companys) + "')");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                List<SysUser> models = new List<SysUser>();
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    models = GetSysUser(reader);
                }
                return models;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("SysUser", "RecordID", recordId);
        }
        public bool Delete(string recordId, DbOperator dbOperator)
        {
            return CommonDelete("SysUser", "RecordID", recordId, dbOperator);
        }
        public bool DeleteByCompanyId(string companyId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where CPID=@CPID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CPID", companyId);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool ResetPassword(string userAccount, string newPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set Password=@Password,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where UserAccount=@UserAccount");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Password", newPassword);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                dbOperator.AddParameter("UserAccount", userAccount);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}
