using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CITempPayAgt
    {
        public string OrderNO { set; get; }
        public int PayWay { set; get; }
        public decimal Amount { set; get; }
        public string PKID { set; get; }
        public string OnlineOrderID { set; get; }
        public string PayDate { set; get; }
    }
}
