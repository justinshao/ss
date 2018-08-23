using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.Interface.Entities
{
    public class BaseResult
    {
        /// <summary>
        /// 返回结果 SUCCESS/FAIL
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因,例如：签名失败 参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }
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
    }
}
