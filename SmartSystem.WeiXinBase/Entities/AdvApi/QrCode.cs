using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}  
namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 二维码创建返回结果
    /// </summary>
    public class QrCode
    {
        [JsonProperty(PropertyName = "ticket")]
        public string Ticket { get; set; }
        [JsonProperty(PropertyName = "expire_seconds")]
        public int ExpireSeconds { get; set; }
    }
}
