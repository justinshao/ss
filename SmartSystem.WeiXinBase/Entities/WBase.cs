using System;

namespace SmartSystem.WeiXinBase
{
    public class WBase : IWBase
    {
        /// <summary>
        /// 获取接收方微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 获取发送方微信号，若为普通用户，则是一个OpenID
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 获取消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
