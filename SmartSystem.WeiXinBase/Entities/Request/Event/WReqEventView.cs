namespace SmartSystem.WeiXinBase
{
    public class WReqEventView : WReqEventBase
    {
        public override EventType Event
        {
            get { return EventType.View; }
        }

        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }
}
