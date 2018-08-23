namespace SmartSystem.WeiXinBase
{
    public class WRespVoice : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.Voice; }
        }

        public Voice Voice { get; set; }

        public WRespVoice()
        {
            Voice = new Voice();
        }
    }
}
