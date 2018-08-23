using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
namespace Common.DataAccess
{
	/// <summary>
	/// 数据访问 Helper 类。
	/// </summary>
	public sealed class DaoHelper {
		private static DataTable _providers = null;
		private DbProviderFactory _pf = null;

		/// <summary>
		/// 静态构造函数，初始化数据提供程序列表。
		/// </summary>
		static DaoHelper() {
			_providers = DbProviderFactories.GetFactoryClasses();
		}
		/// <summary>
		/// 初始化 DAOHelper 类的新实例。
		/// </summary>
		public DaoHelper() {
		}
		/// <summary>
		/// 用指定的数据提供程序名称，初始化 DAOHelper 类的新实例。
		/// </summary>
		/// <param name="providerName">数据提供程序名称。</param>
        public DaoHelper(string providerName, string databaseprovider)
        {
			this.ProviderName = providerName;
            this.m_databaseprovider = databaseprovider;
		}

		private string m_providerName = "";
        private string m_databaseprovider = "";
		/// <summary>
		/// 获取或设置数据提供程序名称。
		/// </summary>
		public string ProviderName {
			get { return m_providerName; }
			set {
				if(string.IsNullOrEmpty(value)){
					throw new ArgumentNullException("value");
				}

				if(m_providerName != value) {
					ChangeProvider(value);
				}
			}
		}

		/// <summary>
		/// 查找指定数名称的据提供程序行。
		/// </summary>
		/// <param name="providerInvariantName">数据提供程序名称。</param>
		/// <returns></returns>
		private DataRow FindProviderRow(string providerInvariantName) {
			if(_providers != null) {
				return _providers.Rows.Find(providerInvariantName);
			}
			throw new ProvidersNotFoundException();
		}

		/// <summary>
		/// 根据参数提供的数据提供程序名称，更改当前的数据提供程序工厂。
		/// </summary>
		/// <param name="providerName">数据提供程序名称。</param>
		private void ChangeProvider(string providerName) {
			if(string.IsNullOrEmpty(providerName)) {
				throw new ArgumentNullException("value");
			}

			DataRow providerRow = FindProviderRow(providerName);
			if(providerRow == null){
				throw new ProviderNotFoundException(providerName);				
			}
			_pf = DbProviderFactories.GetFactory(providerRow);
			m_providerName = providerName;
			
		}

		/// <summary>
		/// 创建数据源枚举器。
		/// </summary>
		/// <returns>返回创建成功的数据源枚举器。</returns>
		public DbDataSourceEnumerator CreateDataSourceEnumerator() {
			if(_pf == null) {
				throw new ProviderFactoryNullException();
			}
			return _pf.CreateDataSourceEnumerator();
		}

		/// <summary>
		/// 创建数据库连接。
		/// </summary>
		/// <returns>返回成功创建的连接对象。</returns>
		public DbConnection CreateConnection() {
			if(_pf == null) {
				throw new ProviderFactoryNullException();
			}
            if (!string.IsNullOrWhiteSpace(this.m_databaseprovider) && this.m_databaseprovider.ToLower() == "mysql") {
                return new MySqlConnection();
            }
			return _pf.CreateConnection();
		}

		/// <summary>
		/// 创建数据库连接，并用指定的数据库连接字符串打开。
		/// </summary>
		/// <param name="connectionString">数据库连接字符串。</param>
		/// <returns>返回成功创建并打开的连接对象。</returns>
		public DbConnection CreateConnection(string connectionString){
			DbConnection result = CreateConnection();
			result.ConnectionString = connectionString;
			result.Open();
			return result;
		}

		/// <summary>
		/// 创建数据库连接字符串生成器。
		/// </summary>
		/// <returns>返回成功创建的连接对象。</returns>
		public DbConnectionStringBuilder CreateConnectionStringBuilder() {
			if(_pf == null) {
				throw new ProviderFactoryNullException();
			}
			return _pf.CreateConnectionStringBuilder();
		}

		/// <summary>
		/// 创建数据适配器。
		/// </summary>
		/// <returns></returns>
		public DataAdapter CreateAdapter() {
			if(_pf == null) {
				throw new ProviderFactoryNullException();
			}
			return _pf.CreateDataAdapter();
		}
	}
}
