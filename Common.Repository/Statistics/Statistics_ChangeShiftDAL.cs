using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Statistics;
using Common.DataAccess;
using Common.Entities.Statistics;
namespace Common.SqlRepository.Statistics
{
    public class Statistics_ChangeShiftDAL:IStatistics_ChangeShift
    {
        /// <summary>
        /// 删除当班统计数据
        /// </summary>
        /// <param name="changeshiftid">当班编号</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(string changeshiftid, DbOperator db)
        {
            string strSql = "delete from Statistics_ChangeShift where changeshiftid=@changeshiftid";
            db.ClearParameters();
            db.AddParameter("changeshiftid", changeshiftid);
            return (db.ExecuteNonQuery(strSql) == 1 ? true : false);
        }
        /// <summary>
        /// 添加当班统计记录
        /// </summary>
        /// <param name="changeshift">当班统计对象模型</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Insert(Statistics_ChangeShift changeshift, DbOperator db)
        {
            string strSql = string.Format(@"insert into Statistics_ChangeShift(ParkingID,ParkingName,BoxID,ChangeShiftID,AdminID,StartWorkTime,EndWorkTime,Receivable_Amount,
                                                        Real_Amount,Diff_Amount,Cash_Amount,Cash_Count,Temp_Amount,Temp_Count,OnLineTemp_Amount,OnLineTemp_Count,StordCard_Amount,
                                                        StordCard_Count,OnLine_Amount,OnLine_Count,Discount_Amount,Discount_Count,ReleaseType_Normal,ReleaseType_Charge,ReleaseType_Free,
                                                        ReleaseType_Catch,VIPExtend_Count,
                                                        Entrance_Count,Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount,
                                                        MonthCardExtend_Count,MonthCardExtend_Amount,OnLineStordCard_Count,OnLineStordCard_Amount,StordCardRecharge_Count,
                                                        StordCardRecharge_Amount,CashDiscount_Count,CashDiscount_Amount,OnLineDiscount_Count,OnLineDiscount_Amount,HaveUpdate,LastUpdateTime)
                                                  values(@ParkingID,@ParkingName,@BoxID,@ChangeShiftID,@AdminID,@StartWorkTime,@EndWorkTime,@Receivable_Amount,
                                                        @Real_Amount,@Diff_Amount,@Cash_Amount,@Cash_Count,@Temp_Amount,@Temp_Count,@OnLineTemp_Amount,@OnLineTemp_Count,@StordCard_Amount,
                                                        @StordCard_Count,@OnLine_Amount,@OnLine_Count,@Discount_Amount,@Discount_Count,@ReleaseType_Normal,@ReleaseType_Charge,@ReleaseType_Free,
                                                        @ReleaseType_Catch,@VIPExtend_Count,
                                                        @Entrance_Count,@Exit_Count,@VIPCard,@StordCard,@MonthCard,@JobCard,@TempCard,@OnLineMonthCardExtend_Count,@OnLineMonthCardExtend_Amount,
                                                        @MonthCardExtend_Count,@MonthCardExtend_Amount,@OnLineStordCard_Count,@OnLineStordCard_Amount,@StordCardRecharge_Count,
                                                        @StordCardRecharge_Amount,@CashDiscount_Count,@CashDiscount_Amount,@OnLineDiscount_Count,@OnLineDiscount_Amount,1,getdate())");
            db.ClearParameters();
            db.AddParameter("@ParkingID", changeshift.ParkingID);
            db.AddParameter("@ParkingName", changeshift.ParkingName);
            db.AddParameter("@BoxID", changeshift.BoxID);
            db.AddParameter("@ChangeShiftID", changeshift.ChangeShiftID);
            db.AddParameter("@AdminID", changeshift.AdminID);
            db.AddParameter("@StartWorkTime", changeshift.StartWorkTime);
            db.AddParameter("@EndWorkTime", changeshift.EndWorkTime <new DateTime(2013,1,1) ? DateTime.MinValue : changeshift.EndWorkTime);
            db.AddParameter("@Receivable_Amount", changeshift.Receivable_Amount);
            db.AddParameter("@Real_Amount",  changeshift.Real_Amount);
            db.AddParameter("@Diff_Amount",  changeshift.Diff_Amount);
            db.AddParameter("@Cash_Amount",  changeshift.Cash_Amount);
            db.AddParameter("@Cash_Count", changeshift.Cash_Count);
            db.AddParameter("@Temp_Amount",  changeshift.Temp_Amount);
            db.AddParameter("@Temp_Count",  changeshift.Temp_Count);
            db.AddParameter("@OnLineTemp_Amount",  changeshift.OnLineTemp_Amount);
            db.AddParameter("@OnLineTemp_Count",  changeshift.OnLineTemp_Count);
            db.AddParameter("@StordCard_Amount",  changeshift.StordCard_Amount);
            db.AddParameter("@StordCard_Count",changeshift.StordCard_Count);
            db.AddParameter("@OnLine_Amount",  changeshift.OnLine_Amount);
            db.AddParameter("@OnLine_Count",  changeshift.OnLine_Count);
            db.AddParameter("@Discount_Amount",  changeshift.Discount_Amount);
            db.AddParameter("@Discount_Count",  changeshift.Discount_Count);
            db.AddParameter("@ReleaseType_Normal",  changeshift.ReleaseType_Normal);
            db.AddParameter("@ReleaseType_Charge", changeshift.ReleaseType_Charge);
            db.AddParameter("@ReleaseType_Free",  changeshift.ReleaseType_Free);
            db.AddParameter("@ReleaseType_Catch",  changeshift.ReleaseType_Catch);
            db.AddParameter("@VIPExtend_Count", changeshift.VIPExtend_Count);
            db.AddParameter("@Entrance_Count", changeshift.Entrance_Count);
            db.AddParameter("@Exit_Count", changeshift.Exit_Count);
            db.AddParameter("@VIPCard",  changeshift.VIPCard);
            db.AddParameter("@StordCard",  changeshift.StordCard);
            db.AddParameter("@MonthCard",  changeshift.MonthCard);
            db.AddParameter("@JobCard",  changeshift.JobCard);
            db.AddParameter("@TempCard",  changeshift.TempCard);
            db.AddParameter("@OnLineMonthCardExtend_Count",  changeshift.OnLineMonthCardExtend_Count);
            db.AddParameter("@OnLineMonthCardExtend_Amount",  changeshift.OnLineMonthCardExtend_Amount);
            db.AddParameter("@MonthCardExtend_Count",  changeshift.MonthCardExtend_Count);
            db.AddParameter("@MonthCardExtend_Amount",  changeshift.MonthCardExtend_Amount);
            db.AddParameter("@OnLineStordCard_Count",  changeshift.OnLineStordCard_Count);
            db.AddParameter("@OnLineStordCard_Amount",  changeshift.OnLineStordCard_Amount);
            db.AddParameter("@StordCardRecharge_Count",  changeshift.StordCardRecharge_Count);
            db.AddParameter("@StordCardRecharge_Amount",  changeshift.StordCardRecharge_Amount);
            db.AddParameter("@CashDiscount_Count",  changeshift.CashDiscount_Count);
            db.AddParameter("@CashDiscount_Amount",  changeshift.CashDiscount_Amount);
            db.AddParameter("@OnLineDiscount_Count",  changeshift.OnLineDiscount_Count);
            db.AddParameter("@OnLineDiscount_Amount", changeshift.OnLineDiscount_Amount);
            return db.ExecuteNonQuery(strSql) == 1 ? true : false;
        }
    }
}
