using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.AliPayServices.Entities
{
    public class PreAliPayOrderModel
    {
        public string out_trade_no { get; set; }
        public string total_amount { get; set; }
        public string subject { get; set; }
        public string store_id { get; set; }
        public string timeout_express = "10m";
    }
}
