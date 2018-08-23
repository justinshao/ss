namespace SmartSystem.WeiXinBase
{
    public class WReqEventClick : WReqEventBase
    {
        public override EventType Event
        {
            get { return EventType.Click; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
    }
}
