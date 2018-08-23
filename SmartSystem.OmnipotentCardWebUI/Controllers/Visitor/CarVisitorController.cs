using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using Common.Services;
using Common.Entities;
using Common.Entities.WX;
using Common.Entities.Parking;
using Common.Utilities;
using Common.Services.Park;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Visitor
{
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login,REGISTERACCOUNT")]
    public class CarVisitorController : WeiXinController
    {
        public ActionResult Index()
        {
            try
            {
                ViewBag.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
                ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
                ViewBag.Villages = VillageServices.QueryVillageByEmployeeMobilePhone(WeiXinUser.MobilePhone);

                return View();
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("CarVisitor", "获取访客车场失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "获取访客车场失败" });
            }
          
        }
        /// <summary>
        /// 获取车场数据
        /// </summary>
        /// <param name="plateNumber"></param>
        /// <returns></returns>
        public JsonResult GetParkingData(string villageId)
        {
            try
            {
                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageId(villageId);
                return Json(MyResult.Success("", parkings));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("CarVisitor", "获取车场信息失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取车场信息失败"));
            }
        }
        public ActionResult SaveCarVisitor() {
            try
            {
                VisitorInfo vistor = new VisitorInfo();
                vistor.RecordID = GuidGenerator.GetGuid().ToString();
                if (string.IsNullOrWhiteSpace(Request["villageId"]))
                    throw new MyException("获取小区编号失败");

                if (string.IsNullOrWhiteSpace(Request["plateNo"]))
                    throw new MyException("获取车牌号失败");

                if (string.IsNullOrWhiteSpace(Request["ParkingIds"]))
                {
                    throw new MyException("获取车场编号失败");
                }
                string villageId = Request["villageId"].ToString();

                DateTime start = DateTime.Now;
                if (string.IsNullOrWhiteSpace(Request["startTime"]) || !DateTime.TryParse(Request["startTime"], out start))
                    throw new MyException("获取开始时间失败");

                DateTime end = DateTime.Now;
                if (string.IsNullOrWhiteSpace(Request["endTime"]) || !DateTime.TryParse(Request["endTime"], out end))
                    throw new MyException("获取结束时间失败");

                vistor.PlateNumber = Request["plateNo"].ToString();
                vistor.BeginDate = start;
                vistor.EndDate = end;
                vistor.VisitorCount = int.Parse(Request["VisitorCount"].ToString());
                vistor.VisitorMobilePhone = Request["VisitorMobilePhone"].ToString();
                vistor.AccountID = WeiXinUser.AccountID;
                vistor.CreateTime = DateTime.Now;
                vistor.VisitorState = 1;
                vistor.VID = villageId;
                vistor.IsExamine = 1;
                vistor.OperatorID = WeiXinUser.AccountID;
                vistor.VisitorSource = 1;

                string [] parkingIds = Request["ParkingIds"].ToString().TrimEnd(',').Split(',');
                List<ParkVisitor> carVisitors = new  List<ParkVisitor>();
                for (int i = 0; i < parkingIds.Length; i++) {
                    ParkVisitor car = new ParkVisitor();
                    car.RecordID = GuidGenerator.GetGuid().ToString();
                    car.VisitorID = vistor.RecordID;
                    car.PKID = parkingIds[i];
                    car.VID = villageId;
                    car.AlreadyVisitorCount = 0;
                    carVisitors.Add(car);
                }
                vistor.ParkVisitors = carVisitors;
                bool result = ParkVisitorServices.Add(vistor);
                if (!result) throw new MyException("保存访客信息失败");
                return RedirectToAction("CarVisitorRecord", "CarVisitor");
            }
            catch (MyException ex)
            {
                return PageAlert("Index", "CarVisitor", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("CarVisitor", "获取访客车场失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "获取访客车场失败" });
            }
        }
        public ActionResult CarVisitorRecord() {
            int total = 0;
            List<VisitorInfo> visitors = ParkVisitorServices.GetVisitorInfoPage(WeiXinUser.AccountID,1,50,out total);
            return View(visitors);
        }
        public ActionResult CancelVisitor(string visitorId)
        {
            try{
                bool result = ParkVisitorServices.CancelVisitor(visitorId);
                if (!result) throw new MyException("取消访客信息失败");

                return RedirectToAction("CarVisitorRecord", "CarVisitor", new { RemindUserContent = "取消成功" });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("CarVisitor", "取消访客失败", ex, LogFrom.WeiXin);
                return RedirectToAction("CarVisitorRecord", "CarVisitor", new { message = "取消访客失败" });
            }
        }
    }
}
