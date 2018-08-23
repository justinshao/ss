using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository.WeiXin;
using System.Data;

namespace Common.SqlRepository.WeiXin
{
    public class WXothersDAL : IWXohters
   {
       public WX_Info QueryByOpenId(string openid) {
           StringBuilder sb = new StringBuilder();
           sb.Append("select * from WX_Info where OpenID=@ID");
           using (DbOperator dboperator = ConnectionManager.CreateConnection()) {
               dboperator.ClearParameters();
               dboperator.AddParameter("ID", openid);
               using (DbDataReader reader = dboperator.ExecuteReader(sb.ToString())) {
                   if (reader.Read()) {
                        return DataReaderToModel<WX_Info>.ToModel(reader);
                   }
                   return null;
               }
           }
       }

       public string GetUrl(string plateno) {
           if (string.IsNullOrEmpty(plateno)) {
               throw new ArgumentNullException("plateno");
           }
           StringBuilder sb = new StringBuilder();
           sb.Append("select * from ParkIORecord where PlateNumber=@plateno order by ");
           using (DbOperator dboperator = ConnectionManager.CreateConnection()) {
               dboperator.ClearParameters();
               dboperator.AddParameter("plateno", plateno);
               using (DataTable  dt= dboperator.ExecuteTable(sb.ToString()))
               {
                   if (dt.Rows.Count>0) {
                       return dt.Rows[0]["EntranceImage"].ToString();
                   }
                   return string.Empty;
               }
           }
       }
        

       

   }
}
