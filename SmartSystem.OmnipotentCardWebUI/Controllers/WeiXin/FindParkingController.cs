using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using System.Configuration;
using Common.Services.BaiDu;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinInerface;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 找停车场
    /// </summary>
    //[PageBrowseRecord]
    //[CheckWeiXinPurview(Roles = "Login")]
    public class FindParkingController : WeiXinController
    {
        private static string baiduMapAk = ConfigurationManager.AppSettings["BaiDuMapAK"];
        private static string searchParkingRadius = ConfigurationManager.AppSettings["SearchParkingRadius"] ?? "4000";
        public ActionResult Index()
        {
            try
            {
                ViewBag.BaiDuMapAk = baiduMapAk;
                string locationLatLng = string.Empty;
                bool IsWeiXinLocation = false;
                string cityName = string.Empty;
                if (WeiXinUser != null)
                {
                    WX_UserLocation location = WXUserLocationServices.QueryByOpenId(WeiXinUser.OpenID);
                  
                    if (location != null)
                    {
                        double[] latlng = BaiDuMapHelper.wgs2bd(double.Parse(location.Latitude), double.Parse(location.Longitude));
                        locationLatLng = string.Format("{0},{1}", latlng[0], latlng[1]);
                        IsWeiXinLocation = true;

                        cityName = BaiDuLocationService.GetCity(latlng[0].ToString(), latlng[1].ToString());
                    }

                }
                if (string.IsNullOrWhiteSpace(cityName))
                {
                    BaiDuUserLocation baiduLocation = BaiDuLocationService.GetLocation();

                    if (string.IsNullOrWhiteSpace(locationLatLng))
                        locationLatLng = string.Format("{0},{1}", baiduLocation.content.point.y, baiduLocation.content.point.x);

                    cityName = baiduLocation.content.address_detail.city;
                }
                ViewBag.LocationLatLng = locationLatLng;
                ViewBag.IsWeiXinLocation = IsWeiXinLocation ? "1" : "0";
                ViewBag.CityName = cityName;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "查找车场信息失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "ErrorPrompt", new { message = "查找车场失败" });
            }
        }
        public ActionResult GetParkingSuggestion(string query, string city, string lat, string lng)
        {
            try
            {
                string location = !string.IsNullOrWhiteSpace(lat) && !string.IsNullOrWhiteSpace(lng) ? string.Format("{0},{1}", lat, lng) : string.Empty;
                query = HttpUtility.UrlEncode(query);
                city = HttpUtility.UrlEncode(city);
                PlaceSuggestion model = BaiDuLocationService.GetPlaceSuggestion(query, city, location);
                List<PlaceSuggestionResult> result = model.result.Where(p => p.location != null).Take(8).ToList();
                if (!model.IsSuccess) throw new MyException("查询周边车场失败");

                return Json(MyResult.Success(model.message, result));

            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "根据关键字查询地理名称", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("查询周边车场失败"));
            }
        }
        [HttpPost]
        public ActionResult GeParkingLocation(string lat, string lng)
        {
            try
            {
                List<BaseParkinfo> parks = QueryParkingService.QueryParkinfo(lat.ToDouble(), lng.ToDouble(), int.Parse(searchParkingRadius));
                BaiDuParking result = BaiDuLocationService.RoundSearch("停车场", lat, lng, int.Parse(searchParkingRadius));

                List<BaiDuParkingLocation> models = MergeParking(parks, result);
                return Json(MyResult.Success("获取成功", models));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取车场失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取车场失败"));
            }
        }
        /// <summary>
        /// 合并系统车场和百度车场
        /// </summary>
        /// <param name="parks"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<BaiDuParkingLocation> MergeParking(List<BaseParkinfo> parks, BaiDuParking result)
        {
            List<BaiDuParkingLocation> models = new List<BaiDuParkingLocation>();
            if (result.IsSuccess)
            {
                foreach (var item in result.results)
                {
                    bool needExclude = false;
                    foreach (var park in parks)
                    {
                        if (string.IsNullOrWhiteSpace(park.Coordinate) || !park.Coordinate.Contains(','))
                            continue;

                        var lats = park.Coordinate.Split(',');
                        var distance = XY.DistanceTo(item.location.lat.ToDouble(), item.location.lng.ToDouble(), lats[0].ToDouble(), lats[1].ToDouble());
                        if (distance <= 100)
                        {
                            needExclude = true;
                            break;
                        }
                    }
                    if (!needExclude)
                    {
                        BaiDuParkingLocation model = new BaiDuParkingLocation();
                        model.id = item.uid;
                        model.street_id = item.street_id;
                        model.uid = item.uid;
                        model.lat = item.location.lat;
                        model.lng = item.location.lng;
                        model.name = item.name;
                        model.address = item.address;
                        model.type = 1;
                        models.Add(model);
                    }
                }
            }
            foreach (var park in parks)
            {
                if (string.IsNullOrWhiteSpace(park.Coordinate) || !park.Coordinate.Contains(','))
                    continue;

                var lats = park.Coordinate.Split(',');

                BaiDuParkingLocation model = new BaiDuParkingLocation();
                model.id = park.PKID.ToString();
                model.quantity = park.SpaceBitNum;
                model.lat = lats[0];
                model.lng = lats[1];
                model.name = park.PKName;
                model.address = park.Address;
                model.type = 0;
                models.Add(model);
            }
            return models;
        }
       
        /// <summary>
        /// 车场明细
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="type">0-自己车场 1-百度车场</param>
        /// <returns></returns>
        public ActionResult ParkingDetail(string id, int type, string formlat, string formlng, string tolng, string tolat, string city)
        {
            try
            {
                BaiDuParkingDetail model = new BaiDuParkingDetail();

                BaiDuRoutematrix result = BaiDuLocationService.GetRoutematrix(formlng, formlat, tolng, tolat);
                string distanceDes = string.Empty;
                if (result.IsSuccess && result.result != null && result.result.elements != null && result.result.elements.Count > 0)
                {
                    model.DistanceDes = "当前距离" + result.result.elements[0].distance.text + ",大约需要" + result.result.elements[0].duration.text;
                  
                }

                model.CurrLatitude = formlat;
                model.CurrLongitude = formlng;
                model.ImageUrl = "/Content/mobile/images/parking_default_notimage.png";
                if (type == 0)
                {
                    BaseParkinfo park = QueryParkingService.GetBaseParkinfoByPKID(id);
                    if (park == null) throw new MyException("获取车场明细失败");

                    model.ParkName = park.PKName;
                    model.ParkAddress = park.Address;
                    model.TotalParkQuantity = park.CarBitNum == 0 ? "---" : park.CarBitNum.ToString();
                    model.SurplusParkQuantity =  park.CarBitNum == 0 ? "---":park.SpaceBitNum.ToString();
                    model.ParkType = park.NeedFee== YesOrNo.Yes ? "收费停车场" : "免费停车场";
                    model.PriceInfo = park.FeeRemark;
                    if (!string.IsNullOrWhiteSpace(park.Coordinate) && park.Coordinate.Contains(','))
                    {
                        string[] lnglat = park.Coordinate.Split(',');
                        model.ParkLatitude = lnglat[1];
                        model.ParkLongitude = lnglat[0];
                    }

                }
                else
                {
                    BaiDuPlaceDetail baiduModel = BaiDuLocationService.GetBaiDuParkingDetail(id);
                    if (!baiduModel.IsSuccess || baiduModel.result == null || baiduModel.result.location == null)
                    {
                        throw new MyException("获取车场明细失败");
                    }
                    model.TotalParkQuantity = "---";
                    model.SurplusParkQuantity = "---";
                    model.ParkName = baiduModel.result.name;
                    model.ParkAddress = baiduModel.result.address;
                    model.ParkType = "未知";
                    model.ParkLatitude = baiduModel.result.location.lat;
                    model.ParkLongitude = baiduModel.result.location.lng;
                    model.PriceInfo = "暂无收费信息";
                }
                return Json(MyResult.Success("获取成功", model));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error("获取明细失败"));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "获取车场明细失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取明细失败"));
            }
        }
    }
}
