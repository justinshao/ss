using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Entities.ZXAPI
{
    public class QueryParkInfoModel
    {
        public string app_id { get; set; }
        public string parkCode { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
    }
}