using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Queryparkout
{
    public class QueryparkoutDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QueryparkoutAttributesRes attributes { set; get; }
    }
}
