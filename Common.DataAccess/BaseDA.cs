using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using System.Configuration;
using Common.Entities;

namespace Common.DataAccess
{
    public class BaseDA : IDisposable {
        #region field
        private static string connStr = string.Empty;
        private static string provider = string.Empty;
        #endregion

        #region property
        protected DbOperator dbOperator { get; set; }
        #endregion

        #region constructor
        static BaseDA() {
            try {
                //初始化数据库连接
                connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                provider = ConfigurationManager.ConnectionStrings["ConnectionString"].ProviderName;
            } catch { throw new Exception("访问客户端应用程序配置文件数据库连接字符串失败！"); }
        }

        protected BaseDA()
        {
            dbOperator = CreateDbOperator();
        }

        protected static DbOperator CreateDbOperator()
        {
            return new DbOperator(provider, connStr,SystemDefaultConfig.DatabaseProvider);
        }
        #endregion

        #region protected method
        protected object GetValueForNullableType(object v) {
            if (v == null) {
                return DBNull.Value;
            } else {
                return v;
            }
        }
        protected object GetValueForDateTime(DateTime v) {
            if (v == DateTime.MinValue) {
                return DBNull.Value;
            } else {
                return v;
            }
        }
        protected string To_OralceDateTimeSqlFormat(string parameter) {
            return "to_date(:" + parameter + ", 'YYYY-MM-DD HH24:MI:SS')";
            //return "to_date(:" + parameter + ", 'YYYY-MM-DD')";
        }
        protected string BuildStrWhere(IList<Decimal> Id) {
            string strWhere = string.Empty;
            foreach (Decimal id in Id) {
                strWhere += id.ToString() + ",";
            }
            strWhere = strWhere.Remove(strWhere.Length - 1);
            return strWhere;
        }
        #endregion

        #region public method
        public void Dispose() {
            if (dbOperator != null)
                dbOperator.Dispose();
        }
        #endregion
    }
}
