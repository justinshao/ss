using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 车位占用
    /// </summary>
    public enum LotOccupy
    {
        /// <summary>
        /// 转临停
        /// </summary>
        [Description("转临停")]
        ChangeToTemp = 0,
        /// <summary>
        /// 禁止进出
        /// </summary>
        [Description("禁止进出")]
        ProhibitedInAndOut = 1,
    }
}
