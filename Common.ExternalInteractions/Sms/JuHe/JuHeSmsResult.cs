using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExternalInteractions.Sms.JuHe
{
    public class JuHeSmsResult
    { /// <summary>
        /// 错误代码
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string reason { get; set; }
        public SmsDetail result { get; set; }
        public bool SendResult
        {
            get
            {
                return error_code == 0;
            }
        }
    }
    public class SmsDetail
    {
        /// <summary>
        /// 发送数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 扣除条数
        /// </summary>
        public int fee { get; set; }
        /// <summary>
        /// 短信ID
        /// </summary>
        public string sid { get; set; }
    }
}
