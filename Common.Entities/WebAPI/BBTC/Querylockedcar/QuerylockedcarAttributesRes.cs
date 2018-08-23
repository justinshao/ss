using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerylockedcarAttributesRes 
    {
        public string lockId { set; get; }
        public string parkCode { set; get; }
        public string carNo { set; get; }
        public string lockTime { set; get; }
        public string featureCode { set; get; }
    }
}
