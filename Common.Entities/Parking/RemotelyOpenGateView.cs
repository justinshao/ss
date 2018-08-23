using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class RemotelyOpenGateView
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VillageName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IoState { get; set; }
    }
}
