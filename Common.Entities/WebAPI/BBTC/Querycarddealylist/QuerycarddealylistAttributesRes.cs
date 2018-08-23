using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerycarddealylistAttributesRes
    {
        public string cardId { set; get; }
        public string physicalNo { set; get; }
        public string issueTime { set; get; }
        public string payTime { set; get; }
        public float money { set; get; }
        public string beginDate { set; get; }
        public string endDate { set; get; }
        public string carNo { set; get; }
    }
}
