using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
namespace Common.DataAccess
{
    public class DbOperator : IDisposable {
        const int COMMAND_TIMEOUT = 30;
        private int m_transCount = 0;
        private bool disposed = false;
        private string databaseprovider = string.Empty;
        internal DbOperator(string provider, string connectionString, string databaseprovider)
        {
            if (string.IsNullOrEmpty(provider)) {
                throw new ArgumentNullException("provider");
            }
            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentNullException("connectionString");
            }
            DaoHelper helper = new DaoHelper(provider,databaseprovider);
            conn = helper.CreateConnection(connectionString);
            cmd = conn.CreateCommand();
            cmd.CommandTimeout = COMMAND_TIMEOUT;
        }
        public DbOperator(DbCommand command) {
            if (command == null) {
                throw new ArgumentNullException("command");
            }
            cmd = command;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            conn = cmd.Connection;
        }
       
        private DbConnection conn = null;
        private DbCommand cmd = null;

        private DbParameter CreateParameter(string name) {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            return param;
        }
        public DbParameter CreateParameter(string name, object value) {
            DbParameter param = CreateParameter(name); 
            if (value!=null&&value.GetType() == typeof(DateTime) && (DateTime)value == DateTime.MinValue)
            {
                value = DBNull.Value; 
            }
            param.Value = value;
            return param;
        }
        private DbParameter CreateParameter(string name, object value, DbType type) {
            DbParameter param = CreateParameter(name, value);
            param.DbType = type;
            return param;
        }
        public void Prepare() {
            if (cmd == null) { throw new CommandNullException(); }
            cmd.CommandText = "";
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
        }

        public void ClearParameters() {
            cmd.Parameters.Clear();
        }
        public void Cancel() {
            cmd.Cancel();
        }
        /// <summary>
        /// 开启数据库事务
        /// </summary>
        public void BeginTransaction() {
            if (cmd == null) {
                throw new CommandNullException();
            }
            if (cmd.Connection == null) {
                throw new ConnectionNullException();
            }
            if (cmd.Transaction == null) {
                cmd.Transaction = cmd.Connection.BeginTransaction();
            }
            m_transCount++;
        }
        /// <summary>
        /// 提交数据库事务
        /// </summary>
        public void CommitTransaction() {
            if (cmd.Transaction != null) {
                m_transCount--;
                if (m_transCount > 0) {
                    return;
                }
                cmd.Transaction.Commit();
            }
        }
        /// <summary>
        /// 回滚数据库事务
        /// </summary>
        public void RollbackTransaction() {
            if (cmd.Transaction != null) {
                cmd.Transaction.Rollback();
                m_transCount = 0;
            }
        }

        #region AddParameter
        public void AddParameter(DbParameter param) {
            cmd.Parameters.Add(param);
        }

        public DbParameter AddParameter(string name) {
            DbParameter param = CreateParameter(name);
            cmd.Parameters.Add(param);
            return param;
        }
        public DbParameter AddParameter(string name, object value) {
            DbParameter param = CreateParameter(name, value);
            cmd.Parameters.Add(param);
            return param;
        }
        public DbParameter AddParameter(string name, object value, DbType type) {
            DbParameter param = CreateParameter(name, value, type);
            cmd.Parameters.Add(param);
            return param;
        }

        public DbParameter AddParameter(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            DbParameter param = CreateParameter(name, value, type);
            param.Direction = direction;
            param.Size = size;
            cmd.Parameters.Add(param);
            return param;
        }
        #endregion

