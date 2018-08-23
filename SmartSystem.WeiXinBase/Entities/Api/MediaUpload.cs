using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace SmartSystem.WeiXinBase
{
    namespace System.Runtime.CompilerServices
    {
        public class ExtensionAttribute : Attribute { }
    }  
    public class MediaUpload
    {
        [JsonProperty(PropertyName = "type")]
        public MediaType Type { get; set; }
        [JsonProperty(PropertyName = "media_id")]
        public string MediaID { get; set; }
        [JsonProperty(PropertyName = "thumb_media_id")]
        public string ThumbMediaID { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public long CreatedAt { get; set; }
    }

    public enum MediaType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 语音
        /// </summary>
        Voice,
        /// <summary>
        /// 视频
        /// </summary>
        Video,
        /// <summary>
        /// thumb
        /// </summary>
        Thumb
    }
}
