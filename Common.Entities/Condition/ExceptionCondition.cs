using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class ExceptionCondition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeStart { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeEnd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Server { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Track { get; set; }
        public LogFrom? logFrom { get; set; }
    }
}
