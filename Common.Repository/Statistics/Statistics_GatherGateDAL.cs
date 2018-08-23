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
    public class Statistics_GatherGateDAL:IStatistics_GatherGate
    {
        /// <summary>
        /// 判断道口统计数据是否存在
        /// </summary>
        /// <param name="gateid">通道编号</param>
        /// <param name="gathertime">统计时间</param>
        /// <returns></returns>
        public bool IsExists(string gateid, DateTime gathertime)
        {
            bool _flag = false;
            string strSql = "select count(0) Count from Statistics_GatherGate where GateID=@gateid and GatherTime=@gathertime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("gateid", gateid);
                dboperator.AddParameter("gathertime", gathertime);
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
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="gathertime">统计时间</param>
        /// <returns></returns>
        public bool Delete(string parkingid, DateTime gathertime,DbOperator db)
        {
            string strSql = "delete from statistics_gathergate where parkingid=@parkingid and GatherTime=@gathertime";
            db.ClearParameters();
            db.AddParameter("parkingid", parkingid);
            db.AddParameter("gathertime", gathertime);
            return (db.ExecuteNonQuery(strSql) > 0 ? true : false);
        }
        public bool Insert(Statistics_GatherGate gategather, DbOperator db)
        {
            string strSql = string.Format(@"insert into statistics_gathergate(StatisticsGatherID,ParkingID,ParkingName,BoxID,BoxName,GateID,GateName,GatherTime,Receivable_Amount, 
                                           Real_Amount,Diff_Amount,Cash_Amount,Cash_Count,StordCard_Amount,StordCard_Count,OnLine_Amount,OnLine_Count,Discount_Amount,Discount_Count,Temp_Amount,Temp_Count,OnLineTemp_Amount,OnLineTemp_Count,
                                           ReleaseType_Normal,ReleaseType_Charge,ReleaseType_Free,ReleaseType_Catch,VIPExtend_Count,
                                           Entrance_Count,Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,
                                           OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount,MonthCardExtend_Count,MonthCardExtend_Amount,
                                           OnLineStordCard_Count,OnLineStordCard_Amount,StordCardRecharge_Count,StordCardRecharge_Amount,CashDiscount_Count,CashDiscount_Amount,OnLineDiscount_Count,OnLineDiscount_Amount,HaveUpdate,LastUpdateTime) 
                                           values(@StatisticsGatherID,@ParkingID,@ParkingName,@BoxID,@BoxName,@GateID,@GateName,@GatherTime,@Receivable_Amount,@Real_Amount,@Diff_Amount,@Cash_Amount,@Cash_Count,
                                           @StordCard_Amount,@StordCard_Count,@OnLine_Amount,@OnLine_Count,@Discount_Amount,@Discount_Count,@Temp_Amount,@Temp_Count,@OnLineTemp_Amount,@OnLineTemp_Count,@ReleaseType_Normal,@ReleaseType_Charge,
                                           @ReleaseType_Free,@ReleaseType_Catch,@VIPExtend_Count,@Entrance_Count,@Exit_Count,@VIPCard,@StordCard,
                                           @MonthCard,@JobCard,@TempCard,@OnLineMonthCardExtend_Count,@OnLineMonthCardExtend_Amount,@MonthCardExtend_Count,
                                           @MonthCardExtend_Amount,@OnLineStordCard_Count,@OnLineStordCard_Amount,@StordCardRecharge_Count,
                                           @StordCardRecharge_Amount,@CashDiscount_Count,@CashDiscount_Amount,@OnLineDiscount_Count,@OnLineDiscount_Amount,1,getdate())");
            db.ClearParameters();
            db.AddParameter("@StatisticsGatherID", gategather.StatisticsGatherID);
            db.AddParameter("@ParkingID",  gategather.ParkingID);
            db.AddParameter("@ParkingName",  gategather.ParkingName);
            db.AddParameter("@BoxID", gategather.BoxID);
            db.AddParameter("@BoxName",gategather.BoxName);
            db.AddParameter("@GateID", gategather.GateID);
            db.AddParameter("@GateName", gategather.GateName);
            db.AddParameter("@GatherTime", gategather.GatherTime);
            db.AddParameter("@Receivable_Amount",  gategather.Receivable_Amount);
            db.AddParameter("@Real_Amount",  gategather.Real_Amount);
            db.AddParameter("@Diff_Amount",  gategather.Diff_Amount);
            db.AddParameter("@Cash_Amount", gategather.Cash_Amount);
            db.AddParameter("@Cash_Count",  gategather.Cash_Count);
            db.AddParameter("@StordCard_Amount", gategather.StordCard_Amount);
            db.AddParameter("@StordCard_Count",  gategather.StordCard_Count);
            db.AddParameter("@OnLine_Amount",  gategather.OnLine_Amount);
            db.AddParameter("@OnLine_Count", gategather.OnLine_Count);
            db.AddParameter("@Discount_Amount",  gategather.Discount_Amount);
            db.AddParameter("@Discount_Count", gategather.Discount_Count);
            db.AddParameter("@Temp_Amount",  gategather.Temp_Amount);
            db.AddParameter("@Temp_Count", gategather.Temp_Count);
            db.AddParameter("@OnLineTemp_Amount", gategather.OnLineTemp_Amount);
            db.AddParameter("@OnLineTemp_Count",  gategather.OnLineTemp_Count);
            db.AddParameter("@ReleaseType_Normal", gategather.ReleaseType_Normal);
            db.AddParameter("@ReleaseType_Charge",gategather.ReleaseType_Charge);
            db.AddParameter("@ReleaseType_Free",  gategather.ReleaseType_Free);
            db.AddParameter("@ReleaseType_Catch",  gategather.ReleaseType_Catch);
            db.AddParameter("@VIPExtend_Count",  gategather.VIPExtend_Count);
            db.AddParameter("@Entrance_Count",  gategather.Entrance_Count);
            db.AddParameter("@Exit_Count",  gategather.Exit_Count);
            db.AddParameter("@VIPCard",  gategather.VIPCard);
            db.AddParameter("@StordCard", gategather.StordCard);
            db.AddParameter("@MonthCard",  gategather.MonthCard);
            db.AddParameter("@JobCard",  gategather.JobCard);
            db.AddParameter("@TempCard",  gategather.TempCard);
            db.AddParameter("@OnLineMonthCardExtend_Count", gategather.OnLineMonthCardExtend_Count);
            db.AddParameter("@OnLineMonthCardExtend_Amount",  gategather.OnLineMonthCardExtend_Amount);
            db.AddParameter("@MonthCardExtend_Count",  gategather.MonthCardExtend_Count);
            db.AddParameter("@MonthCardExtend_Amount",  gategather.MonthCardExtend_Amount);
            db.AddParameter("@OnLineStordCard_Count",  gategather.OnLineStordCard_Count);
            db.AddParameter("@OnLineStordCard_Amount",  gategather.OnLineStordCard_Amount);
            db.AddParameter("@StordCardRecharge_Count", gategather.StordCardRecharge_Count);
            db.AddParameter("@StordCardRecharge_Amount",  gategather.StordCardRecharge_Amount);
            db.AddParameter("@CashDiscount_Count",  gategather.CashDiscount_Count);
            db.AddParameter("@CashDiscount_Amount",  gategather.CashDiscount_Amount);
            db.AddParameter("@OnLineDiscount_Count",  gategather.OnLineDiscount_Count);
            db.AddParameter("@OnLineDiscount_Amount",  gategather.OnLineDiscount_Amount);
            return (db.ExecuteNonQuery(strSql) > 0 ? true : false);
        }
    }
}
