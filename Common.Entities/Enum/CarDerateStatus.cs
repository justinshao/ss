using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 车辆优免状态
    /// </summary>
    public enum CarDerateStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used = 1,
        /// <summary>
        /// 已结算
        /// </summary>
        [Description("已结算")]
        Settlementd = 2,
        /// <summary>
        /// 已作废
        /// </summary>
        [Description("已作废")]
        Obsolete = 3
    }
}
