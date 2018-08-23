using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIDiscountPlateNumber
    {
        public string IORecordID{set;get;}
        public string DerateID{set;get;}
        public string VID{set;get;}
        public string SellerID{set;get;}
        public string DerateMoney { set; get; }
        public string ProxyNo { set; get; }
    }
}
