namespace SmartSystem.WeiXinBase
{
    public class WRespVideo : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.Video; }
        }

        public Video Video { get; set; }

        public WRespVideo()
        {
            Video = new Video();
        }
    }
}
