using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinBase
{
    public class JsapiTicket
    {
        public int expires_in { get; set; }
        public string ticket { get; set; }
    }
}
