using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXMenuAccessRecordDAL : IWXMenuAccessRecord
    {
        public bool Create(WX_MenuAccessRecord model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WX_MenuAccessRecord(MenuName,OpenID,AccessTime,CompanyID)");
            strSql.Append(" values(@MenuName,@OpenID,@AccessTime,@CompanyID)");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MenuName", model.MenuName);
                dbOperator.AddParameter("OpenID",model.OpenID);
                dbOperator.AddParameter("AccessTime", DateTime.Now);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}