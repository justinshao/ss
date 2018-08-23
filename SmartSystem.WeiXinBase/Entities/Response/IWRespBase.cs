
namespace SmartSystem.WeiXinBase
{
    public interface IWRespBase:IWBase
    {
        WRespType MsgType { get; }
        //string Content { get; set; }
        bool FuncFlag { get; set; }
    }
}
