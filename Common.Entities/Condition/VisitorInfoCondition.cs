using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Condition
{
    public class VisitorInfoCondition
    {
        public string VisitorMobilePhone { set; get; }
        public  string PlateNumber { set; get; }
        public  string VisitorName { set; get; }
        public  DateTime StartTime { set; get; }
        public  DateTime EndTime { set; get; }
        public  int VisitorState { set; get; }

    }
}
