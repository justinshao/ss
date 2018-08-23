using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkCarTypeDAL : BaseDAL, IParkCarType
    {

        public bool Add(ParkCarType model)
        {
            if (model == null) return false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(new List<ParkCarType>() { model }, dbOperator);
            }
        }

        public bool Add(List<ParkCarType> models, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            if (models == null || models.Count == 0) return false;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ParkCarType(CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ)");
            bool hasData = false;
            int index = 1;
            foreach (var p in models)
            {
                p.DataStatus = DataStatus.Normal;
                p.LastUpdateTime = DateTime.Now;
                p.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                strSql.AppendFormat(" SELECT @CarTypeID{0},@CarTypeName{0},@PKID{0},@BaseTypeID{0},@RepeatIn{0},@RepeatOut{0}", index);
                strSql.AppendFormat(",@AffirmIn{0},@AffirmOut{0},@InBeginTime{0},@InEdnTime{0},@MaxUseMoney{0},@AllowLose{0},@LpDistinguish{0},@InOutEditCar{0}",index);
                strSql.AppendFormat(",@InOutTime{0},@CarNoLike{0},@LastUpdateTime{0},@HaveUpdate{0},@IsAllowOnlIne{0},@Amount{0},@MaxMonth{0},@MaxValue{0},@DataStatus{0}",index);
                strSql.AppendFormat(",@OverdueToTemp{0},@LotOccupy{0},@Deposit{0},@MonthCardExpiredEnterDay{0},@AffirmBegin{0},@AffirmEnd{0},@IsNeedCapturePaper{0},@IsNeedAuthentication{0},@IsDispatch{0},@OnlineUnit{0},@IsIgnoreHZ{0}  UNION ALL", index);

                dbOperator.AddParameter("CarTypeID" + index, p.CarTypeID);
                dbOperator.AddParameter("CarTypeName" + index, p.CarTypeName);
                dbOperator.AddParameter("PKID" + index, p.PKID);
                dbOperator.AddParameter("BaseTypeID" + index, (int)p.BaseTypeID);
                dbOperator.AddParameter("RepeatIn" + index, (int)p.RepeatIn);
                dbOperator.AddParameter("RepeatOut" + index, (int)p.RepeatOut);
                dbOperator.AddParameter("AffirmIn" + index, (int)p.AffirmIn);
                dbOperator.AddParameter("AffirmOut" + index, (int)p.AffirmOut);
                dbOperator.AddParameter("InBeginTime" + index, p.InBeginTime);
                dbOperator.AddParameter("InEdnTime" + index, p.InEdnTime);
                dbOperator.AddParameter("MaxUseMoney" + index, p.MaxUseMoney);
                dbOperator.AddParameter("AllowLose" + index, (int)p.AllowLose);
                dbOperator.AddParameter("LpDistinguish" + index, (int)p.LpDistinguish);
                dbOperator.AddParameter("InOutEditCar" + index, (int)p.InOutEditCar);
                dbOperator.AddParameter("InOutTime" + index, p.InOutTime);
                dbOperator.AddParameter("CarNoLike" + index, (int)p.CarNoLike);
                dbOperator.AddParameter("LastUpdateTime" + index, p.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate" + index, p.HaveUpdate);
                dbOperator.AddParameter("IsAllowOnlIne" + index, (int)p.IsAllowOnlIne);

                //dbOperator.AddParameter("Amount" + index, p.Amount);
                //dbOperator.AddParameter("MaxMonth" + index, p.MaxMonth);

                if ((int)p.BaseTypeID == 5)
                {
                    dbOperator.AddParameter("Amount" + index, p.SeasonAmount/3);
                    dbOperator.AddParameter("MaxMonth" + index, p.MaxSeason * 3);
                }
                else if ((int)p.BaseTypeID == 6)
                {
                    dbOperator.AddParameter("Amount" + index, p.YearAmount/12);
                    dbOperator.AddParameter("MaxMonth" + index, p.MaxYear * 12);
                }
                else
                {
                    dbOperator.AddParameter("Amount" + index, p.Amount);
                    dbOperator.AddParameter("MaxMonth" + index, p.MaxMonth);
                }



                dbOperator.AddParameter("MaxValue" + index, p.MaxValue);
                dbOperator.AddParameter("DataStatus" + index, (int)p.DataStatus);
                dbOperator.AddParameter("OverdueToTemp" + index, (int)p.OverdueToTemp);
                dbOperator.AddParameter("LotOccupy" + index, (int)p.LotOccupy);
                dbOperator.AddParameter("Deposit" + index, p.Deposit);
                dbOperator.AddParameter("MonthCardExpiredEnterDay" + index, p.MonthCardExpiredEnterDay);
                dbOperator.AddParameter("AffirmBegin" + index, p.AffirmBegin);
                dbOperator.AddParameter("AffirmEnd" + index, p.AffirmEnd);
                dbOperator.AddParameter("IsNeedCapturePaper" + index, p.IsNeedCapturePaper);
                dbOperator.AddParameter("IsNeedAuthentication" + index, p.IsNeedAuthentication);
                dbOperator.AddParameter("IsDispatch" + index, p.IsDispatch);
                dbOperator.AddParameter("OnlineUnit" + index, p.OnlineUnit);
                dbOperator.AddParameter("IsIgnoreHZ" + index, p.IsIgnoreHZ);

                hasData = true;
                index++;
            }
            if (hasData)
            {
                return dbOperator.ExecuteNonQuery(strSql.Remove(strSql.Length - 10, 10).ToString()) > 0;
            }
            return false;
        }
        public bool Update(ParkCarType model)
        {

            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkCarType set CarTypeName=@CarTypeName,PKID=@PKID,BaseTypeID=@BaseTypeID,RepeatIn=@RepeatIn,RepeatOut");
            strSql.Append("=@RepeatOut,AffirmIn=@AffirmIn,AffirmOut=@AffirmOut,InBeginTime=@InBeginTime,InEdnTime=@InEdnTime,MaxUseMoney=@MaxUseMoney,AllowLose=@AllowLose,LpDistinguish=@LpDistinguish,InOutEditCar");
            strSql.Append("=@InOutEditCar,InOutTime=@InOutTime,CarNoLike=@CarNoLike,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsAllowOnlIne=@IsAllowOnlIne,Amount=@Amount,MaxMonth=@MaxMonth,MaxValue=@MaxValue");
            strSql.Append(",OverdueToTemp=@OverdueToTemp,LotOccupy=@LotOccupy,Deposit=@Deposit,MonthCardExpiredEnterDay=@MonthCardExpiredEnterDay,AffirmBegin=@AffirmBegin,AffirmEnd=@AffirmEnd,IsNeedCapturePaper=@IsNeedCapturePaper,IsNeedAuthentication=@IsNeedAuthentication,IsDispatch=@IsDispatch,OnlineUnit=@OnlineUnit");
            strSql.Append(",IsIgnoreHZ=@IsIgnoreHZ where CarTypeID=@CarTypeID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                dbOperator.AddParameter("CarTypeName", model.CarTypeName);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("BaseTypeID", (int)model.BaseTypeID);
                dbOperator.AddParameter("RepeatIn", (int)model.RepeatIn);
                dbOperator.AddParameter("RepeatOut", (int)model.RepeatOut);
                dbOperator.AddParameter("AffirmIn", (int)model.AffirmIn);
                dbOperator.AddParameter("AffirmOut", (int)model.AffirmOut);
                dbOperator.AddParameter("InBeginTime", model.InBeginTime);
                dbOperator.AddParameter("InEdnTime", model.InEdnTime);
                dbOperator.AddParameter("MaxUseMoney", model.MaxUseMoney);
                dbOperator.AddParameter("AllowLose", (int)model.AllowLose);
                dbOperator.AddParameter("LpDistinguish", (int)model.LpDistinguish);
                dbOperator.AddParameter("InOutEditCar", (int)model.InOutEditCar);
                dbOperator.AddParameter("InOutTime", model.InOutTime);
                dbOperator.AddParameter("CarNoLike", (int)model.CarNoLike);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("IsAllowOnlIne", (int)model.IsAllowOnlIne);

                if ((int)model.BaseTypeID == 5) { 
                    dbOperator.AddParameter("Amount", model.SeasonAmount/3);
                    dbOperator.AddParameter("MaxMonth", model.MaxSeason * 3);
                }
                else if ((int)model.BaseTypeID == 6) { 
                    dbOperator.AddParameter("Amount", model.YearAmount/12);
                    dbOperator.AddParameter("MaxMonth", model.MaxYear *12);
                }
                else{ 
                    dbOperator.AddParameter("Amount", model.Amount);
                    dbOperator.AddParameter("MaxMonth", model.MaxMonth);
                }

                
                dbOperator.AddParameter("MaxValue", model.MaxValue);
                dbOperator.AddParameter("OverdueToTemp", (int)model.OverdueToTemp);
                dbOperator.AddParameter("LotOccupy", (int)model.LotOccupy);
                dbOperator.AddParameter("Deposit", model.Deposit);
                dbOperator.AddParameter("MonthCardExpiredEnterDay", model.MonthCardExpiredEnterDay);
                dbOperator.AddParameter("AffirmBegin", model.AffirmBegin);
                dbOperator.AddParameter("AffirmEnd", model.AffirmEnd);
                dbOperator.AddParameter("IsNeedCapturePaper", model.IsNeedCapturePaper);
                dbOperator.AddParameter("IsNeedAuthentication", model.IsNeedAuthentication);
                dbOperator.AddParameter("IsDispatch", model.IsDispatch);
                dbOperator.AddParameter("OnlineUnit", model.OnlineUnit);
                dbOperator.AddParameter("IsIgnoreHZ", model.IsIgnoreHZ);
                int i= dbOperator.ExecuteNonQuery(strSql.ToString()) ;
                return i> 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("ParkCarType", "CarTypeID", recordId);
        }

        public bool QueryGrant(string recordId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string str = "select * from ParkGrant where CarTypeID=@CarTypeID and DataStatus!=2";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeID", recordId);
                using (DbDataReader reader = dbOperator.ExecuteReader(str.ToString()))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public ParkCarType QueryParkCarTypeByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.Append(" WHERE CarTypeID=@CarTypeID AND DataStatus!=@DataStatus");
             using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkCarType>.ToModel(reader);
                    }
                    return null;
                   
                }
            }
        }
        public List<ParkCarType> QueryParkCarTypeByRecordIds(List<string> recordIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.AppendFormat(" WHERE CarTypeID in('{0}') AND DataStatus!=@DataStatus",string.Join("','",recordIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkCarType> lists = new List<ParkCarType>();
                    while (reader.Read())
                    {
                        lists.Add( DataReaderToModel<ParkCarType>.ToModel(reader));
                    }
                    return lists;
                   
                }
            }
        }
        public List<ParkCarType> QueryParkCarTypeByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.Append(" WHERE PKID=@PKID AND DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkCarType> lists = new List<ParkCarType>();
                    while (reader.Read())
                    {
                        lists.Add(DataReaderToModel<ParkCarType>.ToModel(reader));
                    }
                    return lists;
                   
                }
            }
          
        }

        public List<ParkCarType> QueryParkCarTypeByParkingIds(List<string> parkingIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.AppendFormat(" WHERE PKID IN('{0}') AND DataStatus!=@DataStatus", string.Join("','", parkingIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkCarType> lists = new List<ParkCarType>();
                    while (reader.Read())
                    {
                        lists.Add(DataReaderToModel<ParkCarType>.ToModel(reader));
                    }
                    return lists;
                    
                }
            }
        }

        public List<ParkCarType> QueryCardTypesByBaseCardType(string parkingId, BaseCarType baseCarType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.Append(" WHERE PKID =@PKID AND DataStatus!=@DataStatus and BaseTypeID=@BaseTypeID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BaseTypeID", (int)baseCarType);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("PKID", parkingId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkCarType> lists = new List<ParkCarType>();
                    while (reader.Read())
                    {
                        lists.Add(DataReaderToModel<ParkCarType>.ToModel(reader));
                    }
                    return lists;
                    
                }
            }
        }
        public ParkCarType QueryCarTypesByCarTypeName(string parkingId,string carTypeName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut");
            strSql.Append(",AffirmIn,AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar");
            strSql.Append(",InOutTime,CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus");
            strSql.Append(",OverdueToTemp,LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsNeedAuthentication,IsDispatch,OnlineUnit,IsIgnoreHZ FROM ParkCarType");
            strSql.Append(" WHERE PKID =@PKID AND DataStatus!=@DataStatus and CarTypeName=@CarTypeName");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CarTypeName", carTypeName);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("PKID", parkingId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkCarType>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        private List<ParkCarType> GetParkCarModel(DbDataReader reader)
        {
            List<ParkCarType> models = new List<ParkCarType>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<ParkCarType>.ToModel(reader)); 
            }
            return models;
        }
       
    }
}
