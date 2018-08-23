using System;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的图片消息
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class WReqImage : WReqBase
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public override WReqType MsgType
        {
            get { return WReqType.Image; }
        }
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }
    }
}
