using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Threading;

namespace Common.DataAccess
{
    internal abstract class Reconstruct
    {
        public ManualResetEvent Event { get; set; }
        public Exception Exception { get; set; }
    }
    internal class ReconstructPaging : Reconstruct
    {
        public m_Paging Method { get; set; }
        public DbDataReader Result { get; set; }
        public int rowCount { get; set; }
    }
    internal class ReconstructTable : Reconstruct
    {
        public t_Paging Method { get; set; }
        public DataTable Result { get; set; }
        public int rowCount { get; set; }
    }
    internal class ReconstructReader : Reconstruct
    {
        public m_Reader Method { get; set; }
        public DbDataReader Result { get; set; }
    }
    internal class ReconstructTables : Reconstruct
    {
        public t_DataTable Method { get; set; }
        public DataTable Result { get; set; }
    }
    internal delegate DbDataReader m_Paging(DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount);
    internal delegate DataTable t_Paging(DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount);
    internal delegate DataTable t_DataTable(DbOperator dbOperator, string sql);
    internal delegate DbDataReader m_Reader(DbOperator dbOperator, string sql);
    public static class SqlWrapper
    {
        public static DbDataReader Paging(this DbOperator op, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            return Paging(op, sql, pageIndex, pageSize, out rowCount, true);
        }
        public static DbDataReader Paging(this DbOperator op, string sql, int pageIndex, int pageSize, out int rowCount, bool doCount)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            rowCount = Counting(op, sql);

            if (pageIndex < 1) { throw new ArgumentOutOfRangeException("pageIndex"); }
            if (pageSize < 1) { throw new ArgumentOutOfRangeException("pageSize"); }

            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            string paging = string.Format("select t2.* from (SELECT *, ROW_NUMBER() OVER(order by (select 1)) as rowNum from ({0}) as t) t2 where t2.rowNum>={1} and t2.rowNum<={2}", sql, start, end);

