using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinInerface;
using Common.Entities.Parking;
using Common.Entities;
using Common.Services;
using Common.Entities.WX;
using Common.Entities.Statistics;
using Common.Utilities.Helpers;
using Common.Entities.Other;
using Common.Utilities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.XFJM
{
    /// <summary>
    /// 消费减免主页
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    [CheckSellerPurview]
    public class XFJMMainController : XFJMController
    {
        public ActionResult Index()
        {
            if (SellerLoginUser == null) {
                return RedirectToAction("Index", "XFJMLogin");
            }
            string sellerName = SellerLoginUser.SellerName;
            if (sellerName.Length > 8) {
                sellerName = sellerName.Substring(0, 7) + "...";
            }
            ViewBag.SellerName = sellerName;
            ViewBag.SellerBalance = (double)SellerLoginUser.Balance + (double)SellerLoginUser.Creditline;
            ViewBag.ParkDerates = ParkSellerDerateServices.WXGetParkDerate(SellerLoginUser.SellerID, SellerLoginUser.VID);
            return View();
        }
        /// <summary>
        /// 获取商家余额度
        /// </summary>
        /// <param name="plateNumber"></param>
        /// <returns></returns>
        public JsonResult GetSellerBalance()
        {
            try
            {
                ParkSeller seller = ParkSellerDerateServices.WXGetSellerInfo(SellerLoginUser.SellerID, SellerLoginUser.VID);
                if (seller != null) {
                    Session["SmartSystem_SellerLoginUser"] = seller;
                }
                double sellerBalance = (double)SellerLoginUser.Balance + (double)SellerLoginUser.Creditline;
                return Json(MyResult.Success("", sellerBalance));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取商家余额失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取商家余额失败"));
            }
        }
        /// <summary>
        /// 获取待打折的车辆
        /// </summary>
        /// <param name="plateNumber"></param>
        /// <returns></returns>
        public JsonResult GetWaitDiscountCar(string plateNumber)
        {
            try
            {
                plateNumber = plateNumber.ToPlateNo();
                List<ParkIORecord> models = ParkSellerDerateServices.WXGetIORecordByPlateNumber(plateNumber, SellerLoginUser.VID, SellerLoginUser.SellerID);
                foreach(var item in models){
                    item.LongTime = ((int)(DateTime.Now - item.EntranceTime).TotalMinutes).ToString()+"分钟";
                }
                return Json(MyResult.Success("", models));
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取待打折的车辆",ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取待打折的车牌失败"));
            }
        }
        /// <summary>
        /// 打折
        /// </summary>
        /// <returns></returns>
        public ActionResult Discount(string IORecordID, string DerateID,decimal DerateMoney)
        {
            try
            {
                ConsumerDiscountResult result = ParkSellerDerateServices.WXDiscountPlateNumber(IORecordID, DerateID, SellerLoginUser.VID, SellerLoginUser.SellerID, DerateMoney);
                return Json(MyResult.Success("", result));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "打折异常", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("打折异常"));
            }
        }
        /// <summary>
        /// 获取优免规则
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParkCarDerate()
        {
            try
            {
                List<ParkDerate> models = ParkSellerDerateServices.WXGetParkDerate(SellerLoginUser.SellerID, SellerLoginUser.VID);
                return Json(MyResult.Success("", models));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取优免规则异常", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取优免规则异常"));
            }
        }
         /// <summary>
        /// 打折记录
        /// </summary>
        /// <returns></returns>
        public ActionResult DiscountRecord()
        {
            ViewBag.StartTime = DateTime.Now.AddDays(-7).Date.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            List<BaseParkinfo> parkinfos = ParkingServices.QueryParkingByVillageId(SellerLoginUser.VID);
            List<EnumContext> parkingContexts = new List<EnumContext>();
            foreach (var item in parkinfos) {
                EnumContext model = new EnumContext();
                model.EnumString = item.PKID;
                model.Description = item.PKName;
                parkingContexts.Add(model);
            }
            ViewBag.CarDerateParkings = parkingContexts;
            ViewBag.CarDerateStatus = EnumHelper.GetEnumContextList(typeof(CarDerateStatus), true);
           
            return View();
        }
        /// <summary>
        /// 打折记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDiscountRecordData(string parkingId, string plateNumber, DateTime? starttime, DateTime? endtime, int status, int page)
        {
            try
            {
                int pageSize = 10;
                InParams inparams = new InParams();
                inparams.PlateNumber = plateNumber;
                inparams.StartTime = starttime.HasValue?starttime.Value:DateTime.MinValue;
                inparams.EndTime = endtime.HasValue ? endtime.Value : DateTime.MinValue;
                inparams.Status = status;
                inparams.SellerID = SellerLoginUser.SellerID;
                inparams.ParkingID = parkingId;

                Pagination model = ParkSellerDerateServices.WXGetParkCarDerate(JsonHelper.GetJsonString(inparams), pageSize, page);
                List<ParkCarDerate> carDerateLists = new List<ParkCarDerate>();
                if (model != null && model.CarDerateList != null) {
                    carDerateLists = model.CarDerateList;
                }
                var models = from p in carDerateLists
                             select new
                             {
                                 CarDerateID = p.CarDerateID,
                                 CarDerateNo = p.CarDerateNo,
                                 DerateID = p.DerateID,
                                 FreeTime = p.FreeTime,
                                 FreeMoney = p.FreeMoney,
                                 PlateNumber = p.PlateNumber,
                                 Status = p.Status.GetDescription(),
                                 CreateTime=p.CreateTimeToString,
                                 SellerName = p.SellerName,
                                 RuleName = p.RuleName,
                                 ParkingName = p.ParkingName,
                                 ExpiryTimeToString = p.ExpiryTimeToString
                             };
                return Json(MyResult.Success("", models));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXin_XFJM", "获取打折记录失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取打折记录失败"));
            }
        }
        /// <summary>
        /// 取消打折
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelDiscount()
        {
            return View();
        }
    }
}
