using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum ArticleType
    {
        /// <summary>
        /// 文字
        /// </summary>
        [Description("文字")]
        Text = 0,
        /// <summary>
        /// 链接
        /// </summary>
        [Description("链接")]
        Url = 1,
        /// <summary>
        /// 功能模块
        /// </summary>
        [Description("功能模块")]
        Module = 2,
    }
}
