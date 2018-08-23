namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的事件
    /// </summary>
    public class WReqEventBase : WReqBase
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public override WReqType MsgType
        {
            get { return WReqType.Event; }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual EventType Event
        {
            get { return EventType.Enter; }
        }
    }
}
