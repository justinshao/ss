using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIGetMonthlyInfoByPK
    {
        public string cmd { set; get; }
        public string PKID { set; get; }
        public int PageIndex { set; get; }
        

    }
}
