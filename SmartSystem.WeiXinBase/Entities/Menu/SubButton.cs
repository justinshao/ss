using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 子菜单
    /// </summary>
    public class SubButton : BaseButton
    {
        /// <summary>
        /// 子按钮数组，按钮个数应为2~5个
        /// </summary>
        [JsonProperty(PropertyName = "sub_button")]
        public List<SingleButton> SubButtons { get; set; }

        public SubButton()
        {
            SubButtons = new List<SingleButton>();
        }

        public SubButton(string name)
            : this()
        {
            Name = name;
        }
    }
}
