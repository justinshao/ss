using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Entities.ZXAPI
{
    public class VipCardModel
    {
        public string app_id { set; get; }
        public List<carNumbers> carNumbers { set; get; }
        public string parkCode { set; get; }
        public string cardId { set; get; }
        public string effDate { set; get; }
        public string expDate { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
        public int orderType { set; get; }
    }
    
    public class carNumbers
    {
        public string carNumber { set; get; }
    }
}