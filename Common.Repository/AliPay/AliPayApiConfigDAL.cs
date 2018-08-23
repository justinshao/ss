using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.AliPay;
using Common.Entities.AliPay;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository.AliPay
{
    public class AliPayApiConfigDAL : BaseDAL, IAliPayApiConfig
    {

        public bool Add(AliPayApiConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Ali_ApiConfig(RecordId,SystemName,SystemDomain,AppId,PublicKey,PrivateKey,PayeeAccount,CompanyID,Status,SupportSuperiorPay,AliPaySignType)");
                strSql.Append(" values(@RecordId,@SystemName,@SystemDomain,@AppId,@PublicKey,@PrivateKey,@PayeeAccount,@CompanyID,@Status,@SupportSuperiorPay,@AliPaySignType)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordId", model.RecordId);
                dbOperator.AddParameter("SystemName", model.SystemName);
                dbOperator.AddParameter("SystemDomain", model.SystemDomain);
                dbOperator.AddParameter("AppId", model.AppId);
                dbOperator.AddParameter("PublicKey", model.PublicKey);
                dbOperator.AddParameter("PrivateKey", model.PrivateKey);
                dbOperator.AddParameter("PayeeAccount",model.PayeeAccount);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("SupportSuperiorPay", model.SupportSuperiorPay);
                dbOperator.AddParameter("AliPaySignType", model.AliPaySignType);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(AliPayApiConfig model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Ali_ApiConfig set SystemName=@SystemName,SystemDomain=@SystemDomain,AppId=@AppId,PublicKey=@PublicKey,PrivateKey=@PrivateKey,PayeeAccount=@PayeeAccount,Status=@Status,SupportSuperiorPay=@SupportSuperiorPay,AliPaySignType=@AliPaySignType");
                strSql.Append(" where CompanyID=@CompanyID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("SystemName", model.SystemName);
                dbOperator.AddParameter("SystemDomain", model.SystemDomain);
                dbOperator.AddParameter("AppId", model.AppId);
                dbOperator.AddParameter("PublicKey", model.PublicKey);
                dbOperator.AddParameter("PrivateKey", model.PrivateKey);
                dbOperator.AddParameter("PayeeAccount", model.PayeeAccount);
                dbOperator.AddParameter("Status", model.Status);
                dbOperator.AddParameter("SupportSuperiorPay", model.SupportSuperiorPay);
                dbOperator.AddParameter("AliPaySignType", model.AliPaySignType);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public AliPayApiConfig QueryByRecordID(string recordId)
        {
            string sql = @"select * from Ali_ApiConfig where RecordId=@RecordId";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordId", recordId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<AliPayApiConfig>.ToModel(reader);
                    }
                }
            }
            return null;
        }
        public AliPayApiConfig QueryByCompanyID(string companyId)
        {
            string sql = @"select * from Ali_ApiConfig where CompanyID=@CompanyID";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<AliPayApiConfig>.ToModel(reader);
                    }
                }
            }
            return null;
        }
        public List<AliPayApiConfig> QueryAll() {
            List<AliPayApiConfig> configs = new List<AliPayApiConfig>();
            string sql = @"select * from Ali_ApiConfig";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while(reader.Read())
                    {
                        configs.Add(DataReaderToModel<AliPayApiConfig>.ToModel(reader));
                    }
                }
            }
            return configs;
        }
    }
}
