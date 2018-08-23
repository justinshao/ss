namespace SmartSystem.WeiXinBase
{
    public class WReqEventSubscribe : WReqEventBase
    {
        public override EventType Event
        {
            get
            {
                return EventType.Subscribe;
            }
        }
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
