using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QueryparkoutAttributes 
    {
        public string parkCode { set; get; }
        public string carNo { set; get; }
        public string cardNo { set; get; }
        public string beginDate { set; get; }
        public string endDate { set; get; }
        public int pageSize { set; get; }
        public int pageIndex { set; get; }
    }
}
