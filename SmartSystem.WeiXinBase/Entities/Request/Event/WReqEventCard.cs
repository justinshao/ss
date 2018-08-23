using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    public class WReqEventCard : WReqEventBase
    {
        public override EventType Event
        {
            get
            {
                return EventType.Card;
            }
        }
        /// <summary>
        /// 卡券编号
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 赠送方账号（一个OpenID），"IsGiveByFriend”为1时填写该参数。
        /// </summary>
        public string FriendUserName { get; set; }
        /// <summary>
        /// 是否为转赠，1代表是，0代表否。
        /// </summary>
        public int IsGiveByFriend { get; set; }
        /// <summary>
        /// code序列号。自定义code及非自定义code的卡券被领取后都支持事件推送。
        /// </summary>
        public string UserCardCode { get; set; }
        /// <summary>
        /// 转赠前的code序列号。
        /// </summary> 
        public string OldUserCardCode { get; set; }
        /// <summary>
        /// 领取场景值，用于领取渠道数据统计。可在生成二维码接口及添加JS API接口中自定义该字段的整型值。
        /// </summary>
        public int OuterId { get; set; }
        /// <summary>
        /// 核销来源。支持开发者统计API核销（FROM_API）、公众平台核销（FROM_MP）、卡券商户助手核销（FROM_MOBILE_HELPER）（核销员微信号）
        /// </summary>
        public string ConsumeSource { get; set; }
    }
}
