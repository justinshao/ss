using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.Interface.Entities
{
    public class ReturnResult:BaseResult
    {
        /// <summary>
        /// 以下字段在return_code为SUCCESS的时候有返回
        /// 业务结果 SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 当result_code为SUCCESS时可能会有
        /// 业务结果对象
        /// </summary>
        public string data { get; set; }
    }
}
