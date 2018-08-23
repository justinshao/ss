using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class ParkInoModelcs
    {
        public int seqno { set; get; }
        public string code { set; get; }
        public CommRequest commRequest { set; get; }
        public string uid { set; get; }
        public string batchCode { set; get; }
        public int bizSn { set; get; }
        public string actTime { set; get; }
        public string parkingSpotId { set; get; }
        public string platformId { set; get; }
        public int totBerthNum { set; get; }
        public int monthlyBerthNum { set; get; }
        public int guesBerthNum { set; get; }
        public int totRemainNum { set; get; }
        public int monthlyRemainNum { set; get; }
        public int guestRemainNum { set; get; }

    }
}
