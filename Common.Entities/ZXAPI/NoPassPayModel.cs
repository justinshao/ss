using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Entities.ZXAPI
{
    public class NoPassPayModel
    {
        public string app_id { set; get; }
        public string carNumber { set; get; }
        public string recordId { set; get; }
        public string orderId { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
    }
}