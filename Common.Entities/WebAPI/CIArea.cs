using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIArea
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaName { set; get; }
    }
}
