using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum RequestSourceClient
    {
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeiXin = 0,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 1,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 255
    }
}
