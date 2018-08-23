using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;

namespace Common.Services
{
    public class CityServices
    {
        public static List<BaseProvince> QueryAllProvinces() {
            ICity factory = CityFactory.GetFactory();
            return factory.QueryAllProvinces();
        }

        public static List<BaseCity> QueryAllCitys() {
            ICity factory = CityFactory.GetFactory();
            return factory.QueryAllCitys();
        }

        public static List<BaseCity> QueryCitysByProvinceId(int provinceId) {
            ICity factory = CityFactory.GetFactory();
            return factory.QueryCitysByProvinceId(provinceId);
        }

        public static BaseCity QueryCitysByCityId(int cityId)
        {
            ICity factory = CityFactory.GetFactory();
            return factory.QueryCitysByCityId(cityId);
        }
    }
}
