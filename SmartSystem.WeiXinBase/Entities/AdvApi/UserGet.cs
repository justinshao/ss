using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinBase
{
    public class UserGet
    {
        public int total { get; set; }
        public int count { get; set; }
        public UserOpenIdList data { get; set; }
        public string next_openid { get; set; }
    }
}