        #region ExecuteReader
        public DbDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior, int timeout) {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.CommandTimeout = timeout;
            CheckParam();
            return cmd.ExecuteReader(behavior);
        }
        public DbDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior) {
            return ExecuteReader(sql, type, behavior, COMMAND_TIMEOUT);
        }
        public DbDataReader ExecuteReader(string sql, CommandType type, int timeout) {
            return ExecuteReader(sql, type, CommandBehavior.Default, timeout);
        }
        public DbDataReader ExecuteReader(string sql, CommandType type) {
            return ExecuteReader(sql, type, CommandBehavior.Default, COMMAND_TIMEOUT);
        }
        public DbDataReader ExecuteReader(string sql, CommandBehavior behavior, int timeout) {
            return ExecuteReader(sql, CommandType.Text, behavior, timeout);
        }
        public DbDataReader ExecuteReader(string sql, CommandBehavior behavior) {
            return ExecuteReader(sql, CommandType.Text, behavior, COMMAND_TIMEOUT);
        }
        public DbDataReader ExecuteReader(string sql, int timeout) {
            return ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }
        public DbDataReader ExecuteReader(string sql) {
            return ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, COMMAND_TIMEOUT);
        }
        #endregion

        #region ExecuteTable
        public DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior, int timeout) {
            using (DbDataReader dr = ExecuteReader(sql, type, behavior, timeout)) {
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }
        public DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior) {
            return ExecuteTable(sql, type, behavior, COMMAND_TIMEOUT);
        }
        public DataTable ExecuteTable(string sql, CommandType type, int timeout) {
            return ExecuteTable(sql, type, CommandBehavior.Default, timeout);
        }
        public DataTable ExecuteTable(string sql, CommandType type) {
            return ExecuteTable(sql, type, CommandBehavior.Default, COMMAND_TIMEOUT);
        }
        public DataTable ExecuteTable(string sql, CommandBehavior behavior, int timeout) {
            return ExecuteTable(sql, CommandType.Text, behavior, timeout);
        }
        public DataTable ExecuteTable(string sql, CommandBehavior behavior) {
            return ExecuteTable(sql, CommandType.Text, behavior, COMMAND_TIMEOUT);
        }
        public DataTable ExecuteTable(string sql, int timeout) {
            return ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }
        public DataTable ExecuteTable(string sql) {
            return ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, COMMAND_TIMEOUT);
        }

        public DataTable ExecuteTableWithoutType(string sql, CommandType type, CommandBehavior behavior, int timeout)
        {
            using (DbDataReader dr = ExecuteReader(sql, type, behavior, timeout))
            {
                return SqlWrapper.LoadDataTableWithoutType(dr);
            }
        }

        public DataTable ExecuteTableWithoutType(string sql)
        {
            return ExecuteTableWithoutType(sql, CommandType.Text, CommandBehavior.Default, COMMAND_TIMEOUT);
        }
        #endregion

        public DataSet ExecuteDataSet(string sql,params string[] tableName) {
            using (DbDataReader dr = ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, COMMAND_TIMEOUT))
            {
                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.Upsert,tableName);
                return ds;
            }
        }

        #region ExecuteScalar
        public object ExecuteScalar(string sql, CommandType type, int timeout) {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.CommandTimeout = timeout;
            CheckParam();
            return cmd.ExecuteScalar();
        }
        public object ExecuteScalar(string sql, CommandType type) {
            return ExecuteScalar(sql, type, COMMAND_TIMEOUT);
        }
        public object ExecuteScalar(string sql, int timeout) {
            return ExecuteScalar(sql, CommandType.Text, timeout);
        }
        public object ExecuteScalar(string sql) {
            return ExecuteScalar(sql, CommandType.Text, COMMAND_TIMEOUT);
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(string sql, CommandType type, int timeout) {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException("sql"); }
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.CommandTimeout = timeout;
            CheckParam();
            return cmd.ExecuteNonQuery();
        }

        private void CheckParam()
        {
            foreach (DbParameter param in cmd.Parameters)
            {
                if (param.Value == null)
                    param.Value = DBNull.Value;
            }
        }
        public int ExecuteNonQuery(string sql, CommandType type) {
            return ExecuteNonQuery(sql, type, COMMAND_TIMEOUT);
        }
        public int ExecuteNonQuery(string sql, int timeout) {
            return ExecuteNonQuery(sql, CommandType.Text, timeout);
        }
        public int ExecuteNonQuery(string sql) {
            return ExecuteNonQuery(sql, CommandType.Text, COMMAND_TIMEOUT);
        }
        #endregion

        #region IDisposable 成员
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        protected void Dispose(bool disposing)
        {
            if (disposing) { }
            if (!disposed)
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                disposed = true;
            }
        }
    }

}