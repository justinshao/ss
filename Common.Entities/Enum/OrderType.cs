using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum OrderType
    {
        /// <summary>
        /// 临时卡缴费
        /// </summary>
        [Description("临时卡缴费")]
        TempCardPayment = 1,
        /// <summary>
        /// 月卡缴费
        /// </summary>
        [Description("月卡缴费")]
        MonthCardPayment = 2,
        /// <summary>
        /// VIP卡续期
        /// </summary>
        [Description("VIP卡续期")]
        VIPCardRenewal = 3,
        /// <summary>
        /// 储值卡充值
        /// </summary>
        [Description("储值卡充值")]
        ValueCardRecharge = 4,
        /// <summary>
        /// 临时卡续期
        /// </summary>
        [Description("临时卡续期")]
        TempCardRenewal = 5,
        /// <summary>
        /// 储值卡缴费
        /// </summary>
        [Description("储值卡缴费")]
        ValueCardPayment = 6,
        /// <summary>
        /// 子区域临停缴费
        /// </summary>
        [Description("子区域临停缴费")]
        AreaTempCardPayment = 7,
        /// <summary>
        /// 子区域储值卡缴费
        /// </summary>
        [Description("子区域储值卡缴费")]
        AreaValueCardPayment = 8,
        /// <summary>
        /// 卡片发卡押金
        /// </summary>
        [Description("卡片发卡押金")]
        CardDeposit = 9,
        /// <summary>
        /// 商家充值
        /// </summary>
        [Description("商家充值")]
        BusinessRecharge = 10,
        /// <summary>
        /// 出场商家优免
        /// </summary>
        [Description("出场商家优免")]
        BusinessPreferential = 11,

        /// <summary>
        /// 车位预定
        /// </summary>
        [Description("车位预定")]
        ReserveBit = 12,
    }
}
