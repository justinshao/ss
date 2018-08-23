namespace SmartSystem.WeiXinBase
{
    public class WRespMusic : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.Music; }
        }

        public Music Music { get; set; }
        public string ThumbMediaId { get; set; }

        public WRespMusic()
        {
            Music = new Music();
        }
    }
}
