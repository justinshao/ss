using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum ParkGrantState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 停止
        /// </summary>
        [Description("停止")]
        Stop = 1,
        /// <summary>
        /// 暂停
        /// </summary>
        [Description("暂停")]
        Pause = 2
    }
}
