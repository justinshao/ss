using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class ewm
    {
        [JsonProperty(PropertyName = "ticket")]
        public string Ticket { get; set; }
        [JsonProperty(PropertyName = "expire_seconds")]
        public int ExpireSeconds { get; set; }
        
    }
}