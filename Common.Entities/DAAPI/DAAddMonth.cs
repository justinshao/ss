using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.DAAPI
{
    public class DAAddMonth
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BaseEmployee employye { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EmployeePlate plate { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BaseCard card { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkGrant grant { set; get; }

    }
}
