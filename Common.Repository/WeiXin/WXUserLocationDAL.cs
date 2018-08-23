using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXUserLocationDAL : IWXUserLocation
    {
        public bool Create(WX_UserLocation model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "insert into WX_UserLocation(OpenId,Latitude,Longitude,[Precision],LastReportedTime,CompanyID) values(@OpenId,@Latitude,@Longitude,@Precision,@LastReportedTime,@CompanyID)";
                dbOperator.AddParameter("OpenId", model.OpenId);
                dbOperator.AddParameter("Latitude", model.Latitude);
                dbOperator.AddParameter("Longitude", model.Longitude);
                dbOperator.AddParameter("Precision", model.Precision);
                dbOperator.AddParameter("LastReportedTime", model.LastReportedTime);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                int result = dbOperator.ExecuteNonQuery(strSql);
                return result > 0;
            }
        }
        public bool Update(WX_UserLocation model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                string strSql = "update WX_UserLocation set Latitude=@Latitude,Longitude=@Longitude,[Precision]=@Precision,LastReportedTime=@LastReportedTime where openid=@openid";
                dbOperator.AddParameter("openId", model.OpenId);
                dbOperator.AddParameter("Latitude", model.Latitude);
                dbOperator.AddParameter("Longitude", model.Longitude);
                dbOperator.AddParameter("Precision", model.Precision);
                dbOperator.AddParameter("LastReportedTime", model.LastReportedTime);
                int result = dbOperator.ExecuteNonQuery(strSql);
                return result > 0;
            }
        }
        public WX_UserLocation QueryByOpenId(string companyId,string openId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strSql = "select id,OpenId,Latitude,Longitude,[Precision],LastReportedTime from WX_UserLocation where openId=@openId and CompanyID=@CompanyID";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("openId", openId);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        return new WX_UserLocation()
                        {
                            ID = reader.GetInt32DefaultZero(0),
                            OpenId = reader.GetStringDefaultEmpty(1),
                            Latitude = reader.GetStringDefaultEmpty(2),
                            Longitude = reader.GetStringDefaultEmpty(3),
                            Precision = reader.GetStringDefaultEmpty(4),
                            LastReportedTime = reader.GetDateTimeDefaultMin(5),

                        };
                    }
                    return null;
                }
            }
        }
        public WX_UserLocation QueryByOpenId(string openId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strSql = "select id,OpenId,Latitude,Longitude,[Precision],LastReportedTime from WX_UserLocation where openId=@openId";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("openId", openId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        return new WX_UserLocation()
                        {
                            ID = reader.GetInt32DefaultZero(0),
                            OpenId = reader.GetStringDefaultEmpty(1),
                            Latitude = reader.GetStringDefaultEmpty(2),
                            Longitude = reader.GetStringDefaultEmpty(3),
                            Precision = reader.GetStringDefaultEmpty(4),
                            LastReportedTime = reader.GetDateTimeDefaultMin(5),

                        };
                    }
                    return null;
                }
            }
        }
    }
}
