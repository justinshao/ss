using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 过期转临停
    /// </summary>
    public enum OverdueToTemp
    {
        /// <summary>
        /// 过期转临停
        /// </summary>
        [Description("过期转临停")]
        ExpiredToTemp = 0,
        /// <summary>
        /// 禁止进出
        /// </summary>
        [Description("禁止进出")]
        ProhibitedInAndOut = 1,
    }
}
