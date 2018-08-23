using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ConnectionStateModel
{
    public class ConnectionStateResult
    {
        public int PageIndex { set; get; }
        public int Total { set; get; }
        public string ResData { set; get; }
    }
}
