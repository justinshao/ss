namespace SmartSystem.WeiXinBase
{
    public class WRespImage : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.Image; }
        }

        public Image Image { get; set; }

        public WRespImage()
        {
            Image = new Image();
        }
    }
}
