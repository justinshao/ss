using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class userin
    {
        [JsonProperty(PropertyName = "subscribe")]
         public int Subscribe { get; set; }
        [JsonProperty(PropertyName = "openid")]
         public string Openid { get; set; }
        [JsonProperty(PropertyName = "nickname")]
         public string Nickname { get; set;}

    }
}