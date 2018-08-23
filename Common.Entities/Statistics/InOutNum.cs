using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class InOutNum
    {
        public DateTime StartTime { set; get; }
        public string EndTime { set; get; }
        public string InCount { set; get; }
        public string OutCount { set; get; }
        public string PJZ { set; get; }
    }
}
