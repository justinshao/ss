using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIMonthlyPayAgt
    {
        public string CardID { set; get; }
        public string PKID { set; get; }
        public int MonthNum { set; get; }
        public decimal Amount { set; get; }
        public string AccountID { set; get; }
        public int PayWay { set; get; }
       
        public string OnlineOrderID { set; get; }
        public string PayDate { set; get; }
    }
}
