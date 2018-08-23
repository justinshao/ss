using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataAccess
{
    [Serializable]
    public class PageInfo
    {
        public PageInfo() {
            PageSize = 10;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
