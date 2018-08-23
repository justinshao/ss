using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class CouponByMonth
    {
        public int Status { get; set; }
        public CouponMonth Result { get; set; }
        public int Price { get; set; }
    }
    public class CouponMonth
    {
        public List<CouponByMon> CouponList { get; set; }
    }
    public class CouponByMon
    {
        public decimal Full { get; set; }
        public decimal Cut { get; set; }
        public DateTime ReceiveTime { get; set; }
        public DateTime DeadlineTime { get; set; }
        public int Status { get; set; }
        public string CouponID { get; set; }
        public decimal CapPrice { get; set; }
        public string CouponType { get; set; }
        public string Brife { get; set; }
        public decimal CutPrice { get; set; }
    }
}
