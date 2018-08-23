using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Servers
{
    public class GetMoneyZX
    {
        public string carNumber { set; get; }
        public string parkCode { set; get; }
        public string parkName { set; get; }
        public string vplType { set; get; }
        public string account { set; get; }
        public string inTime { set; get; }
        public string outTime { set; get; }
        public string recordId { set; get; }
        public string orderNo { set; get; }
        public string respCode { set; get; }
        public string app_id { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
    }
}
