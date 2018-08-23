using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.DAAPI
{
    public class DAMonthCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string plate_num { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Parkarea { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmpName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Start_time { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string End_time { set; get; }
    }
}
