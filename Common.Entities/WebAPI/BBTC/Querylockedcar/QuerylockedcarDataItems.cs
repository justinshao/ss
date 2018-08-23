using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Querylockedcar
{
    public class QuerylockedcarDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public QuerylockedcarAttributesRes attributes { set; get; }
    }
}
