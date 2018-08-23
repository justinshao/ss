using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SmartSystem.WeiXinServices.Payment
{
    public class WeiXinPayConst
    {
        #region Value Const
        /// <summary>
        /// V2:支付请求中 用于加密的秘钥Key，可用于验证商户的唯一性，对应支付场景中的AppKey
        /// </summary>
        public static readonly string PaySignKey = "V2.PaySignKey";

        #endregion

        #region 统一支付相关Url （V3接口）

        /// <summary>
        /// 统一预支付Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        /// <summary>
        /// 订单查询Url 
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
        /// <summary>
        /// 退款申请Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderRefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        /// <summary>
        /// 取消支付Url
        /// </summary>
        public const string WeiXin_Pay_ClosePayUrl = "https://api.mch.weixin.qq.com/pay/closeorder";

        #endregion
    }
}
