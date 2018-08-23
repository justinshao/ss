using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Condition
{
    public class VisitorReportCondition
    {
        public List<string> ParkingIds { get; set; }
        public int? VisitorSource { get; set; }
        public int? VisitorState { get; set; }
        public string MoblieOrName { get; set; }
        public DateTime? BeginTime { set; get; }
        public DateTime? EndTime { set; get; }
        public string PlateNumber { get; set; }
    }
}
