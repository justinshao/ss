using Common.IRepository.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository.WeiXin
{
    public class WXUserDAL : IWXUser
    {
        /// <summary>
        /// 获取关注的用户信息
        /// </summary>
        /// <param name="sPlateNo">用户绑定的车辆</param>
        /// <returns></returns>
        public WX_Info GetWXInfoByPlateNo(string sPlateNo)
        {
            if (sPlateNo.IsEmpty())
            {
                return null;
            }
            //SQL语句
            //只获取关注的用户信息
            string strSql = "select top 1 * from WX_Info a inner join WX_CarInfo b on a.AccountID = b.AccountID where b.PlateNo=@PlateNo and a.FollowState=1";

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PlateNo", sPlateNo);

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        return DataReaderToModel<WX_Info>.ToModel(dr);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
