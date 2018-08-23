using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class AddMonthlyInfoResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Res { set; get; }
    }
}
