using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 创建卡券
    /// </summary>
    public class ReqCard
    {
        /// <summary>
        /// ReqCard.CardType，券类型 
        /// </summary>
        public string card_type { get; set; }
        public CardInfo card_info { get; set; }

        public class CardInfo
        {
            /// <summary>
            /// 基本信息，必填，其他字段同一类别必填
            /// </summary>
            public BaseInfo base_info { get; set; }

            /// <summary>
            /// 团购券专用，团购详情。string(24)，
            /// </summary>
            public string deal_detail { get; set; }
            /// <summary>
            /// 代金券专用，表示起用金额。（单位为分）
            /// </summary>
            public int? least_cost { get; set; }
            /// <summary>
            /// 代金券专用，表示减免金额。（单位为分）
            /// </summary>
            public int? reduce_cost { get; set; }
            /// <summary>
            /// 折扣券专用，表示打折额度（百分比）。填30就是七折。
            /// </summary>
            public int? discount { get; set; }
            /// <summary>
            /// 礼品券专用，填写礼品的名称。string(3072)
            /// </summary>
            public string gift { get; set; }
            /// <summary>
            /// 优惠券专用，填写优惠详情。string(3072)
            /// </summary>
            public string default_detail { get; set; }
            /// <summary>
            /// 特殊票务券(会议门票)。会议详情。string(3072)
            /// </summary>
            public string meeting_detail { get; set; }
            /// <summary>
            /// 特殊票务券(景区门票)。票类型，例如平日全票，套票等。string(3072)
            /// </summary>
            public string ticket_class { get; set; }
            /// <summary>
            /// 特殊票务券(景区门票)。导览图url。string(128)
            /// </summary>
            /// 
            public string guide_url { get; set; }
            /// <summary>
            /// 会场导览图。
            /// </summary>
            public string map_url { get; set; }
            /// <summary>
            /// 特殊票务券(电影票)。电影票详情。string(3072)
            /// </summary>
            public string detail { get; set; }

            #region 飞机票
            /// <summary>
            /// 特殊票务券(飞机票)。起点，上限为18个汉字。string(54)
            /// </summary>
            public string from { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。终点，上限为18个汉字。string(54)
            /// </summary>
            public string to { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。航班。string(24)
            /// </summary>
            public string flight { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。入口，上限为4个汉字。string(12)
            /// </summary>
            public string gate { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。在线值机的链接。string(128)
            /// </summary>
            public string check_in_url { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。机型，上限为8个汉字。string(24)
            /// </summary>
            public string air_model { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。起飞时间。Unix时间戳格式。string(128)
            /// </summary>
            public string departure_time { get; set; }
            /// <summary>
            /// 特殊票务券(飞机票)。降落时间。Unix时间戳格式。string(128)
            /// </summary>
            public string landing_time { get; set; }

            /// <summary>
            /// string(3072)。自定义会员信息类目，会员卡激活后显示。
            /// </summary>
            public CustomField custom_field1 { get; set; }
            public CustomField custom_field2 { get; set; }
            public CustomField custom_field3 { get; set; }
            public CustomCell custom_cell1 { get; set; }

            public class CustomField
            {
                /// <summary>
                /// FIELD_NAME_TYPE_LEVEL等级；FIELD_NAME_TYPE_COUPON优惠券；FIELD_NAME_TYPE_STAMP印花；FIELD_NAME_TYPE_DISCOUNT折扣；FIELD_NAME_TYPE_ACHIEVEMEN成就；FIELD_NAME_TYPE_MILEAGE里
                /// </summary>
                public string name_type { get; set; }
                /// <summary>
                /// 点击类目跳转外链url
                /// </summary>
                public string url { get; set; }
            }
            public class CustomCell
            {
                /// <summary>
                /// 入口名称。
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 入口右侧提示语，6个汉字内。
                /// </summary>
                public string tips { get; set; }
                /// <summary>
                /// 入口跳转链接。
                /// </summary>
                public string url { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 基本属性
        /// </summary>
        public class BaseInfo
        {
            /// <summary>
            /// string(128)，卡券的商户logo，建议像素为300*300。http://mmbiz.qpic.cn/
            /// </summary>
            public string logo_url { get; set; }
            /// <summary>
            /// ReqCard.CodeType，Code展示类。
            /// </summary>
            public string code_type { get; set; }
            /// <summary>
            /// string（36）,商户名字,字数上限为12个汉字。
            /// </summary>
            public string brand_name { get; set; }
            /// <summary>
            /// string（27）,卡券名，字数上限为9个汉字。(建议涵盖卡券属性、服务及金额)。
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// string（54）,券名，字数上限为18个汉字。
            /// </summary>
            public string sub_title { get; set; }
            /// <summary>
            /// ReqCard.Color，券颜色。按色彩规范标注填写Color010-Color100。
            /// </summary>
            public string color { get; set; }
            /// <summary>
            /// string（48）,卡券使用提醒，字数上限为16个汉字。
            /// </summary>
            public string notice { get; set; }
            /// <summary>
            /// string（3072），卡券使用说明，字数上限为1024个汉字。
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// Json结构，商品信息。
            /// </summary>
            public Sku sku { get; set; }
            /// <summary>
            /// Json结构,使用日期，有效期的信息。
            /// </summary>
            public DateInfo date_info { get; set; }


            /// <summary>
            ///非必填，是否自定义Code码。填写true或false，默认为false。通常自有优惠码系统的开发者选择自定义Code码，并在卡券投放时带入Code码
            /// </summary>
            public bool? use_custom_code { get; set; }
            /// <summary>
            /// 非必填，是否指定用户领取，填写true或false。默认为false。通常指定特殊用户群体投放卡券或防止刷券时选择指定用户领取。
            /// </summary>
            public bool? bind_openid { get; set; }
            /// <summary>
            /// 非必填，string（24），客服电话。
            /// </summary>
            public string service_phone { get; set; }
            /// <summary>
            /// 非必填，门店位置poiid。调用POI门店管理接口获取门店位置poiid。具备线下门店的商户为必填。
            /// </summary>
            public int[] location_id_list { get; set; }
            /// <summary>
            /// 非必填，string（36）,第三方来源名，例如同程旅游、大众点评。
            /// </summary>
            public string source { get; set; }
            /// <summary>
            /// 非必填，string（15）,自定义跳转外链的入口名字。
            /// </summary>
            public string custom_url_name { get; set; }
            /// <summary>
            /// 非必填，string（128）,自定义跳转的URL。
            /// </summary>
            public string custom_url { get; set; }
            /// <summary>
            /// 非必填，string（18）,显示在入口右侧的提示语。
            /// </summary>
            public string custom_url_sub_title { get; set; }

            /// <summary>
            /// 非必填，string（15）,营销场景的自定义入口名称。
            /// </summary>
            public string promotion_url_name { get; set; }
            /// <summary>
            /// 非必填，string（128）,入口跳转外链的地址链接。
            /// </summary>
            public string promotion_url { get; set; }

            /// <summary>
            /// 非必填，string（18）,显示在营销入口右侧的提示语。
            /// </summary>
            public string promotion_url_sub_title { get; set; }
            /// <summary>
            /// 非必填，每人可领券的数量限制,不填写默认为50。
            /// </summary>
            public int? get_limit { get; set; }
            /// <summary>
            /// 非必填，卡券领取页面是否可分享。
            /// </summary>
            public bool? can_share { get; set; }
            /// <summary>
            /// 非必填，卡券是否可转赠。
            /// </summary>
            public bool? can_give_friend { get; set; }

            /// <summary>
            /// string(3072)。积分清零规则。
            /// </summary>
            public string bonus_cleared { get; set; }
            /// <summary>
            /// string(3072)。积分规则。
            /// </summary>
            public string bonus_rules { get; set; }
            /// <summary>
            /// string(3072)。储值说明。
            /// </summary>
            public string balance_rules { get; set; }
            /// <summary>
            /// string(3072)。特权说明。
            /// </summary>
            public string prerogative { get; set; }

            /// <summary>
            /// 电影票详情。
            /// </summary>
            //public string detail { get; set; }
            /// <summary>
            /// 起飞时间
            /// </summary>
            //public int? departure_time { get; set; }
            ///// <summary>
            ///// 降落时间。
            ///// </summary>
            //public int? landing_time { get; set; }
            ///// <summary>
            ///// 登机口。如发生登机口变更，建议商家实时调用该接口变更。
            ///// </summary>
            //public string gate { get; set; }
            /// <summary>
            /// 登机时间，只显示“时分”不显示日期，按Unix时间戳格式填写。如发生登机时间变更，建议商家实时调用该接口变更。
            /// </summary>
            public int? boarding_time { get; set; }



            public class CustomField
            {
                /// <summary>
                /// FIELD_NAME_TYPE_LEVEL等级；FIELD_NAME_TYPE_COUPON优惠券；FIELD_NAME_TYPE_STAMP印花；FIELD_NAME_TYPE_DISCOUNT折扣；FIELD_NAME_TYPE_ACHIEVEMEN成就；FIELD_NAME_TYPE_MILEAGE里
                /// </summary>
                public string name_type { get; set; }
                /// <summary>
                /// 点击类目跳转外链url
                /// </summary>
                public string url { get; set; }
            }
            public class CustomCell
            {
                /// <summary>
                /// 入口名称。
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 入口右侧提示语，6个汉字内。
                /// </summary>
                public string tips { get; set; }
                /// <summary>
                /// 入口跳转链接。
                /// </summary>
                public string url { get; set; }
            }

        }

        #region 属性中的类别
        public class CardType
        {
            public const string 团购券类型 = "GROUPON";
            public const string 代金券类型 = "CASH";
            public const string 折扣券类型 = "DISCOUNT";
            public const string 礼品券类型 = "GIFT";
            public const string 优惠券类型 = "GENERAL_COUPON";
            public const string 会议门票类型 = "MEETING_TICKET";
            public const string 景区门票类型 = "SCENIC_TICKET";
            public const string 电影票类型 = "MOVIE_TICKET";
            public const string 飞机票类型 = "BOARDING_PASS";
        }

        public class CodeType
        {
            public const string 文本 = "CODE_TYPE_TEXT";
            public const string 一维码 = "CODE_TYPE_BARCODE";
            public const string 二维码 = "CODE_TYPE_QRCODE";
            public const string 一维码无code显示 = "CODE_TYPE_ONLY_BARCODE";
            public const string 二维码无code显 = "CODE_TYPE_ONLY_QRCODE";
        }

        public class Color
        {
            public const string Color010 = "#55bd47";
            public const string Color020 = "#10ad61";
            public const string Color030 = "#35a4de";
            public const string Color040 = "#3d78da";
            public const string Color050 = "#9058cb";
            public const string Color060 = "#de9c33";
            public const string Color070 = "#ebac16";
            public const string Color080 = "#f9861f";
            public const string Color081 = "#f08500";
            public const string Color090 = "#e75735";
            public const string Color100 = "#d54036";
            public const string Color101 = "#cf3e36";
        }

        public class DateInfo
        {
            /// <summary>
            /// 使用时间的类型，DATE_TYPE_FIX_TIME_RANGE 表示固定日期区间，DATE_TYPE_FIX_TERM表示固定时长（自领取后按天算)
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 时间戳。type为DATE_TYPE_FIX_TIME_RANGE时专用，表示起用时间。从1970年1月1日00:00:00至起用时间的秒数，最终需转换为字符串形态传入。（东八区时间，单位为秒）
            /// </summary>
            public long? begin_timestamp { get; set; }
            /// <summary>
            /// 时间戳。type为DATE_TYPE_FIX_TIME_RANGE时专用，表示结束时间，建议设置为截止日期的23:59:59过期。（东八区时间，单位为秒）
            /// </summary>
            public long? end_timestamp { get; set; }
            /// <summary>
            /// type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天内有效，领取后当天有效填写0。（单位为天）
            /// </summary>
            public int? fixed_term { get; set; }
            /// <summary>
            /// type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天开始生效，领取后当天生效填写0。（单位为天）
            /// </summary>
            public int? fixed_begin_term { get; set; }
        }

        public class Sku
        {
            /// <summary>
            /// 卡券库存的数量，不支持填写0，上限为100000000。
            /// </summary>
            public int quantity { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 卡券投放（二维码）
    /// </summary>
    public class ReqCardPutQuickmark
    {
        /// <summary>
        /// 暂不知道是干嘛的，写死
        /// </summary>
        public string action_name = "QR_CARD";
        /// <summary>
        /// 指定二维码的有效时间，范围是60 ~ 1800秒。不填默认为永久有效。
        /// </summary>
        public int? expire_seconds { get; set; }

        public ActionInfo action_info { get; set; }

        public class ActionInfo
        {
            public Card card { get; set; }
        }

        public class Card
        {
            /// <summary>
            /// string(32)，必填，卡券ID。
            /// </summary>
            public string card_id { get; set; }
            /// <summary>
            /// string(20)，use_custom_code字段为true的卡券必须填写，非自定义code不必填写。
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// string(32)，指定领取者的openid，只有该用户能领取。bind_openid字段为true的卡券必须填写，非指定openid不必填写。
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// unsigned int,
            /// </summary>card
            public bool? is_unique_code { get; set; }
            /// <summary>
            /// 领取场景值，用于领取渠道的数据统计，默认值为0，字段类型为整型，长度限制为60位数字。用户领取卡券后触发的事件推送中会带上此自定义场景值。
            /// </summary>
            public int? outer_id { get; set; }
            /// <summary>
            ///  指定二维码的有效时间，范围是60 ~ 1800秒。不填默认为永久有效。（在调用投放二维码卡券接口时候此属性不需要设置。投放渠道数据统计需要）
            /// </summary>
            public int? expire_seconds { get; set; }
        }
    }

    /// <summary>
    /// 卡券投放（货架）
    /// </summary>
    public class ReqCardPutLandingPage
    {
        /// <summary>
        /// 页面的banner图片链接，须调用。
        /// </summary>
        public string banner { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string page_title { get; set; }
        /// <summary>
        /// 页面是否可以分享,填入true/false
        /// </summary>
        public bool can_share { get; set; }
        /// <summary>
        /// 页面是否可以分享,填入true/false，(SceneType取值)
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// 页面是否可以分享,填入true/false
        /// </summary>
        public List<CardList> card_list { get; set; }

        public class CardList
        {
            public string card_id { get; set; }
            /// <summary>
            /// 缩略图url
            /// </summary>
            public string thumb_url { get; set; }
        }

        public class SceneType
        {
            public const string 附近 = "SCENE_NEAR_BY";
            public const string 自定义菜单 = "SCENE_MENU";
            public const string 二维码 = "SCENE_QRCODE";
            public const string 公众号文章 = "SCENE_ARTICLE";
            public const string h5页面 = "SCENE_H5";
            public const string 自动回复 = "SCENE_IVR";
            public const string 卡券自定义cell = "SCENE_CARD_CUSTOM_CELL";
        }
    }

    /// <summary>
    /// 会议门票
    /// </summary>
    public class ReqCardMeeting
    {
        /// <summary>
        /// 卡券Code码。
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 要更新门票序列号所述的card_id，生成券时use_custom_code 填写true 时必填。
        /// </summary>
        public string card_id { get; set; }
        /// <summary>
        /// 场时间，Unix时间戳格式。
        /// </summary>
        public long? begin_time { get; set; }
        /// <summary>
        /// 结束时间，Unix时间戳格式。
        /// </summary>
        public long? end_time { get; set; }
        /// <summary>
        /// 区域。
        /// </summary>
        public string zone { get; set; }
        /// <summary>
        /// 入口。
        /// </summary>
        public string entrance { get; set; }
        /// <summary>
        /// 座位号。
        /// </summary>
        public string seat_number { get; set; }
    }

    /// <summary>
    /// 电影门票
    /// </summary>
    public class ReqCardMovie
    {
        /// <summary>
        /// 卡券Code码。
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 要更新门票序列号所述的card_id，生成券时use_custom_code 填写true 时必填。
        /// </summary>
        public string card_id { get; set; }
        /// <summary>
        /// 电影票的类别，如2D、3D。
        /// </summary>
        public string ticket_class { get; set; }
        /// <summary>
        /// string（12）。该场电影的影厅信息。
        /// </summary>
        public string screening_room { get; set; }
        /// <summary>
        /// 座位号。
        /// </summary>
        public string[] seat_number { get; set; }
        /// <summary>
        /// 电影的放映时间，Unix时间戳格式。
        /// </summary>
        public long show_time { get; set; }
        /// <summary>
        /// 放映时长，填写整数。
        /// </summary>
        public int duration { get; set; }
    }

    /// <summary>
    /// 飞机票
    /// </summary>
    public class ReqCardBoard
    {
        /// <summary>
        /// 卡券Code码。
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 要更新门票序列号所述的card_id，生成券时use_custom_code 填写true 时必填。
        /// </summary>
        public string card_id { get; set; }
        /// <summary>
        /// 电子客票号，上限为14个数字。string（14）
        /// </summary>
        public string etkt_bnr { get; set; }
        /// <summary>
        /// 二维码数据。乘客用于值机的二维码字符串，微信会通过此数据为用户生成值机用的二维码。string（3072）
        /// </summary>
        public string qrcode_data { get; set; }
        /// <summary>
        /// 乘客座位号。string（12）	
        /// </summary>
        public string seat { get; set; }
        /// <summary>
        /// 是否取消值机。填写true或false。true代表取消，如填写true上述字段（如calss等）均不做判断，机票返回未值机状态，乘客可重新值机。默认填写false。
        /// </summary>
        public bool? is_cancel { get; set; }
        /// <summary>
        /// 舱等，如头等舱等，上限为5个汉字。
        /// </summary>
        public string className { get; set; }
    }
}
