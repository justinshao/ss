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
    public class WXArticleDAL : BaseDAL, IWXArticle
    {

        public bool Create(WX_Article model, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WX_Article(Title,ImagePath,Description,Url,[Text],ArticleType,Sort,GroupID,DataStatus,CreateTime,CompanyID)");
            strSql.Append(" values(@Title,@ImagePath,@Description,@Url,@Text,@ArticleType,@Sort,@GroupID,@DataStatus,@CreateTime,@CompanyID)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("Title", model.Title);
            dbOperator.AddParameter("ImagePath", model.ImagePath);
            dbOperator.AddParameter("Description", model.Description);
            dbOperator.AddParameter("Url", model.Url);
            dbOperator.AddParameter("Text", model.Text);
            dbOperator.AddParameter("ArticleType", (int)model.ArticleType);
            dbOperator.AddParameter("Sort", model.Sort);
            dbOperator.AddParameter("GroupID", model.GroupID);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("CreateTime", DateTime.Now);
            dbOperator.AddParameter("CompanyID", model.CompanyID);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(WX_Article model, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Article set Title=@Title,ImagePath=@ImagePath,Description=@Description,Url=@Url,[Text]=@Text");
            strSql.Append(",ArticleType=@ArticleType,Sort=@Sort,GroupID=@GroupID where ID=@ID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("ID", model.ID);
            dbOperator.AddParameter("Title", model.Title);
            dbOperator.AddParameter("ImagePath", model.ImagePath);
            dbOperator.AddParameter("Description", model.Description);
            dbOperator.AddParameter("Url", model.Url);
            dbOperator.AddParameter("Text", model.Text);
            dbOperator.AddParameter("ArticleType", (int)model.ArticleType);
            dbOperator.AddParameter("Sort", model.Sort);
            dbOperator.AddParameter("GroupID", model.GroupID);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(int id)
        {
            string strSql = "update WX_Article set DataStatus=@DataStatus where ID=@ID";
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ID", id);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(int id, DbOperator dbOperator)
        {
            string strSql = "update WX_Article set DataStatus=@DataStatus where ID=@ID";
            dbOperator.ClearParameters();
            dbOperator.AddParameter("ID", id);
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool DeleteByGroupID(string groupId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return DeleteByGroupID(groupId, dbOperator);
            }
         
        }
        public bool DeleteByGroupID(string groupId, DbOperator dbOperator)
        {
            string strSql = "update WX_Article set DataStatus=@DataStatus where GroupID=@GroupID";
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GroupID", groupId);
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;

        }
        public List<WX_Article> QueryAll(string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Article where DataStatus!=@DataStatus and CompanyID=@CompanyID order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Article> articles = new List<WX_Article>();
                    while (reader.Read())
                    {
                        articles.Add(DataReaderToModel<WX_Article>.ToModel(reader));
                    }
                    return articles;
                }
            }
        }

        public List<WX_Article> QueryByGroupID(string groupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Article where GroupID=@GroupID and DataStatus!=@DataStatus order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GroupID", groupId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Article> articles = new List<WX_Article>();
                    while (reader.Read())
                    {
                        articles.Add(DataReaderToModel<WX_Article>.ToModel(reader));
                    }
                    return articles;
                }
            }
        }

        public WX_Article QueryById(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Article where ID=@ID and DataStatus!=@DataStatus order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ID", id);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<WX_Article>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
    }
}
