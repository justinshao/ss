using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using System.Data.Common;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
namespace Common.SqlRepository.Statistics
{
    public class Statistics_GatherDAL : IStatistics_Gather
    {
        /// <summary>
        /// 统计数据是否存在
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="gathertime">统计时间</param>
        /// <returns></returns>
        public bool IsExistsGather(string parkingid, DateTime gathertime)
        {
            bool _isexists = false;
            string strSql = "select count(0) Count from Statistics_Gather where parkingid=@parkingid and gathertime=@gathertime";
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
        /// 删除数据统计
        /// </summary>
        /// <param name="ParkingID">车场名称</param>
        /// <param name="GatherTime">统计时间</param>
        /// <param name="dbhelper">数据操作对象</param>
        /// <returns></returns>
        public bool DeleteGather(string parkingid, DateTime gathertime,DbOperator dboperator)
        {
            string strSql = "delete from statistics_gather where parkingid=@ParkingID and GatherTime=@GatherTime";
            dboperator.ClearParameters();
            dboperator.AddParameter("ParkingID", parkingid);
            dboperator.AddParameter("gathertime", gathertime);
            return (dboperator.ExecuteNonQuery(strSql) >0 ? true : false);
        }

        /// <summary>
        /// 删除时间段统计信息
        /// </summary>
        /// <param name="parkingid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool DeleteGatherTime(string parkingid, DateTime start,DateTime end)
        {
            string strSql = "delete from statistics_gather where  GatherTime>=@start and GatherTime<=@end";
            if (parkingid != null)
            {
                strSql += " and parkingid=@ParkingID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                if (parkingid != null)
                {
                    dboperator.AddParameter("ParkingID", parkingid);
                }
                dboperator.AddParameter("start", start);
                dboperator.AddParameter("end", end);
                return (dboperator.ExecuteNonQuery(strSql) >= 0 ? true : false);
            }
        }

        /// <summary>
        /// 插入汇总数据
        /// </summary>
        /// <param name="gather">对象</param>
        /// <param name="dboperator">数据库连接对象</param>
        /// <returns></returns>
        public bool Insert(Statistics_Gather gather, DbOperator dboperator)
        {
            string strSql = string.Format(@"insert into statistics_gather(StatisticsGatherID,ParkingID,ParkingName,GatherTime,Receivable_Amount, 
                                           Real_Amount,Diff_Amount,Cash_Amount,Cash_Count,StordCard_Amount,StordCard_Count,OnLine_Amount,OnLine_Count,
                                           Discount_Amount,Discount_Count,Temp_Amount,Temp_Count,OnLineTemp_Amount,OnLineTemp_Count,
                                           ReleaseType_Normal,ReleaseType_Charge,ReleaseType_Free,ReleaseType_Catch,VIPExtend_Count,
                                           Entrance_Count,Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,
                                           OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount,MonthCardExtend_Count,MonthCardExtend_Amount,
                                           OnLineStordCard_Count,OnLineStordCard_Amount,StordCardRecharge_Count,StordCardRecharge_Amount,CashDiscount_Count,CashDiscount_Amount,
                                           OnLineDiscount_Count,OnLineDiscount_Amount,HaveUpdate,LastUpdateTime) 
                                           values(@StatisticsGatherID,@ParkingID,@ParkingName,@GatherTime,@Receivable_Amount,@Real_Amount,@Diff_Amount,@Cash_Amount,@Cash_Count,
                                           @StordCard_Amount,@StordCard_Count,@OnLine_Amount,@OnLine_Count,@Discount_Amount,@Discount_Count,@Temp_Amount,@Temp_Count,@OnLineTemp_Amount,
                                           @OnLineTemp_Count,@ReleaseType_Normal,@ReleaseType_Charge,
                                           @ReleaseType_Free,@ReleaseType_Catch,@VIPExtend_Count,@Entrance_Count,@Exit_Count,@VIPCard,@StordCard,
                                           @MonthCard,@JobCard,@TempCard,@OnLineMonthCardExtend_Count,@OnLineMonthCardExtend_Amount,@MonthCardExtend_Count,
                                           @MonthCardExtend_Amount,@OnLineStordCard_Count,@OnLineStordCard_Amount,@StordCardRecharge_Count,
                                           @StordCardRecharge_Amount,@CashDiscount_Count,@CashDiscount_Amount,@OnLineDiscount_Count,@OnLineDiscount_Amount,1,getdate())");
            dboperator.ClearParameters();
            dboperator.AddParameter("StatisticsGatherID",gather.StatisticsGatherID);
            dboperator.AddParameter("ParkingID",gather.ParkingID);
            dboperator.AddParameter("ParkingName",gather.ParkingName);
            dboperator.AddParameter("GatherTime",gather.GatherTime);
            dboperator.AddParameter("Receivable_Amount",gather.Receivable_Amount);
            dboperator.AddParameter("Real_Amount",gather.Real_Amount);
            dboperator.AddParameter("Diff_Amount",gather.Diff_Amount);
            dboperator.AddParameter("Cash_Amount",gather.Cash_Amount);
            dboperator.AddParameter("Cash_Count",gather.Cash_Count);
            dboperator.AddParameter("StordCard_Amount",gather.StordCard_Amount);
            dboperator.AddParameter("StordCard_Count",gather.StordCard_Count);
            dboperator.AddParameter("OnLine_Amount",gather.OnLine_Amount);
            dboperator.AddParameter("OnLine_Count",gather.OnLine_Count);
            dboperator.AddParameter("Discount_Amount",gather.Discount_Amount);
            dboperator.AddParameter("Discount_Count",gather.Discount_Count);
            dboperator.AddParameter("Temp_Amount",gather.Temp_Amount);
            dboperator.AddParameter("Temp_Count",gather.Temp_Count);
            dboperator.AddParameter("OnLineTemp_Amount",gather.OnLineTemp_Amount);
            dboperator.AddParameter("OnLineTemp_Count",gather.OnLineTemp_Count);
            dboperator.AddParameter("ReleaseType_Normal",gather.ReleaseType_Normal);
            dboperator.AddParameter("ReleaseType_Charge",gather.ReleaseType_Charge);
            dboperator.AddParameter("ReleaseType_Free",gather.ReleaseType_Free);
            dboperator.AddParameter("ReleaseType_Catch",gather.ReleaseType_Catch);
            dboperator.AddParameter("VIPExtend_Count",gather.VIPExtend_Count);
            dboperator.AddParameter("Entrance_Count",gather.Entrance_Count);
            dboperator.AddParameter("Exit_Count",gather.Exit_Count);
            dboperator.AddParameter("VIPCard",  gather.VIPCard);
            dboperator.AddParameter("StordCard",  gather.StordCard);
            dboperator.AddParameter("MonthCard",  gather.MonthCard);
            dboperator.AddParameter("JobCard",  gather.JobCard);
            dboperator.AddParameter("TempCard",  gather.TempCard);
            dboperator.AddParameter("OnLineMonthCardExtend_Count",  gather.OnLineMonthCardExtend_Count);
            dboperator.AddParameter("OnLineMonthCardExtend_Amount",  gather.OnLineMonthCardExtend_Amount);
            dboperator.AddParameter("MonthCardExtend_Count",  gather.MonthCardExtend_Count);
            dboperator.AddParameter("MonthCardExtend_Amount", gather.MonthCardExtend_Amount);
            dboperator.AddParameter("OnLineStordCard_Count", gather.OnLineStordCard_Count);
            dboperator.AddParameter("OnLineStordCard_Amount",gather.OnLineStordCard_Amount);
            dboperator.AddParameter("StordCardRecharge_Count",gather.StordCardRecharge_Count);
            dboperator.AddParameter("StordCardRecharge_Amount",gather.StordCardRecharge_Amount);
            dboperator.AddParameter("CashDiscount_Count",gather.CashDiscount_Count);
            dboperator.AddParameter("CashDiscount_Amount",gather.CashDiscount_Amount);
            dboperator.AddParameter("OnLineDiscount_Count",gather.OnLineDiscount_Count);
            dboperator.AddParameter("OnLineDiscount_Amount",gather.OnLineDiscount_Amount);
            return (dboperator.ExecuteNonQuery(strSql) > 0 ? true : false);
        }
    }
}
