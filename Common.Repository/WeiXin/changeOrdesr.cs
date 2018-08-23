using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

namespace Common.SqlRepository.WeiXin
{
    public class changeOrdesr
    {
        public static bool Update(string id)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("UPDATE ParkOrder SET Status = '2' WHERE RecordID = '" + id + "");
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}
