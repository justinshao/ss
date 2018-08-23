using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum OrderSource
    {
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeiXin = 1,
        /// <summary>
        /// APP
        /// </summary>
        [Description("APP")]
        APP = 2,
        /// <summary>
        /// 中心收费
        /// </summary>
        [Description("中心收费")]
        CenterCharge = 3,
        /// <summary>
        /// 岗亭
        /// </summary>
        [Description("岗亭")]
        BoxCharge = 4,
        /// <summary>
        /// 管理处
        /// </summary>
        [Description("管理处")]
        ManageOffice = 5,

        /// <summary>
        /// 第三方
        /// </summary>
        [Description("第三方平台")]
        Other = 6,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 7,
    }
}
