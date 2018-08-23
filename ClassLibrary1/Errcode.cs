using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class Errcode
    {
        [JsonProperty(PropertyName = "errcode")]
        public int Errcodes { get; set;}
        [JsonProperty(PropertyName = "errmsg")]
        public string Errmsg{get; set; }
    }
}
