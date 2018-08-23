using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 卡状态
    /// </summary>
    public enum CardStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 挂失
        /// </summary>
        [Description("挂失")]
        Lost = 2,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disable = 3,
        /// <summary>
        /// 注销
        /// </summary>
        [Description("注销")]
        LogOut = 4
    }
}
