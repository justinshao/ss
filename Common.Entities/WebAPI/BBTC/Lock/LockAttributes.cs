using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class LockAttributes 
    {
        public string parkCode { set; get; }
        public string carNo { set; get; }
        public int lockFlag { set; get; }
        public string featureCode { set; get; }
    }
}
