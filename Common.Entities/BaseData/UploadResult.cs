using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.BaseData
{
    public class UploadResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID { set; get; }
    }
}
