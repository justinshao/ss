using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Notifyorderresult
{
    public class NotifyorderresultPostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public NotifyorderresultAttributes attributes { set; get; }
    }
}
