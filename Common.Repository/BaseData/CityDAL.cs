using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class CityDAL:BaseDAL,ICity
    {
        public List<BaseProvince> QueryAllProvinces()
        {
            string sql = "select ProvinceID,ProvinceName,OrderIndex,Longitude,Lititute,Remark from BaseProvince order by OrderIndex desc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseProvince> models = new List<BaseProvince>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseProvince>.ToModel(reader)); 
                    }
                    return models;
                }
            }
        }

        public List<BaseCity> QueryAllCitys()
        {
            string sql = "select CityID,ProvinceID,CityName,OrderIndex,Longitude,Lititute,Remark from BaseCity order by OrderIndex desc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseCity> models = new List<BaseCity>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseCity>.ToModel(reader)); 
                    }
                    return models;
                }
            }
        }

        public List<BaseCity> QueryCitysByProvinceId(int provinceId)
        {
            string sql = "select CityID,ProvinceID,CityName,OrderIndex,Longitude,Lititute,Remark from BaseCity where ProvinceID=@ProvinceID order by OrderIndex desc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ProvinceID", provinceId);

                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseCity> models = new List<BaseCity>();
                    while (reader.Read())
                    { 
                        models.Add(DataReaderToModel<BaseCity>.ToModel(reader)); 
                    }
                    return models;
                }
            }
        }

        public BaseCity QueryCitysByCityId(int city)
        {
            string sql = "select CityID,ProvinceID,CityName,OrderIndex,Longitude,Lititute,Remark from BaseCity where CityID=@CityID";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CityID", city);

                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseCity> models = new List<BaseCity>();
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCity>.ToModel(reader); 
                    }
                    return null;
                }
            }
        }
    }
}
