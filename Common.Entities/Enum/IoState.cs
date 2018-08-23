using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 进出类型
    /// </summary>
    public enum IoState
    {
        /// <summary>
        /// 进
        /// </summary>
        [Description("进")]
        GoIn = 1,
        /// <summary>
        /// 出
        /// </summary>
        [Description("出")]
        GoOut = 2,
    }
}
