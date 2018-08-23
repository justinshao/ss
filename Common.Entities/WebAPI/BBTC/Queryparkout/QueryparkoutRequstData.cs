using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Queryparkout
{
    public class QueryparkoutRequstData
    {
        public string serviceId { set; get; }
        public int resultCode { set; get; }
        public string message { set; get; }
        public List<QueryparkoutDataItems> dataItems { set; get; }  
    }
}
