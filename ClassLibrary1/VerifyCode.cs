using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class VerifyCode
    {
        [JsonProperty(PropertyName = "Status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "Result")]
        public string Result { get; set; }
    }
}
