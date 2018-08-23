using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 优免时间切分方式
    /// </summary>
    public enum DerateSwparate
    {
        /// <summary>
        /// 前面时间
        /// </summary>
        [Description("前面时间")]
        BeforeTime = 0,
        /// <summary>
        /// 后面时间
        /// </summary>
        [Description("后面时间")]
        AfterTime = 1,
    }
}
