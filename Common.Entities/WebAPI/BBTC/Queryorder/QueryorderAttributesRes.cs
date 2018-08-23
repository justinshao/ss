using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QueryorderAttributesRes 
    {
        public string businesserCode { set; get; }
        public string businesserName { set; get; }
        public string parkCode { set; get; }
        public string parkName { set; get; }
        public string orderNo { set; get; }
        public string goodName { set; get; }
        public string cardNo { set; get; }
        public string carNo { set; get; }
        public string startTime { set; get; }
        public int serviceTime { set; get; }
        public string createTime { set; get; }
        public string endTime { set; get; }
        public double serviceFee { set; get; }
        public double transportFee { set; get; }
        public double discountFee { set; get; }
        public double otherFee { set; get; }
        public double deductFee { set; get; }
        public double totalFee { set; get; }
        public int tradeStatus { set; get; }
    }
}
