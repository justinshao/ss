using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class GenerateuncarorderAttributesRes
    {
        public string retcode { set; get; }
        public string orderNo { set; get; }
        public string certificateNo { set; get; }
        public string VcarNo { set; get; }
        public string parkCode { set; get; }
        public string parkName { set; get; }
        public string serviceTime { set; get; }
        public string createTime{set;get;}
        public string endTime { set; get; }
        public string serviceFee { set; get; }
        public string discountFee { set; get; }
        public string totalFee { set; get; }
        public string freeMinute { set; get; }
    }
}
