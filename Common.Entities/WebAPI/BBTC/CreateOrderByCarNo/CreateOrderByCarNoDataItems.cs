using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.CreateOrderByCarNo
{
    public class CreateOrderByCarNoDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public CreateOrderByCarNoAttributesRes attributes { set; get; }
    }
}
