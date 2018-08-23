using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;
using Common.Entities;
using System.Configuration;
using System.Text;
namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 选择车牌控件
    /// </summary>
    public class LicensePlateController : Controller
    {
        private static string firstPlateNo = ConfigurationManager.AppSettings["WeiXinDefaultFirstPlateNo"] ?? "粤B";
        public ActionResult Index(string plateNumber,string parkingId)
        {
            string plateCity = string.Empty;
            string plateArea = string.Empty;
            string number = string.Empty;
            if (!string.IsNullOrWhiteSpace(plateNumber) && plateNumber.Length>=5)
            {
                plateCity = plateNumber.Substring(0, 1);
                plateArea = plateNumber.Substring(1, 1);
                number = plateNumber.Substring(2);
            }
            else {
                if (!string.IsNullOrWhiteSpace(parkingId))
                {
                    BaseParkinfo park = ParkingServices.QueryParkingByParkingID(parkingId);
                    if (park != null && !string.IsNullOrWhiteSpace(park.DefaultPlate) && park.DefaultPlate.Length == 2)
                    {
                        plateCity = park.DefaultPlate.Substring(0, 1);
                        plateArea = park.DefaultPlate.Substring(1, 1);
                    }
                }
                if ((string.IsNullOrWhiteSpace(plateCity) || string.IsNullOrWhiteSpace(plateArea)) && !string.IsNullOrWhiteSpace(firstPlateNo) && firstPlateNo.Length == 2)
                {
                    TxtLogServices.WriteTxtLogEx("LicensePlate", "firstPlateNo:" + firstPlateNo);
                    plateCity = firstPlateNo.Substring(0, 1);
                    plateArea = firstPlateNo.Substring(1, 1);
                }
            }
            TxtLogServices.WriteTxtLogEx("LicensePlate", string.Format("plateCity:{0},plateArea:{1},number:{2}", plateCity, plateArea, number));
            ViewBag.city = string.IsNullOrWhiteSpace(plateCity) ? "粤" : plateCity;
            ViewBag.area = string.IsNullOrWhiteSpace(plateArea) ? "B" : plateArea;
            ViewBag.number = number.ToPlateNo();
            return PartialView();
        }

    }
}
