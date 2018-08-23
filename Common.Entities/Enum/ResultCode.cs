using System.ComponentModel;
namespace Common.Entities
{
    public enum ResultCode
    { 
        [Bindable(false)]
        [Description("默认")]
        Defaut = 0,

        [Bindable(true)]
        [Description("车位已满")]
        NoCarBit = 1,

        [Bindable(true)]
        [Description("异常信息")]
        ErrorMSG = 2,

        [Bindable(true)]
        [Description("重复入场")]
        RepeatIn = 3,

        [Bindable(true)]
        [Description("已出场或无入场记录")]
        RepeatOut = 4,

        [Bindable(false)]
        [Description("卡和车牌不一致")]
        CarNoEqLp = 5,

        /// <summary>
        /// 进出时间小于时间间隔
        /// </summary>
        [Bindable(true)]
        [Description("进出时间小于时间间隔")]
        InOutTime = 6,

        /// <summary>
        /// 过期用户
        /// </summary>
        [Bindable(true)]
        [Description("过期用户")]
        UserExpired = 7,

        /// <summary>
        /// 正确进场
        /// </summary>
        [Bindable(false)]
        [Description("进场")]
        InOK = 8,

        /// <summary>
        /// 无权限进出通道 临停车不可通行 或者黑名单
        /// </summary>
        [Bindable(true)] 
        [Description("无权限进出通道")]
        NoPermissionsInOut = 9,

        /// <summary>
        /// 正确
        /// </summary>
        [Bindable(false)] 
        [Description("出场")]
        OutOK = 10,

        /// <summary>
        /// 找不到入场记录
        /// </summary>
        [Bindable(true)]
        [Description("找不到入场记录")]
        OnFindNo = 11,

        /// <summary>
        /// 系统错误
        /// </summary>
        [Bindable(true)]
        [Description("系统错误")]
        LocalError = 12,

        /// <summary>
        /// 异常放行
        /// </summary>
        [Bindable(true)]
        [Description("异常放行")]
        AbNormal = 13,

        /// <summary>
        /// 车辆锁定
        /// </summary>
        [Bindable(true)]
        [Description("车辆已锁定")]
        CarLocked = 14,

        /// <summary>
        /// 固定转临停
        /// </summary>
        [Bindable(true)]
        [Description("月卡车转临停车")]
        OverdueToTemp = 15,

        /// <summary>
        /// 费率未设置
        /// </summary>
        [Bindable(true)]
        [Description("该区域下车型费率未配置")]
        RateNoSet = 16,
        /// <summary>
        /// 未确认费用 重新确认
        /// </summary>
        [Bindable(false)] 
        [Description("未确认费用 重新确认")]
        ConfirmChargeFee = 17,
        /// <summary>
        /// 未确认费用 确认不收费
        /// </summary> 
        [Bindable(false)] 
        [Description("未确认费用 确认不收费")]
        ConfirmNoChargeFee = 18,
        /// <summary>
        /// 手动抬杠
        /// </summary>
        [Bindable(false)] 
        [Description("手动抬杠")]
        HandGate = 19,

        /// <summary>
        /// 月卡停用
        /// </summary>
        [Bindable(true)]
        [Description("月卡停用")]
        MonthCarStop = 20,
        /// <summary>
        /// 月卡暂停
        /// </summary>
        [Bindable(true)]
        [Description("月卡暂停")]
        MonthCarPause = 21,

        /// <summary>
        /// 黑名单车辆
        /// </summary>
        [Bindable(true)]
        [Description("黑名单车辆")]
        BlacklistCar = 22,

        /// <summary>
        /// 车型未选择
        /// </summary>
        [Bindable(true)]
        [Description("未选择车型")]
        CarModeNoSelected = 23,
        /// <summary>
        /// 待出场
        /// </summary>
        [Bindable(false)] 
        [Description("待出场")]
        WaitExit = 24,
        /// <summary>
        /// 待入场
        /// </summary>
        [Bindable(false)] 
        [Description("待入场")]
        WaitEnter = 25,
        [Bindable(false)] 
        [Description("遥控器开闸")]
        YKQOpen = 26,
        /// <summary>
        /// 设备链接
        /// </summary>
        [Bindable(false)]
        [Description("设备连接")]
        DeviceConnected = 27,
        /// <summary>
        /// 设备断开
        /// </summary>
        [Bindable(false)]
        [Description("设备断开")]
        DeviceDisConnected = 28,
    }
}
