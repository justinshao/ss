using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZHDZ
{
    public class ZHIORecord
    {
        public int ID { set; get; }
        public string ReserveID { set; get; }
        public string RecordID { set; get; }
        public string parking_id { set; get; }
        public string license_plate { set; get; }
        public int type { set; get; }
        public int is_monthly_rent { set; get; }
        public DateTime time { set; get; }
        public decimal fee_total { set; get; }
        public string OrderID { set; get; }
    }
}
