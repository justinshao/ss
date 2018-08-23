using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.Enum;
using Common.Services;
using Common.Entities;
using Common.Services.Park;
using SmartSystem.Interface.Services;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities.Parking;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 扫码优免
    /// </summary>
    public class QRCodeDerateController : WeiXinController
    {
        /// <summary>
        /// 扫码优免进入
        /// </summary>
        /// <param name="vid">小区编号</param>
        /// <param name="qid">二维码编号</param>
        /// <param name="type">0-长久二维码 1-临时二维码</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public ActionResult Index(string vid, string qid,string sign)
        {
            TxtLogServices.WriteTxtLogEx("QRCodeDerate", "进入扫码打折：vid={0},qid={1},sign={2}", vid, qid, sign);
            try
            {
                if (SourceClient != RequestSourceClient.WeiXin)
                {
                    throw new MyException("请在微信中打开");
                }
                if (!CheckSignature(vid, qid,sign))
                {
                    throw new MyException("验证签名失败");
                }

                if (SourceClient == RequestSourceClient.WeiXin)
                {
                    if (string.IsNullOrWhiteSpace(WeiXinOpenId))
                    {
                        ParkDerate derate = ParkDerateServices.Query(qid);
                        if (derate == null) throw new MyException("获取优免券信息失败");

                        ParkSeller seller = ParkSellerServices.QueryBySellerId(derate.SellerID);
                        if (seller == null) throw new MyException("获取商家信息失败");

                        BaseVillage village = VillageServices.QueryVillageByRecordId(seller.VID);
                        if (village == null) throw new MyException("获取小区信息失败");

                        string id = string.Format("QRCodeDerate_Index_vid={0}^qid={1}^sign={2}^companyId={3}", vid, qid,sign, village.CPID);
                        return RedirectToAction("Index", "WeiXinAuthorize", new { id = id });
                    }
                }
                ViewBag.PlateNumber = OnlineOrderServices.QueryLastPaymentPlateNumber(PaymentChannel.WeiXinPay, WeiXinOpenId);
                ViewBag.VillageId = vid;
                ViewBag.QId = qid;
                return View();
            }
            catch (MyException ex) {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", "扫码打折异常：描述：{0}", ex.Message);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", "扫码打折异常：描述：{0}，明细：{1}",ex.Message,ex.StackTrace);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "扫码失败，未知异常" });
            }
        }
        private bool CheckSignature(string vid, string qid, string sign)
        {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(vid))
                throw new MyException("签名参数不正确");
            paraMap.Add("vid", vid);

            if (string.IsNullOrWhiteSpace(qid))
                throw new MyException("签名参数不正确");
            paraMap.Add("qid", qid);

            if (string.IsNullOrWhiteSpace(sign))
                throw new MyException("签名参数不正确");
            paraMap.Add("sign", sign);

            if (SignatureServices.Signature(paraMap) == paraMap["sign"])
            {
                return true;
            }
            return false;
        }
        public ActionResult SubmitDerate(string vid, string qid,string plateNumber)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", "licensePlate:{0},villageId:{1},QId:{2}", plateNumber, vid, qid);

                ParkDerateQRcode qrCode = ParkDerateQRcodeServices.QueryByRecordId(qid);
                if (qrCode == null) throw new MyException("二维码不存在");

                if (qrCode.DerateQRcodeType != 0 && qrCode.DerateQRcodeType != 1)
                {
                    throw new MyException("优免类型不正确");
                }
                if (qrCode.DerateQRcodeType == 0)
                {
                    TxtLogServices.WriteTxtLogEx("QRCodeDerate", "长久二维码开始提交优免");

                    //长久二维码
                    string errorMsg = string.Empty;
                    string parkingId = string.Empty;
                    bool result = false;//DiscountServices.NewQRCodeDiscount(villageId, QId, licensePlate, out errorMsg, out parkingId);
                    TxtLogServices.WriteTxtLogEx("QRCodeDerate", "长久二维码开始提交优免结果：{0}，消息：{1}", result.ToString(), errorMsg);

                    if (!string.IsNullOrWhiteSpace(errorMsg))
                    {
                        return RedirectToAction("BrowseError", "QRCodeDerate", new { errorMsg = errorMsg });
                    }
                    if (!result) throw new Exception("保存优免信息出错!");

                    if (!string.IsNullOrWhiteSpace(parkingId))
                    {
                        //PKParkinfo parking = ParkingServices.GetParkinfo(parkingId);
                        //if (parking == null || parking.IsMobilePayment == 0)
                        //{
                        //    parkingId = string.Empty;
                        //}
                    }
                    return RedirectToAction("DerateSuccess", "QRCodeDerate", new { msg = "", parkingId = parkingId, plateNumber = plateNumber });
                }
                else
                {
                    TxtLogServices.WriteTxtLogEx("QRCodeDerate", "临时二维码开始提交优免");
                    //临时二维码
                    string errorMsg = string.Empty;
                    string parkingId = string.Empty;
                    bool result = false;//DiscountServices.NewQRCodeCarderateDiscount(villageId, QId, licensePlate, out errorMsg, out parkingId);
                     TxtLogServices.WriteTxtLogEx("QRCodeDerate","临时二维码提交优免结果：{0}，消息：{1}", result.ToString(), errorMsg);

                    if (!string.IsNullOrWhiteSpace(errorMsg))
                    {
                        return RedirectToAction("BrowseError", "QRCodeDerate", new { errorMsg = errorMsg });
                    }
                    if (!result) throw new Exception("保存优免信息出错!");
                    if (!string.IsNullOrWhiteSpace(parkingId))
                    {
                        //PKParkinfo parking = ParkingServices.GetParkinfo(parkingId);
                        //if (parkingId == null || parking.IsMobilePayment == 0 || parking.IsSupportBSGOnLinePay == 0)
                        //{
                        //    parkingId = string.Empty;
                        //}
                    }
                    return RedirectToAction("DerateSuccess", "QRCodeDerate", new { msg = "", parkingId = parkingId, plateNumber = plateNumber });
                }
            }
            catch (MyException ex)
            {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", "保存优免信息出错：描述：{0}", ex.Message);
                return RedirectToAction("Index", "ErrorPrompt", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("QRCodeDerate", "保存优免信息出错", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "提交优免信息出错" });
            }
        }
        public ActionResult DerateSuccess(string msg, string parkingId, string licensePlate)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", string.Format("领取成功，parkingId：{0}，licensePlate：{1}", parkingId, licensePlate));
                ViewBag.Msg = msg;
                string hrefUrl = string.Empty;

                BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(parkingId);
                if (parking == null) throw new MyException("获取车场信息失败");

                if (parking.MobilePay == YesOrNo.Yes) {

                    string companyId = CompanyServices.GetCompanyId(parkingId, string.Empty);
                    if (string.IsNullOrWhiteSpace(companyId)) throw new MyException("获取单位编号失败");

                    WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                    if (config == null) throw new MyException("获取微信配置失败");

                    hrefUrl = string.Format("{0}/qrl/qrp_ix_pid={1}^pn={2}", SystemDefaultConfig.SystemDomain, parkingId, licensePlate);
                }
                ViewBag.HrefUrl = hrefUrl;
                ViewBag.LicensePlate = licensePlate;
                ViewBag.DerateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return View();
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("QRCodeDerate", "提示优免结果出错", ex);
                return RedirectToAction("BrowseError", "QRCodeDerate", new { errorMsg = "领取优免券成功，但是处理其他业务异常了【不影响优免券的正常使用】" });
            }

        }
        public ActionResult BrowseError(string errorMsg)
        {
            ViewBag.ErrorMessage = errorMsg;
            return View();
        }
    }
}
