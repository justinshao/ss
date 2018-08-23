using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Notifyorderresult
{
    public class NotifyorderresultDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public NotifyorderresultAttributesRes attributes { set; get; }
    }
}
