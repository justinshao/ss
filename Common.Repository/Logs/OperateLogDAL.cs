using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class OperateLogDAL : IOperateLog
    {
        public void Add(OperateLog model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO OperateLog(Operator,OperatorIP,OperateTime,ModuleName,MethodName,OperatorType,LogFrom,OperatorContent) VALUES (@Operator,@OperatorIP,@OperateTime,@ModuleName,@MethodName,@OperatorType,@LogFrom,@OperatorContent);");
                dbOperator.AddParameter("Operator", model.Operator);
                dbOperator.AddParameter("OperatorIP", model.OperatorIP);
                dbOperator.AddParameter("OperateTime", model.OperateTime);
                dbOperator.AddParameter("ModuleName", model.ModuleName);
                dbOperator.AddParameter("MethodName", model.MethodName);
                dbOperator.AddParameter("OperatorType", (int)model.OperateType);
                dbOperator.AddParameter("LogFrom", (int)model.LogFrom);
                dbOperator.AddParameter("OperatorContent", model.OperatorContent);
                dbOperator.ExecuteNonQuery(strSql.ToString());
            }
        }

        public Paging<OperateLog> QueryPage(OperateLogCondition condition, int pagesize, int pageindex, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID,Operator,OperatorIP,OperateTime,ModuleName,MethodName,OperatorType,LogFrom,OperatorContent FROM OperateLog ");
            sql.Append(" WHERE  OperateTime>=@STARTCREATETIME  AND OperateTime<=@ENDCREATETIME");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(condition.UserAccount))
                {
                    sql.Append(" AND Operator like @Operator");
                    dbOperator.AddParameter("Operator", "%" + condition.UserAccount + "%");
                }
                if (!string.IsNullOrEmpty(condition.ModuleName))
                {
                    sql.Append(" AND ModuleName like @ModuleName");
                    dbOperator.AddParameter("ModuleName", "%" + condition.ModuleName + "%");
                }
                if (!string.IsNullOrEmpty(condition.MethodName))
                {
                    sql.Append(" AND MethodName like @MethodName");
                    dbOperator.AddParameter("MethodName", "%" + condition.MethodName + "%");
                }
                if (condition.LogFrom.HasValue)
                {
                    sql.Append(" AND LOGFROM=@LOGFROM");
                    dbOperator.AddParameter("LOGFROM", (int)condition.LogFrom.Value);
                }
                if (condition.OperateType.HasValue)
                {
                    sql.Append(" AND OperatorType=@OperatorType");
                    dbOperator.AddParameter("OperatorType", (int)condition.OperateType.Value);
                }
                List<OperateLog> models = new List<OperateLog>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "OperateTime DESC", pageindex, pagesize, out total))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<OperateLog>.ToModel(reader));
                    }

                }
                return new Paging<OperateLog>(models, pageindex, pagesize, total);
            }
        }
    }
}
