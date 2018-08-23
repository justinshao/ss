using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Services.WeiXin;
using Common.Entities.Enum;
using SmartSystem.WeiXinBase;
using Common.Entities.WX;
using Common.Services;
using Common.Entities;

namespace SmartSystem.WeiXinServices
{
    public class TemplateMessageServices
    {
        /// <summary>
        /// 发送停车场入场通知
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="parkingName">停车场名称</param>
        /// <param name="entranceTime">进场时间</param>
        /// <param name="openId">接受消息的openid</param>
        public static bool SendParkIn(string companyId, string plateNumber, string parkingName, string entranceTime, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.ParkInTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string topColor = "#FF0000";
                string remark = "谢谢您的支持！";

                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = "欢迎您再次进入停车场\r\n", color = "#173177" },
                    keyword1 = new { value = plateNumber, color = "#173177" },
                    keyword2 = new { value = parkingName, color = "#173177" },
                    keyword3 = new { value = entranceTime.ToString(), color = "#173177" },
                    remark = new { value = remark, color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送停车场入场通知失败", ex, LogFrom.WeiXin);
                return false;
            }

        }

        /// <summary>
        /// 发送停车场入场通知
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="parkingName">停车场名称</param>
        /// <param name="entranceTime">进场时间</param>
        /// <param name="openId">接受消息的openid</param>
        public static bool SendParkOut(string companyId, string plateNumber, string parkingName, string entranceTime, string exitTime, string duringTime, string sPayType, string amount, string openId, bool isApp)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.ParkOutTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string topColor = "#FF0000";
                string remark = "";
                if (isApp)
                {
                    remark = "感谢使用SPS智慧停车，无感支付，优感生活！祝您一路顺风~";
                }
                else
                {
                    remark = "您已成功提前支付成功,15分钟内免费自动抬杆离场，超过15分钟需补缴相应停车费用，具体费用见停车场收费标准！感谢使用SPS智慧停车，无感支付，优感生活！祝您一路顺风~";
                }
                string first = "尊敬的SPS车主：\r\n您的车辆" + plateNumber + "已驶出" + parkingName;
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = first, color = "#173177" },
                    keyword1 = new { value = entranceTime, color = "#173177" },
                    keyword2 = new { value = exitTime, color = "#173177" },
                    keyword3 = new { value = duringTime.ToString(), color = "#173177" },
                    keyword4 = new { value = sPayType.ToString(), color = "#173177" },
                    keyword5 = new { value = amount.ToString(), color = "#173177" },
                    remark = new { value = remark, color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送停车场出场通知失败", ex, LogFrom.WeiXin);
                return false;
            }

        }


        /// <summary>
        /// 发送停车场缴费成功通知
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="parkingName">停车场名称</param>
        /// <param name="money">支付金额（元）</param>
        /// <param name="entranceTime">进场时间</param>
        /// <param name="payTime">支付时间</param>
        /// <param name="lastExitTime">最后出场时间</param>
        /// <param name="openId">接受消息的openid</param>
        public static bool SendParkCarPaymentSuccess(string companyId,string plateNumber, string parkingName, decimal money, DateTime entranceTime, DateTime payTime, DateTime realPayTime, DateTime lastExitTime, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.ParkCarPaymentSuccessTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string topColor = "#FF0000";
                string payTimeDes = string.Format("{0}月{1}日 {2}时{3}分", realPayTime.Month.ToString().PadLeft(2, '0'), realPayTime.Day.ToString().PadLeft(2, '0'), realPayTime.Hour.ToString().PadLeft(2, '0'), realPayTime.Minute.ToString().PadLeft(2, '0'));

                int minute = (int)(lastExitTime - payTime).TotalMinutes;

                string remark = string.Format("请您于支付时间后{0}分钟内开车驶离停车场，否则还需再进行超时部分的停车费补缴。", minute);
                if (minute < 1)
                {
                    remark = "请您马上驶离停车场，否则还需再进行超时部分的停车费补缴。";
                }
                int totalTime = (int)(payTime - entranceTime).TotalMinutes;
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = "您好，您已缴费成功！", color = "#173177" },
                    keynote1 = new { value = plateNumber, color = "#173177" },
                    keynote2 = new { value = parkingName, color = "#173177" },
                    keynote3 = new { value = totalTime.GetParkingDuration(), color = "#173177" },
                    keynote4 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keynote5 = new { value = payTimeDes, color = "#173177" },
                    remark = new { value = remark, color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送停车场缴费成功通知失败", ex,LogFrom.WeiXin);
                return false;
            }

        }
        /// <summary>
        /// 发送商家充值成功通知
        /// </summary>
        /// <param name="companyId">单位编号</param>
        /// <param name="money">充值金额（元）</param>
        /// <param name="balance">账号预额（元）</param>
        /// <param name="realPayTime">支付时间</param>
        /// <param name="openId">接受消息的openid</param>
        public static bool SendSellerRechargeSuccess(string companyId, decimal money, decimal balance, DateTime realPayTime, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.SellerRechargeTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string topColor = "#FF0000";
                string payTimeDes = string.Format("{0}月{1}日 {2}时{3}分", realPayTime.Month.ToString().PadLeft(2, '0'), realPayTime.Day.ToString().PadLeft(2, '0'), realPayTime.Hour.ToString().PadLeft(2, '0'), realPayTime.Minute.ToString().PadLeft(2, '0'));

                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = "您好，您已充值成功！", color = "#173177" },
                    keyword1 = new { value = payTimeDes, color = "#173177" },
                    keyword2 = new { value = money, color = "#173177" },
                    keyword3 = new { value = balance, color = "#173177" },
                    remark = new { value = "您的充值已成功，可在充值记录查看明细", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送停车场缴费成功通知失败", ex, LogFrom.WeiXin);
                return false;
            }

        }
        private static WX_ApiConfig GetWX_ApiConfig(string companyId) {
            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
         if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)) {
             throw new MyException("获取微信基本配置信息失败");
         }
         return config;
        }
        /// <summary>
        /// 账号充值成功提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="money">充值金额（元）</param>
        /// <param name="openId">接收者编号</param>
        /// <param name="balance">账号余额</param>
        /// <param name="payTime">充值时间</param>
        /// <returns></returns>
        //public static bool SendAccountRechargeSuccess(string orderId, decimal money, decimal balance, string openId, DateTime payTime)
        //{
        //    try
        //    {
        //        string value = WXOtherConfigServices.GetConfigValue(ConfigType.AccountRechargeSuccessTemplateId);
        //        if (string.IsNullOrWhiteSpace(value)) return false;

        //        string payTimeDes = string.Format("{0}月{1}日 {2}时{3}分", payTime.Month.ToString().PadLeft(2, '0'), payTime.Day.ToString().PadLeft(2, '0'), payTime.Hour.ToString().PadLeft(2, '0'), payTime.Minute.ToString().PadLeft(2, '0'));
        //        string topColor = "#FF0000";
        //        WX_ApiConfig config = GetWX_ApiConfig();
        //        var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
        //        var data = new
        //        {
        //            first = new { value = "您好，您的账号已充值成功！", color = "#173177" },
        //            keyword1 = new { value = string.Format("{0}元", money), color = "#173177" },
        //            keyword2 = new { value = string.Format("{0}元", balance), color = "#173177" },
        //            keyword3 = new { value = orderId, color = "#173177" },
        //            keyword4 = new { value = payTimeDes, color = "#173177" },
        //            remark = new { value = "如有疑问，请尽快联系我们", color = "#173177" }
        //        };
        //        return WxAdvApi.SendTemplateMessage(accessToken, openId, value, topColor, data);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送账号充值成功提醒失败", ex, LogFrom.WeiXin);
        //        return false;
        //    }

        //}
        /// <summary>
        /// 月卡充值成功提醒
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="parkingName">停车场名称</param>
        /// <param name="money">支付金额（分）</param>
        /// <param name="lastEffectiveTime">最后有效期</param>
        /// <param name="openId">微信openid</param>
        /// <returns></returns>
        public static bool SendMonthCardRechargeSuccess(string companyId, string plateNumber, string parkingName, decimal money, DateTime lastEffectiveTime, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.MonthCardRechargeSuccessTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string topColor = "#FF0000";

                string effectiveTime = string.Format("{0}年{1}月{2}日", lastEffectiveTime.Year, lastEffectiveTime.Month, lastEffectiveTime.Day);
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = "您好，您的月卡已续期成功！", color = "#173177" },
                    keyword1 = new { value = plateNumber, color = "#173177" },
                    keyword2 = new { value = parkingName, color = "#173177" },
                    keyword3 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keyword4 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keyword5 = new { value = effectiveTime, color = "#173177" },
                    remark = new { value = "感谢您的使用。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送月卡充值成功提醒失败", ex, LogFrom.WeiXin);
                return false;
            }

        }
        ///// <summary>
        ///// 储值卡充值成功提醒
        ///// </summary>
        ///// <param name="orderId">订单编号</param>
        ///// <param name="money">充值金额（分）</param>
        ///// <param name="openId">接收者编号</param>
        ///// <param name="balance">账号余额</param>
        ///// <param name="payTime">充值时间</param>
        ///// <returns></returns>
        //public static bool SendGiftCardRechargeSuccess(string orderId, decimal money, decimal balance, string openId, DateTime payTime)
        //{
        //    try
        //    {
        //        string value = WXOtherConfigServices.GetConfigValue(ConfigType.GiftCardRechargeSuccessTemplateId);
        //        if (string.IsNullOrWhiteSpace(value)) return false;

        //        string payTimeDes = string.Format("{0}月{1}日 {2}时{3}分", payTime.Month.ToString().PadLeft(2, '0'), payTime.Day.ToString().PadLeft(2, '0'), payTime.Hour.ToString().PadLeft(2, '0'), payTime.Minute.ToString().PadLeft(2, '0'));
        //        string topColor = "#FF0000";
        //        WX_ApiConfig config = GetWX_ApiConfig();
        //        var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
        //        var data = new
        //        {
        //            first = new { value = "您好，您的储值卡充值成功！", color = "#173177" },
        //            keyword1 = new { value = string.Format("{0}元", money), color = "#173177" },
        //            keyword2 = new { value = string.Format("{0}元", balance), color = "#173177" },
        //            keyword3 = new { value = orderId, color = "#173177" },
        //            keyword4 = new { value = payTimeDes, color = "#173177" },
        //            remark = new { value = "如有疑问，请尽快联系我们", color = "#173177" }
        //        };
        //        return WxAdvApi.SendTemplateMessage(accessToken, openId, value, topColor, data);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送储值卡充值成功提醒失败", ex, LogFrom.WeiXin);
        //        return false;
        //    }
        //}
        /// <summary>
        /// 发送停车支付同步支付结果失败的 退款成功提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendParkingRefundSuccess(string companyId,string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.ParkingRefundSuccessTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string firstDes = "您好，缴停车费失败了,我们将您支付的钱返还到您的账号了，请查收。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = reason, color = "#173177" },
                    keyword3 = new { value = string.Format("{0}元", money), color = "#173177" },
                    remark = new { value = "如有疑问，请尽快联系我们。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId,accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送退款成功提醒失败", ex, LogFrom.WeiXin);
                return false;
            }
        }
        /// <summary>
        /// 发送预约车位支付同步支付结果失败的 退款成功提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendBookingBitNoRefundSuccess(string companyId,string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.ParkingRefundSuccessTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string firstDes = "您好，由于车场网络原因，您预约车位失败了,我们将您支付的钱返还到您的账号了，请查收。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = reason, color = "#173177" },
                    keyword3 = new { value = string.Format("{0}元", money), color = "#173177" },
                    remark = new { value = "如有疑问，请尽快联系我们。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId,accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送车位预订退款成功提醒失败", ex, LogFrom.WeiXin);
                return false;
            }
        }
        /// <summary>
        /// 商家充值失败退款成功提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendSellerRechargeRefundSuccess(string companyId, string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.ParkingRefundSuccessTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;

                string firstDes = "您好，由于车场网络原因，您商家充值失败了,我们将您支付的钱返还到您的账号了，请查收。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = reason, color = "#173177" },
                    keyword3 = new { value = string.Format("{0}元", money), color = "#173177" },
                    remark = new { value = "如有疑问，请尽快联系我们。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "商家充值失败退款成功提醒", ex, LogFrom.WeiXin);
                return false;
            }
        }
        /// <summary>
        /// 发送停车支付同步支付结果失败的 退款失败提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendParkingRefundFail(string companyId,string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.ParkingRefundFailTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;


                string firstDes = "您好，缴停车费失败了,同时退款失败了。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keyword3 = new { value = reason, color = "#173177" },
                    remark = new { value = "请尽快联系我们，我们将进行人工退款。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId,accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送退款失败通知失败", ex, LogFrom.WeiXin);
                return false;
            }
        }
        /// <summary>
        /// 发送停车支付同步支付结果失败的 退款失败提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendBookingBitNoRefundFail(string companyId,string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId,ConfigType.ParkingRefundFailTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;


                string firstDes = "您好，预约车位失败了,同时退款失败了。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keyword3 = new { value = reason, color = "#173177" },
                    remark = new { value = "请尽快联系我们，我们将进行人工退款。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId,accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送车位预订退款失败通知失败", ex, LogFrom.WeiXin);
                return false;
            }
        }
        /// <summary>
        /// 发送商家充值失败 退款失败提醒
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="reason">原因</param>
        /// <param name="money">金额</param>
        /// <param name="openId">接受者openid</param>
        /// <returns></returns>
        public static bool SendSellerRechargeRefundFail(string companyId, string orderId, string reason, decimal money, string openId)
        {
            try
            {
                string value = WXOtherConfigServices.GetConfigValue(companyId, ConfigType.ParkingRefundFailTemplateId);
                if (string.IsNullOrWhiteSpace(value)) return false;


                string firstDes = "您好，商家充值失败了,同时退款失败了。";
                string topColor = "#FF0000";
                WX_ApiConfig config = GetWX_ApiConfig(companyId);
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
                var data = new
                {
                    first = new { value = firstDes, color = "#173177" },
                    keyword1 = new { value = orderId, color = "#173177" },
                    keyword2 = new { value = string.Format("{0}元", money), color = "#173177" },
                    keyword3 = new { value = reason, color = "#173177" },
                    remark = new { value = "请尽快联系我们，我们将进行人工退款。", color = "#173177" }
                };
                return WxAdvApi.SendTemplateMessage(companyId, accessToken, openId, value, topColor, data);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("SendTemplateMessage", "发送发送商家充值失败通知失败", ex, LogFrom.WeiXin);
                return false;
            }
        }
    }
}
