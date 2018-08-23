using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 计费类型
    /// </summary>
    public enum FeeRuleType
    {
        /// <summary>
        /// 按时间计费
        /// </summary>
        [Description("按时间计费")]
        TimeFee = 0,
        /// <summary>
        /// 按次计费
        /// </summary>
        [Description("按次计费")]
        NumberOfTimes = 1
    }
}
