using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.AliPayServices.Entities
{
    public class AlipayTradeOrderModel
    {
        /// <summary>
        /// 对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body
        /// </summary>
        public string body { set; get; }

        /// <summary>
        /// 商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>
        public string subject { set; get; }

        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>
        public string total_amount { set; get; }
        public string discountable_amount { set; get; }
        public string undiscountable_amount { set; get; }
        public string buyer_logon_id { set; get; }
        /// <summary>
        /// 收款支付宝用户ID。 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>
        public string seller_id { set; get; }

        /// <summary>
        /// 针对用户授权接口，获取用户相关数据时，用于标识用户授权关系
        /// </summary>
        //public string auth_token { set; get; }

        public string buyer_id { set; get; }
        /// <summary>
        /// 停车场的唯一ID 
        /// </summary>
        public string store_id { set; get; }
        public string timeout_express = "10m";
    }
}
