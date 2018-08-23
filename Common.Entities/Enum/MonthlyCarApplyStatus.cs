using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 月租车申请状态
    /// </summary>
    public enum MonthlyCarApplyStatus
    {
        /// <summary>
        /// 申请中
        /// </summary>
        [Description("申请中")]
        Applying = 0,
        /// <summary>
        /// 已通过
        /// </summary>
        [Description("已通过")]
        Passed = 1,
        /// <summary>
        /// 已拒绝
        /// </summary>
        [Description("已拒绝")]
        Refused = 2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancel = 3
    }
}
