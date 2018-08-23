using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Enum
{
    /// <summary>
    /// 报表类型
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// 在场车辆
        /// </summary>
        Presence = 1,
        /// <summary>
        /// 无牌车辆
        /// </summary>
        NoPlateNumber=2,
        /// <summary>
        /// 进出记录
        /// </summary>
        InOut = 3,
        /// <summary>
        /// 异常放行
        /// </summary>
        ExceptionRelease =4,
        /// <summary>
        /// 通道事件
        /// </summary>
        GateEvent = 5,
        /// <summary>
        /// 当班统计
        /// </summary>
        OnDuty=6,
        /// <summary>
        /// 订单明细
        /// </summary>
        Order =7,
        /// <summary>
        /// 月卡续期
        /// </summary>
        CardExtension =8,
        /// <summary>
        /// 临停缴费
        /// </summary>
        TempPay =9,
        /// <summary>
        /// 商家优免
        /// </summary>
        Coupon =10,
        /// <summary>
        /// 日报表
        /// </summary>
        DailyFee = 11,
        /// <summary>
        /// 月报表
        /// </summary>
        MonthFee = 12,
        /// <summary>
        /// 日汇总报表
        /// </summary>
        DailyGather = 13,
        /// <summary>
        /// 月汇总报表
        /// </summary>
        MonthGather=14,

        /// <summary>
        /// 储值卡扣费
        /// </summary>
        RechargePay=15,

        /// <summary>
        /// 商家充值
        /// </summary>
        SellerRecharge=16,
        /// <summary>
        /// 车牌前缀
        /// </summary>
        PlateNumberPrefix = 17,
        /// <summary>
        /// 在线支付
        /// </summary>
        OnLinePay = 18,
        /// <summary>
        /// 月卡信息
        /// </summary>
        MonthCardInfo = 19,
        /// <summary>
        /// 访客信息
        /// </summary>
        ParkVisitorInfo=20
    }
}
