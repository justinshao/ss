using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Queryparkout
{
    public class QueryparkoutPostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public QueryparkoutAttributes attributes { set; get; }
    }
}
