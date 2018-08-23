using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Queryparkspace
{
    public class QueryparkspaceDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QueryparkspaceAttributesRes attributes { set; get; }
    }
}
