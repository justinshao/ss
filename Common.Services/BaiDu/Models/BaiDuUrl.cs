using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common.Services.BaiDu
{
    public class BaiDuUrl
    {
        public static string BaiDuMapAk = ConfigurationManager.AppSettings["BaiDuMapAK"];
        public static string RoundSearchUrl = "http://api.map.baidu.com/place/v2/search?query={0}&location={1},{2}&radius={3}&output={4}&ak=" + BaiDuMapAk + "";
        public static string LocationUrl = "http://api.map.baidu.com/location/ip?&coor=bd09ll&ak=" + BaiDuMapAk + "";
        public static string PlaceSuggestionUrl = "http://api.map.baidu.com/place/v2/suggestion?query={0}&region={1}&location={2}&output={3}&ak=" + BaiDuMapAk + "";
        public static string RoutematrixUrl = "http://api.map.baidu.com/direction/v1/routematrix?output=json&origins={0},{1}&destinations={2},{3}&ak=" + BaiDuMapAk + "";
        public static string PlaceDetailUrl = "http://api.map.baidu.com/place/v2/detail?uid={0}&output=json&scope=2&ak=" + BaiDuMapAk + "";
        public static string DirectionUrl = "http://api.map.baidu.com/direction/v1?mode=driving&origin={0}&destination={1}&origin_region={2}&destination_region={2}&output=json&ak=" + BaiDuMapAk + "";
        public static string GeocoderUrl = "http://api.map.baidu.com/geocoder/v2/?location={0},{1}&output=json&pois=0&ak=" + BaiDuMapAk + "";
    }
}
