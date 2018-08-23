namespace SmartSystem.WeiXinBase
{
    public class WRespText : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.Text; }
        }

        public string Content { get; set; }
    }
}
