namespace SmartSystem.WeiXinBase
{
    public class WRespBase : WBase, IWRespBase
    {
        public virtual WRespType MsgType
        {
            get { return WRespType.Text; }
        }

        public bool FuncFlag { get; set; }
    }
}
