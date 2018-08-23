using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataAccess
{
	/// <summary>
	/// 未找到指定的数据提供程序时引发的异常。
	/// </summary>
	[Serializable]
	public sealed class ProviderNotFoundException : Exception {
        /// <summary>
        /// 初始化 ProviderNotFoundException 类型的新实例
        /// </summary>
		public ProviderNotFoundException() : base("未找到指定的数据提供程序。") { }

        /// <summary>
        /// 根据提供的数据提供程序名称，初始化 ProviderNotFoundException 类型的新实例
        /// </summary>
        /// <param name="providerName"></param>
		public ProviderNotFoundException(string providerName) : base(string.Format("未找到名为 \"{0}\" 的数据提供程序。", providerName)) { }
	}

	/// <summary>
	/// 未找到任何数据提供程序时引发的异常。
	/// </summary>
	[Serializable]
	public sealed class ProvidersNotFoundException : Exception {
        /// <summary>
        /// 初始化 ProvidersNotFoundException 类型的新实例
        /// </summary>
		public ProvidersNotFoundException() : base("未找到任何数据提供程序。") { }
	}

	/// <summary>
	/// 数据提供程序工厂对象为空时引发的异常。
	/// </summary>
	[Serializable]
	public sealed class ProviderFactoryNullException : Exception {
        /// <summary>
        /// 初始化 ProviderFactoryNullException 类型的新实例
        /// </summary>
		public ProviderFactoryNullException() : base("数据提供程序工厂为空引用。可能是 \"DAOHelper.ProviderName\" 未设置。") { }
	}
	/// <summary>
	/// 命令对象为空时引发的异常。
	/// </summary>
	[Serializable]
	public sealed class CommandNullException : Exception {
        /// <summary>
        /// 初始化 CommandNullException 类型的新实例
        /// </summary>
		public CommandNullException() : base("命令对象为空引用。") { }
	}

    /// <summary>
    /// 数据库连接对象为空时引发的异常。
    /// </summary>
    public sealed class ConnectionNullException : Exception {
        /// <summary>
        /// 初始化 ConnectionNullException 类型的新实例
        /// </summary>
        public ConnectionNullException() : base("没有可用的数据库连接。") { }
    }

    /// <summary>
    /// 在当前事务尚未结束时（Commit 或 Rollback）,再次开启事务引发的异常
    /// </summary>
    public sealed class TransactionNotTerminateException : Exception {
        /// <summary>
        /// 初始化 TransactionNotTerminateException 类型的新实例
        /// </summary>
        public TransactionNotTerminateException() : base("之前的事务尚未结束，不能开启新的事务。") { }
    }

    /// <summary>
    /// 在当前事务已结束（Commit 或 Rollback）时，再次提交或回滚事务引发的异常
    /// </summary>
    public sealed class TransactionTerminatedException : Exception {
        /// <summary>
        /// 初始化 TransactionTerminatedException 类型的新实例
        /// </summary>
        public TransactionTerminatedException() : base("当前事务已结束，或是事务尚未开启。") { }
    }
}
