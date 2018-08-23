using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXApiConfigDAL : BaseDAL, IWXApiConfig
    {
        public bool Create(WX_ApiConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WX_ApiConfig(Domain,ServerIP,SystemName,SystemLogo,AppId,AppSecret,Token,CompanyID,Status,SupportSuperiorPay)");
                strSql.Append(" values(@Domain,@ServerIP,@SystemName,@SystemLogo,@AppId,@AppSecret,@Token,@CompanyID,@Status,@SupportSuperiorPay)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Domain", model.Domain);
                dbOperator.AddParameter("ServerIP", model.ServerIP);
                dbOperator.AddParameter("SystemName", model.SystemName);
                dbOperator.AddParameter("SystemLogo", model.SystemLogo);
                dbOperator.AddParameter("AppId", model.AppId);
                dbOperator.AddParameter("AppSecret", model.AppSecret);
                dbOperator.AddParameter("Token",model.Token);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("SupportSuperiorPay", model.SupportSuperiorPay);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(WX_ApiConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update WX_ApiConfig set Domain=@Domain,ServerIP=@ServerIP,SystemName=@SystemName,AppId=@AppId,AppSecret=@AppSecret,Token=@Token,Status=@Status,SupportSuperiorPay=@SupportSuperiorPay ");
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(model.SystemLogo)) {
                    strSql.Append(",SystemLogo=@SystemLogo");
                    dbOperator.AddParameter("SystemLogo", model.SystemLogo);
                }
                strSql.Append(" where CompanyID=@CompanyID");
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("Domain", model.Domain);
                dbOperator.AddParameter("ServerIP", model.ServerIP);
                dbOperator.AddParameter("SystemName", model.SystemName);
                dbOperator.AddParameter("AppId", model.AppId);
                dbOperator.AddParameter("AppSecret", model.AppSecret);
                dbOperator.AddParameter("Token", model.Token);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("SupportSuperiorPay", model.SupportSuperiorPay);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public WX_ApiConfig QueryByCompanyID(string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_ApiConfig where CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<WX_ApiConfig>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public List<WX_ApiConfig> QueryAll()
        {
            List<WX_ApiConfig> configs = new List<WX_ApiConfig>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_ApiConfig");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                   while(reader.Read())
                    {
                        configs.Add(DataReaderToModel<WX_ApiConfig>.ToModel(reader));
                    }
                }
            }
            return configs;
        }
        public WX_ApiConfig QueryByToKen(string token) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_ApiConfig where Token=@Token");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("Token", token);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<WX_ApiConfig>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool UpdatePayConfig(WX_ApiConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update WX_ApiConfig set PartnerKey=@PartnerKey,PartnerId=@PartnerId,CertPwd=@CertPwd");
                dbOperator.ClearParameters();
              
                if (!string.IsNullOrWhiteSpace(model.CertPath))
                {
                    strSql.Append(",CertPath=@CertPath");
                    dbOperator.AddParameter("CertPath", model.CertPath);
                }
                strSql.Append(" where CompanyID=@CompanyID");
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("PartnerKey", model.PartnerKey);
                dbOperator.AddParameter("PartnerId", model.PartnerId);
                dbOperator.AddParameter("CertPwd", model.CertPwd);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}
