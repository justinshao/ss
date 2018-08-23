using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Carddelay
{
    public class CarddelayPostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public CarddelayAttributes attributes { set; get; }
    }
}
