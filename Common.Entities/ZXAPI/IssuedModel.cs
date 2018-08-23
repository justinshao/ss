using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZXAPI
{
    public class IssuedModel
    {
        public string app_id { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
        public string carNumber { set; get; }
        public string recordId { set; get; }
        public string parkCode { set; get; }
        public int amout { set; get; } 
    }
}
