using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface.WXService;
using Common.Utilities.Helpers;
using Common.Entities;
using System.Configuration;
using Common.ExternalInteractions.BWY;
using Common.Entities.Parking;
using Common.Services;
using Common.Entities.BWY;
using Common.ExternalInteractions.SFM;

namespace SmartSystem.WeiXinInerface
{
    public class RechargeService
    {
        private static string BWYInterfaceUrl = ConfigurationManager.AppSettings["BWYInterfaceUrl"] ?? "";
        private static string BWYSessionID = ConfigurationManager.AppSettings["BWYSessionID"] ?? "";
        private static string BWPKID = ConfigurationManager.AppSettings["BWPKID"] ?? "";

        private static string SFMSecretKey = ConfigurationManager.AppSettings["SFMSecretKey"] ?? "";
        private static string SFMInterfaceUrl = ConfigurationManager.AppSettings["SFMInterfaceUrl"] ?? "";
        private static string SFMPKID = ConfigurationManager.AppSettings["SFMPKID"] ?? "";

        /// <summary>
        /// 获取可以缴费和充值的车辆信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<ParkUserCarInfo> GetMonthCarInfoByAccountID(string accountId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetMonthCarInfoByAccountID(accountId);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<ParkUserCarInfo>();
            }
            return JsonHelper.GetJson<List<ParkUserCarInfo>>(result);


        }
        /// <summary>
        /// 获取可以缴费和充值的车辆信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<ParkUserCarInfo> GetMonthCarInfoByPlateNumber(string plateNumber)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetMonthCarInfoByPlateNumber(plateNumber);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<ParkUserCarInfo>();
            }
            return JsonHelper.GetJson<List<ParkUserCarInfo>>(result);


        }
        /// <summary>
        /// 月租卡续费
        /// </summary>
        /// <param name="CardID">卡片ID</param>
        /// <param name="ParkingID">车场ID</param>
        /// <param name="SystemID">平台ID</param>
        /// <param name="MonthNum">月数</param>
        /// <param name="Amount">金额</param>
        /// <returns></returns>
        public static MonthlyRenewalResult WXMonthlyRenewals(string CardID, string ParkingID, int MonthNum, decimal Amount, string AccountID, int PayWay, OrderSource orderSource, string OnlineOrderID, DateTime payDate)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXMonthlyRenewals(CardID, ParkingID, MonthNum, Amount, AccountID, PayWay, (int)orderSource, OnlineOrderID, payDate);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<MonthlyRenewalResult>(result);
        }

        /// <summary>
        /// 车牌号缴费
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="CardNo"></param>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static TempParkingFeeResult WXTempParkingFee(string PlateNumber, string ParkingID, string AccountID, DateTime CalculatDate)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXTempParkingFee(PlateNumber, ParkingID, CalculatDate, AccountID, 1);
            client.Close();
            client.Abort();
            TempParkingFeeResult model = JsonHelper.GetJson<TempParkingFeeResult>(result);
            try
            {
                if (!string.IsNullOrWhiteSpace(BWYInterfaceUrl) && !string.IsNullOrWhiteSpace(BWYSessionID) && !string.IsNullOrWhiteSpace(BWPKID))
                {
                    if (model.Result == APPResult.NotFindIn || model.Result == APPResult.ProxyException || model.Result == APPResult.NoTempCard
                        || model.Result == APPResult.NotFindCard || model.Result == APPResult.OtherException)
                    {
                        BWYOrderQueryResult bwyResult = BWYInterfaceProcess.TempParkingFee(PlateNumber);
                        if (bwyResult != null)
                        {

                            //return TransforTempParkingFeeResult(model, bwyResult, PlateNumber);

                            model = BWYTransforTempParkingFeeResult(model, bwyResult, PlateNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("WXTempParkingFee方法，异常：{0}", ex.Message));
            }

            try
            {
                if (model.Result == APPResult.NotFindIn || model.Result == APPResult.ProxyException || model.Result == APPResult.NoTempCard
                   || model.Result == APPResult.NotFindCard || model.Result == APPResult.OtherException)
                {
                    if (!string.IsNullOrWhiteSpace(SFMInterfaceUrl) && !string.IsNullOrWhiteSpace(SFMSecretKey) && !string.IsNullOrWhiteSpace(SFMPKID))
                    {
                        PlateQueryResult sfmResult = SFMInterfaceProcess.GetCarPrice(PlateNumber);
                        if (sfmResult != null)
                        {

                            model = SFMTransforTempParkingFeeResult(model, sfmResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("WXTempParkingFee方法，异常：{0}", ex.Message));
            }

            return model;
        }

        private static TempParkingFeeResult SFMTransforTempParkingFeeResult(TempParkingFeeResult model, PlateQueryResult sfmResult)
        {
            if (!sfmResult.Success || sfmResult.Data == null)
            {
                return model;
            }
            if (sfmResult.Code == "400")
            {
                model.Result = APPResult.AmountIsNot;
                return model;
            }
            if (sfmResult.Code != "200")
            {
                model.Result = APPResult.OtherException;
                return model;
            }
            PlateNumberInfo payInfo = sfmResult.Data;

            if (payInfo.payAmount <= 0)
            {
                model.Result = APPResult.NoNeedPay;
                return model;
            }

            TempParkingFeeResult result = new TempParkingFeeResult();
            result.OrderSource = PayOrderSource.SFM;
            result.PlateNumber = payInfo.carNo;
            result.CardNo = payInfo.carNo;

            result.ParkingID = SFMPKID;

            result.ExternalPKID = payInfo.parkKey;

            BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(SystemDefaultConfig.SFMPKID);
            if (parking != null)
            {
                result.ParkName = parking.PKName;
            }
            result.EntranceDate = DateTime.Parse(payInfo.enterTime);
            result.OutTime = payInfo.freeTimeout.HasValue ? payInfo.freeTimeout.Value : 10;
            result.isAdd = true;
            result.PayDate = DateTime.Now;
            ParkOrder order = new ParkOrder();
            order.RecordID = payInfo.orderNo;
            order.OrderNo = payInfo.orderNo;
            order.TagID = string.Empty;
            order.OrderType = OrderType.TempCardPayment;
            order.PayWay = OrderPayWay.WeiXin;

            order.DiscountAmount = payInfo.couponAmount.HasValue ? payInfo.couponAmount.Value : 0;
            order.Amount = payInfo.totalAmount.HasValue ? payInfo.totalAmount.Value : 0;
            order.UnPayAmount = 0;
            order.PayAmount = payInfo.payAmount.HasValue ? payInfo.payAmount.Value : 0;

            order.CarderateID = "";
            order.Status = 0;
            order.OrderSource = OrderSource.WeiXin;
            order.OrderTime = DateTime.Now;
            order.PayTime = DateTime.Now;
            order.PKID = SFMPKID;
            order.UserID = "";
            order.OnlineUserID = "";
            order.OnlineOrderNo = "";
            order.Remark = "";
            result.Pkorder = order;
            result.OrderSource = PayOrderSource.SFM;
            return result;
        }

        private static TempParkingFeeResult BWYTransforTempParkingFeeResult(TempParkingFeeResult model, BWYOrderQueryResult bwyResult, string plateNumber)
        {
            if (bwyResult.Result != 0 || bwyResult.Reference == null || bwyResult.Reference.Count == 0)
            {
                return model;
            }
            OrderQueryResultReference reference = bwyResult.Reference.First();
            TempParkingFeeResult result = new TempParkingFeeResult();
            result.OrderSource = PayOrderSource.BWY;
            result.PlateNumber = plateNumber;
            result.CardNo = plateNumber;
            OrderResultReferenceParkingLot parking = reference.ParkingLot;
            if (parking == null)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TransforTempParkingFeeResult方法，OrderResultReferenceParkingLot is null"));
                return model;
            }
            result.ParkingID = BWPKID;
            result.ParkName = parking.Name;
            result.ExternalPKID = parking.Index.ToString();
            OrderResultBill bill = reference.Bill;
            if (bill == null)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TransforTempParkingFeeResult方法，OrderResultBill is null"));
                return model;
            }

            if (bill.PayState == 3)
            {
                result.Result = APPResult.NoNeedPay;
            }
            //if (bill.PayState == 10)
            //{
            //    result.Result = APPResult.NotFindIn;
            //}
            if (bill.PayState == 1 || bill.PayState == 2 || bill.PayState == 10)
            {
                result.Result = APPResult.Normal;
            }

            result.EntranceDate = bill.StartDate;
            result.OutTime = 10;
            result.isAdd = true;
            result.PayDate = DateTime.Now;
            ParkOrder order = new ParkOrder();
            order.ID = bill.Index;
            order.RecordID = bill.Index.ToString();
            order.OrderNo = bill.Index.ToString();
            order.TagID = bill.Index.ToString();
            order.OrderType = OrderType.TempCardPayment;
            order.PayWay = OrderPayWay.WeiXin;


            OrderResultCostDetail detail = reference.CostDetail;
            if (detail == null)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TransforTempParkingFeeResult方法，OrderResultCostDetail is null"));
                return model;
            }
            if (detail != null && detail.Discount != null)
            {
                order.DiscountAmount = detail.Discount.Sums / 100;
            }
            if (detail.Sums == 0)
            {
                result.Result = APPResult.NoNeedPay;
            }

            order.Amount = (detail.Sums - order.DiscountAmount) / 100;
            order.UnPayAmount = 0;
            order.PayAmount = (detail.Sums - order.DiscountAmount) / 100;

            order.CarderateID = "";
            order.Status = 0;
            order.OrderSource = OrderSource.WeiXin;
            order.OrderTime = DateTime.Now;
            order.PayTime = DateTime.Now;
            //order.OldUserulDate = DateTime.MinValue;
            //order.NewUsefulDate = DateTime.MinValue;
            //order.OldMoney = null;
            //order.NewMoney = null;
            order.PKID = parking.Index.ToString();
            order.UserID = "";
            order.OnlineUserID = "";
            order.OnlineOrderNo = "";
            order.Remark = "";
            //order.LastUpdateTime = "";
            //order.HaveUpdate = "";
            //order.DataStatus = "";
            result.Pkorder = order;
            result.OrderSource = PayOrderSource.BWY;
            return result;
        }
        /// <summary>
        /// 验证获取支付订单结果
        /// </summary>
        /// <param name="result"></param>
        public static void CheckCalculatingTempCost(APPResult result, string otherErrorDesc = "")
        {
            if (result == APPResult.OtherException && !string.IsNullOrWhiteSpace(otherErrorDesc))
            {
                throw new MyException(otherErrorDesc);
            }
            switch (result)
            {
                case APPResult.Normal:
                case APPResult.NoNeedPay:
                case APPResult.RepeatPay:
                    {
                        break;
                    }
                case APPResult.NotFindIn:
                    {
                        throw new MyException("该车辆找不到入场记录，请稍后再试");
                    }
                case APPResult.NotSupportedPay:
                    {
                        throw new MyException("该车场不支持手机缴费");
                    }
                case APPResult.ProxyException:
                    {
                        throw new MyException("车场网络异常，暂不能缴费，请稍后再试！");
                    }
                case APPResult.NoTempCard:
                    {
                        throw new MyException("非临停卡，不能缴费");
                    }
                case APPResult.NotFindCard:
                    {
                        throw new MyException("缴费异常【找不到卡片信息】");
                    }
                case APPResult.AmountIsNot:
                    {
                        throw new MyException("计算停车费异常");
                    }
                case APPResult.OrderSX:
                    {
                        throw new MyException("订单已失效");
                    }
                case APPResult.OtherException:
                    {
                        throw new MyException("创建缴费信息异常！");
                    }
                case APPResult.NoBox:
                    {
                        throw new MyException("找不到岗亭信息");
                    }
                case APPResult.NoCarInBox:
                    {
                        throw new MyException("出口未识别到您的爱车");//(联系电话0575-85679216)
                    }
                case APPResult.ManualPay:
                    {
                        throw new MyException("请重新驶入");//
                    }
                case APPResult.NotIn:
                    {
                        throw new MyException("入口无无牌车");
                    }
                default: throw new MyException("缴费异常，请重试");
            }
        }
        /// <summary>
        /// 岗亭缴费
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="CardNo"></param>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static TempParkingFeeResult WXScanCodeTempParkingFee(string boxId, string accountId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXScanCodeTempParkingFee(boxId, accountId, 1);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<TempParkingFeeResult>(result);
        }
        /// <summary>
        /// 临时卡缴费
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="PayWay"></param>
        /// <param name="Amount"></param>
        /// <param name="OrderSource"></param>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="OnlineOrderID"></param>
        /// <returns></returns>
        public static TempStopPaymentResult WXTempStopPayment(string OrderID, int PayWay, decimal Amount, string ParkingID, string AccountID, string OnlineOrderID, DateTime payDate)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXTempStopPayment(OrderID, PayWay, Amount, ParkingID, OnlineOrderID, AccountID, payDate);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<TempStopPaymentResult>(result);
        }
        /// <summary>
        /// <returns>0进成功、1出成功、1001参数错误、1002扫码入场失败、1003代理连接断开，1004通道无车</returns>
        /// </summary>
        /// <param name="parkingId"></param>
        /// <param name="gateNo"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static int WXScanCodeInOut(string parkingId, string gateNo, string openId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.WXScanCodeInOut(parkingId, gateNo, openId);
            client.Close();
            client.Abort();
            return result;
        }
        /// <summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="parkingId">车场编号</param>
        /// <param name="GateID"></param>
        /// <param name="AccountID"></param>
        /// <param name="OrderSource"></param>
        /// <returns></returns>
        public static TempParkingFeeResult WXScanCodeTempParkingFeeByGateID(string parkingId, string GateID, string AccountID, OrderSource OrderSource)
        {
            TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("WXScanCodeTempParkingFeeByGateID方法,GateID:{0},AccountID:{1},OrderSource:{2},parkingId:{3}", GateID, AccountID, (int)OrderSource, parkingId));

            if (parkingId == SystemDefaultConfig.SFMPKID)
            {
                return SFMTempParkingFeeResult(GateID);
            }
            if (parkingId == SystemDefaultConfig.BWPKID)
            {
                return BWYTempParkingFeeResult(GateID);
            }
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXScanCodeTempParkingFeeByGateID(GateID, AccountID, (int)OrderSource);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<TempParkingFeeResult>(result);
        }


        /// <summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="parkingId">车场编号</param>
        /// <param name="GateID"></param>
        /// <param name="AccountID"></param>
        /// <param name="OrderSource"></param>
        /// <returns></returns>
        public static TempParkingFeeResult WXScanCodeTempParkingFeeByParkGateID(string parkingId, string GateID, string AccountID, OrderSource OrderSource)
        {
            TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("WXScanCodeTempParkingFeeByParkGateID,GateID:{0},AccountID:{1},OrderSource:{2},parkingId:{3}", GateID, AccountID, (int)OrderSource, parkingId));

            if (parkingId == SystemDefaultConfig.SFMPKID)
            {
                string[] sPData = GateID.Split('$');
                if (sPData.Length == 2)
                {
                    return SFMTempParkingFeeResult(sPData[0], sPData[1]);
                }
                else
                {
                    TempParkingFeeResult r = new TempParkingFeeResult();
                    r.Result = APPResult.OtherException;
                    return r;
                }

            }
            if (parkingId == SystemDefaultConfig.BWPKID)
            {
                return BWYTempParkingFeeResult(GateID);
            }
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXScanCodeTempParkingFeeByGateID(GateID, AccountID, (int)OrderSource);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<TempParkingFeeResult>(result);
        }


        public static TempParkingFeeResult BWYTempParkingFeeResult(string GateID)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            result.Result = APPResult.NoCarInBox;
            result.ErrorDesc = "未知异常";

            int bwyGateId = 0;
            if (!string.IsNullOrWhiteSpace(SystemDefaultConfig.BWPKID) && int.TryParse(GateID, out bwyGateId))
            {
                try
                {

                    OutCarResult bwyResult = BWYInterfaceProcess.QueryOutCar(bwyGateId);
                    if (bwyResult.Result != 0)
                    {
                        result.Result = APPResult.OtherException;
                        result.ErrorDesc = bwyResult.Desc;
                        return result;
                    }
                    if (bwyResult.Reference == null || string.IsNullOrWhiteSpace(bwyResult.Reference.LPR))
                    {
                        result.Result = APPResult.NoCarInBox;
                        result.ErrorDesc = bwyResult.Desc;
                        return result;
                    }
                    TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("BWYTempParkingFeeResult方法,BWYInterfaceUrl:{0},BWYSessionID:{1},BWPKID:{2}", BWYInterfaceUrl, BWYSessionID, BWPKID));
                    if (!string.IsNullOrWhiteSpace(BWYInterfaceUrl) && !string.IsNullOrWhiteSpace(BWYSessionID) && !string.IsNullOrWhiteSpace(BWPKID))
                    {
                        BWYOrderQueryResult tempResult = BWYInterfaceProcess.TempParkingFee(bwyResult.Reference.LPR);
                        if (tempResult != null)
                        {
                            return BWYTransforTempParkingFeeResult(result, tempResult, bwyResult.Reference.LPR);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("BWYTempParkingFeeResult方法，异常：{0}", ex.Message));
                }
            }
            else
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("BWYTempParkingFeeResult方法，参数无效：{0},GateID:{1}", SystemDefaultConfig.BWPKID, GateID));
            }
            return result;
        }
        public static TempParkingFeeResult SFMTempParkingFeeResult(string GateID)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            result.Result = APPResult.OtherException;
            result.ErrorDesc = "车场或通道编号未配置";

            if (!string.IsNullOrWhiteSpace(SystemDefaultConfig.SFMPKID) && !string.IsNullOrWhiteSpace(GateID))
            {
                try
                {
                    BWYGateMapping gate = BWYGateMappingServices.QueryByGateID(1, GateID);
                    if (gate == null)
                    {
                        result.Result = APPResult.OtherException;
                        result.ErrorDesc = "获取车场信息失败";
                        TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，获取车场信息失败，通道编号：{0}", GateID));
                        return result;
                    }

                    TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法,SFMInterfaceUrl:{0},SFMSecretKey:{1},SFMPKID:{2}", SFMInterfaceUrl, SFMSecretKey, SFMPKID));
                    if (!string.IsNullOrWhiteSpace(SFMInterfaceUrl) && !string.IsNullOrWhiteSpace(SFMSecretKey) && !string.IsNullOrWhiteSpace(SFMPKID))
                    {
                        OutCarInfoResult sfmResult = SFMInterfaceProcess.QueryOutCarOrder(gate.ParkNo, GateID);
                        if (!sfmResult.Success)
                        {
                            result.Result = APPResult.OtherException;
                            result.ErrorDesc = string.Format("{0}[{1}]", sfmResult.Message, sfmResult.Code);
                            return result;
                        }
                        if (sfmResult.Data == null)
                        {
                            result.Result = APPResult.NoCarInBox;
                            result.ErrorDesc = string.Empty;
                            return result;
                        }
                        return SFMTransforTempParkingFeeResult(result, sfmResult);
                    }
                }
                catch (Exception ex)
                {
                    TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，异常：{0}", ex.Message));
                }
            }
            else
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，参数无效：{0},GateID:{1}", SystemDefaultConfig.SFMPKID, GateID));
            }
            return result;
        }

        public static TempParkingFeeResult SFMTempParkingFeeResult(string ParkNo, string GateID)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            result.Result = APPResult.OtherException;
            result.ErrorDesc = "车场或通道编号未配置";

            if (!string.IsNullOrWhiteSpace(SystemDefaultConfig.SFMPKID) && !string.IsNullOrWhiteSpace(ParkNo) && !string.IsNullOrWhiteSpace(GateID))
            {
                try
                {
                    BWYGateMapping gate = BWYGateMappingServices.QueryByGateID(1, ParkNo, GateID);
                    if (gate == null)
                    {
                        result.Result = APPResult.OtherException;
                        result.ErrorDesc = "获取车场信息失败";
                        TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，获取车场信息失败，通道编号：{0}", GateID));
                        return result;
                    }

                    TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法,SFMInterfaceUrl:{0},SFMSecretKey:{1},SFMPKID:{2}", SFMInterfaceUrl, SFMSecretKey, SFMPKID));
                    if (!string.IsNullOrWhiteSpace(SFMInterfaceUrl) && !string.IsNullOrWhiteSpace(SFMSecretKey) && !string.IsNullOrWhiteSpace(SFMPKID))
                    {
                        OutCarInfoResult sfmResult = SFMInterfaceProcess.QueryOutCarOrder(gate.ParkNo, GateID);
                        if (!sfmResult.Success)
                        {
                            result.Result = APPResult.OtherException;
                            result.ErrorDesc = string.Format("{0}[{1}]", sfmResult.Message, sfmResult.Code);
                            return result;
                        }
                        if (sfmResult.Data == null)
                        {
                            result.Result = APPResult.NoCarInBox;
                            result.ErrorDesc = string.Empty;
                            return result;
                        }
                        return SFMTransforTempParkingFeeResult(result, sfmResult);
                    }
                }
                catch (Exception ex)
                {
                    TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，异常：{0}", ex.Message));
                }
            }
            else
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SFMTempParkingFeeResult方法，参数无效：{0},GateID:{1}", SystemDefaultConfig.SFMPKID, GateID));
            }
            return result;
        }

        private static TempParkingFeeResult SFMTransforTempParkingFeeResult(TempParkingFeeResult model, OutCarInfoResult sfmResult)
        {
            if (!sfmResult.Success || sfmResult.Data == null)
            {
                return model;
            }
            if (sfmResult.Code == "400")
            {
                model.Result = APPResult.AmountIsNot;
                return model;
            }
            if (sfmResult.Code != "0000")
            {
                model.Result = APPResult.OtherException;
                return model;
            }
            SMFOutCarInfo payInfo = sfmResult.Data;

            if (!payInfo.payAmount.HasValue || payInfo.payAmount <= 0)
            {
                model.Result = APPResult.NoNeedPay;
                return model;
            }

            TempParkingFeeResult result = new TempParkingFeeResult();
            result.OrderSource = PayOrderSource.SFM;
            result.PlateNumber = payInfo.carNo;
            result.CardNo = payInfo.carNo;

            result.ParkingID = SFMPKID;

            result.ExternalPKID = payInfo.Parking_Key;
            BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(SystemDefaultConfig.SFMPKID);
            if (parking != null)
            {
                result.ParkName = parking.PKName;
            }
            result.EntranceDate = DateTime.Parse(payInfo.enterTime);
            result.OutTime = payInfo.freeTimeout.HasValue ? payInfo.freeTimeout.Value : 10;
            result.isAdd = true;
            result.PayDate = DateTime.Now;
            ParkOrder order = new ParkOrder();
            order.RecordID = payInfo.ParkOrder_OrderNo;
            order.OrderNo = payInfo.ParkOrder_OrderNo;
            order.TagID = "1";
            order.OrderType = OrderType.TempCardPayment;
            order.PayWay = OrderPayWay.WeiXin;

            order.DiscountAmount = payInfo.couponAmount.HasValue ? payInfo.couponAmount.Value : 0;
            order.Amount = payInfo.totalAmount.HasValue ? payInfo.totalAmount.Value : 0;
            order.UnPayAmount = 0;
            order.PayAmount = payInfo.payAmount.HasValue ? payInfo.payAmount.Value : 0;

            order.CarderateID = "";
            order.Status = 0;
            order.OrderSource = OrderSource.WeiXin;
            order.OrderTime = DateTime.Now;
            order.PayTime = DateTime.Now;
            order.PKID = SFMPKID;
            order.UserID = "";
            order.OnlineUserID = "";
            order.OnlineOrderNo = "";
            order.Remark = "";
            result.Pkorder = order;
            result.OrderSource = PayOrderSource.SFM;
            return result;
        }
    }
}
