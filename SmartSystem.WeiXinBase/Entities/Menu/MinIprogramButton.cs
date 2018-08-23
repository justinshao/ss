using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SmartSystem.WeiXinBase
{
    public class MinIprogramButton : SingleButton
    {
        /// <summary>
        /// 不支持小程序的老版本客户端将打开本url
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
         /// <summary>
        /// 小程序AppId
        /// </summary>
        [JsonProperty(PropertyName = "appid")]
        public string AppId { get; set; }
        /// <summary>
        /// 小程序页面路径
        /// </summary>
        [JsonProperty(PropertyName = "pagepath")]
        public string PagePath { get; set; }

        public MinIprogramButton()
            : base(WButtonType.MinIprogram.ToString().ToLower())
        { }
    }
}
