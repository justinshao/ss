using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinServices.Payment
{
    /// <summary>
    /// V3 关闭支付
    /// </summary>
    [XmlRoot("xml")]
    public class ClosePayMessage : ReturnMessage
    {
        [XmlElement("result_code")]
        public string Result_Code { get; set; }
        [XmlElement("appid")]
        public string AppId { get; set; }
        [XmlElement("mch_id")]
        public string Mch_Id { get; set; }
        [XmlElement("nonce_str")]
        public string Nonce_Str { get; set; }
        [XmlElement("sign")]
        public string Sign { get; set; }
        [XmlElement("err_code")]
        public string Err_Code { get; set; }
        [XmlElement("err_code_des")]
        public string Err_Code_Des { get; set; }
        /// <summary>
        /// 退款申请是否接收成功（不代表退款成功，退款结果需要查询）
        /// </summary>
        public bool Success
        {
            get
            {
                return Return_Code.ToLower() == "success"
                    && Result_Code.ToLower() == "success";
            }
        }
    }
}
