using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.IRepository;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ExceptionsDAL : IExceptions
    {
        public void AddExceptions(Exceptions exceptions)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO EXCEPTIONS(SOURCE,SERVER,DESCRIPTION,DETAIL,TRACK,[DateTime],LOGFROM) VALUES (@SOURCE,@SERVER,@DESCRIPTION,@DETAIL,@TRACK,@CREATETIME,@LOGFROM);");
                dbOperator.AddParameter("SOURCE", exceptions.Source);
                dbOperator.AddParameter("SERVER", exceptions.Server);
                dbOperator.AddParameter("DESCRIPTION", exceptions.Description);
                dbOperator.AddParameter("DETAIL", exceptions.Detail);
                dbOperator.AddParameter("TRACK", exceptions.Track);
                dbOperator.AddParameter("CREATETIME", exceptions.DateTime);
                dbOperator.AddParameter("LOGFROM", (int)exceptions.LogFrom);
                dbOperator.ExecuteNonQuery(strSql.ToString());
            }
        }

        public List<Exceptions> LoadExceptions(ExceptionCondition condition, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM EXCEPTIONS ");
            sql.Append(" WHERE  [DateTime]>=@STARTCREATETIME  AND [DateTime]<=@ENDCREATETIME");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", condition.TimeStart.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", condition.TimeEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(condition.Description))
                {
                    sql.Append(" AND DESCRIPTION=@DESCRIPTION");
                    dbOperator.AddParameter("DESCRIPTION", condition.Description);
                }
                if (!string.IsNullOrEmpty(condition.Detail))
                {
                    sql.Append(" AND DETAIL LIKE @DETAIL");
                    dbOperator.AddParameter("DETAIL", "%" + condition.Detail + "%");
                }
                if (!string.IsNullOrEmpty(condition.Server))
                {
                    sql.Append(" AND SERVER=@SERVER");
                    dbOperator.AddParameter("SERVER", condition.Server);
                }

                if (!string.IsNullOrEmpty(condition.Source))
                {
                    sql.Append(" AND SOURCE LIKE @SOURCE");
                    dbOperator.AddParameter("SOURCE", "%" + condition.Source + "%");
                }
                if (condition.logFrom.HasValue)
                {
                    sql.Append(" AND LOGFROM=@LOGFROM");
                    dbOperator.AddParameter("LOGFROM", (int)condition.logFrom);
                }

                List<Exceptions> excetions = new List<Exceptions>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "[DateTime] DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        excetions.Add(DataReaderToModel<Exceptions>.ToModel(reader));
                    }

                }
                return excetions;

            }
        }
    }
}
