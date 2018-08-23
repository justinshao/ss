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
    public class WXInteractionInfoDAL : IWXInteractionInfo
    {
        public bool Add(WX_InteractionInfo model) {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WX_InteractionInfo(ReplyID,OpenID,MsgType,InteractionContent,CreateTime,CompanyID)");
                strSql.Append(" values(@ReplyID,@OpenID,@MsgType,@InteractionContent,@CreateTime,@CompanyID)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ReplyID", model.ReplyID);
                dbOperator.AddParameter("OpenID", model.OpenID);
                dbOperator.AddParameter("MsgType", model.MsgType);
                dbOperator.AddParameter("InteractionContent", model.InteractionContent);
                dbOperator.AddParameter("CreateTime", model.CreateTime);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public int QueryMaxIdByOpenId(string openId) {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strSql = "select max(id) id from WX_InteractionInfo where openId=@openId";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("openId", openId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return reader.GetInt32DefaultZero(0);
                    }
                    return 0;
                }
            }
        }
    }
}
