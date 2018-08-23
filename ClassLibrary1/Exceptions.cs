using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary1
{
    [Serializable]
    public sealed class ProviderFactoryNullException : Exception
    {
        /// <summary>
        /// 初始化 ProviderFactoryNullException 类型的新实例
        /// </summary>
		public ProviderFactoryNullException() : base("数据提供程序工厂为空引用。可能是 \"DAOHelper.ProviderName\" 未设置。") { }
    }
}