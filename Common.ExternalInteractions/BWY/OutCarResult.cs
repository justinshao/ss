using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExternalInteractions.BWY
{
    public class OutCarResult
    {
        /// <summary>
        /// 0-成功
        /// </summary>
        public int Result { get; set; }
        public string Desc { get; set; }
        public OutCarInfo Reference { get; set; }
    }
    public class OutCarInfo {
        public int Index { get; set; }
        public string LPR { get; set; }
        public string LPRColor { get; set; }
        public int AutoPay { get; set; }
        public int Balance { get; set; }
    }
}
