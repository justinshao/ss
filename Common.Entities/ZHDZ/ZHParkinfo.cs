using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZHDZ
{
    public class ZHParkinfo
    {
        public int area_id { set; get; }
        public string parking_id { set; get; }
        public string name { set; get; }
        public string address { set; get; }
        public int contain_total { set; get; }
        public int retain_total { set; get; }
        public float longitude { set; get; }
        public float latitude { set; get; }
        public string fee_description { set; get; }
        public string ProxyNo { set; get; }
        public string DataStatus { set; get; }
    }
}
