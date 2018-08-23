using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Bookparkspace
{
    public class BookparkspacePostData
    {
        public string serviceId { set; get; }
        public string requestType { set; get; }
        public string token { set; get; }
        public BookparkspaceAttributes attributes { set; get; }
    }
}
