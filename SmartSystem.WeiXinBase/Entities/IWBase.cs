using System;
namespace SmartSystem.WeiXinBase
{
    public interface IWBase
    {
        /// <summary>
        /// 获取接收方微信号
        /// </summary>
        string ToUserName { get; set; }
        /// <summary>
        /// 获取发送方微信号，若为普通用户，则是一个OpenID
        /// </summary>
        string FromUserName { get; set; }
        /// <summary>
        /// 获取消息创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
