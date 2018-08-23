using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class OperateLogCondition
    {
        public LogFrom? LogFrom { get; set; }
        public string UserAccount { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public OperateType? OperateType { get; set; }
        public string ModuleName { get; set; }
        public string MethodName { get; set; }
    }
}
