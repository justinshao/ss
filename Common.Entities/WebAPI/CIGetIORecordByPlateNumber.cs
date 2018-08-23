using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIGetIORecordByPlateNumber
    {
        public string PlateNumber { set; get; }
        public string VID { set; get; }
        public string SellerID { set; get; }
        public string ProxyNo { set; get; }
    }
}
