namespace SmartSystem.WeiXinBase
{
    public interface IWReqBase : IWBase
    {
        WReqType MsgType { get; }
        long MsgId { get; set; }
    }
}
