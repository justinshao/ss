namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 进入会话
        /// </summary>
        Enter,
        /// <summary>
        /// 订阅
        /// </summary>
        Subscribe,
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        Location,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UnSubscribe,
        /// <summary>
        /// 自定义菜单点击事件
        /// </summary>
        Click,
        /// <summary>
        /// 二维码扫描
        /// </summary>
        Scan,
        /// <summary>
        ///  URL跳转
        /// </summary>
        View,
        /// <summary>
        /// 群发结果
        /// </summary>
        Masssendjobfinish,
        /// <summary>
        /// 未定义
        /// </summary>
        UnDefine,
        /// <summary>
        /// 卡券
        /// </summary>
        Card,
    }
}