            return op.ExecuteReader(paging);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="op"></param>
        /// <param name="sql"></param>
        /// <param name="sequence">需要排序的字段（必须填写）</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static DbDataReader Paging(this DbOperator op, string sql, string sequence, int pageIndex, int pageSize, out int rowCount)
        {
            return Paging(op, sql, sequence, pageIndex, pageSize, out rowCount, true);
        }
        public static DbDataReader Paging(this DbOperator op, string sql, string sequence, int pageIndex, int pageSize, out int rowCount, bool doCount)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            if (pageIndex < 1) { throw new ArgumentOutOfRangeException("pageIndex"); }
            if (pageSize < 1) { throw new ArgumentOutOfRangeException("pageSize"); }
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            rowCount = 0;
            sql = sql.Trim().TrimEnd(';');
            if (doCount) rowCount = Counting(op, sql);
            string paging = string.Format("select t2.* from (SELECT *, ROW_NUMBER() OVER(order by {1}) as rowNum from ({0}) as t) t2 where t2.rowNum>={2} and t2.rowNum<={3}", sql, sequence, start, end);
            return op.ExecuteReader(paging);
        }
        private static DbDataReader Pagings(DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = Counting(dbOperator, sql);
            return null;
        }
        private static DbDataReader Pagings(DbOperator dbOperator, string sql, string sequence, int pageIndex, int pageSize, out int rowCount)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            if (pageIndex < 1) { throw new ArgumentOutOfRangeException("pageIndex"); }
            if (pageSize < 1) { throw new ArgumentOutOfRangeException("pageSize"); }
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            rowCount = Counting(dbOperator, sql);
            string paging = string.Format("select t2.* from (SELECT ROW_NUMBER() OVER(order by {1}) as rowNum,* from ({0}) as t) t2 where t2.rowNum>{2} and t2.rowNum<={3}", sql, sequence, start, end);
            return dbOperator.ExecuteReader(paging);
        }
        private static DbDataReader Readers(DbOperator dbOperator, string sql)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            return dbOperator.ExecuteReader(sql);
        }
        public static DataTable PagingTables(DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            using (DbDataReader reader = Pagings(dbOperator, sql, pageIndex, pageSize, out rowCount))
            {
                if (reader.HasRows)
                {
                    DataTable result = ConstructDataTableSchema(reader);
                    result.BeginLoadData();
                    object[] row = new object[result.Columns.Count];
                    while (reader.Read())
                    {
                        reader.GetValues(row);
                        result.LoadDataRow(row, true);
                    }
                    result.EndLoadData();
                    return result;
                }
                return null;
            }
        }
        public static DataTable BeginPagingTable(this DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            t_Paging paging = new t_Paging(PagingTables);
            ReconstructTable item = new ReconstructTable() { Event = new ManualResetEvent(false), Method = paging };
            IAsyncResult result = paging.BeginInvoke(dbOperator, sql, pageIndex, pageSize, out rowCount, new AsyncCallback(EndPagdingTable), item);
            item.Event.WaitOne();
            if (item.Exception != null)
                throw item.Exception;
            rowCount = item.rowCount;
            return (DataTable)item.Result;
        }
        public static void EndPagdingTable(IAsyncResult result)
        {
            ReconstructTable item = result.AsyncState as ReconstructTable;
            int rowCount = 0;
            try
            {
                item.Result = item.Method.EndInvoke(out rowCount, result);
                item.rowCount = rowCount;
            }
            catch (Exception ex)
            {
                item.Exception = ex;
            }
            finally
            {
                item.Event.Set();
            }
        }
        public static DbDataReader BeginPaging(this DbOperator dbOperator, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            m_Paging paging = new m_Paging(Pagings);
            ReconstructPaging item = new ReconstructPaging() { Event = new ManualResetEvent(false), Method = paging };
            IAsyncResult result = paging.BeginInvoke(dbOperator, sql, pageIndex, pageSize, out rowCount, new AsyncCallback(EndPagding), item);
            item.Event.WaitOne();
            if (item.Exception != null)
                throw item.Exception;
            rowCount = item.rowCount;
            return (DbDataReader)item.Result;
        }
        public static void EndPagding(IAsyncResult result)
        {
            ReconstructPaging item = result.AsyncState as ReconstructPaging;
            int rowCount = 0;
            try
            {
                item.Result = item.Method.EndInvoke(out rowCount, result);
                item.rowCount = rowCount;
            }
            catch (Exception ex)
            {
                item.Exception = ex;
            }
            finally
            {
                item.Event.Set();
            }
        }
        public static DbDataReader BeginExecuteReader(this DbOperator dbOperator, string sql)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            m_Reader readers = new m_Reader(Readers);
            ReconstructReader item = new ReconstructReader() { Event = new ManualResetEvent(false), Method = readers };
            IAsyncResult result = readers.BeginInvoke(dbOperator, sql, new AsyncCallback(EndExecuteReader), item);
            item.Event.WaitOne();
            if (item.Exception != null)
                throw item.Exception;
            return (DbDataReader)item.Result;
        }
        public static void EndExecuteReader(IAsyncResult result)
        {
            ReconstructReader item = result.AsyncState as ReconstructReader;
            try
            {
                item.Result = item.Method.EndInvoke(result);
            }
            catch (Exception ex)
            {
                item.Exception = ex;
            }
            finally
            {
                item.Event.Set();
            }
        }
        private static DataTable Tables(DbOperator dbOperator, string sql)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            return dbOperator.ExecuteTable(sql);
        }
        public static DataTable BeginExecuteTable(this DbOperator dbOperator, string sql)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            t_DataTable paging = new t_DataTable(Tables);
            ReconstructTables item = new ReconstructTables() { Event = new ManualResetEvent(false), Method = paging };
            IAsyncResult result = paging.BeginInvoke(dbOperator, sql, new AsyncCallback(EndExecuteTable), item);
            item.Event.WaitOne();
            if (item.Exception != null)
                throw item.Exception;
            return (DataTable)item.Result;
        }
        public static void EndExecuteTable(IAsyncResult result)
        {
            ReconstructTables item = result.AsyncState as ReconstructTables;
            try
            {
                item.Result = item.Method.EndInvoke(result);
            }
            catch (Exception ex)
            {
                item.Exception = ex;
            }
            finally
            {
                item.Event.Set();
            }
        }
        public static int Counting(this DbOperator op, string sql)
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            string m_sql = sql.ToUpper();
            int index1 = m_sql.IndexOf("SELECT ");
            if (m_sql.IndexOf("*/") > 0)
                index1 = m_sql.IndexOf("*/") - 4;
            int index2 = m_sql.IndexOf(" FROM");
            if (index1 >= 0 && index2 >= 0)
            {
                string sqlSelect = m_sql.Substring(0, index1 + 6);
                string sqlFrom = m_sql.Substring(index2, m_sql.Length - index2);

                int indexDis = m_sql.IndexOf(" DISTINCT");
                if (indexDis > 0)
                {
                    string disSql = m_sql.Substring(indexDis, index2 - indexDis);
                    if (disSql.Contains("DISTINCT"))
                    {
                        string[] disSqls = disSql.Split(' ');
                        string param = string.Empty;
                        for (int i = 0; i < disSqls.Length; i++)
                        {
                            if (disSqls[i].Contains("DISTINCT"))
                            {
                                param = disSqls[i];
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(param))
                            m_sql = sqlSelect + " COUNT(1) NEWROWNUMBER " + sqlFrom;
                        else
                        {
                            m_sql = sqlSelect + " COUNT(" + param + ") NEWROWNUMBER " + sqlFrom;
                        }
                    }
                    else
                    {
                        m_sql = sqlSelect + " COUNT(1) NEWROWNUMBER " + sqlFrom;
                    }
                }
                else
                {
                    m_sql = sqlSelect + " COUNT(1) NEWROWNUMBER " + sqlFrom;
                }

            }
            if (m_sql.LastIndexOf("ORDER BY") > 1)
                m_sql = m_sql.Remove(m_sql.LastIndexOf("ORDER BY"));
            if (m_sql.LastIndexOf("GROUP BY") > 1 && m_sql.LastIndexOf("GROUP BY") > m_sql.LastIndexOf("WHERE")
                 && m_sql.LastIndexOf("GROUP BY") > m_sql.LastIndexOf("JOIN"))//Group by不在子查询中时需要在外面再加一层select count
                m_sql = string.Format("SELECT COUNT(1) FROM ({0}) NEWTAB", m_sql.ToUpper());
            try
            {
                return Convert.ToInt32(op.ExecuteScalar(m_sql));
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(op.ExecuteScalar(string.Format("SELECT COUNT(1) FROM ({0})", sql.ToUpper())));//查询字段中有from时会出现异常，需要按这种方式查询总数
            }
        }
        public static DataTable PagingTable(this DbOperator op, string sql, int pageIndex, int pageSize, out int rowCount)
        {
            return op.PagingTable(sql, pageIndex, pageSize, out rowCount, true);
        }

        public static DataTable PagingTable(this DbOperator op, string sql, int pageIndex, int pageSize, out int rowCount, bool doCount)
        {
            using (DbDataReader reader = Paging(op, sql, pageIndex, pageSize, out rowCount, doCount))
            {
                DataTable result = LoadDataTableWithoutType(reader);
                return result;
            }
        }

        public static DataTable QueryTable(this DbOperator op, string sql, PageInfo pageInfo)
        {
            if (pageInfo == null)
                return op.ExecuteTableWithoutType(sql);
            int rowCount;
            var result = op.PagingTable(sql, pageInfo.PageIndex, pageInfo.PageSize, out rowCount);
            pageInfo.RowCount = rowCount;
            return result;
        }

        internal static DataTable LoadDataTableWithoutType(DbDataReader reader)
        {
            DataTable result = null;
            if (reader.HasRows)
            {
                result = ConstructDataTableSchema(reader);
                result.BeginLoadData();
                object[] row = new object[result.Columns.Count];
                while (reader.Read())
                {
                    reader.GetValues(row);
                    result.LoadDataRow(row, true);
                }
                result.EndLoadData();
            }
            return result;
        }
        private static DataTable ConstructDataTableSchema(DbDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            if (schema != null && schema.Rows.Count > 0)
            {
                DataTable result = new DataTable();
                foreach (DataRow item in schema.Rows)
                {
                    result.Columns.Add(item[0].ToString());
                }
                return result;
            }
            return null;
        }
        //private static string DealTotalCountSql(string sql)
        //{
        //    try
        //    {
        //        sql = sql.ToUpper();
        //        int index1 = sql.IndexOf("SELECT ");
        //        int index2 = sql.IndexOf(" FROM");

        //        if (index1 >= 0 && index2 >= 0)
        //        {
        //            return sql.Replace(sql.Substring(index1 + 6, index2 - index1 - 6), " 1 ");
        //        }

        //        return sql;
        //    }
        //    catch {
        //        return sql;
        //    }
        //}
    }
}
