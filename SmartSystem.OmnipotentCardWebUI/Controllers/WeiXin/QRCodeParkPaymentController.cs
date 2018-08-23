using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinInerface;
using Common.Entities.WX;
using Common.Entities.Enum;
using Common.Entities.Order;
using SmartSystem.WXInerface;
using Common.Services;
using Common.Entities;
using Common.Services.WeiXin;
using Common.Utilities;
using SmartSystem.WeiXinServices;
using System.Text;
using SmartSystem.AliPayServices;
using Common.Entities.AliPay;
using Common.Services.AliPay;
using Common.ExternalInteractions.BWY;
using Common.Entities.Parking;
using Common.Services.Park;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 扫描缴费
    /// </summary>
    public class QRCodeParkPaymentController : WeiXinController
    {
        /// <summary>
        /// 扫描缴费
        /// </summary>
        /// <param name="pid">车场编号</param>
        /// <param name="pn">车牌号</param>
        /// <param name="bid">岗亭编号</param>
        /// <returns></returns>
        public ActionResult Index(string pid, string pn, string bid,string personId="")
        {
            TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "进入Index，pid：{0}, pn：{1}, bid：{2},personId:{3}", pid, pn, bid, personId);

            try
            {
                if (SourceClient != RequestSourceClient.AliPay && SourceClient != RequestSourceClient.WeiXin)
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "请在微信或支付宝中打开" });
                }
                string companyId = CompanyServices.GetCompanyId(pid, bid);
                if (string.IsNullOrWhiteSpace(companyId)) throw new MyException("获取单位编号失败");

                if (SourceClient == RequestSourceClient.AliPay)
                {
                    if (string.IsNullOrWhiteSpace(AliPayUserId))
                    {
                        string id = string.Format("QRCodeParkPayment_Index_pid={0}^pn={1}^bid={2}^companyId={3}^personId={4}", pid, pn, bid, companyId, personId);
                        return RedirectToAction("Index", "AliPayAuthorize", new { id = id });
                    }
                }
                else if (SourceClient == RequestSourceClient.WeiXin)
                {
                    if (string.IsNullOrWhiteSpace(WeiXinOpenId))
                    {
                        string id = string.Format("QRCodeParkPayment_Index_pid={0}^pn={1}^bid={2}^companyId={3}^personId={4}", pid, pn, bid, companyId, personId);
                        return RedirectToAction("Index", "WeiXinAuthorize", new { id = id });
                    }
                }

                if (!string.IsNullOrWhiteSpace(bid))
                {
                    return RedirectToAction("ComputeParkingFee", "QRCodeParkPayment", new { bid = bid });
                }
                else
                {
                    if (SourceClient == RequestSourceClient.WeiXin || SourceClient == RequestSourceClient.AliPay)
                    {
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "车牌号：" + pn);
                        if (!string.IsNullOrWhiteSpace(pn))
                        {
                            return RedirectToAction("ComputeParkingFee", "QRCodeParkPayment", new { pid = pid, licensePlate = pn });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "请在微信或支付宝中打开" });
                    }
                }
                ViewBag.CurrParkingId = pid;

                if (SourceClient == RequestSourceClient.AliPay)
                {
                    ViewBag.PlateNumber = OnlineOrderServices.QueryLastPaymentPlateNumber(PaymentChannel.AliPay, AliPayUserId);
                }
                else
                {
                    ViewBag.PlateNumber = OnlineOrderServices.QueryLastPaymentPlateNumber(PaymentChannel.WeiXinPay, WeiXinOpenId);
                }
                
                if (!string.IsNullOrWhiteSpace(personId))
                {
                    Session["SmartSystem_WeiXinTg_personid"] = personId;
                }
                
                return View();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("QRCodeParkPayment", "Index方法处理异常", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "处理异常" });
            }
        }
        //private string GetCompanyId(string pid, string bid) {
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(pid))
        //        {
        //            BaseCompany company = CompanyServices.QueryByParkingId(pid);
        //            if (company != null)
        //            {
        //                return company.CPID;
        //            }
        //        }
        //        if (!string.IsNullOrWhiteSpace(bid))
        //        {
        //            BaseCompany company = CompanyServices.QueryByBoxID(bid);
        //            if (company != null)
        //            {
        //                return company.CPID;
        //            }
        //        }
        //    }
        //    catch (Exception ex) {
        //        ExceptionsServices.AddExceptionToDbAndTxt("QRCodeParkPayment", "GetCompanyId方法处理异常", ex, LogFrom.WeiXin);

        //    }
        //    return string.Empty;
        //}
        /// <summary>
        /// 计算停车费
        /// </summary>
        /// <param name="pid">车场编号</param>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="bid">岗亭编号</param>
        /// <param name="gid">通道编号</param>
        /// <returns></returns>
        public ActionResult ComputeParkingFee(string pid, string licensePlate, string bid, string gid)
        {
            licensePlate = licensePlate.ToPlateNo();
            TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "进入ComputeParkingFee，pid：{0}, licensePlate：{1}, bid：{2},gid:{3},opendId:{4}", pid, licensePlate, bid, gid, WeiXinOpenId);
            bool IsShowPlateNumber = true;
            try
            {
                OnlineOrder model = new OnlineOrder();
                model.OrderTime = DateTime.Now;

                TempParkingFeeResult result = null;
                bool bClient = false;
                if (!string.IsNullOrWhiteSpace(gid))
                {
                    //result = RechargeService.WXScanCodeTempParkingFeeByGateID(pid, gid, string.Empty, GetOrderSource());
                    result = ParkingFeeService.GetParkingFeeByGateID(pid, gid);
                    if (result == null)
                    {
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "GetParkingFeeByGateID Result:null");
                    }
                    else
                    {
                        bClient = true;
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "GetParkingFeeByGateID Result:{0}", (int)result.Result);
                    }
                    if (result == null)
                    {
                        result = RechargeService.WXScanCodeTempParkingFeeByParkGateID(pid, gid, string.Empty, GetOrderSource());
                    }
                    TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "Result:{0}", (int)result.Result);
                    if (result == null || result.Result == APPResult.NoNeedPay || result.Result == APPResult.RepeatPay)
                    {
                        int type = result.Result == APPResult.NoNeedPay ? 0 : 1;
                        return RedirectToAction("NotNeedPayment", "QRCodeParkPayment", new { licensePlate = result.PlateNumber, type = type, surplusMinutes = result.OutTime, entranceTime = result.EntranceDate, backUrl = "/QRCode/Index" });
                    }
                    if (result.Result == APPResult.NoLp)
                    {
                        return RedirectToAction("Index", "ScanCodeInOut", new { pid = pid, io = gid, source = 1 });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(bid))
                {
                    result = ParkingFeeService.GetParkingFeeByBoxID(bid);

                    if (result == null)
                    {
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "GetParkingFeeByBoxID Result:null");
                        result = RechargeService.WXScanCodeTempParkingFee(bid, string.Empty);
                    }
                    else
                    {
                        bClient = true;
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "GetParkingFeeByBoxID Result:{0}", (int)result.Result);
                    }
                    if (result == null || result.Result == APPResult.NoNeedPay || result.Result == APPResult.RepeatPay)
                    {
                        int type = result.Result == APPResult.NoNeedPay ? 0 : 1;
                        return RedirectToAction("NotNeedPayment", "QRCodeParkPayment", new { licensePlate = result.PlateNumber, type = type, surplusMinutes = result.OutTime, entranceTime = result.EntranceDate, backUrl = "/QRCode/Index" });
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(licensePlate))
                    {
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取车牌信息失败" });
                    }
                    result = ParkingFeeService.GetParkingFeeByParkingID(pid, licensePlate);

                    if (result == null)
                    {
                        result = RechargeService.WXTempParkingFee(licensePlate, pid, string.Empty, model.OrderTime);
                    }
                    else
                    {
                        bClient = true; 
                    }
                    if (result == null || result.Result == APPResult.NoNeedPay || result.Result == APPResult.RepeatPay)
                    {
                        int type = result.Result == APPResult.NoNeedPay ? 0 : 1;
                        return RedirectToAction("NotNeedPayment", "QRCodeParkPayment", new { licensePlate = licensePlate, type = type, surplusMinutes = result.OutTime, entranceTime = result.EntranceDate });
                    }
                }
                
                if (!bClient)
                {
                    RechargeService.CheckCalculatingTempCost(result.Result, result.ErrorDesc);
                    if (result.OrderSource == PayOrderSource.Platform)
                    {
                        bool testResult = CarService.WXTestClientProxyConnectionByPKID(result.ParkingID);
                        if (!testResult)
                        {
                            throw new MyException("车场网络异常，暂无法缴停车费！");
                        }
                        //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo);
                        int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(result.Pkorder.OrderNo, result.ParkingID, result.Pkorder.TagID);
                        TxtLogServices.WriteTxtLogEx("QRCodeParkPayment", "ComputeParkingFee，Tag:{0}, OrderNo：{1}, PKID：{2}\r\nResult:{3}", result.Pkorder.TagID, result.Pkorder.OrderNo, result.ParkingID, interfaceOrderState);

                        if (interfaceOrderState != 1)
                        {
                            string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                            string companyId = GetCompanyId(pid, bid, gid);
                            return RedirectToAction("Index", "ErrorPrompt", new { message = msg, returnUrl = "/QRCode/Index?companyId=" + companyId + "" });
                        }
                    }
                }
                model.OrderSource = result.OrderSource;
                model.ExternalPKID = result.ExternalPKID;
                model.ParkCardNo = result.CardNo;
                model.PKID = result.ParkingID;
                model.PKName = result.ParkName;
                model.InOutID = result.Pkorder.TagID;
                model.TagID = result.Pkorder.TagID;
                model.PlateNo = result.PlateNumber;
                model.EntranceTime = result.EntranceDate;
                model.ExitTime = model.OrderTime.AddMinutes(result.OutTime);
                model.Amount = result.Pkorder.Amount;
                model.PayDetailID = result.Pkorder.OrderNo;
                model.DiscountAmount = result.Pkorder.DiscountAmount;
                ViewBag.Result = result.Result;
                ViewBag.PayAmount = result.Pkorder.PayAmount;
                ViewBag.IsShowPlateNumber = IsShowPlateNumber;

                if (result.ImageUrl.IsEmpty())
                {
                    ParkIORecord record = ParkIORecordServices.QueryLastExitIORecordByPlateNumber(model.PlateNo);
                    string htmlurl = "";
                    if (record != null && record.EntranceImage.IsEmpty() == false)
                    {
                        htmlurl = "http://www.yft166.net/Pic/" + record.EntranceImage.Replace('\\', '/');
                    }
                    else
                    {
                        result.ImageUrl = htmlurl;
                    }
                    result.ImageUrl = htmlurl;
                }
                //显示图片的
                ViewBag.url = result.ImageUrl;

                return View(model);

            }
            catch (MyException ex)
            {
                string companyId = GetCompanyId(pid, bid, gid);
                string parkingId = GetParkingId(pid, bid, gid);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message, parkingId = parkingId, returnUrl = "/QRCode/Index?companyId=" + companyId + "" });
            }
            catch (Exception ex)
            {
                string companyId = GetCompanyId(pid, bid, gid);
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "扫描缴费计算缴费金额失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "计算缴费金额失败", returnUrl = "/QRCode/Index?companyId=" + companyId + "" });
            }
        }
        private OrderSource GetOrderSource()
        {
            OrderSource orderSource = OrderSource.WeiXin;
            if (SourceClient == RequestSourceClient.Other)
            {
                throw new MyException("获取客户端来源失败");
            }
            if (SourceClient == RequestSourceClient.AliPay)
            {
                orderSource = OrderSource.AliPay;
            }
            return orderSource;
        }
        /// <summary>
        /// 不需要缴费提醒页面
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="type">提醒类型0-不需要缴费 1-缴过费了，目前还不需要再次缴费</param>
        /// <returns></returns>
        public ActionResult NotNeedPayment(string licensePlate, int type, int surplusMinutes, DateTime entranceTime, string backUrl = "/QRCodeParkPayment/Index")
        {
            licensePlate = licensePlate.ToPlateNo();
            ViewBag.BackUrl = backUrl;
            ViewBag.Type = type;
            ViewBag.SurplusMinutes = surplusMinutes;
            ViewBag.EntranceTime = entranceTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.ParkingTotalDuration = entranceTime.GetParkingDuration(DateTime.Now);
            return View();
        }
        public ActionResult SubmitParkingPaymentRequest(OnlineOrder model)
        {
            try
            {
                if (model.OrderSource == PayOrderSource.Platform)
                {
                    //int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(model.PayDetailID);

                    int interfaceOrderState = InterfaceOrderServices.OrderWhetherEffective(model.PayDetailID, model.PKID, model.InOutID);
                    if (interfaceOrderState != 1)
                    {
                        string msg = interfaceOrderState == 2 ? "重复支付" : "订单已失效";
                        string companyId = GetCompanyId(model.PKID, string.Empty, string.Empty);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = msg, returnUrl = "/QRCode/Index?companyId=" + companyId + "" });
                    }
                }
                model.OrderID = IdGenerator.Instance.GetId();
                model.Status = OnlineOrderStatus.WaitPay;
                model.OrderType = OnlineOrderType.ParkFee;
                model.Amount = model.Amount;

                BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
                if (company == null) throw new MyException("获取单位信息失败");

                if (model.PaymentChannel == PaymentChannel.AliPay)
                {
                    if (string.IsNullOrWhiteSpace(AliPayUserId))
                    {
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取用户信息失败，请重新扫码进入" });
                    }
                    AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取支付宝配置信息失败[0001]", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该支付宝暂停使用", "单位编号：" + config.CompanyID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用支付宝支付！" });
                    }
                    if (model.OrderSource == PayOrderSource.Platform && (CurrLoginAliPayApiConfig == null || CurrLoginAliPayApiConfig.CompanyID != config.CompanyID))
                    {
                        string loginCompanyId = CurrLoginAliPayApiConfig != null ? CurrLoginAliPayApiConfig.CompanyID : string.Empty;
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "支付宝配置信息和当前支付宝配置信息不匹配，不能支付", string.Format("支付单位：{0},用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                        if (CurrLoginAliPayApiConfig == null)
                        {
                            return RedirectToAction("Index", "ErrorPrompt", new { message = "获取支付宝授权信息失败，请重试！" });
                        }
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "支付宝配置信息和当前支付宝配置信息不匹配，不能支付！" });
                    }
                    model.AccountID = string.Empty;
                    model.CardId = string.Empty;
                    model.PayeeChannel = PaymentChannel.AliPay;
                    model.PaymentChannel = PaymentChannel.AliPay;
                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PayeeAccount;
                    model.Payer = AliPayUserId;
                    model.PayAccount = AliPayUserId;
                    model.CompanyID = config.CompanyID;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(WeiXinOpenId))
                    {
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取用户信息失败，请重新扫码进入" });
                    }

                    WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                    if (config == null)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                    }
                    if (!config.Status)
                    {
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });
                    }
                    if (model.OrderSource == PayOrderSource.Platform && (CurrLoginWeiXinApiConfig == null || config.CompanyID != CurrLoginWeiXinApiConfig.CompanyID))
                    {
                        string loginCompanyId = CurrLoginWeiXinApiConfig != null ? CurrLoginWeiXinApiConfig.CompanyID : string.Empty;
                        ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "车场所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                        if (CurrLoginWeiXinApiConfig == null)
                        {
                            return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信授权信息失败，请重试！" });
                        }
                        return RedirectToAction("Index", "ErrorPrompt", new { message = "车场所属公众号和当前公众号不匹配，不能支付！" });
                    }

                    model.PayeeUser = config.SystemName;
                    model.PayeeAccount = config.PartnerId;
                    model.PayAccount = WeiXinOpenId;
                    model.Payer = model.PayAccount;
                    model.AccountID = model.AccountID;
                    model.CardId = model.AccountID;
                    model.PayeeChannel = PaymentChannel.WeiXinPay;
                    model.PaymentChannel = PaymentChannel.WeiXinPay;
                    model.CompanyID = config.CompanyID;
                }

                bool result = OnlineOrderServices.Create(model);
                if (!result) throw new MyException("生成待缴费订单失败");

                switch (model.PaymentChannel)
                {
                    case PaymentChannel.WeiXinPay:
                        {
                            return RedirectToAction("ParkCarPayment", "WeiXinPayment", new { orderId = model.OrderID, source = 1 });
                        }
                    case PaymentChannel.AliPay:
                        {
                            return RedirectToAction("ParkCarPayment", "AliPayment", new { orderId = model.OrderID, source = 1 });
                        }
                    default: throw new MyException("支付方式错误");
                }
            }
            catch (MyException ex)
            {
                return RedirectToAction("ComputeParkingFee", "QRCodeParkPayment", new { licensePlate = model.PlateNo, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "保存临停缴费订单失败", ex, LogFrom.WeiXin);
                return RedirectToAction("ComputeParkingFee", "QRCodeParkPayment", new { licensePlate = model.PlateNo, RemindUserContent = "提交支付失败" });
            }
        }
        public ActionResult QRCodePaySuccess(decimal orderId)
        {
            OnlineOrder order = OnlineOrderServices.QueryByOrderId(orderId);
            return View(order);
        }

        private string GetParkingId(string pid, string bid, string gid)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(pid))
                {
                    return pid;
                }
                if (!string.IsNullOrWhiteSpace(bid))
                {
                    ParkBox box = ParkBoxServices.QueryByRecordId(bid);
                    if (box != null)
                    {
                        ParkArea area = ParkAreaServices.QueryByRecordId(box.AreaID);
                        if (area != null)
                        {
                            return area.PKID;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(gid))
                {
                    ParkGate gate = ParkGateServices.QueryByRecordId(gid);
                    if (gate != null)
                    {
                        ParkBox box = ParkBoxServices.QueryByRecordId(gate.BoxID);
                        if (box != null)
                        {
                            ParkArea area = ParkAreaServices.QueryByRecordId(box.AreaID);
                            if (area != null)
                            {
                                return area.PKID;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("QRCodeParkPayment", "GetParkingId方法处理异常", ex, LogFrom.WeiXin);

            }
            return string.Empty;
        }


        private string GetCompanyId(string pid, string bid, string gid)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(gid))
                {
                    ParkGate gate = ParkGateServices.QueryByRecordId(gid);
                    if (gate != null)
                    {
                        bid = gate.BoxID;
                    }
                }
                if (!string.IsNullOrWhiteSpace(pid))
                {
                    BaseCompany company = CompanyServices.QueryByParkingId(pid);
                    if (company != null)
                    {
                        return company.CPID;
                    }
                }
                if (!string.IsNullOrWhiteSpace(bid))
                {
                    BaseCompany company = CompanyServices.QueryByBoxID(bid);
                    if (company != null)
                    {
                        return company.CPID;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("QRCodeParkPayment", "GetCompanyId方法处理异常", ex, LogFrom.WeiXin);

            }
            return string.Empty;
        }
    }


}
