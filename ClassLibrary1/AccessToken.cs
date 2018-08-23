using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class AccessToken
    {

        [JsonProperty(PropertyName = "access_token")]
        public string Accesstoken { get; set; }


        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
        


    }

    
    

}