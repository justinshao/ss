using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class EmployeeCondition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VillageId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeptId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }
        public EmployeeType? EmployeeType { get; set; }
    }
}
