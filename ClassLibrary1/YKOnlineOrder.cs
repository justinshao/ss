using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class YKOnlineOrder
    {
        public int Status { get; set; }
        public OnlineResult Result { get; set; }
    }
    public class OnlineResult
    {
        public string appId { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string partnerId { get; set; }
        public string prepayId { get; set; }
        public string timeStamp { get; set; }
        public string sign { get; set; }
        public string strsign { get; set; }
    }
}
