using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIOrder
    {
        public string RecordID { set; get; }
        public string OrderNo { set; get; }
        public int OrderType { set; get; }
        public int PayWay { set; get; }
        public decimal Amount { set; get; }
        public decimal UnPayAmount { set; get; }
        public decimal PayAmount { set; get; }
        public int Status { set; get; }
        public string OrderTime { set; get; }
        public string OldUserulDate { set; get; }
        public string NewUsefulDate { set; get; }
        public string OnlineUserID { set; get; }
        public string OnlineOrderNo { set; get; }
    }
}
