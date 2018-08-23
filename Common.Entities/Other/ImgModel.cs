using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Other
{
    public class ImgModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgPath { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgStream { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgBig
        {
            get;
            set;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgSmall
        {
            get;
            set;
        }
    }
}
