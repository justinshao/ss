using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class InParkModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int seqno { set; get; }

        /// <summary>
        /// 业务编号：
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// 通用请求字段：
        /// </summary>
        public CommRequest commRequest { set; get; }

        /// <summary>
        /// 工号：
        /// </summary>
        public string uid { set; get; }

        /// <summary>
        /// 终端业务操作流水记录列表：
        /// </summary>
        public  List<BusinessLog> businessLogList { set; get; }
       

    }
}
