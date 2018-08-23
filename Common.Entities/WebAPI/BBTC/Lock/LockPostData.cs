using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Lock
{
    public class LockPostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public LockAttributes attributes { set; get; }
    }
}
