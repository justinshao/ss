using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Querycarddealylist
{
    public class QuerycarddealylistRequstData
    {
        public string serviceId { set; get; }
        public int resultCode { set; get; }
        public string message { set; get; }
        public List<QuerycarddealylistDataItems> dataItems { set; get; }  
    }
}
