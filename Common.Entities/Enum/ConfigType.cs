using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Common.Core.Attributes;

namespace Common.Entities.Enum
{
    public enum ConfigType
    {
        /// <summary>
        /// 停车场缴费成功通知
        /// </summary>
        /// </summary>
        [Description("停车场缴费成功通知模板编号")]
        [EnumDefaultValue("{{first.DATA}}<br>车牌号码：{{keynote1.DATA}}<br>停靠位置：{{keynote2.DATA}}<br>停车时长：{{keynote3.DATA}}<br>缴费金额：{{keynote4.DATA}}<br>支付时间：{{keynote5.DATA}}<br>{{remark.DATA}}")]
        ParkCarPaymentSuccessTemplateId = 0,
        ///// <summary>
        ///// 充值成功通知
        ///// </summary>
        ///// </summary>
        //[Description("充值成功通知模板编号")]
        //[EnumDefaultValue("{{first.DATA}}<br>充值金额：{{keyword1.DATA}}<br>当前余额：{{keyword2.DATA}}<br>交易单号：{{keyword3.DATA}}<br>充值时间：{{keyword4.DATA}}<br>{{remark.DATA}}")]
        //AccountRechargeSuccessTemplateId = 1,
        /// <summary>
        /// 包月续费成功提醒
        /// </summary>
        /// </summary>
        [Description("包月续费成功提醒模板编号")]
        [EnumDefaultValue("{{first.DATA}}<br>车牌号：{{keyword1.DATA}}<br>停车场：{{keyword2.DATA}}<br>续费金额：{{keyword3.DATA}}<br>实际支付：{{keyword4.DATA}}<br>包月截止：{{keyword5.DATA}}<br>{{remark.DATA}}")]
        MonthCardRechargeSuccessTemplateId = 2,
        ///// <summary>
        ///// 充值成功通知
        ///// </summary>
        ///// </summary>
        //[Description("充值成功通知模板编号")]
        //[EnumDefaultValue("{{first.DATA}}<br>充值金额：{{keyword1.DATA}}<br>当前余额：{{keyword2.DATA}}<br>交易单号：{{keyword3.DATA}}<br>充值时间：{{keyword4.DATA}}<br>{{remark.DATA}}")]
        //GiftCardRechargeSuccessTemplateId = 3,
        /// <summary>
        /// 退款成功通知
        /// </summary>
        /// </summary>
        [Description("退款成功通知模板编号")]
        [EnumDefaultValue("{{first.DATA}}<br>订单号：{{keyword1.DATA}}<br>退款原因：{{keyword2.DATA}}<br>退款金额：{{keyword3.DATA}}<br>{{remark.DATA}}")]
        ParkingRefundSuccessTemplateId = 4,
        /// <summary>
        /// 退款失败提醒
        /// </summary>
        /// </summary>
        [Description("退款失败提醒模板编号")]
        [EnumDefaultValue("{{first.DATA}}<br>订单号：{{keyword1.DATA}}<br>金额：{{keyword2.DATA}}<br>原因：{{keyword3.DATA}}<br>{{remark.DATA}}")]
        ParkingRefundFailTemplateId = 5,
        ///// <summary>
        ///// 挪车提醒
        ///// </summary>
        ///// </summary>
        //[Description("挪车提醒模板编号")]
        //[EnumDefaultValue("{{first.DATA}}<br>请求：{{keyword1.DATA}}<br>时间：{{keyword2.DATA}}<br>{{remark.DATA}}")]
        //MoveCarMessageTemplateId = 6,
        ///// <summary>
        ///// 访客授权通知
        ///// </summary>
        ///// </summary>
        //[Description("访客授权通知模板编号")]
        //[EnumDefaultValue("{{first.DATA}}<br>授权小区：{{keyword1.DATA}}<br>授权车场：{{keyword2.DATA}}<br>授权门禁：{{keyword3.DATA}}<br>进出次数：{{keyword4.DATA}}<br>有效时间：{{keyword5.DATA}}<br>{{remark.DATA}}")]
        //VisitorsTemplateId = 7,
        ///// <summary>
        /////挪车结果通知
        ///// </summary>
        ///// </summary>
        //[Description("挪车结果通知模板编号")]
        //[EnumDefaultValue("{{first.DATA}}<br>挪车车牌：{{keyword1.DATA}}<br>挪车时间：{{keyword2.DATA}}<br>申请结果：{{keyword3.DATA}}<br>{{remark.DATA}}")]
        //MoveCarResultTemplateId = 8,
        /// <summary>
        /// 引导微信用户关注链接地址
        /// </summary>
        /// </summary>
        [Description("引导微信用户关注链接地址")]
        [EnumDefaultValue("http://mp.weixin.qq.com/s?__biz=MzI0MTM0MHzM1MA==&mid=100000000002&idx=1&sn=d90e93eff789f5210d2340dd12e42447c8&scene=0&previewkey=VjfL77X1xuGmyyRn8QCPBMNS9bJajjJKzz%2F0By7ITJA33%3D#wechat_redirect")]
        PromptAttentionPage = 9,
         /// <summary>
        /// 临停缴费微信支付超时时间
        /// </summary>
        /// </summary>
        [Description("临停缴费微信支付超时时间")]
        [EnumDefaultValue("5")]
        TempParkingWeiXinPayTimeOut=10,
        /// <summary>
        /// 聚合短信APPKEY
        /// </summary>
        /// </summary>
        [Description("聚合短信APPKEY")]
        [EnumDefaultValue("https://www.juhe.cn/")]
        JuHeAppKey = 11,
        /// <summary>
        /// 聚合短信验证模板编号
        /// </summary>
        /// </summary>
        [Description("聚合短信验证码模板编号")]
        [EnumDefaultValue("")]
        JuHeSmsTemplateId= 12,
        /// <summary>
        /// 商家充值成功提醒
        /// </summary>
        /// </summary>
        [Description("商家充值成功提醒")]
        [EnumDefaultValue("{{first.DATA}}<br>时间：{{keyword1.DATA}}<br>充值金额：{{keyword2.DATA}}<br>账户余额：{{keyword3.DATA}}<br>{{remark.DATA}}")]
        SellerRechargeTemplateId = 13,

        /// <summary>
        /// 入场提醒
        /// </summary>
        /// </summary>
        [Description("入场提醒")]
        [EnumDefaultValue("{{first.DATA}}<br>车牌号：{{keynote1.DATA}}<br>停车场：{{keynote2.DATA}}<br>入场时间：{{keynote3.DATA}}<br>{{remark.DATA}}")]
        ParkInTemplateId = 14,

        /// <summary>
        /// 离场提醒
        /// </summary>
        /// </summary>
        [Description("离场提醒")]
        [EnumDefaultValue("{{first.DATA}}<br>入场时间：{{keynote1.DATA}}<br>离场时间：{{keynote2.DATA}}<br>停车时长：{{keynote3.DATA}}<br>停车费用：{{keynote4.DATA}}<br>{{remark.DATA}}")]
        ParkOutTemplateId = 15,
    }
}
