using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIGetMonthlyInfo
    {
        public string cmd { set; get; }
        public string PlateNumber { set; get; }
        public string PKID { set; get; }
    }
}
