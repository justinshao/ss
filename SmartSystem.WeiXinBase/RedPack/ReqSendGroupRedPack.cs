using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common.Core;

namespace SmartSystem.WeiXinBase.RedPack
{
    /// <summary>
    /// 微信现金裂变红包请求
    /// </summary> 
    [XmlRoot("xml"), Serializable]
    public class ReqSendGroupRedPack : ReqSendRedPackBase
    {
        /// <summary>
        /// 红包金额设置方式 
        /// ALL_RAND—全部随机,商户指定总金额和红包发放总人数，由微信支付随机计算出各红包金额
        /// </summary>
        public string amt_type { get; set; }

        /// <summary>
        /// 设置签名值
        /// </summary> 
        public void SetSign(string key)
        {
            var list = new List<string>();
            list.Add(string.Format("nonce_str={0}", nonce_str));
            list.Add(string.Format("mch_billno={0}", mch_billno));
            list.Add(string.Format("mch_id={0}", mch_id));
            list.Add(string.Format("wxappid={0}", wxappid));
            list.Add(string.Format("send_name={0}", send_name));
            list.Add(string.Format("re_openid={0}", re_openid));
            list.Add(string.Format("total_amount={0}", total_amount));
            list.Add(string.Format("total_num={0}", total_num));
            list.Add(string.Format("amt_type={0}", amt_type));
            list.Add(string.Format("wishing={0}", wishing));
            list.Add(string.Format("act_name={0}", act_name));
            list.Add(string.Format("remark={0}", remark));
            list.Sort();
            var stra = string.Join("&", list.ToArray());
            var str = string.Format("{0}&key={1}", stra, key);
            sign = MD5.Encrypt(str);
        }
    }
}
