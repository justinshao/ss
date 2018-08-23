using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class CityController : Controller
    {
        /// <summary>
        /// 获取省份信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllProvinces()
        {
            JsonResult json = new JsonResult();
            json.Data = CityServices.QueryAllProvinces();
            return json;
        }



        /// <summary>
        /// 根据省份获取下面市区
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public JsonResult GetCitys(int provinceId)
        {
            JsonResult json = new JsonResult();
            json.Data = CityServices.QueryCitysByProvinceId(provinceId);
            return json;
        }
        public JsonResult GetSelectProvincesByCityId(int cityId) {
            try
            {
                BaseCity city = CityServices.QueryCitysByCityId(cityId);
                if (city != null)
                {
                    return Json(MyResult.Success(string.Empty, city.ProvinceID.ToString()));
                }
                return Json(MyResult.Error("所选择城市不存在", string.Empty));
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "根据城市编号获取城市信息失败");
                return Json(MyResult.Error("获取城市信息失败",string.Empty));
            }
        }
    }
}
