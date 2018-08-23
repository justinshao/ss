using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase.RedPack
{
    /// <summary>
    /// 微信裂变红包接口回应
    /// </summary>
    [XmlRoot("xml"), Serializable]
    public class RspSendRedPack
    {
        /// <summary>
        /// 返回状态码,SUCCESS/FAIL.此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息，如非空，为错误原因.如：签名失败、参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果,SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string mch_billno { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 公众账号appid
        /// </summary>
        public string wxappid { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string re_openid { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public int total_amount { get; set; }

        /// <summary>
        /// 发放成功时间
        /// </summary>
        public string send_time { get; set; }

        /// <summary>
        /// 微信单号
        /// </summary>
        public string send_listid { get; set; }
    }
}
