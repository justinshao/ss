using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class TemResult
    {
        public string Result { set; get; }
        public Temresultdata data { set; get; }
    }

    public class Temresultdata
    {
        public string Result { set; get; }
        public string OrderNo { set; get; }
        public string ISOpen { set; get; }
    }
}
