using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinBase
{
    public class SendAllResult : ResError
    {
        public string type { get; set; }
        public long msg_id { get; set; }
    }
}
