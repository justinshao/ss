using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Querylockedcar
{
    public class QuerylockedcarRequstData
    {
        public string serviceId { set; get; }
        public int resultCode { set; get; }
        public string message { set; get; }
        public List<QuerylockedcarDataItems> dataItems { set; get; }  
    }
}
