namespace Common.DataAccess
{
    using Common.DataAccess;
    using System.Configuration;
    using System.Data.Common;
    using System;
    using Common.Entities;

    /// <summary>
    /// 默认连接管理类
    /// </summary>
    public class ConnectionManager {
         

        /// <summary>
        /// 写入连接
        /// </summary>
        /// <returns></returns>
        public static DbOperator CreateConnection() {
            return new DbOperator("System.Data.SqlClient", SystemDefaultConfig.WriteConnectionString, SystemDefaultConfig.DatabaseProvider);
        }

        /// <summary>
        /// 只读连接
        /// </summary>
        /// <returns></returns>
        public static DbOperator CreateReadConnection() {
            return new DbOperator("System.Data.SqlClient", SystemDefaultConfig.ReadConnectionString, SystemDefaultConfig.DatabaseProvider);
        }

        /// <summary>
        /// 提供事务执行
        /// </summary>
        /// <param name="action">数据库操作</param>
        public static void ExcuteWithTransaction(Action<DbOperator> action)
        {
            using (DbOperator dbOperator = CreateConnection())
            {
                dbOperator.BeginTransaction();
                try
                {
                    action(dbOperator);
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
