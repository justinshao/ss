using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkCarModelDAL : BaseDAL, IParkCarModel
    {
        public bool Add(ParkCarModel model)
        {
            if (model == null) return false;

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                UpdateDefault(model, dbOperator);
                return Add(new List<ParkCarModel>() { model }, dbOperator);
            }
        }

        public bool Add(List<ParkCarModel> models, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            if (models == null || models.Count == 0) return false;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ParkCarModel(CarModelID,CarModelName,PKID,IsDefault,LastUpdateTime,HaveUpdate,DataStatus,MaxUseMoney,IsNaturalDay,PlateColor,DayMaxMoney,NightMaxMoney,DayStartTime,DayEndTime,NightStartTime,NightEndTime,NaturalTime)");
            bool hasData = false;
            int index = 1;
            foreach (var p in models)
            {
                p.DataStatus = DataStatus.Normal;
                p.LastUpdateTime = DateTime.Now;
                p.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                strSql.AppendFormat(" SELECT @CarModelID{0},@CarModelName{0},@PKID{0},@IsDefault{0},@LastUpdateTime{0},@HaveUpdate{0},@DataStatus{0},@MaxUseMoney{0},@IsNaturalDay{0},@PlateColor{0},@DayMaxMoney{0},@NightMaxMoney{0},@DayStartTime{0},@DayEndTime{0},@NightStartTime{0},@NightEndTime{0},@NaturalTime{0} UNION ALL", index);
                dbOperator.AddParameter("CarModelID" + index, p.CarModelID);
                dbOperator.AddParameter("CarModelName" + index, p.CarModelName);
                dbOperator.AddParameter("PKID" + index, p.PKID);
                dbOperator.AddParameter("IsDefault" + index, (int)p.IsDefault);
                dbOperator.AddParameter("LastUpdateTime" + index, p.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate" + index, p.HaveUpdate);
                dbOperator.AddParameter("DataStatus" + index, (int)p.DataStatus);
                dbOperator.AddParameter("MaxUseMoney" + index, p.MaxUseMoney);
                dbOperator.AddParameter("IsNaturalDay" + index, p.IsNaturalDay);
                dbOperator.AddParameter("PlateColor" + index, p.PlateColor);
                dbOperator.AddParameter("DayMaxMoney" + index, p.DayMaxMoney);
                dbOperator.AddParameter("NightMaxMoney" + index, p.NightMaxMoney);
                dbOperator.AddParameter("DayStartTime" + index, p.DayStartTime);
                dbOperator.AddParameter("DayEndTime" + index, p.DayEndTime);
                dbOperator.AddParameter("NightStartTime" + index, p.NightStartTime);
                dbOperator.AddParameter("NightEndTime" + index, p.NightEndTime);
                dbOperator.AddParameter("NaturalTime" + index, p.NaturalTime);
                
                hasData = true;
                index++;
            }
            if (hasData)
            {
                return dbOperator.ExecuteNonQuery(strSql.Remove(strSql.Length - 10, 10).ToString()) > 0;
            }
            return false;
        }

        public bool Update(ParkCarModel model)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarModel set CarModelName=@CarModelName,IsDefault=@IsDefault,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,MaxUseMoney=@MaxUseMoney,IsNaturalDay=@IsNaturalDay,PlateColor=@PlateColor ");
            strSql.Append(",DayMaxMoney=@DayMaxMoney,NightMaxMoney=@NightMaxMoney,DayStartTime=@DayStartTime,DayEndTime=@DayEndTime,NightStartTime=@NightStartTime,NightEndTime=@NightEndTime,NaturalTime=@NaturalTime where  CarModelID=@CarModelID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                UpdateDefault(model, dbOperator);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarModelName", model.CarModelName);
                dbOperator.AddParameter("IsDefault", (int)model.IsDefault);
                dbOperator.AddParameter("CarModelID", model.CarModelID);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("MaxUseMoney", model.MaxUseMoney);
                dbOperator.AddParameter("IsNaturalDay", model.IsNaturalDay);
                dbOperator.AddParameter("PlateColor", model.PlateColor);

                dbOperator.AddParameter("DayMaxMoney", model.DayMaxMoney);
                dbOperator.AddParameter("NightMaxMoney", model.NightMaxMoney);
                dbOperator.AddParameter("DayStartTime", model.DayStartTime);
                dbOperator.AddParameter("DayEndTime", model.DayEndTime);
                dbOperator.AddParameter("NightStartTime", model.NightStartTime);
                dbOperator.AddParameter("NightEndTime", model.NightEndTime);
                dbOperator.AddParameter("NaturalTime", model.NaturalTime);
                
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        private void UpdateDefault(ParkCarModel model, DbOperator dbOperator)
        {
            if (model.IsDefault == YesOrNo.No) return;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarModel set IsDefault=@IsDefault,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,PlateColor=@PlateColor");
            strSql.Append(" where  PKID=@PKID and IsDefault=@IsDefault1 and CarModelID!=@CarModelID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("CarModelID", model.CarModelID);
            dbOperator.AddParameter("IsDefault", (int)YesOrNo.No);
            dbOperator.AddParameter("IsDefault1", (int)YesOrNo.Yes);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("PlateColor", model.PlateColor);


            dbOperator.ExecuteNonQuery(strSql.ToString());
        }
        public bool Delete(string recordId)
        {
            return CommonDelete("ParkCarModel", "CarModelID", recordId);
        }

        public ParkCarModel QueryByRecordId(string recordId)
        {
            string sql = string.Format("select * from ParkCarModel where CarModelID =@CarModelID and DataStatus!=@DataStatus;");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarModelID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetParkCarModel(reader).FirstOrDefault();
                }
            }
        }

        public List<ParkCarModel> QueryByParkingId(string parkingId)
        {
            string sql = string.Format("select * from ParkCarModel where PKID =@PKID and DataStatus!=@DataStatus;");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetParkCarModel(reader);
                }
            }
        }

        public List<ParkCarModel> QueryByParkingIds(List<string> parkingIds)
        {
            string sql = string.Format("select * from ParkCarModel where PKID in('{0}') and DataStatus!=@DataStatus;", string.Join("','", parkingIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetParkCarModel(reader);
                }
            }
        }
        private List<ParkCarModel> GetParkCarModel(DbDataReader reader)
        {
            List<ParkCarModel> models = new List<ParkCarModel>();
            while (reader.Read())
            {
               models.Add(DataReaderToModel<ParkCarModel>.ToModel(reader));
            }
            return models;
        }

        public ParkCarModel GetDefaultCarModel(string parkingid, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                string sql = string.Format("select * from ParkCarModel where PKID=@PKID and IsDefault=1 and DataStatus!=@DataStatus;");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", parkingid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        return GetParkCarModel(reader).FirstOrDefault();
                    }
                }
            }
            catch(Exception e)
            {
                errorMsg = e.Message;
            }
            return null;
        }


    }
}
