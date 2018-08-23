using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerypersonsbycarDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QuerypersonsbycarAttributesRes attributes { set; get; }
        public List<QuerypersonsbycarSubItems> subItems { set; get; }
    }
}
