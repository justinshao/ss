using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CISeller_IORecord
    {
        public string RecordID { set; get; }
        public string PlateNumber { set; get; }
        public string EntranceTime { set; get; }
        public string LongTime { set; get; }
        public string PKName { set; get; }
        public string InimgData { set; get; }
        public string IsDiscount { set; get; }
        public string DiscountTime { set; get; }
    }
}
