using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 微信推送过来的地理位置消息
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class WReqLocation : WReqBase
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public override WReqType MsgType
        {
            get { return WReqType.Location; }
        }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        [JsonProperty(PropertyName = "Location_X")]
        public string LocationX { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        [JsonProperty(PropertyName = "Location_Y")]
        public string LocationY { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
    }
}
