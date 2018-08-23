using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Park;
using Common.Entities.WX;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 找内部车
    /// 链接地址 域名+/l/FindInsideCar_Index
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    public class FindInsideCarController : WeiXinController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InsideCarDetail(string plateNo) {
            try
            {
                List<CarParkingResult> result = ParkGrantServices.GetParkGrantByPlateNo(plateNo);
                foreach (var item in result) {
                    if (item.EndTime == DateTime.MinValue && item.StartTime == DateTime.MinValue)
                    {
                        item.StartTime = DateTime.Now;
                    }
                }
                return View(result);
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "找内部车失败", ex, LogFrom.WeiXin);
                return PageAlert("Index", "FindInsideCar", new { RemindUserContent = "找内部车失败" });
            }
        }
    }
}
