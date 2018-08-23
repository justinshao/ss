using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.Entities;

namespace Common.SqlRepository.Park
{
    public class ParkLpPlanDAL : BaseDAL, IParkLpPlan
    {
        /// <summary>
        /// 根据条件获取派车信息总数
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
       public int Search_ParkLpPlansCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from ParkLpPlan i where i.DataStatus!=2";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.PKID =@PKID";
            }
            if (!string.IsNullOrEmpty(paras.Owner) )
            {
                strSql += " and i.EmpName like @EmpName";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and i.ZB_User=@ZB_User";
            }
            strSql += " and ZB_Time>=@StartTime and ZB_Time<=@EndTime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PKID", paras.ParkingID);
                dboperator.AddParameter("EmpName", "%" + paras.Owner + "%");               
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("ZB_User", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _total = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _total;
        }

        /// <summary>
        /// 根据条件获取派车信息
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public List<ParkLpPlan> Search_ParkLpPlans(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkLpPlan> iorecordlist = new List<ParkLpPlan>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.ZB_Time desc ) AS rownum,i.*,u.UserName,                                          
                                          from ParkLpPlan i 
                                          left join SysUser u on u.recordid=i.ZB_User 
                                          where i.DataStatus<2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.PKID =@PKID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and i.EmpName like @EmpName";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and i.ZB_User=@ZB_User";
            }
            strSql += " and ZB_Time>=@StartTime and ZB_Time<=@EndTime";
            strSql += string.Format(" order by i.ZB_Time desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ZB_Time desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PKID", paras.ParkingID);
                dboperator.AddParameter("EmpName", "%" + paras.Owner + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("ZB_User", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkLpPlan>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }


        /// <summary>
        /// 根据派车单ID获取派遣单
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public ParkLpPlan Search_ParkLpPlanByID(string PlanID)
        {
            ParkLpPlan model=new ParkLpPlan();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkLpPlan where PlanID=@PlanID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlanID", PlanID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        model= DataReaderToModel<ParkLpPlan>.ToModel(reader);
                    }
                }
            }

            return model;
        }


        public bool Add(ParkLpPlan model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkLpPlan(PlanID,PKID,PlateNumber,PlanOutTime,PlanRtnTime,EmpName,Reason,Remark,PlanState,ZB_User,ZB_Time,SH_User,SH_Time,SH_State,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@PlanID,@PKID,@PlateNumber,@PlanOutTime,@PlanRtnTime,@EmpName,@Reason,@Remark,@PlanState,@ZB_User,@ZB_Time,@SH_User,@SH_Time,@SH_State,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlanID", model.PlanID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("PlanOutTime", model.PlanOutTime);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlanRtnTime", model.PlanRtnTime);
                dbOperator.AddParameter("EmpName", model.EmpName);
                dbOperator.AddParameter("Reason", model.Reason);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("PlanState", model.PlanState);
                dbOperator.AddParameter("ZB_User", model.ZB_User);
                dbOperator.AddParameter("ZB_Time", model.ZB_Time);
                dbOperator.AddParameter("SH_User", model.SH_User);
                dbOperator.AddParameter("SH_Time", model.SH_Time);
                dbOperator.AddParameter("SH_State", model.SH_State);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkLpPlan model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkLpPlan set PKID=@PKID,PlateNumber=@PlateNumber,PlanOutTime=@PlanOutTime,PlanRtnTime=@PlanRtnTime,EmpName=@EmpName,Reason=@Reason,Remark=@Remark,ZB_User=@ZB_User,");
                strSql.Append(" ZB_Time=@ZB_Time,SH_User=@SH_User,SH_Time=@SH_Time,SH_State=@SH_State,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where PlanID=@PlanID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlanID", model.PlanID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("PlanOutTime", model.PlanOutTime);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlanRtnTime", model.PlanRtnTime);
                dbOperator.AddParameter("EmpName", model.EmpName);
                dbOperator.AddParameter("Reason", model.Reason);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("ZB_User", model.ZB_User);
                dbOperator.AddParameter("ZB_Time", model.ZB_Time);
                dbOperator.AddParameter("SH_User", model.SH_User);
                dbOperator.AddParameter("SH_Time", model.SH_Time);
                dbOperator.AddParameter("SH_State", model.SH_State);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string PlanID)
        {
            return CommonDelete("ParkLpPlan", "PlanID", PlanID);
        }
    }
}
