using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QueryparkspaceAttributesRes 
    {
        public string parkCode { set; get; }
        public string parkName { set; get; }
        public int totalSpace { set; get; }
        public int restSpace { set; get; }
    }
}
