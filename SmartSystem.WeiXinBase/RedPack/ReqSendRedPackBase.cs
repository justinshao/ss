using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartSystem.WeiXinBase.RedPack
{
    /// <summary>
    /// 微信现金红包请求（基类）
    /// </summary> 
    [XmlRoot("xml"), Serializable]
    public class ReqSendRedPackBase
    {
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 商户订单号（每个订单号必须唯一）组成： mch_id+yyyymmdd+10位一天内不能重复的数字。接口根据商户订单号支持重入， 如出现超时可再调用。
        /// </summary>
        public string mch_billno { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string wxappid { get; set; }

        /// <summary>
        /// 红包发送者名称
        /// </summary>
        public string send_name { get; set; }

        /// <summary>
        /// 接收红包的种子用户（首个用户） 用户在wxappid下的openid
        /// </summary>
        public string re_openid { get; set; }

        /// <summary>
        /// 红包发放总金额，即一组红包金额总和，包括分享者的红包和裂变的红包，单位分
        /// </summary>
        public int total_amount { get; set; }

        /// <summary>
        /// 红包发放总人数，即总共有多少人可以领到该组红包（包括分享者）
        /// </summary>
        public int total_num { get; set; }

        /// <summary>
        /// 红包祝福语
        /// </summary>
        public string wishing { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string act_name { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }
    }
}
