using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Services;
using Common.Entities.WX;
using Common.Services.WeiXin;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 预停车
    /// 链接地址  域名+/l/AdvanceParking_Index
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    public class AdvanceParkingController : WeiXinController
    {
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult SaveAdvanceParking(string plateNo,DateTime startTime,DateTime endTime,decimal amount) {
            try
            {
                if (string.IsNullOrWhiteSpace(plateNo)) throw new MyException("获取车牌号失败");
                if (startTime == null || startTime== DateTime.MinValue) throw new MyException("获取开始时间失败");
                if (endTime == null || endTime == DateTime.MinValue) throw new MyException("获取结束时间失败");
                if (amount <= 0) throw new MyException("获取预支付金额失败");

                //BaseCompany company = CompanyServices.QueryByParkingId(model.PKID);
                //if (company == null) throw new MyException("获取单位信息失败");

                //WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(company.CPID);
                //if (config == null)
                //{
                //    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取微信配置信息失败", "单位编号：" + company.CPID, LogFrom.WeiXin);
                //    return RedirectToAction("Index", "ErrorPrompt", new { message = "获取微信配置信息失败！" });
                //}
                //if (!config.Status)
                //{
                //    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "该车场暂停使用微信支付", "单位编号：" + company.CPID, LogFrom.WeiXin);
                //    return RedirectToAction("Index", "ErrorPrompt", new { message = "该车场暂停使用微信支付！" });
                //}
                //if (config.CompanyID != WeiXinUser.CompanyID)
                //{
                //    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "微信用户所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, WeiXinUser.CompanyID), LogFrom.WeiXin);
                //    return RedirectToAction("Index", "ErrorPrompt", new { message = "微信用户所属公众号和当前公众号不匹配，不能支付！" });
                //}
                //if (CurrLoginWeiXinApiConfig == null || config.CompanyID != CurrLoginWeiXinApiConfig.CompanyID)
                //{
                //    string loginCompanyId = CurrLoginWeiXinApiConfig != null ? CurrLoginWeiXinApiConfig.CompanyID : string.Empty;
                //    ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "车场所属公众号和当前公众号不匹配，不能支付", string.Format("支付单位：{0},微信用户单位：{1}", config.CompanyID, loginCompanyId), LogFrom.WeiXin);
                //    return RedirectToAction("Index", "ErrorPrompt", new { message = "车场所属公众号和当前公众号不匹配，不能支付！" });
                //}

                AdvanceParking model = new AdvanceParking();
                model.OrderId = IdGenerator.Instance.GetId();
                model.OrderState = 0;
                model.PlateNo = plateNo;
                model.StartTime = startTime;
                model.EndTime = endTime;
                model.Amount = amount;
                model.WxOpenId = WeiXinUser.OpenID;
                model.CompanyID = WeiXinUser.CompanyID;
                model.CreateTime = DateTime.Now;
               bool result =  AdvanceParkingServices.Add(model);
               if (!result) throw new MyException("保存预停车信息失败");
               return RedirectToAction("AdvanceParkingPayment", "WeiXinPayment", new { orderId = model.OrderId});
              
            }
            catch (MyException ex) {
                return PageAlert("Index", "AdvanceParking", new { RemindUserContent = ex.Message });
  
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "保存预停车信息失败", ex, LogFrom.WeiXin);
                return PageAlert("Index", "AdvanceParking", new { RemindUserContent = "保存预停车信息失败" });
            }
        }
        public ActionResult PaymentSuccess(decimal orderId) {
            AdvanceParking order = AdvanceParkingServices.QueryByOrderId(orderId);
            return View(order);
        }
    }
}
