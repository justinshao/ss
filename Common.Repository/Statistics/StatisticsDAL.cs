using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities;
using Common.Entities.Parking;
using Common.DataAccess;
using System.Data.Common;
using Common.Core;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
using Common.Utilities;
using Common.Entities.Condition;
using Common.Entities.Order;

namespace Common.SqlRepository.Statistics
{
    public class StatisticsDAL : IStatistics
    {
        #region 在场车辆
        /// <summary>
        /// 获取在场车辆记录数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_PresenceCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from ParkIORecord where DataStatus!=2 and isexit=0";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and CarTypeID=@CardType";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and PlateNumber like @PlateNumber";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and entrancegateid=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and EntranceTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and EntranceTime<=@EndTime";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
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

        public int Search_PresenceCountSmall(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from ParkIORecord a left join ParkTimeseries b on a.IORecordID=b.RecordID  where b.DataStatus!=2 and b.isexit=0";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and a.parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and a.CarTypeID=@CardType";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and a.PlateNumber like @PlateNumber";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and a.EnterGateID=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and a.EnterTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and a.ExitTime<=@EndTime";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
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
        /// 获取在场车辆记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_Presence(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            if (paras == null)
                return iorecordlist;
            string strSql = string.Format(@"select i.*,a.AreaName,p.PKName,g.GateName InGateName,o.EmployeeName,o.MobilePhone,ct.CarTypeName,u.UserName InOperatorName 
                                         from ParkIORecord i 
                                         left join BaseCard c on i.cardid=c.cardid 
                                         left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                         left join ParkArea a on a.AreaID=i.AreaID 
                                         left join BaseParkinfo p on p.pkid=i.parkingid 
                                         left join ParkGate g on i.entrancegateid=g.GateID 
                                         left join ParkCarType ct on i.CarTypeID=ct.CarTypeID 
                                         left join sysuser u on u.recordid=i.EntranceOperatorID 
                                         where i.DataStatus<2 and i.isexit=0");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.EntranceTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.EntranceTime<=@EndTime";
            }
            strSql += " order by i.EntranceTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        /// <summary>
        /// 在场车辆
        /// </summary>
        /// <param name="iorecord">参数</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_Presence(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            if (paras == null)
                return iorecordlist;

            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.EntranceTime desc ) AS rownum,i.*,a.AreaName,h.CarModelName,p.PKName,g.GateName InGateName,o.EmployeeName,o.MobilePhone,ct.CarTypeName,u.UserName InOperatorName 
                                         from ParkIORecord i 
                                         left join BaseCard c on i.cardid=c.cardid 
                                         left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                         left join ParkArea a on a.AreaID=i.AreaID 
                                         left join BaseParkinfo p on p.pkid=i.parkingid 
                                         left join ParkGate g on i.entrancegateid=g.GateID 
                                         left join ParkCarType ct on i.CarTypeID=ct.CarTypeID 
                                         left join ParkCarModel h on i.CarModelID=h.CarModelID 
                                         left join sysuser u on u.recordid=i.EntranceOperatorID 
                                         where i.DataStatus!=2 and i.isexit=0", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.EntranceTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.EntranceTime<=@EndTime";
            }
            strSql += string.Format(" order by i.EntranceTime desc) AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by EntranceTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }

        /// <summary>
        /// 查找小车场在场车辆
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public List<ParkIORecord> Search_PresenceSmall(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            if (paras == null)
                return iorecordlist;

            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.EntranceTime desc ) AS rownum,j.TimeseriesID,j.EnterTime as EntranceTime,i.PlateNumber,i.EntranceCertificateNo,a.AreaName,h.CarModelName,p.PKName,g.GateName InGateName,o.EmployeeName,o.MobilePhone,ct.CarTypeName,u.UserName InOperatorName 
                                         from ParkTimeseries j left join  ParkIORecord i  on j.IORecordID=i.RecordID 
                                         left join BaseCard c on i.cardid=c.cardid 
                                         left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                         left join ParkArea a on a.AreaID=i.AreaID 
                                         left join BaseParkinfo p on p.pkid=i.parkingid 
                                         left join ParkGate g on j.EnterGateID=g.GateID 
                                         left join ParkCarType ct on i.CarTypeID=ct.CarTypeID 
                                         left join ParkCarModel h on i.CarModelID=h.CarModelID 
                                         left join sysuser u on u.recordid=i.EntranceOperatorID 
                                         where j.DataStatus!=2 and j.isexit=0", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and j.EnterGateID=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and j.EnterTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and j.ExitTime<=@EndTime";
            }
            strSql += string.Format(" order by j.EnterTime desc) AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by EntranceTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }

        /// <summary>
        //  手动设置出场
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public bool SetExit(string id, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkIORecord set ExitTime=@ExitTime,IsExit=@IsExit,ReleaseType=@ReleaseType,LastUpdateTime=@LastUpdateTime,Remark=@Remark ");
            strSql.Append(" where ID=@id");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("ExitTime", DateTime.Now.ToString());
            dbOperator.AddParameter("IsExit", 1);
            dbOperator.AddParameter("ReleaseType", 2);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now.ToString());
            dbOperator.AddParameter("Remark", "手动放行车辆");
            dbOperator.AddParameter("id", id);
            int cnt = dbOperator.ExecuteNonQuery(strSql.ToString());
            return cnt > 0;
        }
        /// <summary>
        //  手动设置出场前查询车辆
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public bool QueryByIORecordId(string id)
        {

            string strSql = "select count(1) Count from ParkIORecord ";
            if (!string.IsNullOrEmpty(id))
            {
                strSql += "where ID=" + id;
            }
            int count;
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                count = Convert.ToInt32(dboperator.ExecuteScalar(strSql));
            }
            if (count > 1)
                return true;
            else
                return false;
        }
        #endregion

        #region 在场无牌车辆
        /// <summary>
        /// 查询无牌车辆总数
        /// </summary>
        /// <param name="iorecord">进出记录对象</param>
        /// <returns></returns>
        public int Search_NoPlateNumberCount(InParams paras)
        {
            int _total = 0;
            if (paras == null)
                return _total;
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            string strSql = string.Format(@"select count(1) Count from ParkIORecord i where DataStatus!=2 and i.isexit=0 and i.platenumber like '无车牌%'");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.IsInTime)
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.EntranceTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.EntranceTime<=@EndTime";
                }
            }
            else
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.ExitTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.ExitTime<=@EndTime";
                }
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
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
        /// 获得无牌车辆记录
        /// </summary>
        /// <param name="iorecord">进出记录对象</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_NoPlateNumber(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            if (paras == null)
                return iorecordlist;
            string strSql = string.Format(@"select i.*,a.AreaName AreaName,o.EmployeeName,o.MobilePhone,
                                            (select p.PKName from BaseParkinfo p where p.pkid=i.parkingid) PKName,g.GateName InGateName,eg.GateName OutGateName,ct.CarTypeName,u.UserName InOperatorName,
                                            case i.releasetype when 0 then '正常放行' when 1 then '收费放行' when 2 then '免费放行' when 3 then '异常放行' end ReleaseTypeName 
                                            from ParkIORecord i
                                            left join BaseCard c on c.cardid=i.cardid
                                            left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                            left join ParkArea a on a.AreaID=i.AreaID 
                                            left join ParkGate g on i.entrancegateid=g.GateID 
                                            left join ParkGate eg on eg.GateID=i.exitgateid 
                                            left join ParkCarType ct on i.CarTypeID=ct.CarTypeID 
                                            left join SysUser u on u.recordid=i.EntranceOperatorID 
                                            where i.DataStatus!=2 and i.isexit=0 and i.platenumber like '无车牌%'");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.EntranceTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.EntranceTime<=@EndTime";
            }
            strSql += " order by i.EntranceTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        /// <summary>
        /// 获得无牌车辆记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_NoPlateNumber(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            if (paras == null)
                return iorecordlist;
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.EntranceTime desc ) AS rownum,i.*,a.AreaName,o.EmployeeName EmployeeName,o.MobilePhone MobilePhone,
                                            (select p.PKName from BaseParkinfo p where p.pkid=i.parkingid) PKName,g.GateName InGateName,eg.GateName OutGateName,ct.CarTypeName,u.UserName InOperatorName,
                                            case i.releasetype when 0 then '正常放行' when 1 then '收费放行' when 2 then '免费放行' when 3 then '异常放行' end ReleaseTypeName 
                                            from ParkIORecord i
                                            left join BaseCard c on c.cardid=i.cardid
                                            left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                            left join ParkArea a on a.AreaID=i.AreaID 
                                            left join ParkGate g on i.entrancegateid=g.GateID 
                                            left join ParkGate eg on eg.GateID=i.exitgateid 
                                            left join ParkCarType ct on i.CarTypeID=ct.CarTypeID 
                                            left join sysuser u on u.recordid=i.EntranceOperatorID 
                                            where i.DataStatus!=2 and i.isexit=0 and i.platenumber like '无车牌%'", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.IsInTime)
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.EntranceTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.EntranceTime<=@EndTime";
                }
            }
            else
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.ExitTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.ExitTime<=@EndTime";
                }
            }
            strSql += string.Format(" order by i.EntranceTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by EntranceTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        #endregion

        #region 月卡信息
        public int Search_MonthCardInfoCount(InParams paras)
        {
            DateTime qs = DateTime.Now;
            DateTime js = DateTime.Now.AddDays(7);
            int _total = 0;
            if (paras == null)
                return _total;
            string strSql = string.Format(@"select count(1) Count
                                            from parkgrant g left join employeeplate e on g.plateid=e.plateid 
                                            left join parkcartype car on car.cartypeid=g.cartypeid
                                            left join baseparkinfo p on p.pkid=g.pkid
                                            left join basecard b on b.cardid=g.cardid
                                            left join baseemployee em on em.employeeid=b.employeeid where g.datastatus!=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and g.pkid=@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and car.cartypeid=@CarTypeID";
            }
            if (!string.IsNullOrEmpty(paras.Mobile))
            {
                strSql += " and (em.mobilephone like @Mobile or em.EmployeeName like @EmployeeName)";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and e.plateno like @PlateNumber";
            }
            if (!string.IsNullOrEmpty(paras.Addr))
            {
                strSql += " and em.FamilyAddr like @FamilyAddr";
            }
            if (paras.Due)
            {
                strSql+=" and g.EndDate between @qs and @js ";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("Mobile", "%" + paras.Mobile + "%");
                dboperator.AddParameter("EmployeeName", "%" + paras.Mobile + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("FamilyAddr", "%" + paras.Addr + "%");
                dboperator.AddParameter("qs", qs);
                dboperator.AddParameter("js", js);
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
        public List<MonthCardInfoModel> Search_MonthCardInfo(InParams paras)
        {
            DateTime qs = DateTime.Now;
            DateTime js = DateTime.Now.AddDays(7);
            List<MonthCardInfoModel> monthcardlist = new List<MonthCardInfoModel>();
            string strSql = string.Format(@"select p.PKName,e.plateno as PlateNumber,car.CarTypeName,em.EmployeeName,em.mobilephone as Mobile,g.BeginDate as StartTime,g.EndDate as EndTime,em.FamilyAddr 
                                            from parkgrant g left join employeeplate e on g.plateid=e.plateid 
                                            left join parkcartype car on car.cartypeid=g.cartypeid
                                            left join baseparkinfo p on p.pkid=g.pkid
                                            left join basecard b on b.cardid=g.cardid
                                            left join baseemployee em on em.employeeid=b.employeeid where g.datastatus!=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and g.pkid=@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and car.cartypeid=@CarTypeID";
            }
            if (!string.IsNullOrEmpty(paras.Mobile))
            {
                strSql += " and (em.mobilephone like @Mobile or em.EmployeeName like @EmployeeName)";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and e.plateno like @PlateNumber";
            }
            if (!string.IsNullOrEmpty(paras.Addr))
            {
                strSql += " and em.FamilyAddr like @FamilyAddr";
            }
            if (paras.Due)
            {
                strSql += " and g.EndDate between @qs and @js ";
            }
            strSql += " order by platenumber";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("Mobile", "%" + paras.Mobile + "%");
                dboperator.AddParameter("EmployeeName", "%" + paras.Mobile + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("FamilyAddr", "%" + paras.Addr + "%");
                dboperator.AddParameter("qs", qs);
                dboperator.AddParameter("js", js);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        monthcardlist.Add(DataReaderToModel<MonthCardInfoModel>.ToModel(dr));
                    }
                }
            }
            return monthcardlist;
        }
        public List<MonthCardInfoModel> Search_MonthCardInfo(InParams paras, int PageSize, int PageIndex)
        {
            DateTime qs = DateTime.Now;
            DateTime js = DateTime.Now.AddDays(7);
            List<MonthCardInfoModel> monthcardlist = new List<MonthCardInfoModel>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY e.plateno desc ) AS rownum, p.PKName,e.plateno as PlateNumber,em.EmployeeName,car.CarTypeName,em.mobilephone as Mobile,g.BeginDate as StartTime,g.EndDate as EndTime,em.FamilyAddr 
                                            from parkgrant g left join employeeplate e on g.plateid=e.plateid 
                                            left join parkcartype car on car.cartypeid=g.cartypeid
                                            left join baseparkinfo p on p.pkid=g.pkid
                                            left join basecard b on b.cardid=g.cardid
                                            left join baseemployee em on em.employeeid=b.employeeid where g.datastatus!=2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and g.pkid=@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and car.cartypeid=@CarTypeID";
            }
            if (!string.IsNullOrEmpty(paras.Mobile))
            {
                strSql += " and (em.mobilephone like @Mobile or em.EmployeeName like @EmployeeName)";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and e.plateno like @PlateNumber";
            }
            if (!string.IsNullOrEmpty(paras.Addr))
            {
                strSql += " and em.FamilyAddr like @FamilyAddr";
            }
            if (paras.Due)
            {
                strSql += " and g.EndDate between @qs and @js ";
            }
            strSql += string.Format(" order by PlateNumber desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by PlateNumber desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("Mobile", "%" + paras.Mobile + "%");
                dboperator.AddParameter("EmployeeName", "%" + paras.Mobile + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("FamilyAddr", "%" + paras.Addr + "%");
                dboperator.AddParameter("qs", qs);
                dboperator.AddParameter("js", js);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        monthcardlist.Add(DataReaderToModel<MonthCardInfoModel>.ToModel(dr));
                    }
                }
            }
            return monthcardlist;
        }
        #endregion

        #region 车牌前缀查询
        public List<PlateNumberPrefixModel> Search_PlateNumberPrefix(InParams paras)
        {
            string strSql = "select count(1) as Number,p.PlateNumberPrefix from ("
                            + "select left(platenumber,2) PlateNumberPrefix from parkiorecord where DataStatus!=2 and parkingid=@ParkingID and EntranceTime>=@StartTime and entrancetime<=@EndTime) p group by p.PlateNumberPrefix order by number desc";

            List<PlateNumberPrefixModel> iorecordlist = new List<PlateNumberPrefixModel>();
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<PlateNumberPrefixModel>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        #endregion

        #region 进出记录查询
        /// <summary>
        /// 进出记录
        /// </summary>
        /// <param name="iorecord">进出记录对象模型</param>
        /// <param name="owner">车主</param>
        /// <param name="isexit">是否出场</param>
        /// <returns></returns>
        public int Search_InOutRecordsCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from ParkIORecord i where i.DataStatus!=2";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            if (paras.AreaID != "-1")
            {
                strSql += " and i.AreaID=@AreaID";
            }
            if (paras.IsExit != -1)
            {
                strSql += " and i.isexit=@IsExit";
            }

            if (paras.IsInTime)
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.EntranceTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.EntranceTime<=@EndTime";
                }
            }
            else
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.ExitTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.ExitTime<=@EndTime";
                }
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and i.ExitOperatorID=@OutOperator";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);
                dboperator.AddParameter("AreaID", paras.AreaID);
                dboperator.AddParameter("IsExit", paras.IsExit);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("OutOperator", paras.OutOperator);
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
        /// 获取进出记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_InOutRecords(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            string strSql = string.Format(@"select i.*,car.CarModelName,a.AreaName,o.EmployeeName,o.MobilePhone Tel,
                                          (select p.PKName from BaseParkinfo p where p.pkid=i.parkingid) PKName,
                                          g.GateName InGateName,eg.GateName OutGateName,ct.CarTypeName,u.UserName InOperatorName,um.UserName OutOperatorName,
                                          case i.releasetype when 0 then '正常放行' when 1 then '收费放行' when 2 then '免费放行' when 3 then '异常放行' end ReleaseTypeName 
                                          from ParkIORecord i 
                                          left join BaseCard c on i.cardid=c.cardid 
                                          left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                          left join parkarea a on a.areaid=i.areaid 
                                          left join parkgate g on i.entrancegateid=g.gateid 
                                          left join parkgate eg on eg.gateid=i.exitgateid 
                                          left join ParkCarType ct on i.cartypeid=ct.cartypeid 
                                          left join SysUser u on u.recordid=i.EntranceOperatorID 
                                          left join SysUser um on um.recordid=i.ExitOperatorID 
                                          left join ParkCarModel car on car.CarModelID=i.CarModelID 
                                          where i.DataStatus<2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CarType != "-1")
            {
                strSql += " and i.carmodelid=@CarType";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            if (paras.AreaID != "-1")
            {
                strSql += " and i.AreaID=@AreaID";
            }
            if (paras.IsExit != -1)
            {
                strSql += " and i.isexit=@IsExit";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.EntranceTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.EntranceTime<=@EndTime";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and i.cardid in (select cardid from BaseCard where EmployeeID in (select EmployeeID from BaseEmployee where EmployeeName like @Owner and DataStatus!=2) and DataStatus!=2)";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and i.ExitOperatorID=@OutOperator";
            }
            strSql += " order by i.EntranceTime desc";
            //strSql += " limit " + (PageIndex - 1) * PageSize + "," + PageSize;
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CarType", paras.CarType);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);
                dboperator.AddParameter("AreaID", paras.AreaID);
                dboperator.AddParameter("IsExit", paras.IsExit);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", paras.Owner);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("OutOperator", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        /// <summary>
        /// 获取进出记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_InOutRecords(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.EntranceTime desc ) AS rownum,i.*,i.NetWeight-i.Tare as JZWeight,car.CarModelName,a.AreaName,o.EmployeeName,o.MobilePhone,
                                          (select p.PKName from BaseParkinfo p where p.PKID=i.ParkingID) PKName,g.GateName InGateName,
                                          eg.GateName OutGateName,ct.CarTypeName,u.UserName InOperatorName,um.UserName OutOperatorName,
                                          case i.releasetype when 0 then '正常放行' when 1 then '收费放行' when 2 then '免费放行' when 3 then '异常放行' end ReleaseTypeName  
                                          from ParkIORecord i 
                                          left join BaseCard c on i.cardid=c.cardid 
                                          left join BaseEmployee o on c.EmployeeID=o.EmployeeID 
                                          left join parkarea a on a.areaid=i.areaid 
                                          left join parkgate g on i.entrancegateid=g.gateid 
                                          left join parkgate eg on eg.gateid=i.exitgateid 
                                          left join ParkCarType ct on i.cartypeid=ct.cartypeid 
                                          left join SysUser u on u.recordid=i.EntranceOperatorID 
                                          left join SysUser um on um.recordid=i.ExitOperatorID 
                                          left join ParkCarModel car on car.CarModelID=i.CarModelID 
                                          where i.DataStatus<2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.CarType != "-1")
            {
                strSql += " and i.carmodelid=@CarType";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and i.CarTypeID=@CardType";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and i.entrancegateid=@InGateID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            if (paras.AreaID != "-1")
            {
                strSql += " and i.AreaID=@AreaID";
            }
            if (paras.IsExit != -1)
            {
                strSql += " and i.isexit=@IsExit";
            }
            if (paras.IsInTime)
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.EntranceTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.EntranceTime<=@EndTime";
                }
            }
            else
            {
                if (paras.StartTime != null)
                {
                    strSql += " and i.ExitTime>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and i.ExitTime<=@EndTime";
                }
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and i.cardid in (select cardid from BaseCard where EmployeeID in (select EmployeeID from BaseEmployee where EmployeeName like @Owner and DataStatus!=2) and DataStatus!=2)";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and i.ExitOperatorID=@OutOperator";
            }
            strSql += string.Format(" order by i.EntranceTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by EntranceTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("CarType", paras.CarType);
                dboperator.AddParameter("CardType", paras.CardType);
                dboperator.AddParameter("InGateID", paras.InGateID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);
                dboperator.AddParameter("AreaID", paras.AreaID);
                dboperator.AddParameter("IsExit", paras.IsExit);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", paras.Owner);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("OutOperator", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        #endregion

        #region 异常放行
        /// <summary>
        /// 异常旅行数量
        /// </summary>
        /// <param name="iorecord">进出记录模型</param>
        /// <returns></returns>
        public int Search_ExceptionReleaseCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord i where i.releasetype=3 and i.DataStatus!=2");
            if (!string.IsNullOrWhiteSpace(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.exittime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.exittime<=@EndTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and e.ExitOperatorID=@AdminID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("AdminID", paras.AdminID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
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
        public List<ParkIORecord> Search_ExceptionRelease(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            string strSql = string.Format(@"select i.*,car.CarModelName,card.CarTypeName,a.AreaName,
                                           p.PKName,g.GateName InGateName,eg.GateName OutGateName,u.UserName InOperatorName,'异常放行' ReleaseTypeName 
                                           from ParkIORecord i left join ParkArea a on a.areaid=i.areaid 
                                           left join BaseParkinfo p on p.PKID=i.parkingid 
                                           left join ParkGate g on i.entrancegateid=g.gateid 
                                           left join ParkGate eg on eg.gateid=i.exitgateid 
                                           left join SysUser u on u.recordid=i.ExitOperatorID 
                                           left join ParkCarModel car on car.CarModelID=i.CarModelID 
                                           left join ParkCarType card on card.CarTypeID=i.CarTypeID
                                           where i.releasetype=3 and i.DataStatus!=2");

            if (!string.IsNullOrWhiteSpace(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.exittime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.exittime<=@EndTime";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber";
            }
            strSql += " order by i.exittime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;
        }
        public List<ParkIORecord> Search_ExceptionRelease(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY i.exittime desc ) AS rownum,i.*,car.CarModelName,card.CarTypeName,a.AreaName,o.Amount,
                                           p.PKName,g.GateName InGateName,eg.GateName OutGateName,u.UserName InOperatorName,'异常放行' ReleaseTypeName 
                                          from ParkIORecord i left join ParkArea a on a.areaid=i.areaid 
                                           left join BaseParkinfo p on p.PKID=i.parkingid 
                                           left join ParkGate g on i.entrancegateid=g.gateid 
                                           left join ParkGate eg on eg.gateid=i.exitgateid 
                                           left join SysUser u on u.recordid=i.ExitOperatorID 
                                           left join ParkCarModel car on car.CarModelID=i.CarModelID 
                                           left join ParkCarType card on card.CarTypeID=i.CarTypeID
                                           left join ParkOrder o on i.RecordID=o.TagID
                                           where i.releasetype=3 and i.DataStatus!=2", PageSize * PageIndex);

            if (!string.IsNullOrWhiteSpace(paras.ParkingID))
            {
                strSql += " and i.parkingid =@ParkingID";
            }
            if (paras.OutGateID != "-1")
            {
                strSql += " and i.exitgateid=@OutGateID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and i.exittime>=@StartTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and i.ExitOperatorID=@AdminID";
            }
            if (paras.EndTime != null)
            {
                strSql += " and i.exittime<=@EndTime";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber";
            }
            strSql += string.Format(" order by i.exittime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by exittime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("OutGateID", paras.OutGateID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("AdminID", paras.AdminID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        iorecordlist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return iorecordlist;

        }
        #endregion

        #region 通道事件
        /// <summary>
        /// 通道事件数量
        /// </summary>
        /// <param name="parkevent">通道事件对象</param>
        /// <returns></returns>
        public int Search_GateEventsCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from ParkEvent e where e.DataStatus!=2 ");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and e.ParkingID =@ParkingID";
            }
            if (paras.EventID != -1)
            {
                strSql += " and e.EventID=@EventID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and e.CarTypeID=@CarTypeID";
            }
            if (paras.CarType != "-1")
            {
                strSql += " and e.CarModelID=@CarModelID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and e.GateID=@GateID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and e.PlateNumber like @PlateNumber";
            }
            if (paras.InOrOut != -1)
            {
                strSql += " and e.IOState=@InOrOut";
            }
            if (paras.StartTime != null)
            {
                strSql += " and rectime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and rectime<=@EndTime";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("EventID", paras.EventID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("CarModelID", paras.CarType);
                dboperator.AddParameter("GateID", paras.InGateID);
                dboperator.AddParameter("PlateNumber", paras.PlateNumber);
                dboperator.AddParameter("InOrOut", paras.InOrOut);
                if (paras.StartTime != null)
                {
                    dboperator.AddParameter("StartTime", paras.StartTime);
                }
                if (paras.EndTime != null)
                {
                    dboperator.AddParameter("EndTime", paras.EndTime);
                }
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
        /// 查询通道事件记录
        /// </summary>
        /// <param name="parkevent">通道事件对象模型</param>
        /// <returns></returns>
        public List<ParkEvent> Search_GateEvents(InParams paras)
        {
            List<ParkEvent> parkeventlist = new List<ParkEvent>();
            string strSql = string.Format(@"select e.*,p.PKName ParkingName,g.GateName,card.CarTypeName,car.CarModelName,u.UserName Operator,case e.IOState when 1 then '进' when 2 then '出' else '' end IOStateName
                                           from parkevent e left join BaseParkinfo p on p.pkid=e.parkingid 
                                            left join parkgate g on e.gateid=g.gateid 
                                           left join SysUser u on u.recordid=e.OperatorID 
                                           left join ParkCarModel car on car.CarModelID=e.CarModelID 
                                           left join ParkCarType card on card.CarTypeID=e.CarTypeID
                                           where e.DataStatus!=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and e.ParkingID =@ParkingID";
            }
            if (paras.EventID != -1)
            {
                strSql += " and e.EventID=@EventID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and e.CarTypeID=@CarTypeID";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and e.OperatorID=@AdminID";
            }
            if (paras.CarType != "-1")
            {
                strSql += " and e.CarModelID=@CarModelID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and e.GateID=@GateID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and e.PlateNumber like @PlateNumber";
            }
            if (paras.InOrOut != -1)
            {
                strSql += " and e.IOState=@InOrOut";
            }
            if (paras.StartTime != null)
            {
                strSql += " and rectime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and rectime<=@EndTime";
            }
            strSql += " order by RecTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("EventID", paras.EventID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("CarModelID", paras.CarType);
                dboperator.AddParameter("GateID", paras.InGateID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InOrOut", paras.InOrOut);
                dboperator.AddParameter("AdminID", paras.AdminID);
                if (paras.StartTime != null)
                {
                    dboperator.AddParameter("StartTime", paras.StartTime);
                }
                if (paras.EndTime != null)
                {
                    dboperator.AddParameter("EndTime", paras.EndTime);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        parkeventlist.Add(DataReaderToModel<ParkEvent>.ToModel(dr));
                    }
                }
            }
            if (parkeventlist != null && parkeventlist.Count > 0)
            {
                foreach (var e in parkeventlist)
                {
                    e.EventName = ((ResultCode)e.EventID).GetDescription();
                }
            }
            return parkeventlist;
        }
        /// <summary>
        /// 查询通道进出记录
        /// </summary>
        /// <param name="parkevent">通道事件对象模型</param>
        /// <param name="PageSize">页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkEvent> Search_GateEvents(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkEvent> parkeventlist = new List<ParkEvent>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY e.rectime desc ) AS rownum,e.*,p.PKName ParkingName,g.GateName,card.CarTypeName,car.CarModelName,u.UserName Operator,case e.IOState when 1 then '进' when 2 then '出' else '' end IOStateName
                                           from parkevent e left join BaseParkinfo p on p.pkid=e.parkingid 
                                           left join parkgate g on e.gateid=g.gateid 
                                           left join SysUser u on u.recordid=e.OperatorID 
                                           left join ParkCarModel car on car.CarModelID=e.CarModelID 
                                           left join ParkCarType card on card.CarTypeID=e.CarTypeID
                                           where e.DataStatus!=2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and e.ParkingID =@ParkingID";
            }
            if (paras.EventID != -1)
            {
                strSql += " and e.EventID=@EventID";
            }
            if (paras.CardType != "-1")
            {
                strSql += " and e.CarTypeID=@CarTypeID";
            }
            if (paras.CarType != "-1")
            {
                strSql += " and e.CarModelID=@CarModelID";
            }
            if (paras.InGateID != "-1")
            {
                strSql += " and e.GateID=@GateID";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and e.OperatorID=@AdminID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and e.PlateNumber like @PlateNumber";
            }
            if (paras.InOrOut != -1)
            {
                strSql += " and e.IOState=@InOrOut";
            }
            if (paras.StartTime != null)
            {
                strSql += " and rectime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and rectime<=@EndTime";
            }
            strSql += string.Format(" order by e.RecTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by RecTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);


            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("EventID", paras.EventID);
                dboperator.AddParameter("CarTypeID", paras.CardType);
                dboperator.AddParameter("CarModelID", paras.CarType);
                dboperator.AddParameter("GateID", paras.InGateID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("InOrOut", paras.InOrOut);
                dboperator.AddParameter("AdminID", paras.AdminID);
                if (paras.StartTime != null)
                {
                    dboperator.AddParameter("StartTime", paras.StartTime);
                }
                if (paras.EndTime != null)
                {
                    dboperator.AddParameter("EndTime", paras.EndTime);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        parkeventlist.Add(DataReaderToModel<ParkEvent>.ToModel(dr));
                    }
                }
            }

            if (parkeventlist != null && parkeventlist.Count > 0)
            {
                foreach (var e in parkeventlist)
                {
                    e.EventName = ((ResultCode)e.EventID).GetDescription();
                }
            }

            return parkeventlist;
        }

        /// <summary>
        /// 通道事件数量
        /// </summary>
        /// <param name="parkevent">通道事件对象</param>
        /// <returns></returns>
        public int Search_DevConnectionCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from ParkEvent e where e.DataStatus!=2 and ( e.EventID=27 or  e.EventID=28)");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and e.ParkingID =@ParkingID";
            }

            if (paras.InGateID != "-1")
            {
                strSql += " and e.GateID=@GateID";
            }

            if (paras.StartTime != null)
            {
                strSql += " and rectime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and rectime<=@EndTime";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);

                dboperator.AddParameter("GateID", paras.InGateID);

                if (paras.StartTime != null)
                {
                    dboperator.AddParameter("StartTime", paras.StartTime);
                }
                if (paras.EndTime != null)
                {
                    dboperator.AddParameter("EndTime", paras.EndTime);
                }
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
        /// 查询通道进出记录
        /// </summary>
        /// <param name="parkevent">通道事件对象模型</param>
        /// <param name="PageSize">页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkEvent> Search_DevConnection(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkEvent> parkeventlist = new List<ParkEvent>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY e.rectime desc ) AS rownum,e.*,p.PKName ParkingName,g.GateName,u.UserName Operator,case e.IOState when 1 then '进' when 2 then '出' else '' end IOStateName
                                           from parkevent e left join BaseParkinfo p on p.pkid=e.parkingid 
                                           left join parkgate g on e.gateid=g.gateid 
                                           left join SysUser u on u.recordid=e.OperatorID 
                                        
                                           where e.DataStatus!=2 and (e.EventID=27 or e.EventID=28)", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and e.ParkingID =@ParkingID";
            }

            if (paras.InGateID != "-1")
            {
                strSql += " and e.GateID=@GateID";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and e.OperatorID=@AdminID";
            }

            if (paras.StartTime != null)
            {
                strSql += " and rectime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and rectime<=@EndTime";
            }
            strSql += string.Format(" order by e.RecTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by RecTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);


            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);

                dboperator.AddParameter("GateID", paras.InGateID);

                dboperator.AddParameter("AdminID", paras.AdminID);
                if (paras.StartTime != null)
                {
                    dboperator.AddParameter("StartTime", paras.StartTime);
                }
                if (paras.EndTime != null)
                {
                    dboperator.AddParameter("EndTime", paras.EndTime);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        parkeventlist.Add(DataReaderToModel<ParkEvent>.ToModel(dr));
                    }
                }
            }
            return parkeventlist;
        }
        #endregion

        #region 当班统计
        /// <summary>
        /// 当班统计数量
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_OnDutyCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from statistics_changeshift where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and parkingid =@ParkingID";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and AdminID=@AdminID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and Startworktime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                //strSql += " and EndWorkTime<=@EndTime";
                if (paras.StartTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd") ||
                      paras.EndTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd"))
                {
                    strSql += " and (EndWorkTime<=@EndTime or EndWorkTime is null)";
                }
                else
                {
                    //strSql += " and (s.EndWorkTime<=@EndTime or s.EndWorkTime is null)";
                    strSql += " and EndWorkTime<=@EndTime";
                }
            }
            if (paras.BoxID != null && paras.BoxID != "-1")
            {
                strSql += " and boxid=@BoxID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("AdminID", paras.AdminID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("BoxID", paras.BoxID);
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
        /// 获取当班统计记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<Statistics_ChangeShift> Search_OnDuty(InParams paras)
        {
            List<Statistics_ChangeShift> gatherlist = new List<Statistics_ChangeShift>();
            string strSql = "select s.* from statistics_changeshift s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid =@ParkingID";
            }
            if (paras.AdminID != "-1")
            {
                strSql += " and s.AdminID=@AdminID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.Startworktime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                //strSql += " and s.EndWorkTime<=@EndTime";
                if (paras.StartTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd") ||
                      paras.EndTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd"))
                {
                    strSql += " and (s.EndWorkTime<=@EndTime or s.EndWorkTime is null)";
                }
                else
                {
                    //strSql += " and (s.EndWorkTime<=@EndTime or s.EndWorkTime is null)";
                    strSql += " and s.EndWorkTime<=@EndTime";
                }
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and s.boxid=@BoxID";
            }
            strSql += " order by s.Startworktime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("AdminID", paras.AdminID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("BoxID", paras.BoxID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_ChangeShift>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        /// <summary>
        /// 获取当班统计记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<Statistics_ChangeShift> Search_OnDuty(InParams paras, int PageSize, int PageIndex)
        {
            List<Statistics_ChangeShift> gatherlist = new List<Statistics_ChangeShift>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY s.startworktime desc ) AS rownum,
s.*,h.UserName as AdminName,y.BoxName from statistics_changeshift s left join SysUser h on s.AdminID=h.RecordID left join ParkBox y on s.BoxID=y.BoxID where 1=1", PageIndex * PageSize);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid =@ParkingID";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and s.AdminID=@AdminID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.Startworktime>=@StartTime";
            }
            if (paras.EndTime != null)
            {

                if (paras.StartTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd") || 
                      paras.EndTime.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd")  )
                {
                    strSql += " and (s.EndWorkTime<=@EndTime or s.EndWorkTime is null)";
                }
                else
                {
                    //strSql += " and (s.EndWorkTime<=@EndTime or s.EndWorkTime is null)";
                    strSql += " and s.EndWorkTime<=@EndTime";
                }
            }
            if (paras.BoxID != null && paras.BoxID != "-1")
            {
                strSql += " and s.boxid=@BoxID";
            }
            strSql += string.Format(" order by s.Startworktime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by Startworktime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("AdminID", paras.AdminID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("BoxID", paras.BoxID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_ChangeShift>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        #endregion

        #region 订单明细
        /// <summary>
        /// 获得订单笔数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_OrdersCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from parkorder o left join parkiorecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join sysuser s on s.recordid=o.UserID                                             
                                             left join parkgrant g on g.gid=o.tagid
                                             left join BaseCard u on u.CardID=g.cardid  
                                             where 1=1");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (paras.OrderType != null && paras.OrderType != 0)
            {
                strSql += " and o.OrderType =@OrderType";
            }

            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }

            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber or u.CardNo like @PlateNumber";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("OrderType", paras.OrderType);

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
        /// 查询订单
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkOrder> Search_Orders(InParams paras)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select o.ID,u.CardNo,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,dic.dicvalue OrderTypeName,s.UserName Operator,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' else '其它' end OrderSourceName,o.RecordID,p.PKName,p.PKID,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,w.AccountName Mobile
                                             from parkorder o left join parkiorecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID 
                                             left join parkgrant g on g.gid=o.tagid
                                             left join BaseCard u on u.CardID=g.cardid 
                                             left join sysuser s on s.recordid=o.UserID
                                             left join WX_Account w on w.accountid=o.onlineuserid
                                             left join SysDictionary dic on dic.dicid=o.ordertype and dic.catagrayname='OrderType'                                        
                                             where 1=1");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }

            if (paras.OrderType != null && paras.OrderType != 0)
            {
                strSql += " and o.OrderType =@OrderType";
            }

            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber or u.CardNo like @PlateNumber";
            }
            strSql += " order by o.ordertime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("OrderType", paras.OrderType);

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkOrder> Search_Orders(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum, o.ID,u.CardNo,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,dic.dicvalue OrderTypeName,s.UserName Operator,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' else '其它' end OrderSourceName,o.RecordID,p.PKName,p.PKID,o.OrderNo,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,w.AccountName MobilePhone
                                             from parkorder o left join parkiorecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID
                                             left join parkgrant g on g.gid=o.tagid
                                             left join BaseCard u on u.CardID=g.cardid 
                                             left join sysuser s on s.recordid=o.UserID
                                             left join WX_Account w on w.accountid=o.onlineuserid
                                             left join SysDictionary dic on dic.dicid=o.ordertype and dic.catagrayname='OrderType'                                          
                                             where 1=1", PageSize * PageIndex);
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (paras.OrderType != null && paras.OrderType != 0)
            {
                strSql += " and o.OrderType =@OrderType";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and i.platenumber like @PlateNumber or u.CardNo like @PlateNumber";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("OrderType", paras.OrderType);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }
        #endregion

        #region 线上支付
        /// <summary>
        /// 获得线上支付笔数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_OnlinePaysCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from   ParkOrder as aa

                                           left join OnlineOrder ff on aa.OnlineOrderNo= ff.OrderID
                                        left join BaseParkinfo bp on aa.PKID=bp.PKID
                                        left join ParkIORecord br on aa.TagID=br.RecordID
										left join WX_Info mm on ff.Payer=mm.OpenID
                                       where  aa.PayWay>1   ");

                                         

            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and aa.PKID =@ParkingID";
            }
            if (paras.PayWay > 0)
            {
                strSql += " and aa.PayWay =@PayWay";
            }

            if (paras.StartTime != null)
            {
                strSql += " and aa.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and aa.OrderTime<=@EndTime";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and br.PlateNumber like @PlateNumber ";
            }


            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PayWay", paras.PayWay);
                //dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");


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
        /// 查询线上支付
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<OnlineOrder> Search_OnlinePays(InParams paras)
        {
            List<OnlineOrder> onlineorderlist = new List<OnlineOrder>();
            string strSql = string.Format(@"select aa.OrderNo,bp.PKName,br.PlateNumber,
                                        (CASE WHEN  aa.PayWay = 2 then '微信' when aa.PayWay=3 then'支付宝' when aa.PayWay=4 then'网银' when aa.PayWay=5 then'电子钱包' when aa.PayWay=6 then'优免劵' when aa.PayWay=7 then'余额'END)  as fs,
                                        (case when  aa.OrderType=1 then '临时卡缴费' when aa.OrderType=2 then '月卡缴费'when aa.OrderType=3 then 'VIP卡续期'when aa.OrderType=4 then '储值卡充值'when aa.OrderType=5 then '临时卡续期'when aa.OrderType=6 then '储值卡缴费' end)as lx,
										(CASE WHEN  aa.Status = -1 then '失效' when aa.Status=0 then'未生效' when aa.Status=1 then'生效' when aa.Status=2 then'未确认'END)  as zt,
                                        mm.NickName,ff.SyncResultTimes,ff.LastSyncResultTime,ff.RefundOrderId,aa.OrderTime,aa.PayTime,ff.MonthNum,aa.PayAmount
                                        from  ParkOrder as aa
                                        left join OnlineOrder ff on aa.OnlineOrderNo= ff.OrderID                                      
                                        left join BaseParkinfo bp on aa.PKID=bp.PKID
                                        left join ParkIORecord br on aa.TagID=br.RecordID
										left join WX_Info mm on ff.Payer=mm.OpenID
                                       where  aa.PayWay>1 

                                               ");
            
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and aa.PKID =@ParkingID";
            }
            if (paras.PayWay > 0)
            {
                strSql += " and aa.PayWay =@PayWay";
            }

            if (paras.StartTime != null)
            {
                strSql += " and aa.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and aa.OrderTime<=@EndTime";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and br.PlateNumber like @PlateNumber ";
            }
            strSql += " order by aa.OrderTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PayWay", paras.PayWay);
                //dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        onlineorderlist.Add(DataReaderToModel<OnlineOrder>.ToModel(dr));
                    }
                }
            }
            return onlineorderlist;
        }
        /// <summary>
        /// 查询线上支付
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<OnlineOrder> Search_OnlinePays(InParams paras, int PageSize, int PageIndex)
        {
            List<OnlineOrder> onlineorderlist = new List<OnlineOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY aa.OrderTime desc ) AS rownum, aa.OrderNo,bp.PKName,br.PlateNumber,
                                        (CASE WHEN  aa.PayWay = 2 then '微信' when aa.PayWay=3 then'支付宝' when aa.PayWay=4 then'网银' when aa.PayWay=5 then'电子钱包' when aa.PayWay=6 then'优免劵' when aa.PayWay=7 then'余额'END)  as fs,
                                        (case when  aa.OrderType=1 then '临时卡缴费' when aa.OrderType=2 then '月卡缴费'when aa.OrderType=3 then 'VIP卡续期'when aa.OrderType=4 then '储值卡充值'when aa.OrderType=5 then '临时卡续期'when aa.OrderType=6 then '储值卡缴费' end)as lx,
										(CASE WHEN  aa.Status = -1 then '失效' when aa.Status=0 then'未生效' when aa.Status=1 then'生效' when aa.Status=2 then'未确认'END)  as zt,
                                        mm.NickName,ff.SyncResultTimes,ff.LastSyncResultTime,ff.RefundOrderId,aa.OrderTime,aa.PayTime,ff.MonthNum,aa.PayAmount
                                        from  ParkOrder as aa
                                        left join OnlineOrder ff on aa.OnlineOrderNo= ff.OrderID                                      
                                        left join BaseParkinfo bp on aa.PKID=bp.PKID
                                        left join ParkIORecord br on aa.TagID=br.RecordID
										left join WX_Info mm on ff.Payer=mm.OpenID
                                       where  aa.PayWay>1                                     
                                             ", PageSize * PageIndex);

                                      

            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and aa.PKID =@ParkingID";
            }
            if (paras.PayWay > 0)
            {
                strSql += " and aa.PayWay =@PayWay";
            }


            if (paras.StartTime != null)
            {
                strSql += " and aa.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and aa.OrderTime<=@EndTime";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and br.PlateNumber like @PlateNumber ";
            }
            //strSql += " order by aa.OrderTime desc";
            strSql += string.Format(" order by aa.OrderTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by OrderTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PayWay", paras.PayWay);
                //dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        onlineorderlist.Add(DataReaderToModel<OnlineOrder>.ToModel(dr));
                    }
                }
            }
            return onlineorderlist;
        }
        #endregion

        #region 车辆进出场数量
        /// <summary>
        /// 获得车辆进出场记录数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_ParkIOsCount(InParams paras)
        {
            int _total = 0;
           
            return _total;
        }
        /// <summary>
        /// 查询车辆进出场数量
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_ParkIOs(InParams paras)
        {
            List<ParkIORecord> parkiolist = new List<ParkIORecord>();
            string strSql = string.Format(@"select b.PKName,sum (CASE WHEN EntranceTime>=@StartTime and EntranceTime<@EndTime THEN 1 ELSE 0 END) as EntranceNumber ,
                                            sum (CASE WHEN ExitTime >=@StartTime and ExitTime<@EndTime THEN 1 ELSE 0 END) as ExitNumber 
                                               from ParkIORecord as i left join BaseParkinfo b on i.ParkingID=b.PKID
                                                where 1=1
                                               ");

            if (!string.IsNullOrEmpty(paras.ParkingID) && (paras.ParkingID != "-1"))
            {
                strSql += " and i.ParkingID =@ParkingID";
            }

            strSql += " group by b.PKName";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);


                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        parkiolist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return parkiolist;
        }
        /// <summary>
        /// 查询车辆进出场数量
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkIORecord> Search_ParkIOs(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkIORecord> parkiolist = new List<ParkIORecord>();
            string strSql = string.Format(@"select b.PKName,sum (CASE WHEN EntranceTime>=@StartTime and EntranceTime<@EndTime THEN 1 ELSE 0 END) as EntranceNumber ,
                                            sum (CASE WHEN ExitTime >=@StartTime and ExitTime<@EndTime THEN 1 ELSE 0 END) as ExitNumber 
                                               from ParkIORecord as i left join BaseParkinfo b on i.ParkingID=b.PKID
                                                where 1=1
                                               ", PageSize * PageIndex);

            if (!string.IsNullOrEmpty(paras.ParkingID) && (paras.ParkingID!="-1"))
            {
                strSql += " and i.ParkingID =@ParkingID";
            }

            strSql += " group by b.PKName";
            //strSql += string.Format(" order by aa.OrderTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by aa.OrderTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        parkiolist.Add(DataReaderToModel<ParkIORecord>.ToModel(dr));
                    }
                }
            }
            return parkiolist;
        }
        #endregion


        #region 月卡续期
        /// <summary>
        /// 获取卡片续期笔数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_CardExtensionCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count 
                                            from parkorder o 
                                            left join parkgrant g on g.gid=o.tagid
                                            left join BaseCard u on u.CardID=g.cardid 
                                            left join BaseEmployee w on w.EmployeeID=u.EmployeeID 
                                            left join SysUser su on su.recordid=o.userid 
                                            left join BaseParkinfo p on o.PKID=p.PKID 
                                            where o.status=1 and ordertype=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and w.EmployeeName like @Owner";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and u.cardno like @PlateNumber";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", "%" + paras.Owner + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("AdminID", paras.AdminID);
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
        /// 获得卡片续期记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkOrder> Search_CardExtension(InParams paras)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select p.PKName,u.CardNo,w.EmployeeName,
                                            case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优免券' when 7 then '储值余额' else '其他' end PayWayName,
                                            o.Amount,o.OrderTime,o.NewUsefulDate,o.OldUserulDate,o.Remark,su.UserName Operator 
                                            from parkorder o 
                                            left join parkgrant g on g.gid=o.tagid
                                            left join BaseCard u on u.CardID=g.cardid  
                                            left join BaseEmployee w on w.EmployeeID=u.EmployeeID 
                                            left join SysUser su on su.recordid=o.userid 
                                            left join BaseParkinfo p on o.PKID=p.PKID 
                                            where o.Status=1 and ordertype=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and w.EmployeeName like @Owner";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and u.cardno like @PlateNumber";
            }
            strSql += " order by o.OrderTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", "%" + paras.Owner + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }
        /// <summary>
        /// 获取卡片续期记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkOrder> Search_CardExtension(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum,p.PKName,u.CardNo,w.EmployeeName,
                                           case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优免券' when 7 then '储值余额' else '其他' end PayWayName,
                                           o.Amount,o.OrderTime,o.NewUsefulDate,o.OldUserulDate,o.Remark,su.UserName Operator 
                                           from parkorder o 
                                           left join parkgrant g on g.gid=o.tagid
                                           left join BaseCard u on u.CardID=g.cardid 
                                           left join BaseEmployee w on w.EmployeeID=u.EmployeeID 
                                           left join SysUser su on su.recordid=o.userid 
                                           left join BaseParkinfo p on o.PKID=p.PKID 
                                           where o.status=1 and ordertype=2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and w.EmployeeName like @Owner";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and u.cardno like @PlateNumber";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", "%" + paras.Owner + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("AdminID", paras.AdminID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }
        #endregion

        #region 储值卡充值
        /// <summary>
        /// 获取卡片充值笔数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_CardRechargeCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count 
                                            from parkorder o 
                                         
                                            left join BaseCard u on u.CardID=o.tagid
                                            left join BaseEmployee w on w.EmployeeID=u.EmployeeID 
                                            left join SysUser su on su.recordid=o.userid 
                                            left join BaseParkinfo p on o.PKID=p.PKID 
                                            where o.status=1 and ordertype=4");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and w.EmployeeName like @Owner";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and u.cardno like @PlateNumber";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", "%" + paras.Owner + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("AdminID", paras.AdminID);
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
        /// 获取卡片充值记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkOrder> Search_CardRecharge(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum,p.PKName,u.CardNo,w.EmployeeName,
                                           case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优免券' when 7 then '储值余额' else '其他' end PayWayName,
                                           o.Amount,o.OrderTime,o.OldMoney,o.NewMoney,o.Remark,su.UserName Operator 
                                           from parkorder o 
                               
                                           left join BaseCard u on u.CardID=o.tagid 
                                           left join BaseEmployee w on w.EmployeeID=u.EmployeeID 
                                           left join SysUser su on su.recordid=o.userid 
                                           left join BaseParkinfo p on o.PKID=p.PKID 
                                           where o.status=1 and ordertype=4", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrEmpty(paras.Owner))
            {
                strSql += " and w.EmployeeName like @Owner";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(paras.PlateNumber))
            {
                strSql += " and u.cardno like @PlateNumber";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Owner", "%" + paras.Owner + "%");
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("AdminID", paras.AdminID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }
        #endregion

        #region 临停缴费
        /// <summary>
        /// 临停缴费记录数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_TempPaysCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join SysUser s on s.recordid=o.UserID                                            
                                             where (o.ordertype=1 or o.ordertype=7)");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }

            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }

            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }

            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);
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
        /// 获取临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkOrder> Search_TempPays(InParams paras)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"select o.ID,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,
                                            (select a.username from SysUser a where recordid=o.userid) Operator,i.EntranceImage,i.ExitImage,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' else '其它' end OrderSourceName,o.RecordID,p.PKName,p.PKID,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,s.AccountName Mobile
                                             from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join WX_Account s on s.AccountID=o.UserID  
                                             where (o.ordertype=1 or o.ordertype=7)");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            strSql += " order by o.ordertime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }


        /// <summary>
        /// 获取临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ReportPoundNoteModel> Search_TempPaysPound(InParams paras)
        {
            List<ReportPoundNoteModel> orderlist = new List<ReportPoundNoteModel>();
            string strSql = string.Format(@"select o.ID,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,i.NetWeight,i.Tare,i.NetWeight+i.Tare as ZZWeight,i.Goods,i.Shipper,i.Shippingspace,
                                            (select a.username from SysUser a where recordid=o.userid) Operator,i.EntranceImage,i.ExitImage,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' else '其它' end OrderSourceName,o.RecordID,p.PKName,p.PKID,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,s.AccountName Mobile
                                             from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join WX_Account s on s.AccountID=o.UserID  
                                             where (o.ordertype=1 or o.ordertype=7)");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            strSql += " order by o.ordertime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ReportPoundNoteModel>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }

        /// <summary>
        /// 获取临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkOrder> Search_TempPays(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum,o.ID,o.RecordID,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,o.TagID,ReleaseType,
                                            (select a.username from SysUser a where recordid=o.userid) Operator,i.EntranceImage,i.ExitImage,h.CarModelName,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' when 6 then '第三方平台' else '其它' end OrderSourceName,o.OrderNo,p.PKName,p.PKID,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,s.AccountName Mobile
                                             from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join WX_Account s on s.AccountID=o.UserID   
                                              left join  ParkCarModel h on h.CarModelID=i.CarModelID 
                                             where (o.ordertype=1 or o.ordertype=7)", PageSize * PageIndex);
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.DiffAmount)
            {
                strSql += " and o.PayAmount!=o.Amount";
            }
            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }

        public AmoutModel Search_TempPayCount(InParams paras)
        {
            AmoutModel model = new AmoutModel();
            string strSql = string.Format(@"SELECT  sum(o.Amount) as YS,sum(o.UnPayAmount) as WS,sum(o.DiscountAmount) as ZK 
                                             from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join WX_Account s on s.AccountID=o.UserID   
                                              left join  ParkCarModel h on h.CarModelID=i.CarModelID 
                                             where (o.ordertype=1 or o.ordertype=7)");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (!paras.DiffAmount)
            {
                strSql += " and o.UnPayAmount=0";
            }
            else
            {
                strSql += "and o.UnPayAmount!=0";
            }
            if (paras.ReleaseType != -1)
            {
                strSql += " and i.ReleaseType=@ReleaseType";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                dboperator.AddParameter("ReleaseType", paras.ReleaseType);

                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        model = DataReaderToModel<AmoutModel>.ToModel(dr);
                    }
                }
            }
            return model;
        }
        #endregion

        #region 储值卡缴费

        /// <summary>
        /// 储值卡缴费纪录数
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int Search_RechargePaysCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join SysUser s on s.recordid=o.UserID                                            
                                             where (o.ordertype=6 or o.ordertype=8)");
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
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
        /// 获取临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkOrder> Search_RechargePays(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum,o.ID,o.Remark,o.PayAmount,o.UnPayAmount,o.DiscountAmount,o.TagID,
                                            (select a.username from SysUser a where recordid=o.userid) Operator,i.EntranceImage,i.ExitImage,
                                             case o.OrderSource when 1 then '微信' when 2 then 'APP' when 3 then '中心缴费' when 4 then '岗亭收费' when 5 then '管理处' else '其它' end OrderSourceName,o.OrderNo,p.PKName,p.PKID,
                                             case o.PayWay when 1 then '现金' when 2 then '微信' when 3 then '支付宝' when 4 then '网银' when 5 then '电子钱包' when 6 then '优惠券' when 7 then '余额' end PayWayName, 
                                             o.Amount,o.OrderTime,i.PlateNumber,i.EntranceTime,i.ExitTime,s.AccountName Mobile
                                             from parkorder o left join ParkIORecord i on i.recordid=o.tagid 
                                             left join BaseParkinfo p on p.PKID=o.PKID left join WX_Account s on s.AccountID=o.UserID   
                                             where (o.ordertype=6 or o.ordertype=8)", PageSize * PageIndex);
            if (paras.IsNoConfirm)
            {
                strSql += " and o.status=2";
            }
            else
            {
                strSql += " and o.status=1";
            }
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and o.PKID =@ParkingID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and i.PlateNumber like @PlateNumber";
            }
            if (paras.StartTime != null)
            {
                strSql += " and o.ordertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.ordertime<=@EndTime";
            }
            if (paras.OrderSource > 0)
            {
                strSql += " and o.OrderSource=@OrderSource";
            }
            switch (paras.OnLineOffLine)
            {
                // 1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                //线下
                case 0:
                    strSql += " and o.PayWay in (1,6,7)";
                    break;
                //线上
                case 1:
                    strSql += " and o.PayWay in (2,3,4,5)";
                    break;
                default:
                    break;
            }
            if (!paras.Zero)
            {
                strSql += " and o.Amount!= 0";
            }
            else
            {
                strSql += " and o.Amount= 0";
            }
            if (paras.BoxID != "-1")
            {
                strSql += " and i.exitgateid in (select gateid from parkgate where boxid=@BoxID)";
            }
            if (paras.OutOperator != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("OrderSource", paras.OrderSource);
                dboperator.AddParameter("BoxID", paras.BoxID);
                dboperator.AddParameter("AdminID", paras.OutOperator);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }

        #endregion

        #region 车辆优免
        /// <summary>
        /// 车辆优免记录数
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public int Search_CarDeratesCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(*) Count from ParkCarDerate c left join parkderate d on d.derateid=c.derateid 
                                            left join parkseller s on s.sellerid=d.sellerid where c.DataStatus!=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and c.pkid =@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and c.CreateTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and c.CreateTime<=@EndTime";
            }
            if (paras.Status > -1)
            {
                strSql += " and c.status=@Status";
            }
            if (paras.SellerID != "-1")
            {
                strSql += " and s.sellerid =@SellerID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and c.PlateNumber like @PlateNumber";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
                {
                    dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                }
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("SellerID", paras.SellerID);
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
        /// 查询商家优免记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public List<ParkCarDerate> Search_CarDerates(InParams paras)
        {
            List<ParkCarDerate> carderatelist = new List<ParkCarDerate>();
            string strSql = string.Format(@"select c.FreeTime,c.PlateNumber,c.CardNo,c.ExpiryTime,c.CreateTime,c.Status,c.FreeMoney,c.AreaID,
                                           p.PKName,a.AreaName,s.SellerName,d.Name RuleName,i.EntranceImage,i.ExitImage,
                                           case c.status when 0 then '正常' when 1 then '已使用' when 2 then '已结算' when 3 then '已作废' else '' end StatusName 
                                           from parkcarderate c 
                                           left join parkderate d on d.derateid=c.derateid
                                           left join BaseParkinfo p on p.pkid=c.pkid 
                                           left join parkarea a on a.areaid=c.areaid
                                           left join parkiorecord i on i.recordid=c.IORecordID 
                                           left join parkseller s on s.sellerid=d.sellerid where c.DataStatus!=2");
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and c.pkid =@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and c.CreateTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and c.CreateTime<=@EndTime";
            }
            if (paras.Status > -1)
            {
                strSql += " and c.status=@Status";
            }
            if (paras.SellerID != "-1")
            {
                strSql += " and s.sellerid =@SellerID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and c.PlateNumber like @PlateNumber";
            }
            strSql += " order by c.CreateTime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
                {
                    dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                }
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("SellerID", paras.SellerID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        carderatelist.Add(DataReaderToModel<ParkCarDerate>.ToModel(dr));
                    }
                }
            }

            return carderatelist;
        }
        /// <summary>
        /// 查询商家优免记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<ParkCarDerate> Search_CarDerates(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkCarDerate> carderatelist = new List<ParkCarDerate>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY c.createtime desc ) AS rownum,c.FreeTime,c.PlateNumber,c.CardNo,c.ExpiryTime,c.CreateTime,c.Status,c.FreeMoney,c.AreaID,
                                           p.PKName ParkingName,a.AreaName,s.SellerName,d.Name RuleName,i.EntranceImage,i.ExitImage,
                                           case c.status when 0 then '正常' when 1 then '使用中' when 2 then '已结算' when 3 then '已作废' else '' end StatusName 
                                           from parkcarderate c 
                                           left join parkderate d on d.derateid=c.derateid
                                           left join BaseParkinfo p on p.pkid=c.pkid 
                                           left join parkarea a on a.areaid=c.areaid
                                           left join parkiorecord i on i.recordid=c.IORecordID 
                                           left join parkseller s on s.sellerid=d.sellerid where c.DataStatus!=2", PageSize * PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and c.pkid =@ParkingID";
            }

            if (!string.IsNullOrEmpty(paras.VID))
            {
                strSql += " and s.VID =@VID";
            }

            if (paras.StartTime != null)
            {
                strSql += " and c.CreateTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and c.CreateTime<=@EndTime";
            }
            if (paras.Status > -1)
            {
                strSql += " and c.status=@Status";
            }
            if (paras.SellerID != "-1")
            {
                strSql += " and s.sellerid =@SellerID";
            }
            if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
            {
                strSql += " and c.PlateNumber like @PlateNumber";
            }
            strSql += string.Format(" order by c.CreateTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by CreateTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
                {
                    dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                }
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("Status", paras.Status);
                dboperator.AddParameter("SellerID", paras.SellerID);
                dboperator.AddParameter("VID", paras.VID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        carderatelist.Add(DataReaderToModel<ParkCarDerate>.ToModel(dr));
                    }
                }
            }
            return carderatelist;
        }
        #endregion

        #region 日收费 汇总报表
        /// <summary>
        /// 日报表数量
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public int Search_DailyStatisticsCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from (select convert(nvarchar(10),gathertime,120) as GName from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<@EndTime";
            }
            strSql += " group by convert(nvarchar(10), gathertime,120)) a";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
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
        /// 日统计数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> Search_DailyStatistics(InParams paras)
        {
            List<Statistics_Gather> statisticsgatherlist = new List<Statistics_Gather>();
            string strSql = "select convert(nvarchar(10),gathertime,120) as KeyName,sum(s.Receivable_Amount) Receivable_Amount"
                         + ",sum(s.Real_Amount) Real_Amount,sum(s.Diff_Amount) Diff_Amount,sum(s.Cash_Amount) Cash_Amount,sum(s.StordCard_Amount) StordCard_Amount"
                         + ",sum(s.OnLine_Amount) OnLine_Amount,sum(s.Discount_Amount) Discount_Amount,sum(s.ReleaseType_Normal) ReleaseType_Normal,sum(s.ReleaseType_Charge) ReleaseType_Charge,sum(s.ReleaseType_Free) ReleaseType_Free"
                         + ",sum(s.ReleaseType_Catch) ReleaseType_Catch,sum(s.VIPExtend_Count) VIPExtend_Count,sum(s.Entrance_Count) Entrance_Count,sum(s.Exit_Count) Exit_Count,sum(s.OnLineMonthCardExtend_Count) OnLineMonthCardExtend_Count"
                         + ",sum(s.OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount,sum(s.MonthCardExtend_Count) MonthCardExtend_Count,sum(s.MonthCardExtend_Amount) MonthCardExtend_Amount"
                         + ",sum(s.OnlineStordCard_Count) OnLineStordCard_Count,sum(s.OnLineStordCard_Amount) OnLineStordCard_Amount,sum(s.StordCardRecharge_Count) StordCardRecharge_Count"
                         + ",sum(s.StordCardRecharge_Amount) StordCardRecharge_Amount,sum(s.Temp_Amount) Temp_Amount,sum(OnLineTemp_Amount) OnLineTemp_Amount"
                         + " from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<@EndTime";
            }
            strSql += " group by convert(nvarchar(10),gathertime,120) order by convert(nvarchar(10),gathertime,120) desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        statisticsgatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return statisticsgatherlist;
        }
        /// <summary>
        /// 日统计数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<Statistics_Gather> Search_DailyStatistics(InParams paras, int PageSize, int PageIndex)
        {
            List<Statistics_Gather> statisticsgatherlist = new List<Statistics_Gather>();
            string strSql = "select top " + PageSize + " temp.* from ( select top " + PageIndex * PageSize + " convert(nvarchar(10),gathertime,120) as KeyName,sum(s.Receivable_Amount) Receivable_Amount"
                         + ",sum(s.Real_Amount) Real_Amount,sum(s.Diff_Amount) Diff_Amount,sum(s.Cash_Amount) Cash_Amount,sum(s.StordCard_Amount) StordCard_Amount"
                         + ",sum(s.OnLine_Amount) OnLine_Amount,sum(s.Discount_Amount) Discount_Amount,sum(s.ReleaseType_Normal) ReleaseType_Normal,sum(s.ReleaseType_Charge) ReleaseType_Charge,sum(s.ReleaseType_Free) ReleaseType_Free"
                         + ",sum(s.ReleaseType_Catch) ReleaseType_Catch,sum(s.VIPExtend_Count) VIPExtend_Count,sum(s.Entrance_Count) Entrance_Count,sum(s.Exit_Count) Exit_Count,sum(s.OnLineMonthCardExtend_Count) OnLineMonthCardExtend_Count"
                         + ",sum(s.OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount,sum(s.MonthCardExtend_Count) MonthCardExtend_Count,sum(s.MonthCardExtend_Amount) MonthCardExtend_Amount"
                         + ",sum(s.OnlineStordCard_Count) OnLineStordCard_Count,sum(s.OnLineStordCard_Amount) OnLineStordCard_Amount,sum(s.StordCardRecharge_Count) StordCardRecharge_Count"
                         + ",sum(s.StordCardRecharge_Amount) StordCardRecharge_Amount,sum(s.Temp_Amount) Temp_Amount,sum(OnLineTemp_Amount) OnLineTemp_Amount"
                         + " from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<@EndTime";
            }
            strSql += " group by convert(nvarchar(10),gathertime,120) order by convert(nvarchar(10),gathertime,120) asc) as temp order by temp.KeyName desc";
            //strSql += string.Format(" order by c.CreateTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by CreateTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        statisticsgatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return statisticsgatherlist;
        }
        #endregion

        #region 月收费 汇总报表
        /// <summary>
        /// 月统计数量
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public int Search_MonthStatisticsCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from (select convert(nvarchar(7),gathertime,120) as KeyName from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<=@EndTime";
            }
            strSql += " group by convert(nvarchar(7), gathertime,120)) a";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
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
        /// 月统计数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> Search_MonthStatistics(InParams paras)
        {
            List<Statistics_Gather> gatherlist = new List<Statistics_Gather>();
            string strSql = "select convert(nvarchar(7),gathertime,120) as KeyName,sum(s.Receivable_Amount) Receivable_Amount"
                         + ",sum(s.Real_Amount) Real_Amount,sum(s.Diff_Amount) Diff_Amount,sum(s.Cash_Amount) Cash_Amount,sum(s.StordCard_Amount) StordCard_Amount"
                         + ",sum(s.OnLine_Amount) OnLine_Amount,sum(s.Discount_Amount) Discount_Amount,sum(s.ReleaseType_Normal) ReleaseType_Normal,sum(s.ReleaseType_Charge) ReleaseType_Charge,sum(s.ReleaseType_free) ReleaseType_Free"
                         + ",sum(s.ReleaseType_Catch) ReleaseType_Catch,sum(s.VIPExtend_Count) VIPExtend_Count,sum(s.Entrance_Count) Entrance_Count,sum(s.Exit_Count) Exit_Count,sum(s.OnLineMonthCardExtend_Count) OnLineMonthCardExtend_Count"
                         + ",sum(s.OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount,sum(s.MonthCardExtend_Count) MonthCardExtend_Count,sum(s.MonthCardExtend_Amount) MonthCardExtend_Amount"
                         + ",sum(s.OnlineStordCard_Count) OnLineStordCard_Count,sum(s.OnLineStordCard_Amount) OnLineStordCard_Amount,sum(s.StordCardRecharge_Count) StordCardRecharge_Count"
                         + ",sum(s.StordCardRecharge_Amount) StordCardRecharge_Amount,sum(s.Temp_Amount) Temp_Amount,sum(s.OnLineTemp_Amount) OnLineTemp_Amount"
                         + " from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<=@EndTime";
            }
            strSql += " group by convert(nvarchar(7),gathertime,120) order by convert(nvarchar(7),gathertime,120)";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        /// <summary>
        /// 月统计数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<Statistics_Gather> Search_MonthStatistics(InParams paras, int PageSize, int PageIndex)
        {
            List<Statistics_Gather> gatherlist = new List<Statistics_Gather>();
            string strSql = "select top " + PageSize + " temp.* from ( select top " + PageIndex * PageSize + " convert(nvarchar(7),gathertime,120) as KeyName,sum(s.Receivable_Amount) Receivable_Amount"
                         + ",sum(s.Real_Amount) Real_Amount,sum(s.Diff_Amount) Diff_Amount,sum(s.Cash_Amount) Cash_Amount,sum(s.StordCard_Amount) StordCard_Amount"
                         + ",sum(s.OnLine_Amount) OnLine_Amount,sum(s.Discount_Amount) Discount_Amount,sum(s.ReleaseType_Normal) ReleaseType_Normal,sum(s.ReleaseType_Charge) ReleaseType_Charge,sum(s.ReleaseType_free) ReleaseType_Free"
                         + ",sum(s.ReleaseType_Catch) ReleaseType_Catch,sum(s.VIPExtend_Count) VIPExtend_Count,sum(s.Entrance_Count) Entrance_Count,sum(s.Exit_Count) Exit_Count,sum(s.OnLineMonthCardExtend_Count) OnLineMonthCardExtend_Count"
                         + ",sum(s.OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount,sum(s.MonthCardExtend_Count) MonthCardExtend_Count,sum(s.MonthCardExtend_Amount) MonthCardExtend_Amount"
                         + ",sum(s.OnlineStordCard_Count) OnLineStordCard_Count,sum(s.OnLineStordCard_Amount) OnLineStordCard_Amount,sum(s.StordCardRecharge_Count) StordCardRecharge_Count"
                         + ",sum(s.StordCardRecharge_Amount) StordCardRecharge_Amount,sum(s.Temp_Amount) Temp_Amount,sum(s.OnLineTemp_Amount) OnLineTemp_Amount"
                         + " from statistics_gather s where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and s.parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and s.gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and s.gathertime<=@EndTime";
            }
            strSql += " group by convert(nvarchar(7),gathertime,120) order by convert(nvarchar(7),gathertime,120) asc) as temp order by temp.KeyName desc";
            ////strSql += string.Format(" order by c.CreateTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by CreateTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        #endregion
        /// <summary>
        /// 取得临停前5的车场
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> GetParkingTempTop5(List<string> Parkings, DateTime starttime, DateTime endtime)
        {
            List<Statistics_Gather> gatherlist = new List<Statistics_Gather>();
            string condition = "1=1";
            string strtemp = string.Empty;
            if (Parkings != null && Parkings.Count > 0)
            {
                bool isfirst = true;
                foreach (string parking in Parkings)
                {
                    if (isfirst)
                    {
                        strtemp = "'" + parking + "'";
                        isfirst = false;
                    }
                    else
                    {
                        strtemp = strtemp + ",'" + parking + "'";
                    }
                }
                condition += " and s.parkingid in (" + strtemp + ")";
            }
            string strSql = string.Format(@"select top 5 t.*,p.pkname ParkingName from (select sum(s.TempCard) TempCard,s.ParkingID
                                          from statistics_gather s where gathertime>=@StartTime and {0} group by s.ParkingID) t left join baseparkinfo p on p.pkid=t.parkingid order by t.TempCard", condition);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }

        #region 按日分组统计数据
        /// <summary>
        /// 按日分组统计数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> GetStatisticsGroupByDay(List<string> Parkings, DateTime StartTime, DateTime EndTime)
        {
            List<Statistics_Gather> gatherlist = new List<Statistics_Gather>();
            string condition = "1=1";
            string strtemp = string.Empty;
            if (Parkings != null && Parkings.Count > 0)
            {
                bool isfirst = true;
                foreach (string parking in Parkings)
                {
                    if (isfirst)
                    {
                        strtemp = "'" + parking + "'";
                        isfirst = false;
                    }
                    else
                    {
                        strtemp = strtemp + ",'" + parking + "'";
                    }
                }
                condition += " and parkingid in (" + strtemp + ")";
            }
            string strSql = string.Format(@"select convert(nvarchar(10),gathertime,120) as KeyName,sum(Entrance_Count) Entrance_Count,
                                            sum(online_amount) OnLine_Amount,sum(receivable_amount) Receivable_Amount,sum(real_amount) Real_Amount,
                                            sum(cash_amount) Cash_Amount,sum(vipcard) VIPCard,sum(stordcard) StordCard,sum(Temp_Count) Temp_Count,sum(Temp_Amount) Temp_Amount,
                                            sum(monthcard) MonthCard,sum(jobcard) JobCard,sum(tempcard) TempCard,
                                            sum(releasetype_free) ReleaseType_Free,sum(releasetype_catch) ReleaseType_Catch,
                                            sum(releasetype_normal) ReleaseType_Normal,sum(releasetype_charge) ReleaseType_Charge,sum(OnLineTemp_Amount) OnLineTemp_Amount,sum(OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount
                                            from statistics_gather where {0} and gathertime>=@starttime and gathertime<=@endtime 
                                            group by convert(nvarchar(10),gathertime,120) order by convert(nvarchar(10),gathertime,120)", condition);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("StartTime", StartTime);
                dboperator.AddParameter("EndTime", EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        #endregion

        #region 按月份分组统计数据
        /// <summary>
        /// 按月份分组
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> GetStatisticsGroupByMonth(List<string> Parkings, DateTime StartTime, DateTime EndTime)
        {
            List<Statistics_Gather> gatherlist = new List<Statistics_Gather>();
            string condition = "1=1";
            string strtemp = string.Empty;
            if (Parkings != null && Parkings.Count > 0)
            {
                bool isfirst = true;
                foreach (string parking in Parkings)
                {
                    if (isfirst)
                    {
                        strtemp = "'" + parking + "'";
                        isfirst = false;
                    }
                    else
                    {
                        strtemp = strtemp + ",'" + parking + "'";
                    }
                }
                condition += " and parkingid in (" + strtemp + ")";
            }
            string strSql = string.Format(@"select convert(nvarchar(7),gathertime,120) as KeyName,sum(Entrance_Count) Entrance_Count,
                                          sum(receivable_amount) Receivable_Amount,sum(real_amount) Real_Amount,sum(cash_amount) Cash_Amount,sum(online_amount) OnLine_Amount,
                                          sum(OnLineTemp_Count) OnLineTemp_Count,sum(OnLineMonthCardExtend_Count) OnLineMonthCardExtend_Count,sum(Temp_Count) Temp_Count,
                                          sum(OnLineMonthCardExtend_Amount) OnLineMonthCardExtend_Amount,sum(MonthCardExtend_Count) MonthCardExtend_Count,
                                          sum(MonthCardExtend_Amount) MonthCardExtend_Amount,sum(Cash_Count) Cash_Count 
                                          from statistics_gather 
                                          where {0} and gathertime>=@StartTime 
                                          and gathertime<=@EndTime group by convert(nvarchar(7),gathertime,120) order by convert(nvarchar(7),gathertime,120)", condition);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("StartTime", StartTime);
                dboperator.AddParameter("EndTime", EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }
            return gatherlist;
        }
        #endregion

        #region 进出高峰
        /// <summary>
        /// 获取进出高峰数
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<Statistics_Gather> Search_InOutPeak(string ParkingID, DateTime StartTime, DateTime EndTime)
        {
            List<Statistics_Gather> _gatherlist = new List<Statistics_Gather>();
            string strSql = "select convert(nvarchar(13),gathertime,120) as KeyName,Entrance_Count,Exit_Count from statistics_gather where parkingid=@ParkingID ";
            if (StartTime > DateTime.MinValue)
            {
                strSql += " and gathertime>=@StartTime";
            }
            if (EndTime > DateTime.MinValue)
            {
                strSql += " and gathertime<=@EndTime";
            }
            strSql += " order by gathertime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", ParkingID);
                dboperator.AddParameter("StartTime", StartTime);
                dboperator.AddParameter("EndTime", EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        _gatherlist.Add(DataReaderToModel<Statistics_Gather>.ToModel(dr));
                    }
                }
            }

            if (_gatherlist != null && _gatherlist.Count > 0)
            {
                foreach (Statistics_Gather g in _gatherlist)
                {
                    int tempnum = int.Parse(g.KeyName.Substring(11, 2)) + 1;
                    if (tempnum < 10)
                    {
                        g.KeyName = "0" + tempnum;
                    }
                    else
                    {
                        g.KeyName = tempnum.ToString();
                    }
                }
            }
            if (_gatherlist.Count < 24)
            {
                for (int i = 1; i <= 24; i++)
                {
                    Statistics_Gather temp = new Statistics_Gather
                    {
                        KeyName = i.ToString(),
                        Entrance_Count = 0,
                        Exit_Count = 0
                    };
                    if (i < 10)
                    {
                        temp.KeyName = "0" + i;
                    }
                    if (_gatherlist.Select(u => u.KeyName).Contains(temp.KeyName))
                    {
                        continue;
                    }
                    _gatherlist.Add(temp);
                }
            }
            return _gatherlist;
        }
        #endregion

        #region 商家充值
        public int Search_SellerRechargeCount(InParams paras)
        {
            int _total = 0;
            string strSql = string.Format(@"select count(1) Count 
                                            from parkorder o 
                                            left join ParkSeller u on u.SellerID=o.tagid
                                            left join SysUser su on su.recordid=o.userid                                     
                                            where o.status=1 and ordertype=10");
            if (paras.SellerID != null && paras.SellerID != "-1")
            {
                strSql += " and u.SellerID =@SellerID";
            }

            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("SellerID", paras.SellerID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("AdminID", paras.AdminID);
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

        public List<ParkOrder> Search_SellerRecharges(InParams paras, int PageSize, int PageIndex)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY o.ordertime desc ) AS rownum,u.SellerName,o.OrderNo,
                                           o.Amount,o.OrderTime,o.OldMoney,o.NewMoney,o.Remark,su.UserName Operator 
                                            from parkorder o 
                                            left join ParkSeller u on u.SellerID=o.tagid
                                            left join SysUser su on su.recordid=o.userid                                     
                                            where o.status=1 and ordertype=10", PageSize * PageIndex);
            if (paras.SellerID != null && paras.SellerID != "-1")
            {
                strSql += " and u.SellerID =@SellerID";
            }

            if (paras.StartTime != null)
            {
                strSql += " and o.OrderTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and o.OrderTime<=@EndTime";
            }
            if (paras.AdminID != null && paras.AdminID != "-1")
            {
                strSql += " and o.userid=@AdminID";
            }
            strSql += string.Format(" order by o.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("SellerID", paras.SellerID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                dboperator.AddParameter("AdminID", paras.AdminID);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(dr));
                    }
                }
            }
            return orderlist;
        }

        public List<InOutNum> Search_InOutNum(DateTime starttime, DateTime startend)
        {
            List<InOutNum> listinoutnum = new List<InOutNum>();
            while (starttime <= startend)
            {
                InOutNum model = new InOutNum();
                model.EndTime = starttime.ToString("yyyy-MM-dd");
                string strsql = "select count(ID) as countnum from ParkEvent where   IOState=1 and CONVERT(VARCHAR(10),RecTime,120)=@RecTime";
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    string aa = starttime.ToShortDateString();
                    dboperator.ClearParameters();
                    dboperator.AddParameter("RecTime", starttime.ToShortDateString());
                    using (DbDataReader dr = dboperator.ExecuteReader(strsql))
                    {
                        if (dr.Read())
                        {
                            model.InCount = dr["countnum"].ToString();

                        }
                    }
                }

                strsql = "select count(ID) as countnum from ParkEvent where   IOState=2  and CONVERT(VARCHAR(10),RecTime,120)=@RecTime";
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    dboperator.ClearParameters();
                    dboperator.AddParameter("RecTime", starttime.ToShortDateString());

                    using (DbDataReader dr = dboperator.ExecuteReader(strsql))
                    {
                        if (dr.Read())
                        {
                            model.OutCount = dr["countnum"].ToString();

                        }
                    }
                }
                model.PJZ = ((int.Parse(model.InCount) + int.Parse(model.OutCount)) / 2).ToString();
                listinoutnum.Add(model);
                starttime = starttime.AddDays(1);
            }
            return listinoutnum;
        }

        public List<ParkEvent> Search_Event(DateTime timedate)
        {
            List<ParkEvent> list = new List<ParkEvent>();
            string strsql = "select * from ParkEvent where  CONVERT(VARCHAR(10),RecTime,120)=@RecTime";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                string aa = timedate.ToShortDateString();
                dboperator.ClearParameters();
                dboperator.AddParameter("RecTime", timedate.ToShortDateString());
                using (DbDataReader dr = dboperator.ExecuteReader(strsql))
                {
                    while (dr.Read())
                    {
                        list.Add(DataReaderToModel<ParkEvent>.ToModel(dr));
                    }
                }
            }
            return list;
        }
        #endregion

        public List<ParkVisitorReportModel> QueryParkVisitorReport(VisitorReportCondition paras, int PageSize, int PageIndex, out int recordTotalCount)
        {
            List<ParkVisitorReportModel> models = new List<ParkVisitorReportModel>();
            string strSql = string.Format(@"select bv.VName,pk.PKName,v.PlateNumber,t.AlreadyVisitorCount,v.OperatorID,
                                           v.BeginDate BeginTime,v.EndDate EndTime,v.VisitorSource,v.VisitorName,v.IsExamine,v.VisitorState,
                                             v.CreateTime,v.VisitorCount,v.VisitorMobilePhone,wx.NickName,u.UserName from ParkVisitor t
                                            left join VisitorInfo v on t.VisitorID=v.RecordID
                                            left join BaseVillage bv on t.VID=bv.VID
                                            left join BaseParkinfo pk on pk.PKID=t.PKID
                                            left join WX_Info wx on wx.AccountID=v.OperatorID
                                            left join SysUser u on u.RecordID=v.OperatorID                                     
                                            where 1=1");

            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                if (paras.ParkingIds != null && paras.ParkingIds.Count > 0)
                {
                    strSql += string.Format(" and pk.PKID in('{0}')", string.Join("','", paras.ParkingIds));
                }
                if (!string.IsNullOrWhiteSpace(paras.MoblieOrName))
                {
                    strSql += " and (v.VisitorMobilePhone like @MoblieOrName or v.VisitorName like @MoblieOrName)";
                    dboperator.AddParameter("MoblieOrName", "%" + paras.MoblieOrName + "%");
                }
                if (!string.IsNullOrWhiteSpace(paras.PlateNumber))
                {
                    strSql += " and v.PlateNumber like @PlateNumber";
                    dboperator.AddParameter("PlateNumber", "%" + paras.PlateNumber + "%");
                }
                if (paras.VisitorSource.HasValue)
                {
                    strSql += " and v.VisitorSource=@VisitorSource";
                    dboperator.AddParameter("VisitorSource", paras.VisitorSource.Value);
                }
                if (paras.VisitorState.HasValue)
                {
                    strSql += " and v.VisitorState=@VisitorState";
                    dboperator.AddParameter("VisitorState", paras.VisitorState.Value);
                }
                if (paras.BeginTime.HasValue)
                {
                    strSql += " and v.CreateTime>=@BeginTime";
                    dboperator.AddParameter("BeginTime", paras.BeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (paras.EndTime.HasValue)
                {
                    strSql += " and v.CreateTime<=@EndTime";
                    dboperator.AddParameter("EndTime", paras.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                recordTotalCount = 0;
                using (DbDataReader reader = dboperator.Paging(strSql, "CreateTime DESC", PageIndex, PageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkVisitorReportModel>.ToModel(reader));
                    }
                }
            }
            return models;
        }

        /// <summary>
        /// 按地址统计停车时长
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="isExit"></param>
        /// <returns></returns>
        public List<Statistics_SumTime> QueryParkSumTime(DateTime starttime, DateTime endtime, bool isExit)
        {
            List<Statistics_SumTime> list = new List<Statistics_SumTime>();
            string strSql = "select SUM( datediff(mi,a.EntranceTime,ISNULL(a.ExitTime,GETDATE()))) as SumMin,";
            strSql += "case when c.FamilyAddr is null then '外来未知' when c.FamilyAddr='' then '内部未知' else c.FamilyAddr end  as FamilyAddr ";
            strSql += " from ParkIORecord a left join BaseCard b on a.CardID=b.CardID ";
            strSql += " left join BaseEmployee c on b.EmployeeID=c.EmployeeID  ";
            strSql += " where a.EntranceTime>=@starttime and a.EntranceTime<=@endtime ";
            if (isExit)
            {
                strSql += " and a.IsExit=1 ";
            }
            strSql += " group by FamilyAddr";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        list.Add(DataReaderToModel<Statistics_SumTime>.ToModel(dr));
                    }
                }
            }
            return list;
        }
    }
}
