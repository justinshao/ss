using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.Helpers;
using Common.Entities;

namespace Common.Services.BaiDu
{
    public class BaiDuLocationService
    {
        /// <summary>
        /// 位置查询
        /// </summary>
        /// <returns></returns>
        public static BaiDuUserLocation GetLocation()
        {
            string result = HttpHelper.Get(BaiDuUrl.LocationUrl);
            BaiDuUserLocation model = JsonHelper.GetJson<BaiDuUserLocation>(result);
            if (!model.IsSuccess)
            {

                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取位置信息", "状态：" + model.status.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取位置信息失败", "状态：" + model.status.ToString(), LogFrom.WeiXin);
               
            }
            return model;
        }
        public static string GetCity(string lat, string lng)
        {
            string result = HttpHelper.Get(BaiDuUrl.GeocoderUrl.ToFormat(lat, lng));
            BaiDuGeocoder model = JsonHelper.GetJson<BaiDuGeocoder>(result);
            if (!model.IsSuccess || model.result == null || model.result.addressComponent == null)
            {
                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取位置信息", "状态：" + model.status.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取城市名称失败", "状态：" + model.status.ToString(), LogFrom.WeiXin);
                return string.Empty;
            }
            return model.result.addressComponent.city;
        }

        /// <summary>
        /// 关注字查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="region"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static PlaceSuggestion GetPlaceSuggestion(string query, string region, string location)
        {

            string url = BaiDuUrl.PlaceSuggestionUrl.ToFormat(query, region, location, "json");
            string result = HttpHelper.Get(url);
            PlaceSuggestion model = JsonHelper.GetJson<PlaceSuggestion>(result);
            if (!model.IsSuccess)
            {
                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取关注字查询", "消息：" + model.message.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取关注字查询", "消息：" + model.message.ToString(), LogFrom.WeiXin);
            }
            return model;
        }
        /// <summary>
        /// 周边查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static BaiDuParking RoundSearch(string keyword, string lat, string lng, int radius)
        {
            var url = BaiDuUrl.RoundSearchUrl.ToFormat(keyword, lat, lng, radius, "json");
            string result = HttpHelper.Get(url);
            BaiDuParking model = JsonHelper.GetJson<BaiDuParking>(result);
            if (!model.IsSuccess)
            {
                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取周边信息", "消息：" + model.message.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取周边信息", "消息：" + model.message.ToString(), LogFrom.WeiXin);
            }
            return model;
        }
        public static BaiDuPlaceDetail GetBaiDuParkingDetail(string uid)
        {
            var url = BaiDuUrl.PlaceDetailUrl.ToFormat(uid);
            string result = HttpHelper.Get(url);
            BaiDuPlaceDetail model = JsonHelper.GetJson<BaiDuPlaceDetail>(result);
            return model;
        }
        public static BaiDuRoutematrix GetRoutematrix(string formlng, string formlat, string tolng, string tolat)
        {
            string url = BaiDuUrl.RoutematrixUrl.ToFormat(formlat, formlng, tolat, tolng);
            string result = HttpHelper.Get(url);
            BaiDuRoutematrix model = JsonHelper.GetJson<BaiDuRoutematrix>(result);
            if (!model.IsSuccess)
            {
                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取距离", "消息：" + model.message.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取距离", "消息：" + model.message.ToString(), LogFrom.WeiXin);

            }
            return model;
        }
        public static BaiDuDirection GetDirectionInfo(string formlng, string formlat, string tolng, string tolat, string cityName)
        {
            string url = BaiDuUrl.DirectionUrl.ToFormat(string.Format("{0},{1}", formlat, formlng), string.Format("{0},{1}", tolat, tolng), cityName);
            string result = HttpHelper.Get(url);
            BaiDuDirection model = JsonHelper.GetJson<BaiDuDirection>(result);
            if (!model.IsSuccess)
            {
                TxtLogServices.WriteTxtLogEx("BaiDuLocationService", "通过百度接口获取距离", "消息：" + model.message.ToString(), string.Empty, LogFrom.WeiXin);
                ExceptionsServices.AddException("BaiDuLocationService", "通过百度接口获取距离", "消息：" + model.message.ToString(), LogFrom.WeiXin);
            }
            return model;
        }
    }
}
