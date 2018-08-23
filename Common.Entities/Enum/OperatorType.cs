using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum OperateType
    {
        /// <summary>
        /// 添加
        /// </summary>
        [Description("添加")]
        Add = 0,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update = 1,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 3
    }
}
