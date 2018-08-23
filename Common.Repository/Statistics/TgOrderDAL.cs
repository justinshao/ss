using Common.DataAccess;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
using Common.Utilities;
using Common.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Common.SqlRepository.Statistics
{
    public class TgOrderDAL : ITgOrder
    {
        public bool addTgOrder(TgOrder modle)
        {
            using (DbOperator operators = ConnectionManager.CreateConnection())
            {
                return Add(modle, operators);
            }
        }

        public bool Add(TgOrder modle, DbOperator operators)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into TgOrder(OrderID,PKID,PKName,PlateNo,Amount,RealPayTime,PersonId,PersonName)");
            sb.AppendFormat("values(@OrderID,@PKID,@PKName,@PlateNo,@Amount,@RealPayTime,@PersonId,@PersonName)");
            operators.ClearParameters();
            operators.AddParameter("OrderID", modle.OrderID);
            operators.AddParameter("PKID", modle.PKID);
            operators.AddParameter("PKName", modle.PKName);
            operators.AddParameter("PlateNo", modle.PlateNo);
            operators.AddParameter("Amount", modle.Amount);
            operators.AddParameter("RealPayTime", modle.RealPayTime);
            operators.AddParameter("PersonId", modle.PersonId);
            operators.AddParameter("PersonName", modle.PersonName);
            return operators.ExecuteNonQuery(sb.ToString()) > 0;
        }
         
        public List<TgOrder> CountTgPersonOrder(InParams paras, int PageSize, int PageIndex)
        {
            List<TgOrder> Personlist = new List<TgOrder>();
            string strSql = "select top " + PageSize + " * from ( select top " + PageIndex * PageSize + " PersonId,PersonName,PKID,PKName,sum(Amount) as Amount,COUNT(OrderID) as count from TgOrder where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and PKID=@PKID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and RealPayTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and RealPayTime<@EndTime";
            }
            strSql += " group by PersonId,PersonName,PKID,PKName order by PersonId desc )a order by PersonId";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PKID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        TgOrder tg = new TgOrder();
                        tg.PersonId = int.Parse(dr["PersonId"].ToString());
                        tg.PersonName = dr["PersonName"].ToString();
                        tg.PKID = dr["PKID"].ToString();
                        tg.PKName = dr["PKName"].ToString();
                        tg.Amount =decimal.Parse( dr["Amount"].ToString());
                        tg.Count = int.Parse(dr["count"].ToString());

                        Personlist.Add(tg);
                    }
                }
            }
            return Personlist; 
        }


        public int CountTgPersonOrderCount(InParams paras)
        {
            int _total = 0;
            string strSql = " select count(1) Count from (select PersonId,PKID from TgOrder where 1=1  ";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and PKID=@PKID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and RealPayTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and RealPayTime<@EndTime";
            }
            strSql += " group by PersonId,PKID) a ";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PKID", paras.ParkingID);
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

        public List<TgOrder> QueryAllCountTgPersonOrder(InParams paras)
        {
            List<TgOrder> Personlist = new List<TgOrder>();
            string strSql = "  select PersonId,PersonName,PKID,PKName,sum(Amount) as Amount,COUNT(OrderID) as count from TgOrder where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and PKID=@PKID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and RealPayTime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and RealPayTime<@EndTime";
            }
            strSql += " group by PersonId,PersonName,PKID,PKName order by PersonId ";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("PKID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        TgOrder tg = new TgOrder();
                        tg.PersonId = int.Parse(dr["PersonId"].ToString());
                        tg.PersonName = dr["PersonName"].ToString();
                        tg.PKID = dr["PKID"].ToString();
                        tg.PKName = dr["PKName"].ToString();
                        tg.Amount = decimal.Parse(dr["Amount"].ToString());
                        tg.Count = int.Parse(dr["count"].ToString());

                        Personlist.Add(tg);
                    }
                }
            }
            return Personlist;
        }
    }
}
