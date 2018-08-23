using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Common.DataAccess;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
using Common.Entities;
using Common.Utilities;
using Common.Utilities.Helpers;


namespace Common.SqlRepository.Statistics
{
    public class tgPersonDAL : ItgPerson
    {
        public bool Add(tgPerson modle) {
            using (DbOperator operators = ConnectionManager.CreateConnection()) {
                return Add(modle, operators);
            }
        }

        public bool Add(tgPerson modle, DbOperator operators)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into PersonTg(name,phone,bz,count,PKID,PKName)");
            sb.AppendFormat("values(@name,@phone,@bz,'0',@PKID,@PKName)");
            operators.ClearParameters();
            operators.AddParameter("name", modle.name);
            operators.AddParameter("phone", modle.phone);
            operators.AddParameter("bz", modle.bz);
            operators.AddParameter("PKID", modle.PKID);
            operators.AddParameter("PKName", modle.PKName);
            return operators.ExecuteNonQuery(sb.ToString()) > 0;
        }

        public bool Update(tgPerson modle) {
            using (DbOperator operators = ConnectionManager.CreateConnection()) {
                return Update(modle, operators);
            }
        }

        public bool Update(tgPerson modle, DbOperator operators) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update PersonTg set name=@name,phone=@phone,bz=@bz,PKID=@PKID,PKName=@PKName ");
            sb.AppendFormat("where id=@id");
            operators.ClearParameters();
            operators.AddParameter("name", modle.name);
            operators.AddParameter("phone", modle.phone);
            operators.AddParameter("bz", modle.bz);
            operators.AddParameter("id", modle.id);
            operators.AddParameter("PKID", modle.PKID);
            operators.AddParameter("PKName", modle.PKName);
            return operators.ExecuteNonQuery(sb.ToString()) > 0;
        }



        public string SearchTg()
        {
            try
            {
                List<tgPerson> Personlist = new List<tgPerson>();
                int counts = 30;
                int rows = 30;
                string strSql = "select * from PersonTg";
                 using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    dboperator.ClearParameters();
                    using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                    {
                        while (dr.Read())
                        {
                            Personlist.Add(DataReaderToModel<tgPerson>.ToModel(dr));
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + counts + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(Personlist) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();

            }
            catch
            {
                return string.Empty;
            }
        }
        public tgPerson QueryPersonByID(int id)
        {
            tgPerson person = new tgPerson();
            string strSql = "select * from PersonTg  where id=@id"; 
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("id", id); 
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        person=DataReaderToModel<tgPerson>.ToModel(dr);
                    }
                }
            }
            return person;
        }
        /// <summary>
        /// 推广人员数量
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public int Search_tgPersonStatisticsCount(InParams paras)
        {
            int _total = 0;
            string strSql = "select count(1) Count from PersonTg  where 1=1";
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
        /// 推广人员数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<tgPerson> Search_tgPersonStatistics(InParams paras)
        {
            List<tgPerson> statisticstgPersonlist = new List<tgPerson>();
            string strSql = "select * from PersonTg  where 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql += " and parkingid=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql += " and gathertime>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql += " and gathertime<@EndTime";
            } 
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
                        statisticstgPersonlist.Add(DataReaderToModel<tgPerson>.ToModel(dr));
                    }
                }
            }
            return statisticstgPersonlist;
        }
        /// <summary>
        /// 推广人员数据
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public List<tgPerson> Search_tgPersonStatistics(InParams paras, int PageSize, int PageIndex)
        {
            List<tgPerson> statisticstgPersonlist = new List<tgPerson>();
            string strSql = "select top " + PageSize + " temp.* from ( select top " + PageIndex * PageSize + " * from PersonTg where 1=1";
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
                        statisticstgPersonlist.Add(DataReaderToModel<tgPerson>.ToModel(dr));
                    }
                }
            }
            return statisticstgPersonlist;
        }





        public string Querypersontg()
        {
            try
            {
                List<tgPerson> Personlist = new List<tgPerson>();
                int counts = 30;
                int rows = 30;
                string strSql = "select * from PersonTg";
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    dboperator.ClearParameters();
                    using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                    {
                        while (dr.Read())
                        {
                            Personlist.Add(DataReaderToModel<tgPerson>.ToModel(dr));
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + counts + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(Personlist) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();

            }
            catch
            {
                return string.Empty;
            }
        }
        public bool Delete(string TableName, string FildName, string value) {
            using (DbOperator operators = ConnectionManager.CreateConnection()) {
                return Delete(TableName, FildName, value, operators);
            }
        }

        public bool Delete(string TableName, string fild, string value, DbOperator operators) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("delete {0} where {1}={2}", TableName, fild, value);
            return operators.ExecuteNonQuery(sb.ToString()) > 0;
        }

       
    }
}
