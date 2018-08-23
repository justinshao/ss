using System;
using System.Xml;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的文本消息
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class WReqText : WReqBase
    {
        public override WReqType MsgType
        {
            get { return WReqType.Text; }
        }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        
        public string Content { get; set; }
    }
}
