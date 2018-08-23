using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.Interface.Entities
{
    public class RequestData
    {
        /// <summary>
        /// 访问编号
        /// </summary>
        public string access_code { get; set; }
        /// <summary>
        /// 业务代码
        /// </summary>
        public string business_code { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型，目前支持SHA256和MD5
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 数据对接 Json格式
        /// </summary>
        public string data { get; set; }
    }
}
