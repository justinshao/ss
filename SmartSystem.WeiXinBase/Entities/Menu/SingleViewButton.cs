using Newtonsoft.Json;

namespace SmartSystem.WeiXinBase
{

    /// <summary>
    /// Url按键
    /// </summary>
    public class SingleViewButton : SingleButton
    {
        /// <summary>
        /// 类型为view时必须
        /// 网页链接，用户点击按钮可打开链接，不超过256字节
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        public SingleViewButton()
            : base(WButtonType.View.ToString().ToLower())
        { }
    }
}
