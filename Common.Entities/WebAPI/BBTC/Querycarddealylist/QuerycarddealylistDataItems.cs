using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Querycarddealylist
{
   public  class QuerycarddealylistDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QuerycarddealylistAttributesRes attributes { set; get; }
    }
}
