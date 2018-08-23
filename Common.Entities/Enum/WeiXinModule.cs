using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Common.Core.Attributes;

namespace Common.Entities.Enum
{
    public enum WeiXinModule
    {
        /// <summary>
        /// 车辆管理
        /// </summary>
        /// </summary>
        [Description("车辆管理")]
        [EnumDefaultValue("/l/CarManage_Index_moduleid=0")]
        CarManage = 0,
        /// <summary>
        /// 锁车解锁
        /// </summary>
        /// </summary>
        [Description("锁车解锁")]
        [EnumDefaultValue("/l/LockCar_Index_moduleid=1")]
        LockCar = 1,
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// </summary>
        [Description("临停缴费")]
        [EnumDefaultValue("/l/ParkingPayment_Index_moduleid=2")]
        ParkingPayment = 2,
        /// <summary>
        /// 月卡续期
        /// </summary>
        /// </summary>
        [Description("月卡续期")]
        [EnumDefaultValue("/l/CardRenewal_Index_moduleid=3")]
        CardRenewal = 3,
        /// <summary>
        /// 缴费记录
        /// </summary>
        /// </summary>
        [Description("缴费记录")]
        [EnumDefaultValue("/l/PaymentRecord_Index_moduleid=4")]
        PaymentRecord = 4,
        /// <summary>
        /// 常见问题
        /// </summary>
        /// </summary>
        [Description("常见问题")]
        [EnumDefaultValue("/l/OtherService_OperatExplanation_moduleid=5")]
        OperatExplanation = 5,
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// </summary>
        [Description("意见反馈")]
        [EnumDefaultValue("/l/OtherService_ProblemFeedback_moduleid=6")]
        OpinionFeedback = 6,
        /// <summary>
        /// 找停车场
        /// </summary>
        /// </summary>
        [Description("找停车场")]
        [EnumDefaultValue("/l/FindParking_Index_moduleid=7")]
        FindParking = 7,
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// </summary>
        [Description("管理员登录")]
        [EnumDefaultValue("/l/AdminLogin_Index_moduleid=8")]
        WeiXinAdminLogin = 8,
        /// <summary>
        /// 车位预约
        /// </summary>
        /// </summary>
        [Description("车位预约")]
        [EnumDefaultValue("/l/ParkBitBooking_Index_moduleid=9")]
        BookingPkBit = 9,
        /// <summary>
        /// 商家减免
        /// </summary>
        /// </summary>
        [Description("商家减免")]
        [EnumDefaultValue("/l/XFJMLogin_Index_moduleid=10")]
        SellerDerate = 10,
        /// <summary>
        /// 访客管理
        /// </summary>
        /// </summary>
        [Description("访客管理")]
        [EnumDefaultValue("/l/CarVisitor_Index_moduleid=11")]
        CarVisitor = 11,
        /// <summary>
        /// 月租车申请
        /// </summary>
        /// </summary>
        [Description("月租车申请")]
        [EnumDefaultValue("/l/MonthlyCarApply_Index_moduleid=12")]
        MonthlyCarApply = 12,
    }
}
