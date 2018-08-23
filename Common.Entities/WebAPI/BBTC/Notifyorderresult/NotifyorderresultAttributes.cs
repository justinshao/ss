using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class NotifyorderresultAttributes
    {
        public string orderNo { set; get; }
        public string parkCode { set; get; }
        public int tradeStatus { set; get; }
        public string isCallBack { set; get; }
        public string notifyUrl { set; get; }
    }
}
