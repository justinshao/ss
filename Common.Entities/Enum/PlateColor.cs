using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum PlateColor
    {
        /// <summary>
        /// 蓝色
        /// </summary>
        [Description("蓝色")]
        Blue = 0,
        /// <summary>
        /// 黄色
        /// </summary>
        [Description("黄色")]
        Yellow = 1,
        /// <summary>
        /// 黑色
        /// </summary>
        [Description("黑色")]
        Black = 2,
        /// <summary>
        /// 白色
        /// </summary>
        [Description("白色")]
        White = 3,

        /// <summary>
        /// 绿色
        /// </summary>
        [Description("绿色")]
        Green = 4
    }
}
