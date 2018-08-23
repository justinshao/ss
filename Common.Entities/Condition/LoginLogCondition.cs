using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class LoginLogCondition
    {
        public LogFrom? LogFrom { get; set; }
        
        public string UserAccount { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
    }
}
