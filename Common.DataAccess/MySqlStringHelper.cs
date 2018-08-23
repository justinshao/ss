using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataAccess.MySql
{
    public partial class MySqlStringHelper
    {
        /// <summary>
        /// 获取连接字符串生成器
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns></returns>
        public static MySqlConnectionStringBuilder GetSqlConnectionStringBuilder(string connectString)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.ConnectionString = connectString;
            return builder;
        }

        public static string BuilderInsertSql(string tableName, List<string> fieldNames)
        {
            if (fieldNames == null || fieldNames.Count == 0) return string.Empty;
            string fields = string.Empty;
            string values = string.Empty;
            foreach (string item in fieldNames)
            {
                fields += "," + item;
                values += ", @" + item.Trim();
            }
            fields = fields.Substring(1);
            values = values.Substring(1);
            string sqlText = "INSERT INTO " + tableName + "(" + fields + ") VALUES(" + values + ")";
            return sqlText;
        }

        /// <summary>
        /// 返回数据库连接字符串的SqlConnectionStringBuilder(判断字符格是否符合)
        /// </summary>
        /// <param name="server">连接到SQL SERVER服务器的实例名称或IP地址</param>
        /// <param name="dataBase">连接的数据库名称</param>
        /// <param name="userName">数据库登录用户名</param>
        /// <param name="password">数据库登录密码</param>
        /// <param name="port">端口</param>
        /// <returns>SqlConnectionStringBuilder</returns>
        public static MySqlConnectionStringBuilder GetConnectBuilder(string server, string dataBase, string userName, string password, uint port = 3306)
        {
            MySqlConnectionStringBuilder sqlBuilder = new MySqlConnectionStringBuilder();
            sqlBuilder.Server = server;
            sqlBuilder.Database = dataBase;
            sqlBuilder.UserID = userName;
            sqlBuilder.Password = password;
            sqlBuilder.Port = port;
            return sqlBuilder;
        }
        public static MySqlConnectionStringBuilder GetConnectBuilder(string connectString)
        {
            return new MySqlConnectionStringBuilder(connectString);
        }

    }
}
