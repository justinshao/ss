using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface ICity
    {
        List<BaseProvince> QueryAllProvinces();

        List<BaseCity> QueryAllCitys();

        List<BaseCity> QueryCitysByProvinceId(int provinceId);

        BaseCity QueryCitysByCityId(int city);
    }
}
