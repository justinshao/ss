using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class InOutParkModel
    {
        public string uid { set; get; }
        public int seqno { set; get; }
        public string code { set; get; }
        public CommRequest commRequest { set; get; }
        public List<CommParkingRecord> commParkingRecord { set; get; }
    }
}
