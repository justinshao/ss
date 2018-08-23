using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace SmartSystem.WeiXinBase
{

    /// <summary>
    /// 整个按钮设置（可以直接用MainButton实例返回JSON对象）
    /// </summary>
    public class MainButton
    {
        /// <summary>
        /// 按钮数组，按钮个数应为2~3个
        /// </summary>
        [JsonProperty(PropertyName = "button")]
        public List<BaseButton> Button { get; set; }

        public MainButton()
        {
            Button = new List<BaseButton>();
        }
    }
}
