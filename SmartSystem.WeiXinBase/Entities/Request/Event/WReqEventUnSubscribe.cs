namespace SmartSystem.WeiXinBase
{
    public class WReqEventUnSubscribe : WReqEventBase
    {
        public override EventType Event
        {
            get
            {
                return EventType.UnSubscribe;
            }
        }
    }
}
