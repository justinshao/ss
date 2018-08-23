using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinInerface;
using Common.Services;
using Common.Entities;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities.Enum;
using System.Configuration;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 扫描进出
    /// </summary>
    public class ScanCodeInOutController : WeiXinController
    {
        public ActionResult Index(string pid, string io, int source = 0)
        {
            try
            {
                if (SourceClient != RequestSourceClient.AliPay && SourceClient != RequestSourceClient.WeiXin)
                {
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "请在微信或支付宝中打开" });
                }
                TxtLogServices.WriteTxtLogEx("ScanCodeInOut", string.Format("微信编号：{0}车场编号：{1},通道编号：{2}", WeiXinOpenId, pid, io));

                BaseCompany company = CompanyServices.QueryByParkingId(pid);
                if (company == null) throw new MyException("获取单位信息失败");

                string plateNumber = string.Empty;
                if (SourceClient == RequestSourceClient.WeiXin)
                {
                    if (string.IsNullOrWhiteSpace(WeiXinOpenId))
                    {
                        string id = string.Format("ScanCodeInOut_Index_pid={0}^io={1}^companyId={2}^source={3}", pid, io, company.CPID, source);
                        return RedirectToAction("Index", "WeiXinAuthorize", new { id = id });
                    }
                    plateNumber = WeiXinOpenId;
                }
                if (SourceClient == RequestSourceClient.AliPay)
                {
                    if (string.IsNullOrWhiteSpace(AliPayUserId))
                    {

                        string id = string.Format("ScanCodeInOut_Index_pid={0}^io={1}^companyId={2}^source={3}", pid, io, company.CPID, source);
                        return RedirectToAction("Index", "AliPayAuthorize", new { id = id });
                    }
                    plateNumber = AliPayUserId;
                }
                if (source == 0)
                {
                    return RedirectToAction("ComputeParkingFee", "QRCodeParkPayment", new { pid = pid, gid = io });
                }
                int result = RechargeService.WXScanCodeInOut(pid, io, plateNumber);
                TxtLogServices.WriteTxtLogEx("ScanCodeInOut", "result:" + result);

                if (result == 0)
                {
                    return RedirectToAction("InSuccess");
                }
                if (result == 1)
                {
                    return RedirectToAction("Index", "QRCodeParkPayment", new { pid = pid, pn = plateNumber });
                }
                else
                {
                    string message = ErrorDescription(result);
                    TxtLogServices.WriteTxtLogEx("ScanCodeInOut", "message:" + message);
                    return RedirectToAction("Index", "ErrorPrompt", new { message = message, returnUrl = "", ShowCustomerServicePhone = true });
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("ScanCodeInOut", "ScanCodeInOut方法处理异常", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "跳转链接失败" });
            }
        }
        //1001参数错误、1002扫码入场失败、1003代理连接断开，1004通道无车
        private string ErrorDescription(int type)
        {
            switch (type)
            {
                case 1001:
                    {
                        return "参数错误";
                    }
                case 1002:
                    {
                        return "扫码入场失败";
                    }
                case 1003:
                    {
                        return "代理连接断开";
                    }
                case 1004:
                    {
                        return "未识别到你的爱车";
                    }
                default: return "未知错误";
            }
        }
        public ActionResult InSuccess()
        {
            return View();
        }
    }
}
