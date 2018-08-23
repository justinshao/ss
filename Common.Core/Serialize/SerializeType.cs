using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// 自定义序列化类型，供自定义序列化用
    /// </summary>
    public enum SerializeType
    {
        /// <summary>
        /// 基元类型
        /// </summary>
        Basic = 0,
        /// <summary>
        /// 对象
        /// </summary>
        Object = 1,
        /// <summary>
        /// 数组
        /// </summary>
        List = 2,
    }
}
