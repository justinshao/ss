using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using System.Data;

namespace Common.IRepository.BB
{
    public class PublicDAL
    {
        private static string sqlstr = "select AppId,AppSecret from WX_ApiConfig";
        public static string Appid() {
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                DataTable dt = dboperator.ExecuteTable(sqlstr, 30000);
                if (dt.Columns.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                    //appsecret = dt.Rows[0][1].ToString();
                }
                else
                {
                   // throw new MyException("获取AppId失败!");
                    return "";
                }
            }
          
        }
        public static string Appsecret() {
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                DataTable dt = dboperator.ExecuteTable(sqlstr, 30000);
                if (dt.Columns.Count > 0)
                {
                    //appid = dt.Rows[0][0].ToString();
                    return dt.Rows[0][1].ToString();
                }
                else
                {
                    //throw new MyException("获取AppId失败!");
                    return "";
                }
            }
        }

        
    }
}
