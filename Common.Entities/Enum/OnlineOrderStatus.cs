using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OnlineOrderStatus
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        [Description("等待支付")]
        WaitPay = 0,
        /// <summary>
        /// 支付中
        /// </summary>
        [Description("支付中")]
        Paying = 1,
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        PaySuccess = 2,
        /// <summary>
        /// 同步支付结果成功
        /// </summary>
        [Description("同步支付结果成功")]
        SyncPayResultSuccess = 3,
        /// <summary>
        /// 同步支付结果失败
        /// </summary>
        [Description("同步支付结果失败")]
        SyncPayResultFail = 4,
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        RefundSuccess = 5,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        RefundFail = 6,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancel = 7,
    }
}
