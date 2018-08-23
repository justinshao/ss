using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Queryorder
{
    public class QueryorderDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QueryorderAttributesRes attributes { set; get; }
    }
}
