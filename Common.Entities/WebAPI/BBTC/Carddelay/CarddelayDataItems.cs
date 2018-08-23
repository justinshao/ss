using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Carddelay
{
    public class CarddelayDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public CarddelayAttributesRes attributes { set; get; }
    }
}
