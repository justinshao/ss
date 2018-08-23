using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Entities.ZXAPI
{
    public class ZXResult
    {
        public string code { set; get; }
        public object data { set; get; }
        public string message { set; get; }
    }
}