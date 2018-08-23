using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Enum;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXKeywordDAL : BaseDAL, IWXKeyword
    {
        public bool Create(WX_Keyword model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WX_Keyword(Keyword,KeywordType,MatchType,ReplyType,[Text],ArticleGroupID,CompanyID,DataStatus,CreateTime)");
            strSql.Append(" values(@Keyword,@KeywordType,@MatchType,@ReplyType,@Text,@ArticleGroupID,@CompanyID,@DataStatus,@CreateTime)");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Keyword", model.Keyword);
                dbOperator.AddParameter("KeywordType", (int)model.KeywordType);
                dbOperator.AddParameter("MatchType", (int)model.MatchType);
                dbOperator.AddParameter("ReplyType", (int)model.ReplyType);
                dbOperator.AddParameter("Text", model.Text);
                dbOperator.AddParameter("ArticleGroupID", model.ArticleGroupID);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("CreateTime", DateTime.Now);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(WX_Keyword model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Keyword set Keyword=@Keyword,KeywordType=@KeywordType,MatchType=@MatchType");
            strSql.Append(",[Text]=@Text,ArticleGroupID=@ArticleGroupID where ID=@ID");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Keyword", model.Keyword);
                dbOperator.AddParameter("KeywordType", (int)model.KeywordType);
                dbOperator.AddParameter("MatchType", (int)model.MatchType);
                dbOperator.AddParameter("Text", model.Text);
                dbOperator.AddParameter("ArticleGroupID", model.ArticleGroupID);
                dbOperator.AddParameter("ID",model.ID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdateReplyType(int id,ReplyType type)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return UpdateReplyType(id, type, dbOperator);
            }
        }
        public bool UpdateReplyType(int id, ReplyType type, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Keyword set ReplyType=@ReplyType where ID=@ID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("ReplyType", (int)type);
            dbOperator.AddParameter("ID", id);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Delete(int id, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Keyword set DataStatus=@DataStatus where ID=@ID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            dbOperator.AddParameter("ID", id);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public WX_Keyword QueryById(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Keyword where ID=@ID and DataStatus!=@DataStatus order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ID", id);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<WX_Keyword>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public WX_Keyword QueryByReplyType(string companyId,ReplyType type, string keyValue)
        {
            return QueryList(companyId,type, keyValue).FirstOrDefault();
        }

        public bool CheckKeyWord(string companyId,string keyValue, ReplyType rType, int? notContainsId = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Keyword where ReplyType=@ReplyType and Keyword=@Keyword and DataStatus!=@DataStatus and CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (notContainsId.HasValue) {
                    strSql.Append(" and ID!=@ID");
                    dbOperator.AddParameter("ID", notContainsId.Value);
                }
                dbOperator.AddParameter("CompanyID", companyId);
                dbOperator.AddParameter("ReplyType", (int)rType);
                dbOperator.AddParameter("Keyword", keyValue);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public List<WX_Keyword> QueryALL(string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Keyword where DataStatus!=@DataStatus and CompanyID=@CompanyID order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Keyword> models = new List<WX_Keyword>();
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Keyword>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<WX_Keyword> QueryKeyWordByIds(List<int> ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from WX_Keyword where id in('{0}') and DataStatus!=@DataStatus order by ID desc",string.Join("','",ids));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Keyword> models = new List<WX_Keyword>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Keyword>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<WX_Keyword> QueryByReplyType(string companyId,ReplyType type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Keyword where ReplyType=@ReplyType and  DataStatus!=@DataStatus and CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CompanyID", companyId);
                dbOperator.AddParameter("ReplyType", (int)type);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                strSql.Append("  order by ID desc");
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Keyword> models = new List<WX_Keyword>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Keyword>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<WX_Keyword> QueryList(string companyId,ReplyType type, string keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Keyword where ReplyType=@ReplyType and  DataStatus!=@DataStatus and CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(keyValue))
                {
                    strSql.Append(" AND  ((Keyword=@Keyword AND MATCHTYPE=@MATCHTYPE) OR (MATCHTYPE=@MATCHTYPE1 AND Keyword LIKE @Keyword1))");
                    dbOperator.AddParameter("Keyword", keyValue);
                    dbOperator.AddParameter("MATCHTYPE", (int)MatchType.Equal);
                    dbOperator.AddParameter("MATCHTYPE1", (int)MatchType.Contains);
                    dbOperator.AddParameter("Keyword1", "%" + keyValue + "%");
                }
                dbOperator.AddParameter("CompanyID", companyId);
                dbOperator.AddParameter("ReplyType", (int)type);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                strSql.Append("  order by ID desc");
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Keyword> models = new List<WX_Keyword>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Keyword>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
