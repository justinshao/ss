using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class MyResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool result { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string msg { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic data { get; set; }
         
        public static MyResult Error(string msg = null, dynamic data = null)
        {
            return new MyResult
            {
                result = false,
                msg = msg,
                data = data
            };
        }
         
        public static MyResult Success(string msg = null, dynamic data = null)
        {
            return new MyResult
            {
                result = true,
                msg = msg,
                data = data
            };
        }
    }
}
