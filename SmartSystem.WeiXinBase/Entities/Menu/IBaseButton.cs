using Newtonsoft.Json;
using System;

namespace SmartSystem.WeiXinBase
{

    public interface IBaseButton
    {
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
    }
}
