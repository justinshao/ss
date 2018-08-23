using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository;
using Common.Entities.WX;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Enum;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXMenuDAL : BaseDAL, IWXMenu
    {

        public bool Create(WX_Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WX_Menu(MenuName,Url,KeywordId,MenuType,Sort,MasterID,DataStatus,CreateTime,CompanyID,MinIprogramAppId,MinIprogramPagePath)");
            strSql.Append(" values(@MenuName,@Url,@KeywordId,@MenuType,@Sort,@MasterID,@DataStatus,@CreateTime,@CompanyID,@MinIprogramAppId,@MinIprogramPagePath)");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MenuName", model.MenuName);
                dbOperator.AddParameter("Url", model.Url);
                dbOperator.AddParameter("KeywordId", model.KeywordId);

                dbOperator.AddParameter("MenuType", (int)model.MenuType);
                dbOperator.AddParameter("Sort", model.Sort);
                dbOperator.AddParameter("MasterID", model.MasterID);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("CreateTime", DateTime.Now);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                dbOperator.AddParameter("MinIprogramAppId", model.MinIprogramAppId);
                dbOperator.AddParameter("MinIprogramPagePath", model.MinIprogramPagePath);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(WX_Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set MenuName=@MenuName,Url=@Url,KeywordId=@KeywordId,MenuType=@MenuType,Sort=@Sort,MasterID=@MasterID,MinIprogramAppId=@MinIprogramAppId,MinIprogramPagePath=@MinIprogramPagePath");
            strSql.Append(" where ID=@ID");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ID", model.ID);
                dbOperator.AddParameter("MenuName", model.MenuName);
                dbOperator.AddParameter("Url", model.Url);

                dbOperator.AddParameter("KeywordId", model.KeywordId);

                dbOperator.AddParameter("MenuType", (int)model.MenuType);
                dbOperator.AddParameter("Sort", model.Sort);

                dbOperator.AddParameter("MasterID", model.MasterID);

                dbOperator.AddParameter("MinIprogramAppId", model.MinIprogramAppId);
                dbOperator.AddParameter("MinIprogramPagePath", model.MinIprogramPagePath);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdateMenuName(int menuId, string menuName, int seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set MenuName=@MenuName,Sort=@Sort");
            strSql.Append(" where ID=@ID");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MenuName", menuName);
                dbOperator.AddParameter("ID", menuId);
                dbOperator.AddParameter("Sort", seq);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdateMenuKeyId(int menuId, int? keyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set KeywordId=@KeywordId");
            strSql.Append(" where ID=@ID");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ID", menuId);
                if (keyId.HasValue)
                {
                    dbOperator.AddParameter("KeywordId", keyId.Value);
                }
                else
                {
                    dbOperator.AddParameter("KeywordId", DBNull.Value);
                }
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdateMenuKeyId(int menuId, int? keyId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set KeywordId=@KeywordId");
            strSql.Append(" where ID=@ID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("ID", menuId);
            if (keyId.HasValue)
            {
                dbOperator.AddParameter("KeywordId", keyId.Value);
            }
            else
            {
                dbOperator.AddParameter("KeywordId", DBNull.Value);
            }
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool UpdateMenuKeyIdToNull(string companyId, int keyId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set KeywordId=@KeywordId where KeywordId=@KeywordId1 and CompanyID=@CompanyID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("KeywordId", DBNull.Value);
            dbOperator.AddParameter("KeywordId1", keyId);
            dbOperator.AddParameter("CompanyID", companyId);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(int menuId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WX_Menu set DataStatus=@DataStatus where ID=@ID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("ID", menuId);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public List<WX_Menu> GetMenus(string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Menu where DataStatus!=@DataStatus and CompanyID=@CompanyID order by ID desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Menu> models = new List<WX_Menu>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Menu>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<WX_Menu> GetMenuByKeyId(string companyId, MenuType type, int keyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WX_Menu where MenuType=@MenuType and KeywordId=@KeywordId and  DataStatus!=@DataStatus and CompanyID=@CompanyID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MenuType", (int)type);
                dbOperator.AddParameter("KeywordId", keyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<WX_Menu> models = new List<WX_Menu>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<WX_Menu>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

    }
}
