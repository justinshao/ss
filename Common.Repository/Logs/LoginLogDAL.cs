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
    public class LoginLogDAL : ILoginLog
    {
        public void Add(LoginLog model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO LoginLog(UserAccount,LoginIP,LoginTime,LogFrom,Remark) VALUES (@UserAccount,@LoginIP,@LoginTime,@LogFrom,@Remark);");
                dbOperator.AddParameter("UserAccount", model.UserAccount);
                dbOperator.AddParameter("LoginIP", model.LoginIP);
                dbOperator.AddParameter("LoginTime", model.LoginTime);
                dbOperator.AddParameter("LogFrom", (int)model.LogFrom);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.ExecuteNonQuery(strSql.ToString());
            }
        }

        public void UpdateLogoutTime(string loginAccount)
        {
            int? lastId = QueryLastLoginLogByAccount(loginAccount);
            if (!lastId.HasValue) return;

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update LoginLog set LogoutTime=@LogoutTime where ID=@ID;");
                dbOperator.AddParameter("LogoutTime", DateTime.Now);
                dbOperator.AddParameter("ID", lastId.Value);
                dbOperator.ExecuteNonQuery(strSql.ToString());
            }
        }
        private int? QueryLastLoginLogByAccount(string acccount)
        {
            string sql = string.Format("select top 1 ID from LoginLog where UserAccount=@UserAccount order by LoginTime desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("UserAccount", acccount);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32DefaultNull(0);
                    }
                    return null;
                }
            }
        }
        public Paging<LoginLog> QueryPage(LoginLogCondition condition, int pagesize, int pageindex, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID,UserAccount,LoginIP,LoginTime,LogoutTime,LOGFROM,Remark FROM LoginLog ");
            sql.Append(" WHERE  LoginTime>=@STARTCREATETIME  AND LoginTime<=@ENDCREATETIME");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(condition.UserAccount))
                {
                    sql.Append(" AND UserAccount=@UserAccount");
                    dbOperator.AddParameter("UserAccount", condition.UserAccount);
                }
                if (condition.LogFrom.HasValue)
                {
                    sql.Append(" AND LOGFROM=@LOGFROM");
                    dbOperator.AddParameter("LOGFROM", (int)condition.LogFrom);
                }
                List<LoginLog> models = new List<LoginLog>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "LoginTime DESC", pageindex, pagesize, out total))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<LoginLog>.ToModel(reader));
                    }

                }
                return new Paging<LoginLog>(models, pageindex, pagesize, total);
            }
        }
    }
}
