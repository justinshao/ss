using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Servers
{
    public class DownDataPageModel
    {
        public int PageCount { set; get; }
        public int DataCount { set; get; }
        public int PageIndex { set; get; }
        public object DataList { set; get; }
    }
}
