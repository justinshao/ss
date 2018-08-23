using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class CreateOrderByCarNoAttributes 
    {
        public string businesserCode { set; get; }
        public string parkCode { set; get; }
        public string orderType { set; get; }
        public string carNo { set; get; }
    }
}
