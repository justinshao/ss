using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum MenuType
    {
        /// <summary>
        /// 跳转链接
        /// </summary>
        [Description("跳转链接")]
        Url = 0,
        /// <summary>
        /// 匹配关键字
        /// </summary>
        [Description("匹配关键字")]
        GKeyValue = 1,
        /// <summary>
        /// 微信模块
        /// </summary>
        [Description("微信模块")]
        WeiXinModule = 2,
        /// <summary>
        /// 小程序
        /// </summary>
        [Description("小程序")]
        MinIprogram = 3
    }
}
