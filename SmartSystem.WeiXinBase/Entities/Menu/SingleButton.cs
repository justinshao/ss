using Newtonsoft.Json;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 所有单击按钮的基类（view，click等）
    /// </summary>
    public abstract class SingleButton : BaseButton
    {
        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        protected SingleButton(string theType)
        {
            Type = theType;
        }
    }
}
