using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum PaymentChannel
    {
        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WeiXinPay = 0,
        /// <summary>
        /// 支付宝支付
        /// </summary>
        [Description("支付宝支付")]
        AliPay = 1
    }
}
