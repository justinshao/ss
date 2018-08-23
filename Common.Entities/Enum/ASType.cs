using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum ASType
    {
        /// <summary>
        /// 小区
        /// </summary>
        [Description("小区")]
        Village = 1,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 2
    }
}
