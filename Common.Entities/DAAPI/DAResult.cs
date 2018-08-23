using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.DAAPI
{
    public class DAResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string result { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string resultmsg { set; get; }
    }
}
