namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信Post过来的请求基础类型
    /// </summary>
    public class WReqBase : WBase, IWReqBase
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public virtual WReqType MsgType
        {
            get { return WReqType.Text; }
        }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
    }
}
