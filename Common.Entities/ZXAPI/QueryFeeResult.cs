using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZXAPI
{
    public class QueryFeeResult
    {
        public string parkCode { set; get; }
        public string inTime { set; get; }
        public string outTtime { set; get; }
        public string staytime { set; get; }
        public Charge charge { set; get; }
    }

    public class Charge
    {
        public string due { set; get; }
        public string paid { set; get; }
        public string unpaid { set; get; }
        public string duration { set; get; }
    }
}
