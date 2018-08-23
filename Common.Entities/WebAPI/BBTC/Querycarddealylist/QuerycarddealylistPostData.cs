using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Querycarddealylist
{
    public class QuerycarddealylistPostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public QuerycarddealylistAttributes attributes { set; get; }
    }
}
