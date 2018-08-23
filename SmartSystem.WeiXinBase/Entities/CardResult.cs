using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 基
    /// </summary>
    public class CardBaseResult
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }

    /// <summary>
    /// 微信创建卡券接口返回值
    /// </summary>
    public class CardCreateResult : CardBaseResult
    {
        public string card_id { get; set; }
    }

    /// <summary>
    /// 微信投放二维码卡券
    /// </summary>
    public class CardPutQuickmarkResult : CardBaseResult
    {
        /// <summary>
        /// 获取的二维码ticket，凭借此ticket调用通过ticket换取二维码接口可以在有效时间内换取二维码。
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        public string url { get; set; }
    }

    /// <summary>
    /// 微信投放货架
    /// </summary>
    public class CardPutLandingPageResult : CardBaseResult
    {
        /// <summary>
        /// 货架链接。
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 货架ID。货架的唯一标识。
        /// </summary>
        public string page_id { get; set; }
    }
    /// <summary>
    /// 微信核销卡券
    /// </summary>
    public class CardConsumeResult : CardBaseResult
    {
        /// <summary>
        /// 卡券ID。
        /// </summary>
        public CardBaseInfo card { get; set; }
        /// <summary>
        /// 用户在该公众号内的唯一身份标识。
        /// </summary>
        public string openid { get; set; }


    }
    /// <summary>
    /// 微信核销卡券(code解码)
    /// </summary>
    public class CardConsumeDeResult : CardBaseResult
    {
        /// <summary>
        /// 解密后获取的真实Code码
        /// </summary>
        public string code { get; set; }

    }
    /// <summary>
    /// 微信管理卡券(获取卡券信息)
    /// </summary>
    public class CardManageGetResult : CardBaseResult
    {
        /// <summary>
        /// 用户openid
        /// </summary>
        public string openid { get; set; }
        public CardBaseInfo card { get; set; }
        /// <summary>
        /// 卡券状态
        /// </summary>
        public string user_card_status { get; set; }
        /// <summary>
        /// 是否能消费
        /// </summary>
        public bool can_consume { get; set; }
    }

    /// <summary>
    /// 卡券ID。
    /// </summary>
    public class CardBaseInfo
    {
        public string card_id { get; set; }
        /// <summary>
        /// 起始使用时间
        /// </summary>
        public long? begin_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public long? end_time { get; set; }
    
    }

    /// <summary>
    /// 卡券信息
    /// </summary>
    public class CardManageGetInfoResult : CardBaseResult
    {
        public Card card { get; set; }

        public class Card
        {
            public string card_type { get; set; }
            public ReqCard.CardInfo groupon { get; set; }
            public ReqCard.CardInfo cash { get; set; }
            public ReqCard.CardInfo discount { get; set; }
            public ReqCard.CardInfo gift { get; set; }
            public ReqCard.CardInfo general_coupon { get; set; }

        }
    }

    /// <summary>
    /// 卡列表
    /// </summary>
    public class CardManageGetListResult : CardBaseResult
    {
        /// <summary>
        /// 卡券ID列表。
        /// </summary>
        public string[] card_id_list { get; set; }
        /// <summary>
        /// 该商户名下卡券ID总数。
        /// </summary>
        public int gtotal_numroupon { get; set; }
    }

    /// <summary>
    /// 修改卡券后返回信息
    /// </summary>
    public class CardManageUpdateResult : CardBaseResult
    {
        /// <summary>
        /// 是否提交审核，false为修改后不会重新提审，true为修改字段后重新提审，该卡券的状态变为审核中。
        /// </summary>
        public bool send_check { get; set; }
    }

    /// <summary>
    /// 拉取卡券概况数据接口
    /// </summary>
    public class CardDataCubeBInfoResult : CardBaseResult
    {
        /// <summary>
        /// 是否提交审核，false为修改后不会重新提审，true为修改字段后重新提审，该卡券的状态变为审核中。
        /// </summary>
        public List<InfoList> list { get; set; }

        public class InfoList
        {
            /// <summary>
            /// 日期信息
            /// </summary>
            public string ref_date { get; set; }
            /// <summary>
            /// card_id
            /// </summary>
            public string card_id { get; set; }
            /// <summary>
            /// 是否付费券。0为非付费券，1为付费券
            /// </summary>
            public int is_pay { get; set; }
            /// <summary>
            /// cardtype:0：折扣券，1：代金券，2：礼品券，3：优惠券，4：团购券（暂不支持拉取特殊票券类型数据，电影票、飞机票、会议门票、景区门票）
            /// </summary>
            public int card_type { get; set; }
            /// <summary>
            /// 浏览次数
            /// </summary>
            public int view_cnt { get; set; }
            /// <summary>
            /// 浏览人数
            /// </summary>
            public int view_user { get; set; }
            /// <summary>
            /// 领取次数
            /// </summary>
            public int receive_cnt { get; set; }
            /// <summary>
            /// 领取人数
            /// </summary>
            public int receive_user { get; set; }
            /// <summary>
            /// 使用次数
            /// </summary>
            public int verify_cnt { get; set; }
            /// <summary>
            /// 使用人数
            /// </summary>
            public int verify_user { get; set; }
            /// <summary>
            /// 转赠次数
            /// </summary>
            public int given_cnt { get; set; }
            /// <summary>
            /// 转赠人数
            /// </summary>
            public int given_user { get; set; }
            /// <summary>
            /// 过期次数
            /// </summary>
            public int expire_cnt { get; set; }
            /// <summary>
            /// 过期人数
            /// </summary>
            public int expire_user { get; set; }
            /// <summary>
            /// 激活人数
            /// </summary>
            public int active_user { get; set; }
            /// <summary>
            /// 有效会员总人数
            /// </summary>
            public int total_user { get; set; }
            /// <summary>
            /// 历史领取会员卡总人数
            /// </summary>
            public int total_receive_user { get; set; }
        }


    }

}
