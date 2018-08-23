using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using System.Data.Common;
using Common.IRepository.Statistics;
namespace Common.SqlRepository.Statistics
{
    public class Statistics_GatherLongTimeDAL:IStatistics_GatherLongTime
    {
        /// <summary>
        /// 停车时长统计是否存在
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="gathertime">统计时间</param>
        /// <returns></returns>
        public bool IsExists(string parkingid,DateTime gathertime)
        {
            bool _isexists = false;
            string strSql = "select count(0) Count from Statistics_GatherLongTime where parkingid=@parkingid and gathertime=@gathertime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("parkingid", parkingid);
                dboperator.AddParameter("gathertime", gathertime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _isexists = (int.Parse(dr["Count"].ToString()) > 0 ? true : false);
                    }
                }
            }
            return _isexists;
        }
        /// <summary>
        /// 添加停车时长数据
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public bool Insert(string strSql)
        {
            bool _flag = false;
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _flag = (int.Parse(dr["Count"].ToString()) > 0 ? true : false);
                    }
                }
            }
            return _flag;
        }
    }
}
