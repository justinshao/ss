using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 卡所属系统(按位与运算，如 1,2,4,8)
    /// </summary>
    public enum CardSystem
    {
        /// <summary>
        /// 车场
        /// </summary>
        [Description("车场")]
        Park = 1,
        /// <summary>
        /// 门禁
        /// </summary>
        [Description("门禁")]
        Door = 2
    }
}
