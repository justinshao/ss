using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Servers
{
    /// <summary>
    /// 类ParkDerateIntervar。
    /// </summary>
    [Serializable]
    public class TableDataCount
    {
        public int PageCount { set; get; }
        public int DataCount { set; get; }
        public string VID { set; get; }
    }
}
