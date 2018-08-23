using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum LogFrom
    {
        /// <summary>
        /// 一卡通
        /// </summary>
        [Description("一卡通")]
        OmnipotentCard = 0,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeiXin = 1,
        /// <summary>
        /// 车场客户端
        /// </summary>
        [Description("车场客户端")]
        ParkClient = 2,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UnKnown = 3,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 4
    }
}
