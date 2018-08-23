using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum MatchType
    {
        /// <summary>
        /// 完全匹配
        /// </summary>
        [Description("完全匹配")]
        Equal = 0,
        /// <summary>
        /// 模糊匹配
        /// </summary>
        [Description("模糊匹配")]
        Contains = 1
    }
}
