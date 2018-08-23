using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerypersonsbycarAttributesRes
    {
        public string personCode { set; get; }
        public string personName { set; get; }
        public string identityCode { set; get; }
        public string sex { set; get; }
        public string birthday { set; get; }
        public string telephone { set; get; }
        public string email { set; get; }
        public string secretKey { set; get; }
        public string areaId { set; get; }
    }
}
