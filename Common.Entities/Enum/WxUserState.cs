using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum WxUserState
    {
         /// <summary>
        /// 取消关注(未关注)
        /// </summary>
        [Description("取消关注")]
        UnAttention = 0,
        /// <summary>
        /// 关注
        /// </summary>
        [Description("关注")]
        Attention = 1
    }
}
