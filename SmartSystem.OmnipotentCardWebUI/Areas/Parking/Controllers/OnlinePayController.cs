using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.AliPayServices;
using SmartSystem.AliPayServices.lib;
using Common.Entities.Parking;
using Common.SqlRepository.WeiXin;
using Common.Entities;
using Common.Utilities;
using Common.Services.Park;
using Common.Entities.Order;
using Common.Entities.WX;
using Common.Entities.Enum;
using Common.Services;
using Common.Entities.AliPay;
using Common.Services.AliPay;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;
using SmartSystem.WeiXinInerface;
using SmartSystem.WXInerface;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    public class OnlinePayController : Controller
    {
        //
        // GET: /Parking/OnlinePay/

        public ActionResult Index()
        {
            return View();
        }
      
        public string Pay()
        {
            string sTag = Request["paytype"];
            //string sBody = Request["body"];
            string sAmount = Request["fee"];
            string sPayAccount = Request["auth_code"];
            string sPlateNumber = Request["PlateNumber"];
            string sPKID = Request["PKID"];

            if (sTag.IsEmpty() || (sTag != "0" && sTag != "1" && sTag != "2"))
            {
                return "2";
            }

            //if (string.IsNullOrEmpty(sBody))
            //{
            //    return "2sBody";
            //}
            if (string.IsNullOrEmpty(sAmount))
            {
                return "2";
            }

            if (sTag != "2")
            {
                if (string.IsNullOrEmpty(sPayAccount))
                {
                    return "2";
                }
            }
            if(string.IsNullOrEmpty(sPlateNumber)){
                return "2";
            }
            if (string.IsNullOrEmpty(sPKID))
            {
                return "2";
            }

            OnlineOrder model = new OnlineOrder();
            model.OrderTime = DateTime.Now;

            TempParkingFeeResult result = RechargeService.WXTempParkingFee(sPlateNumber, sPKID, sPayAccount, model.OrderTime);

            if (result.Result == APPResult.NoNeedPay ) 
            {
                return "3"; //不需要交费
            }
            if (result.Result == APPResult.RepeatPay)
            {
                return "4"; //重复交费
            }

            decimal dAmount = decimal.Parse(sAmount) / 100;
            //if (result.Pkorder.Amount != dAmount)
            //{
            //    return "6"; //金额不一致
            //}
            try
            {
                RechargeService.CheckCalculatingTempCost(result.Result);
            }
            catch (Exception ex)
            {
                return ((int)result.Result).ToString();
            }
            
            if (result.OrderSource == PayOrderSource.Platform)
            {
                bool testResult = CarService.WXTestClientProxyConnectionByPKID(result.ParkingID);
                if (!testResult)
                {
                    throw new MyException("车场网络异常，暂无法缴停车费！");
                }
                //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo);

                int interfaceOrderState =InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo, result.ParkingID, result.Pkorder.TagID);
                if (interfaceOrderState != 1)
                {
                    if (interfaceOrderState == 2)
                    {
                        return "4"; //重复交费
                    }
                    else
                    {
                        return "5"; //订单已失效
                    }
                }
            }

            model.ParkCardNo = result.CardNo;
            model.PKID = result.ParkingID;
            model.PKName = result.ParkName;
            model.InOutID = result.Pkorder.TagID;
            model.PlateNo = result.PlateNumber;
            model.EntranceTime = result.EntranceDate;
            model.ExitTime = model.OrderTime.AddMinutes(result.OutTime);
            //model.Amount = result.Pkorder.Amount;
            model.Amount = dAmount;
            model.PayDetailID = result.Pkorder.OrderNo;
            model.DiscountAmount = result.Pkorder.DiscountAmount;
           // model.OrderSource = PayOrderSource.HAND;
            model.ExternalPKID = result.ExternalPKID;

            model.OrderID = IdGenerator.Instance.GetId();
            model.Status = OnlineOrderStatus.WaitPay;
            model.OrderType = OnlineOrderType.ParkFee;
          
            BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
            if (company == null) throw new MyException("获取单位信息失败");

            if (sTag == "0")
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                if (config == null)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                    return "-1";
                }
                if (!config.Status)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                    return "-1";
                }

                model.PayeeUser = config.SystemName;
                model.PayeeAccount = config.PartnerId;
                model.PayAccount = sPayAccount;
                model.Payer = sPayAccount;
                model.AccountID = model.AccountID;
                model.CardId = model.AccountID;
                model.PayeeChannel = PaymentChannel.WeiXinPay;
                model.PaymentChannel = PaymentChannel.WeiXinPay;
                model.CompanyID = config.CompanyID;

                bool isSuc = OnlineOrderServices.Create(model);
                if (!isSuc) throw new MyException("生成待缴费订单失败");
            }
            else if (sTag == "1")
            {
                //支付宝
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(company.CPID);
                if (config == null)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取支付宝配置信息失败[0001]", "单位编号：" + company.CPID, LogFrom.WeiXin);
                    return "-1";
                }
                if (!config.Status)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该支付宝暂停使用", "单位编号：" + config.CompanyID, LogFrom.WeiXin);
                    return "-1";
                }

                model.AccountID = string.Empty;
                model.CardId = string.Empty;
                model.PayeeChannel = PaymentChannel.AliPay;
                model.PaymentChannel = PaymentChannel.AliPay;
                model.PayeeUser = config.SystemName;
                model.PayeeAccount = config.PayeeAccount;
                model.Payer = sPayAccount;
                model.PayAccount = sPayAccount;
                model.CompanyID = config.CompanyID;

                bool isSuc = OnlineOrderServices.Create(model);
                if (!isSuc) throw new MyException("生成待缴费订单失败");
            }
            else if (sTag == "2")
            {
                //现金支付的
                model.AccountID = string.Empty;
                model.CardId = string.Empty;
                model.Payer = sPayAccount;
                model.PayAccount = sPayAccount;
                model.CompanyID = company.CPID;
            }
            
            //调用刷卡支付,如果内部出现异常则在页面上显示异常原因
            try
            {
                //int status=1;

                string tradeNo = "";
                string sDataInfo = "";
                if (sTag == "0")    //微信
                {
                    if (!MicroPay.Run(model, out sDataInfo))
                    {
                        return "-2";
                    }
                    else
                    {
                        tradeNo = sDataInfo;
                        //发送通知开闸
                        bool isPayState = OnlineOrderServices.PaySuccess(model.OrderID, tradeNo, DateTime.Now, sPayAccount);
                        if (!isPayState) throw new Exception("修改微信订单状态失败");

                        TxtLogServices.WriteTxtLogEx("WXPayReturn", string.Format("WXPayResult:{0}支付完成", tradeNo));
                    }
                }
                else if (sTag == "1")   //支付宝
                {
                    if (AliPayPay.Run(model, out sDataInfo) == false)
                    {
                        return "-3";
                    }
                    else
                    {
                        tradeNo = sDataInfo;

                        bool isPayState = OnlineOrderServices.PaySuccess(model.OrderID, tradeNo, DateTime.Now, sPayAccount);
                        if (!isPayState) throw new Exception("修改支付宝订单状态失败");

                        TxtLogServices.WriteTxtLogEx("AliPayReturn", string.Format("AliPayShowResult:{0}支付完成", tradeNo));

                    }
                }
                else if (sTag == "2") //现金支付的
                {
                    TempStopPaymentResult payResult = RechargeService.WXTempStopPayment(result.Pkorder.OrderNo,(int) OrderPayWay.Cash, dAmount, sPKID, "", model.OrderID.ToString() , DateTime.Now);
                    TxtLogServices.WriteTxtLogEx("CashReturn", string.Format("CashShowResult:{1}:{0} 支付完成", payResult.ToXml(System.Text.Encoding.UTF8 ),dAmount));
                    if (payResult.Result != APPResult.Normal)
                    {
                        return "5";
                    }
                }

                //不是预支付的订单 就暂时不修改了
                //bool results = OnlineOrderServices.UpdatePrepayIdById(tradeNo, model.OrderID);
                //Response.Write("<span style='color:#00CD00;font-size:20px'>" + result + "</span>");
                ParkingFeeService.DeleteParkingFee(model.PlateNo + model.PKID);
                return "0";
            }
            //catch (WxPayException ex)
            //{
            //    return "1";
            //    //Response.Write("<span style='color:#FF0000;font-size:20px'>" + ex.ToString() + "</span>");
            //}
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("PayError", "该支付失败", "单位编号：" + model.CompanyID + "<br/>" + ex.StackTrace, LogFrom.UnKnown);
                return "-4";
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + ex.ToString() + "</span>");
            }

        }
    }
}
