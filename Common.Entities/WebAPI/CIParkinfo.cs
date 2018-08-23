using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI
{
    public class CIParkinfo
    {
        public string PKID { set; get; }
        public string PKName { set; get; }
        public string CarBitNum { set; get; }
        public string CarBitNumLeft { set; get; }
        public string CarBitNumFixed { set; get; }
        public string SpaceBitNum { set; get; }
        public string FeeRemark { set; get; }
        public List<CIArea> Areas { set; get; }
    }
}
