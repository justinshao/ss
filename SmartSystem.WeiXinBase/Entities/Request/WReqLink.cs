using System;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的链接消息
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class WReqLink : WReqBase
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public override WReqType MsgType
        {
            get { return WReqType.Link; }
        }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
    }
}
