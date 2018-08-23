using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class CommRequest
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string clientId { set; get; }

        /// <summary>
        /// 终端序列号
        /// </summary>
        public string tsn { set; get; }

        /// <summary>
        /// *SIM 卡号
        /// </summary>
        public string sim { set; get; }

        /// <summary>
        /// *PSAM 卡号
        /// </summary>
        public string psam { set; get; }

        /// <summary>
        /// *系统版本号
        /// </summary>
        public string sysVer { set; get; }

        /// <summary>
        /// *应用版本号
        /// </summary>
        public string appVer { set; get; }
    }
}
