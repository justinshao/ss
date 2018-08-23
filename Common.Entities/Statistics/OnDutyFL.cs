using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class OnDutyFL
    {
        public string UserName { set; get; }
        public decimal YSJE { set; get; }
        public decimal SSJE { set; get; }
        public decimal SFJM { set; get; }
        public decimal XFJM { set; get; }
        public string CardType { set; get; }
        public int CarNum { set; get; }
    }
}
