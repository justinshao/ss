using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerypersonsbycarSubItemsAttributes
    {
        public string cardId { set; get; }
        public string physicalNo { set; get; }
        public string issueTime { set; get; }
        public string endTime { set; get; }
        public string carNo { set; get; }
        public string cardType { set; get; }
        public Dictionary<string, string> package { set; get; }
    }
}
