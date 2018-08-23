using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkCarTypeSingleDAL : BaseDAL,IParkCarTypeSingle
    {
        public bool Add(ParkCarTypeSingle model)
        {
            if (model == null) return false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(new List<ParkCarTypeSingle>() { model }, dbOperator);
            }
        }

        public bool Add(List<ParkCarTypeSingle> models, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            if (models == null || models.Count == 0) return false;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ParkCarTypeSingle(SingleID,CarTypeID,Week,SingleType,DataStatus,LastUpdateTime,HaveUpdate)");
         
            bool hasData = false;
            int index = 1;
            foreach (var p in models)
            {
                p.DataStatus = DataStatus.Normal;
                p.LastUpdateTime = DateTime.Now;
                p.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                strSql.AppendFormat(" SELECT @SingleID{0},@CarTypeID{0},@Week{0},@SingleType{0},@DataStatus{0},@LastUpdateTime{0},@HaveUpdate{0} UNION ALL", index);

                dbOperator.AddParameter("SingleID" + index, p.SingleID);
                dbOperator.AddParameter("CarTypeID" + index, p.CarTypeID);
                dbOperator.AddParameter("Week" + index, (int)p.Week);
                dbOperator.AddParameter("SingleType" + index, (int)p.SingleType);
                dbOperator.AddParameter("DataStatus" + index, (int)p.DataStatus);
                dbOperator.AddParameter("LastUpdateTime" + index, p.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate" + index, (int)p.HaveUpdate);
                hasData = true;
                index++;
            }
            if (hasData)
            {
                return dbOperator.ExecuteNonQuery(strSql.Remove(strSql.Length - 10, 10).ToString()) > 0;
            }
            return false;
        }

        public bool Update(ParkCarTypeSingle model)
        {

            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarTypeSingle set CarTypeID=@CarTypeID,Week=@Week,SingleType=@SingleType,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where SingleID=@SingleID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                dbOperator.AddParameter("Week", (int)model.Week);
                dbOperator.AddParameter("SingleType", (int)model.SingleType);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", (int)model.HaveUpdate);
                dbOperator.AddParameter("SingleID", model.SingleID);

                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public List<ParkCarTypeSingle> QueryParkCarTypeByCarTypeID(string CarTypeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarTypeSingle where CarTypeID=@CarTypeID and DataStatus!=@DataStatus");
           
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeID", CarTypeID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkCarTypeSingle> lists = new List<ParkCarTypeSingle>();
                    while (reader.Read())
                    {
                        lists.Add(DataReaderToModel<ParkCarTypeSingle>.ToModel(reader));
                    }
                    return lists;

                }
            }

        }
    }
}
