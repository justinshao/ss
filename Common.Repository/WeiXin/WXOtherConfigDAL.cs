using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities.WX;
using Common.Entities.Enum;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository.WeiXin
{
    public class WXOtherConfigDAL : BaseDAL, IWXOtherConfig
    {
        public bool Create(WX_OtherConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WX_OtherConfig(ConfigType,ConfigValue,CompanyID)");
                strSql.Append(" values(@ConfigType,@ConfigValue,@CompanyID)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ConfigType", (int)model.ConfigType);
                dbOperator.AddParameter("ConfigValue", model.ConfigValue);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(WX_OtherConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update WX_OtherConfig set ConfigValue=@ConfigValue where ConfigType=@ConfigType");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ConfigType", (int)model.ConfigType);
                dbOperator.AddParameter("ConfigValue", model.ConfigValue);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public WX_OtherConfig QueryByConfigType(string companyId,ConfigType type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_OtherConfig where ConfigType=@ConfigType and CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ConfigType", (int)type);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<WX_OtherConfig>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<WX_OtherConfig> QueryAll(string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_OtherConfig where CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CompanyID", companyId);
                List<WX_OtherConfig> models = new List<WX_OtherConfig>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_OtherConfig>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
