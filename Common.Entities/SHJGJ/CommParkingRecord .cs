using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class CommParkingRecord
    {
        public int parkingRecordType { set; get; }
        public string parkingSpotId { set; get; }
        public string platformId { set; get; }
        public string berthId { set; get; }
        public string addBerth { set; get; }
        public string carNumber { set; get; }
        public int carType { set; get; }
        public int parkingActType { set; get; }
        public int leavingActType { set; get; }
        public string parkingTime { set; get; }
        public string leavingTime { set; get; }
        public int parkingTimeLength { set; get; }
        public string parkingBatchCode { set; get; }
        public int parkingBizSn { set; get; }
        public string leavingBatchCode { set; get; }
        public int leavingBizSn { set; get; }
        public int factMoney { set; get; }
        public int factDiscount { set; get; }
        public int dueMoney { set; get; }
        public int payMoney { set; get; }
        public int payDiscount { set; get; }
        public int dueBalance { set; get; }
        public int prepayTimeLength { set; get; }
        public int prepayMoney { set; get; }
        public int prepayDiscount { set; get; }
        public int compensateMoney { set; get; }

    }
}
