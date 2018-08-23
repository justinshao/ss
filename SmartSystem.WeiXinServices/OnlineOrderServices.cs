using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Entities.Order;
using Common.Factory.Order;
using Common.IRepository.Order;
using Common.Entities.Enum;
using Common.Services;
using Common.DataAccess;
using Common.Entities;
using SmartSystem.WeiXinInerface;
using Common.Utilities;
using Common.Entities.Condition;
using SmartSystem.WeiXinServices.Payment;
using SmartSystem.AliPayServices;
using Common.Factory.Park;
using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.Services.Park;
using Common.ExternalInteractions.BWY;
using Common.ExternalInteractions.SFM;
using Common.ExternalInteractions.APP;

namespace SmartSystem.WeiXinServices
{

    public class OnlineOrderServices
    {
        private static object order_lock = new object();

        public static bool Create(OnlineOrder model) {
            if (model == null) throw new ArgumentNullException("model");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.Create(model);
        }

        public static OnlineOrder QueryByOrderId(decimal orderId)
        {
            if (orderId <= 0) throw new ArgumentNullException("orderId");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.QueryByOrderId(orderId);
        }

        public static bool UpdatePrepayIdById(string prepayId, decimal orderId) {
            if (string.IsNullOrWhiteSpace(prepayId)) throw new ArgumentNullException("prepayId");
            if (orderId <= 0) throw new ArgumentNullException("orderId");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.UpdatePrepayIdById(prepayId,orderId);
        }
        public static bool UpdatePrepayIdById(string prepayId, string mWebUrl, decimal orderId)
        {
            if (string.IsNullOrWhiteSpace(prepayId)) throw new ArgumentNullException("prepayId");
            if (string.IsNullOrWhiteSpace(mWebUrl)) throw new ArgumentNullException("mWebUrl");
            if (orderId <= 0) throw new ArgumentNullException("orderId");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.UpdatePrepayIdById(prepayId,mWebUrl,orderId);
        }
        public static bool UpdatePayAccount(decimal orderId, string payAccount) {
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.UpdatePayAccount(orderId, payAccount);
        }
        public static bool UpdateSFMCode(OnlineOrder order)
        {
            if (order.OrderSource == PayOrderSource.SFM)
            {
                SFMResult result = SFMInterfaceProcess.CarOrderPay(order.PayDetailID);
                if (result == null) throw new MyException("提交支付失败【SFM】");
                if (!result.Success) throw new MyException("下单失败【SFM】");
                if (string.IsNullOrWhiteSpace(result.Code)) throw new MyException("交易订单号失败【SFM】");

                IOnlineOrder factory = OnlineOrderFactory.GetFactory();
                bool updateResult = factory.UpdateSFMCode(result.Code, order.OrderID);
                if (!updateResult) throw new MyException("修改外部订单编号失败【SFM】");
                return updateResult;
            }
            return true;
        }
        public static bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime,string payAccount="") {
            bool result = false;
            lock (order_lock)
            {
                try
                {
                    IOnlineOrder factory = OnlineOrderFactory.GetFactory();

                    OnlineOrder order = factory.QueryByOrderId(orderId);
                    if (order == null) throw new MyException("订单编号不存在");

                    if (order.Status != OnlineOrderStatus.WaitPay && order.Status != OnlineOrderStatus.Paying)
                    {
                        string message = string.Format("【{0}】,订单编号：{1}，状态：{2}", order.PaymentChannel.GetDescription(), order.OrderID, order.Status.GetDescription());
                        TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "【更改订单状态为已支付失败】订单不是有效状态,备注：{0}，支付账号：{1}", message, order.PayAccount);
                        return false;
                    }
                
                    if (!string.IsNullOrWhiteSpace(payAccount))
                    {
                        result = factory.PaySuccess(order.OrderID, serialNumber, payAccount, payTime);
                    }
                    else
                    {
                        result = factory.PaySuccess(order.OrderID, serialNumber, payTime);
                    }

                    if (result)
                    {
                        order.Status = OnlineOrderStatus.PaySuccess;
                        order.SerialNumber = serialNumber;
                        order.RealPayTime = payTime;
                        TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3}", "PaySuccess", "更改订单状态为已支付", orderId, "更改订单状态为已支付成功");

                        bool noticeResult = NoticePaymentResult(order);
                        string remark = noticeResult ? "通知支付结果成功" : "通知支付结果失败";
                        TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3}", "PaySuccess", "更改订单状态为已支付", orderId, remark);
                    }
                    else
                    {
                        TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "【更改订单状态为已支付失败】【{0}】,订单编号：{1}，状态：{2}", order.PaymentChannel.GetDescription(), order.OrderID, order.Status.GetDescription());
                    }
                }
                catch (Exception ex) {
                    TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "【更改订单状态为已支付失败】订单编号：{0}，Message：{1},StackTrace:{2}", orderId, ex.Message, ex.StackTrace);
                }
                return result;
            }
        }
        private static bool NoticePaymentResult(OnlineOrder order)
        {

            if (order.Status != OnlineOrderStatus.PaySuccess)
            {
                TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "【通知支付结果】订单不是已支付状态,订单编号：{0},支付账号：{1}", order.OrderID, order.PayAccount);
                return false;
            }

            bool noticePayResult = false;
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
             using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.SyncPayResultSuccess(order.OrderID, dbOperator);
                    if (!result) throw new MyException("更改同步成功状态失败");

                    string payDetailId = string.Empty;
                    noticePayResult = SyncNoticePayResult(order.OrderID, order.RealPayTime, order, out payDetailId);
                    if (!noticePayResult) throw new MyException("同步支付结果失败");
                    dbOperator.CommitTransaction();

                    factory.UpdateSyncResultTimes(order.OrderID, payDetailId);
                    SendSyncResultMessage(order);
                    return true;
                }
                catch (MyException ex)
                {
                    dbOperator.RollbackTransaction();
                    factory.SyncPayResultFail(order.OrderID, ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    dbOperator.RollbackTransaction();
                    factory.SyncPayResultFail(order.OrderID, "同步支付结果异常");
                    TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "【{0}】支付账号{3},订单编号：{1}，描述：{2}", order.PaymentChannel.GetDescription(), order.OrderID, ex.Message, order.PayAccount);
                    return false;
                }

            }
        }
        /// <summary>
        /// 同步支付结果
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="payTime"></param>
        private static bool SyncNoticePayResult(decimal orderId, DateTime payTime, OnlineOrder order, out string payDetailId)
        {
            payDetailId = string.Empty;
            int payWay = GetServicePayWay(order.PaymentChannel);
            switch (order.OrderType)
            {
                case OnlineOrderType.MonthCardRecharge:
                    {

                        MonthlyRenewalResult renewalsResult = RechargeService.WXMonthlyRenewals(order.CardId, order.PKID, order.MonthNum, order.Amount, order.AccountID, payWay,OrderSource.WeiXin,order.OrderID.ToString(), payTime);
                        if (renewalsResult.Result != APPResult.Normal)
                        {
                            throw new MyException(SyncResultDescription(renewalsResult.Result));
                        }
                        payDetailId = renewalsResult.Pkorder != null ? renewalsResult.Pkorder.OnlineOrderNo.ToString() : string.Empty;
                        return true;
                    }

                case OnlineOrderType.ParkFee:
                    {
                        if (order.OrderSource == PayOrderSource.BWY)
                        {
                            BWYOrderPaymentResult result = BWYInterfaceProcess.PayNotice((int)(order.Amount * 100), order.ExternalPKID, order.PayDetailID.ToString());
                            if (result.Result != 0)
                            {
                                throw new MyException(result.Desc);
                            }
                        }
                        else if (order.OrderSource == PayOrderSource.SFM)
                        {
                            bool isPayScene = !string.IsNullOrWhiteSpace(order.TagID);
                            TxtLogServices.WriteTxtLogEx("SFMError",string.Format("isPayScene:{0}",isPayScene));
                            SFMResult result = SFMInterfaceProcess.PayNotify(order.InOutID, order.SerialNumber, order.Amount.ToString(), isPayScene);
                            if (result == null) throw new MyException("请求赛菲姆支付通知失败");
                            if (!result.Success) throw new MyException(string.Format("{0}【{1}】", result.Message, result.Code));
                        }
                        else
                        {
                            TempStopPaymentResult result = RechargeService.WXTempStopPayment(order.PayDetailID, payWay, order.Amount, order.PKID, order.AccountID, order.OrderID.ToString(), payTime);
                            if (result.Result != APPResult.Normal)
                            {
                                throw new MyException(SyncResultDescription(result.Result));
                            }
                        }
                        return true;
                    }
                case OnlineOrderType.PkBitBooking:
                    {
                       return PkBitBookingServices.WXReserveBitPay(order.CardId, order.PayDetailID, order.Amount, order.PKID, order.OrderID.ToString());
                     }
                case OnlineOrderType.SellerRecharge:
                    {
                        return SyncSellerRecharge(order, (OrderPayWay)payWay);
                    }
                case OnlineOrderType.APPRecharge:
                    {
                        return 1 == AppBalanceNotify.BalanceRechargeNotify(order.OrderID.ToString(),order.PlateNo, order.Amount);
                    }
                default: throw new MyException("同步类型不存在");
            }
        }

        private static bool SyncSellerRecharge(OnlineOrder order,OrderPayWay payWay)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                IParkSeller factory = ParkSellerFactory.GetFactory();
                ParkSeller model = factory.QueryBySellerId(order.InOutID);
                if (model == null) throw new MyException("商家不存在");

                dbOperator.BeginTransaction();
                try
                {
                    ParkOrder pkOrder = ParkOrderServices.MarkSellerChargeOrder(model, order.Amount, model.SellerID, OrderSource.WeiXin, payWay, dbOperator);
                    if (order == null) throw new MyException("创建充值订单失败");

                    bool result = factory.SellerRecharge(model.SellerID, order.Amount, dbOperator);
                    if (!result) throw new MyException("商家充值失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch (Exception ex){
                    dbOperator.RollbackTransaction();
                    ExceptionsServices.AddExceptions(ex, "商家微信充值通知失败，订单编号："+order.OrderID);
                    return false;
                }
              
            }
        }
        /// <summary>
        /// 再次同步支付结果
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="payTime"></param>
        /// <returns></returns>
        public static void AgainSyncPayResult(decimal orderId, DateTime payTime)
        {
           
            lock (order_lock)
            {
                IOnlineOrder factory = OnlineOrderFactory.GetFactory();

                OnlineOrder order = factory.QueryByOrderId(orderId);
                if (order.Status == OnlineOrderStatus.SyncPayResultFail)
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                    {
                        try
                        {
                            dbOperator.BeginTransaction();
                            bool result = factory.SyncPayResultSuccess(orderId, dbOperator);
                            if (!result) throw new MyException("更改同步成功状态失败");

                            string payDetailId = string.Empty;
                            bool noticePayResult = SyncNoticePayResult(orderId, payTime, order, out payDetailId);
                            if (!noticePayResult) throw new MyException("同步支付结果失败");

                            dbOperator.CommitTransaction();

                            
                            UpdateSyncResultTimes(order.OrderID,payDetailId);
                            SendSyncResultMessage(order);

                        }
                        catch (MyException ex)
                        {
                            dbOperator.RollbackTransaction();
                            factory.SyncPayResultFail(order.OrderID, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            dbOperator.RollbackTransaction();
                            factory.SyncPayResultFail(orderId, "同步支付结果异常");

                            string message = string.Format("【{0}】再次同步支付结果,订单编号：{1}，描述：{2}", order.PaymentChannel.GetDescription(), order.OrderTime, ex.Message);
                            ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", message, ex, LogFrom.WeiXin);

                        }
                    }
                }
            }
        }
        private static void UpdateSyncResultTimes(decimal orderId, string payDetailId)
        {
            try
            {
                IOnlineOrder factory = OnlineOrderFactory.GetFactory();
                factory.UpdateSyncResultTimes(orderId, payDetailId);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", "修改同步次数失败",ex,LogFrom.WeiXin);
            }
        }
        /// <summary>
        /// 取消待支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool CancelOrder(decimal orderId)
        {
            lock (order_lock)
            {
                try
                {
                    IOnlineOrder factory = OnlineOrderFactory.GetFactory();
                    OnlineOrder order = factory.QueryByOrderId(orderId);
                    if (order.Status == OnlineOrderStatus.WaitPay)
                    {
                        bool result = factory.UpdateOrderStatusByOrderId(orderId, OnlineOrderStatus.Cancel);
                        if (result && order.PaymentChannel == PaymentChannel.WeiXinPay)
                        {
                            PaymentServices.CloseOrderPay(order.CompanyID, orderId.ToString());
                        }
                        return result;
                    }
                }
                catch (Exception ex) {
                    ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", "取消订单失败", ex, LogFrom.WeiXin);
                }
            }
            return false;
        }
        /// <summary>
        /// 订单支付中
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool OrderPaying(decimal orderId)
        {
            lock (order_lock)
            {
                try
                {
                    IOnlineOrder factory = OnlineOrderFactory.GetFactory();

                    OnlineOrder order = factory.QueryByOrderId(orderId);
                    if (order.Status == OnlineOrderStatus.WaitPay)
                    {
                        return factory.UpdateOrderStatusByOrderId(orderId, OnlineOrderStatus.Paying);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", "更改订单支付中失败", ex, LogFrom.WeiXin);
                    return false;
                }
              
            }

        }
        /// <summary>
        /// 手动退款（退款失败是调用）
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="certpath"></param>
        /// <returns></returns>
        public static bool ManualRefund(decimal orderId, string certpath)
        {
            TxtLogServices.WriteTxtLogEx("OnlineOrderServices","方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3}", "ManualRefund", "订单退款处理", orderId, "开始订单退款处理");

            bool refundResult = false;
            lock (order_lock)
            {
               
                try
                {
                    IOnlineOrder factory = OnlineOrderFactory.GetFactory();
                    OnlineOrder order = factory.QueryByOrderId(orderId);
                    if (order.Status == OnlineOrderStatus.RefundFail)
                    {
                        refundResult = Refund(order, certpath);
                    }
                    if (refundResult){
                        OperateLogServices.AddOperateLog(OperateType.Other, string.Format("手动执行退款操作，退款订单号：{0}", orderId));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", "手动执行退款操作失败", ex, LogFrom.WeiXin);
                }
            }
            return refundResult;
        }
        private static bool Refund(OnlineOrder model, string certpath)
        {
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            bool refundResult = false;
            string refundOrderId = IdGenerator.Instance.GetId().ToString();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    refundResult = factory.RefundSuccess(model.OrderID, refundOrderId, dbOperator);
                    if (!refundResult) throw new MyException("更改订单为退款成功时失败");
                    switch (model.PaymentChannel) {
                        case PaymentChannel.WeiXinPay: {
                            string refundAmount = ((int)(model.Amount * 100)).ToString();
                            refundResult = PaymentServices.Refund(model.CompanyID,model.SerialNumber, model.OrderID.ToString(), refundAmount, refundAmount, refundOrderId, certpath);
                            TxtLogServices.WriteTxtLogEx("OnlineOrderServices", string.Format("方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3} {4}", "Refund", "订单退款处理", model.OrderID, model.SerialNumber.ToString(), (model.Amount / 100).ToString("F2"), refundOrderId));
                            if (!refundResult) throw new MyException("微信退款失败");
                            break;
                        }
                        case PaymentChannel.AliPay:
                            {
                                refundResult = AliPayApiServices.RefundRequest(model.CompanyID,model.OrderID.ToString(), model.SerialNumber.ToString(), model.Amount.ToString("F2"), "车场网络异常", refundOrderId);
                                TxtLogServices.WriteTxtLogEx("OnlineOrderServices", string.Format("方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3} {4}", "Refund", "订单退款处理", model.OrderID, model.SerialNumber.ToString(), (model.Amount / 100).ToString("F2"), refundOrderId));
                                if (!refundResult) throw new MyException("支付宝退款失败");
                                break;
                            }
                         
                        default: throw new MyException("退款渠道不正确");
                    }
                    dbOperator.CommitTransaction();
                   TxtLogServices.WriteTxtLogEx("OnlineOrderServices","方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3}", "Refund", "订单退款处理", model.OrderID, "退款成功");
                }
                catch (Exception ex)
                {
                    refundResult = false;
                    dbOperator.RollbackTransaction();
                    ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", string.Format("订单退款失败,订单编号{0},描述：{1}", model.OrderID, ex.Message), ex, LogFrom.WeiXin);

                }
                if (!refundResult)
                {
                    factory.RefundFail(model.OrderID);
                }
                if (model.PaymentChannel == PaymentChannel.WeiXinPay)
                {
                    if (!refundResult)
                    {
                        switch (model.OrderType) {
                            case OnlineOrderType.PkBitBooking: {
                                TemplateMessageServices.SendBookingBitNoRefundFail(model.CompanyID, model.OrderID.ToString(), "车场网络异常", model.Amount, model.PayAccount);
                                break;
                            }
                            case OnlineOrderType.SellerRecharge: {
                                TemplateMessageServices.SendSellerRechargeRefundFail(model.CompanyID, model.OrderID.ToString(), "车场网络异常", model.Amount, model.PayAccount);
                                break;
                            }
                            default: {
                                TemplateMessageServices.SendParkingRefundFail(model.CompanyID, model.OrderID.ToString(), "车场网络异常", model.Amount, model.PayAccount);
                                break;
                                
                            }
                        }
                    }
                    else
                    {
                        string message = string.IsNullOrWhiteSpace(model.Remark) ? "车场网络异常" : model.Remark;
                        switch (model.OrderType)
                        {
                            case OnlineOrderType.PkBitBooking:
                                {
                                    TemplateMessageServices.SendBookingBitNoRefundSuccess(model.CompanyID, model.OrderID.ToString(), message, model.Amount, model.PayAccount);
                                    break;
                                }
                            case OnlineOrderType.SellerRecharge:
                                {
                                    TemplateMessageServices.SendSellerRechargeRefundSuccess(model.CompanyID, model.OrderID.ToString(), message, model.Amount, model.PayAccount);
                                    break;
                                }
                            default:
                                {
                                    TemplateMessageServices.SendParkingRefundSuccess(model.CompanyID, model.OrderID.ToString(), message, model.Amount, model.PayAccount);
                                    break;

                                }
                        }
                    }
                }
               
            }
            return refundResult;
        }
        /// <summary>
        /// 订单自动退款处理
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="certpath"></param>
        /// <returns></returns>
        public static bool AutoRefund(decimal orderId, string certpath)
        {
           TxtLogServices.WriteTxtLogEx("OnlineOrderServices","方法名：{0}，操作类型：{1}，订单编号:{2}，备注：{3}", "AutoRefund", "订单退款处理", orderId, "开始订单退款处理");

            bool refundResult = false;
            lock (order_lock)
            {
                TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "进入lock,orderId：{0}", orderId);

                try
                {
                    IOnlineOrder factory = OnlineOrderFactory.GetFactory();
                    OnlineOrder order = factory.QueryByOrderId(orderId);
                    if (order.Status == OnlineOrderStatus.SyncPayResultFail && order.SyncResultTimes >= 3)
                    {
                        refundResult = Refund(order, certpath);

                    }
                }
                catch (Exception ex) {
                    TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "方法名：{0}，操作类型：{1}，订单编号:{2}，Message：{3},StackTrace：{4}", "AutoRefund", "订单退款处理失败", orderId, ex.Message, ex.StackTrace);

                }
                TxtLogServices.WriteTxtLogEx("OnlineOrderServices", "执行完成lock,orderId：{0}", orderId);
            }

            return refundResult;
        }

        public static List<OnlineOrder> QueryBySyncPayResultFail() {
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.QueryBySyncPayResultFail();
        }

        public static List<OnlineOrder> QueryWaitRefund(List<string> parkingIds, DateTime minRealPayTime)
        {
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.QueryWaitRefund(parkingIds,minRealPayTime);
        }
        private static int GetServicePayWay(PaymentChannel type)
        {
            switch (type)
            {
                case PaymentChannel.WeiXinPay:
                    {
                        return 2;
                    }
                case PaymentChannel.AliPay:
                    {
                        return 3;
                    }
                default: throw new MyException("支付类型错误");
            }
        }
        private static string SyncResultDescription(APPResult result)
        {
            switch (result)
            {
                case APPResult.Normal:
                    {
                        return "正常";
                    }
                case APPResult.NoNeedPay:
                    {
                        return "暂不需要缴费";
                    }
                case APPResult.RepeatPay:
                    {
                        return "重复缴费";
                    }
                case APPResult.NotFindIn:
                    {
                        return "该车辆找不到入场记录或车场网络延时";
                    }
                case APPResult.NotSupportedPay:
                    {
                        return "该车场不支持手机缴费";
                    }
                case APPResult.ProxyException:
                    {
                        return "车场网络不给力，暂停线上支付，请稍后再试";
                    }
                case APPResult.NoTempCard:
                    {
                        return "非临停卡，不能缴费";
                    }
                case APPResult.NotFindCard:
                    {
                        return "缴费异常【找不到卡片信息】";
                    }
                case APPResult.AmountIsNot:
                    {
                        return "计算停车费异常";
                    }
                case APPResult.OrderSX:
                    {
                        return "创建缴费信息异常";
                    }
                case APPResult.OtherException:
                    {
                        return "缴费异常【未知原因】";
                    }
                default: throw new MyException("缴费异常，请重试");
            }
        }
        /// <summary>
        /// 发送同步支付模板信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static bool SendSyncResultMessage(OnlineOrder order)
        {
            try
            {
                if (order.PaymentChannel != PaymentChannel.WeiXinPay) return true;

                switch (order.OrderType)
                {
                    case OnlineOrderType.MonthCardRecharge:
                        {
                            return TemplateMessageServices.SendMonthCardRechargeSuccess(order.CompanyID,order.PlateNo, order.PKName, order.Amount, order.ExitTime, order.PayAccount);
                        }
                    case OnlineOrderType.ParkFee:
                        {
                            return TemplateMessageServices.SendParkCarPaymentSuccess(order.CompanyID,order.PlateNo, order.PKName, order.Amount, order.EntranceTime, order.OrderTime, order.RealPayTime, order.ExitTime, order.PayAccount);
                        }
                    case OnlineOrderType.PkBitBooking:
                        {
                            return true;
                        }
                    case OnlineOrderType.SellerRecharge:
                        {
                            IParkSeller factory = ParkSellerFactory.GetFactory();
                            ParkSeller model = factory.QueryBySellerId(order.InOutID);
                            if (model == null) throw new MyException("商家不存在");

                            return TemplateMessageServices.SendSellerRechargeSuccess(order.CompanyID, order.Amount, model.Balance, order.RealPayTime, order.PayAccount);
                        }
                    default: throw new MyException("同步支付结果时找不到订单类型");
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("OnlineOrderServices", "发送模板信息失败：订单编号：" + order.OrderID+"", ex, LogFrom.WeiXin);
                return false;
            }
        }

        public static List<OnlineOrder> QueryPage(OnlineOrderCondition condition, int pageIndex, int pageSize, out int recordTotalCount)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.QueryPage(condition,pageIndex,pageSize,out recordTotalCount);
        }


        public static List<OnlineOrder> QueryAll(OnlineOrderCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.QueryAll(condition);
        }
        public static List<OnlineOrder> ExportQueryPage(OnlineOrderCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.ExportQueryPage(condition);
        }
        public static bool SyncPaymentResult(decimal orderId)
        {
            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            OnlineOrder order = factory.QueryByOrderId(orderId);
            if (order.PaymentChannel != PaymentChannel.WeiXinPay && order.PaymentChannel == PaymentChannel.AliPay)
                throw new MyException("目前只能同步微信或支付宝的支付结果");

            if (order.Status != OnlineOrderStatus.WaitPay && order.Status != OnlineOrderStatus.Paying)
                throw new MyException("只有待支付或支付中的订单才能同步");

            switch (order.PaymentChannel) {
                case PaymentChannel.WeiXinPay: {
                    UnifiedOrderQueryMessage result = PaymentServices.UnifiedOrderQuery(order.CompanyID,order.SerialNumber, order.OrderID.ToString());
                    if (result.Success)
                    {
                        DateTime payTime = GetConversionWeiXinPayTime(result.Tiem_End);
                        return PaySuccess(orderId, result.TransactionId, payTime);
                    }

                    if (result.Return_Code.ToUpper() != "SUCCESS")
                        throw new MyException(string.Format("同步失败：{0}", result.Return_Msg));

                    if (result.Result_Code.ToUpper() != "SUCCESS")
                        throw new MyException(string.Format("同步失败：【{0}】{1}", result.Err_Code, result.Err_Code_Des));

                    if (result.Trade_State.ToUpper() != "SUCCESS")
                    {
                        throw new MyException(string.Format("同步失败：{0}", GetWeiXinPayOrderErrorStateDes(result.Trade_State)));
                    }
                    break;
                }
                case PaymentChannel.AliPay: {
                    DateTime payTime = DateTime.Now;
                    bool result = AliPayApiServices.QueryPayResult(order.CompanyID,order.OrderID.ToString(), order.PrepayId, out payTime);
                    if (result)
                    {
                        return PaySuccess(orderId, order.PrepayId, payTime);
                    }
                    else {
                        throw new MyException("该订单未支付");
                    }
                }
                default: throw new MyException("支付通道不正确");
            }
           
            return false;
        }
        private static DateTime GetConversionWeiXinPayTime(string time)
        {
            DateTime payTime = DateTime.Now;
            if (time.Length == 14)
            {
                string strDate = time.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
                if (!DateTime.TryParse(strDate, out payTime))
                {
                    payTime = DateTime.Now;
                }
            }
            return payTime;
        }
        private static string GetWeiXinPayOrderErrorStateDes(string state)
        {
            switch (state.ToUpper())
            {
                case "REFUND":
                    {
                        return "转入退款";
                    }
                case "NOTPAY":
                    {
                        return "未支付";
                    }
                case "CLOSED":
                    {
                        return "已关闭";
                    }
                case "REVOKED":
                    {
                        return "已撤销";
                    }
                case "USERPAYING":
                    {
                        return "用户支付中";
                    }
                case "PAYERROR":
                    {
                        return "支付失败(其他原因，如银行返回失败)";
                    }
                default: return "未知异常";
            }
        }
        /// <summary>
        /// 获取最后车牌号
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="orderType"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string QueryLastPaymentPlateNumber(PaymentChannel channel,string openId) {
            if (string.IsNullOrWhiteSpace(openId)) return string.Empty;

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            string plateNumber = factory.QueryLastPaymentPlateNumber(channel, openId);
            if (plateNumber.Length > 10) {
                return string.Empty;
            }
            return plateNumber;
        }
        public static string QueryLastPaymentPlateNumber(string accountId) {
            if (string.IsNullOrWhiteSpace(accountId)) return string.Empty;

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            string plateNumber = factory.QueryLastPaymentPlateNumber(accountId);
            if (plateNumber.Length > 10)
            {
                return string.Empty;
            }
            return plateNumber;
        }
        /// <summary>
        /// 修改付款账号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="payAccount"></param>
        /// <param name="payer"></param>
        /// <returns></returns>
        public static bool UpdatePayAccount(decimal orderId, string payAccount, string payer) {

            IOnlineOrder factory = OnlineOrderFactory.GetFactory();
            return factory.UpdatePayAccount(orderId, payAccount,payer);
        }
    }
}
