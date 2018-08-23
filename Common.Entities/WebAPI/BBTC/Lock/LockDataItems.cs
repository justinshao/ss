using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Lock
{
    public class LockDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public LockAttributesRes attributes { set; get; }
    }
}
