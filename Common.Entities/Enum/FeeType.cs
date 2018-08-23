using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 收费模式 (1.24小时,2.12小时3.白天黑夜4.自然天)
    /// </summary>
    public enum FeeType
    {
        /// <summary>
        /// 24小时
        /// </summary>
        [Description("24小时")]
        Hour24 = 1,
        /// <summary>
        /// 12小时
        /// </summary>
        [Description("12小时")]
        Hour12 = 2,
        /// <summary>
        /// 白天/夜间
        /// </summary>
        [Description("白天/夜间")]
        DayAndNight = 3,
        /// <summary>
        /// 自然天
        /// </summary>
        [Description("自然天")]
        NaturalDay = 4,

        /// <summary>
        /// 自定义
        /// </summary>
        [Description("自定义")]
        Custom=5
    }
}
