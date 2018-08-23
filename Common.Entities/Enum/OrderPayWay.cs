using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum OrderPayWay
    {
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash = 1,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeiXin = 2,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 3,
        /// <summary>
        /// 网银
        /// </summary>
        [Description("网银")]
        OnlineBanking = 4,
        /// <summary>
        /// 电子钱包
        /// </summary>
        [Description("电子钱包")]
        Wallet = 5,
        /// <summary>
        /// 优免卷
        /// </summary>
        [Description("优免卷")]
        PreferentialTicket = 6,
        /// <summary>
        /// 储值卡
        /// </summary>
        [Description("储值卡")]
        ValueCard = 7,
        /// <summary>
        /// PP停车
        /// </summary>
        [Description("PP停车")]
        PPPark = 8,
        /// <summary>
        /// 宝宝停车
        /// </summary>
        [Description("BB停车")]
        BBPark = 9,
        /// <summary>
        /// 银联无感支付
        /// </summary>
        [Description("银联无感")]
        UnionPay = 10
    }
}
