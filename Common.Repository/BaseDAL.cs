using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using Common.Core.Expands;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class BaseDAL
    {
        public static bool CommonDelete(string tableName, string filedName, string filedValue, DbOperator dbOperator)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0} set DataStatus={1},LastUpdateTime='{2}', HaveUpdate={3}", tableName, (int)DataStatus.Delete, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SystemDefaultConfig.DataUpdateFlag);
            strSql.AppendFormat("  where {0}='{1}';", filedName, filedValue);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public static bool CommonDelete(string tableName, string filedName, string filedValue)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return CommonDelete(tableName, filedName, filedValue, dbOperator);
            }
        }
        public static List<T> GetTabelWithPageTab<T>(string tableName, int pageSize, int pageIndex, string where, out int pageCount, out string ErrorMessage, string orderbyDesctag = "") where T : new()
        {
            if (pageIndex < 1)
            {
                ErrorMessage = "pageIndex 从1开始";
            }
            ErrorMessage = "";
            pageCount = 0;
            List<T> modes = new List<T>();
            try
            {
                string strWhere = "";
                if (!where.IsEmpty())
                {
                    strWhere = where;
                }
                string sql = string.Format(@"Select *  From {0} WHERE {1}  ", tableName, strWhere);

                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    DbDataReader reader;
                    if (orderbyDesctag.IsEmpty())
                    {
                        reader = dbOperator.Paging(sql.ToString(), pageIndex, pageSize, out pageCount);
                    }
                    else
                    {
                        reader = dbOperator.Paging(sql.ToString(), "[" + orderbyDesctag + "] DESC", pageIndex, pageSize, out pageCount);
                    }
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            modes.Add(DataReaderToModel<T>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "服务异常!" + e.Message;
            }
            return modes;
        }


    }
}
