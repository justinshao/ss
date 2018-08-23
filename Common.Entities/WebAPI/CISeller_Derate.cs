using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CISeller_Derate
    {
        public string DerateID { set; get; }
        public string SellerID { set; get; }
        public string Name { set; get; }
        public string DerateType { set; get; }
    }
}
