using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Entities.ZXAPI
{
    public class SendTimeModel
    {
        public string app_id { set; get; }
        public string duration { set; get; }
        public string parkCode { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }

    }
}