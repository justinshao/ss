using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum KeywordType
    {
        /// <summary>
        /// 回复文字
        /// </summary>
        /// </summary>
        [Description("回复文字")]
        Text = 0,
        /// <summary>
        /// 回复图文
        /// </summary>
        /// </summary>
        [Description("回复图文")]
        Article = 1,
    }
}
