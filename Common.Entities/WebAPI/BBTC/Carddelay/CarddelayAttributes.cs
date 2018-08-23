using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class CarddelayAttributes
    {
        public string parkCode { set; get; }
        public string cardId { set; get; }
        public int month { set; get; }
        public float money { set; get; }
        public string newBeginDate { set; get; }
        public string newEndDate { set; get; }
    }
}
