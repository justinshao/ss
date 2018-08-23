using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum DataStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 2,
    }
}
