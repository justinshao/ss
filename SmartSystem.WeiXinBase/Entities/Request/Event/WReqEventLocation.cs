namespace SmartSystem.WeiXinBase
{
    public class WReqEventLocation : WReqEventBase
    {
        public override EventType Event
        {
            get
            {
                return EventType.Location;
            }
        }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }
    }
}
