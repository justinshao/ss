using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum OnlineOrderType
    {
        /// <summary>
        /// 停车缴费
        /// </summary>
        [Description("停车缴费")]
        ParkFee = 0,
        /// <summary>
        /// 月卡充值
        /// </summary>
        [Description("月卡充值")]
        MonthCardRecharge = 1,
        /// <summary>
        /// 车位预订
        /// </summary>
        [Description("车位预订")]
        PkBitBooking = 2,
        /// <summary>
        /// 商家充值
        /// </summary>
        [Description("商家充值")]
        SellerRecharge = 3,
        /// <summary>
        /// 商家充值
        /// </summary>
        [Description("APP")]
        APPRecharge = 4,
    }
}
