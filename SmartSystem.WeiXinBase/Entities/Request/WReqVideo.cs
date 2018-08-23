using System;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的视频消息
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class WReqVideo : WReqBase
    {
        public override WReqType MsgType
        {
            get { return WReqType.Video; }
        }
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
}
