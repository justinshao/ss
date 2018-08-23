using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum BlackListStatus
    {
        /// <summary>
        /// 不可进不可出
        /// </summary>
        [Description("不可进不可出")]
        NotEnterAndOut = 1,
        /// <summary>
        /// 可进不可出
        /// </summary>
        [Description("可进不可出")]
        CanEnterAndNotOut = 2,
        /// <summary>
        /// 可进可出
        /// </summary>
        [Description("可进可出")]
        CanEnterAndOut = 3,
    }
}
