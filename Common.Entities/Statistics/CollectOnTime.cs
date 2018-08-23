using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class CollectOnTime
    {
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public decimal Amout { set; get; }
        public decimal PayAmout { set; get; }
        public decimal UPayAmout { set; get; }
        public decimal DiscountAmount { set; get; }
        public decimal OnlineAmout { set; get; }
        public decimal XXAmout { set; get; }
        public decimal TemOnlineAmout { set; get; }
        public decimal TemXXAmout { set; get; }
        public decimal MonthOnlineAmout { set; get; }
        public decimal MonthXXAmout { set; get; }

        public int MonthOutCount { set; get; }
        public int TempOutCount { set; get; }
        public int FreeCount { set; get; }
        public decimal MonthAmout { set; get; }
        public decimal TemAmout { set; get; }
    }
}
