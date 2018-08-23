using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
        public class TPLogin
        {
            [JsonProperty(PropertyName = "status")]
            public int Status { get; set; }
            [JsonProperty(PropertyName = "result")]
            public string Result { get; set; }
        }
}
