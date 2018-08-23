using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum ReplyType
    {
        /// <summary>
        /// 关注回复
        /// </summary>
        [Description("关注回复")]
        Subscribe = 0,
        /// <summary>
        /// 自动回复
        /// </summary>
        [Description("自动回复")]
        AutoReplay = 1,
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 2,
        /// <summary>
        /// 浏览
        /// </summary>
        [Description("浏览")]
        Scan = 3,
        /// <summary>
        /// 位置
        /// </summary>
        [Description("位置")]
        Location = 4,
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认回复")]
        Default = 10
    }
}
