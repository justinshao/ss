using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 循环包含时段(1都不含,2只含首时段,3都含)
    /// </summary>
    public enum LoopType
    {
        /// <summary>
        /// 不含免费时段与首时段
        /// </summary>
        [Description("不含免费时段与首时段")]
        AllNotContain = 1,
        /// <summary>
        /// 含首时段不含免费时段
        /// </summary>
        [Description("含首时段不含免费时段")]
        OnlyFirstTime = 2,
        /// <summary>
        /// 含免费时段与首时段
        /// </summary>
        [Description("含免费时段与首时段")]
        AllContain = 3,
    }
}
