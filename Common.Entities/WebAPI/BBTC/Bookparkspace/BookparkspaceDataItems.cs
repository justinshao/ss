using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC.Bookparkspace
{
    public class BookparkspaceDataItems
    {
        public string objectId { set; get; }
        public string operateType { set; get; }
        public BookparkspaceAttributesRes attributes { set; get; }
    }
}
