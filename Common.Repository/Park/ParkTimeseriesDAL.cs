using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities;
using Common.DataAccess;
using Common.Utilities;
using System.Data.Common;
using Common.Core;
using Common.Core.Expands;

namespace Common.SqlRepository
{
    public class ParkTimeseriesDAL : BaseDAL, IParkTimeseries
    {

        public ParkTimeseries AddTimeseries(ParkTimeseries model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
                    model.TimeseriesID = GuidGenerator.GetGuidString();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ParkTimeseries(TimeseriesID,IORecordID,EnterImage,ExitImage,ParkingID,EnterTime,ExitTime,ExitGateID,EnterGateID,IsExit,LastUpdateTime,HaveUpdate,DataStatus,ReleaseType)");
                    strSql.Append(" values(@TimeseriesID,@IORecordID,@EnterImage,@ExitImage,@ParkingID,@EnterTime,@ExitTime,@ExitGateID,@EnterGateID,@IsExit,@LastUpdateTime,@HaveUpdate,@DataStatus,@ReleaseType);");
                    strSql.Append(" select * from ParkTimeseries where TimeseriesID=@TimeseriesID ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("TimeseriesID", model.TimeseriesID);
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("EnterImage", model.EnterImage);
                    dbOperator.AddParameter("ExitImage", model.ExitImage);
                    dbOperator.AddParameter("ParkingID", model.ParkingID);
                    dbOperator.AddParameter("EnterTime", model.EnterTime);
                    dbOperator.AddParameter("ExitTime", model.ExitTime);
                    dbOperator.AddParameter("ExitGateID", model.ExitGateID);
                    dbOperator.AddParameter("EnterGateID", model.EnterGateID);
                    dbOperator.AddParameter("IsExit", model.IsExit);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("ReleaseType", model.ReleaseType);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        { 
                            return DataReaderToModel<ParkTimeseries>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                ErrorMessage = e.Message;
            }
            return null;
        }

        public List<ParkTimeseries> GetAllExitsTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage)
        {
            List<ParkTimeseries> ParkTimeserieses = new List<ParkTimeseries>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkTimeseries where ParkingID=@ParkingID and  IORecordID=@IORecordID and IsExit=1 and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkid);
                    dbOperator.AddParameter("IORecordID", iorecordID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkTimeserieses.Add(DataReaderToModel<ParkTimeseries>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkTimeserieses;
        }

        public int GetAreaCarNum(string areaID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  COUNT(*) from ParkTimeseries t LEFT JOIN parkiorecord i on t.IORecordID=i.RecordID 
                                        LEFT JOIN parkgate g on t.EnterGateID = g.GateID LEFT JOIN parkbox b on b.BoxID = g.BoxID where 
                 b.AreaID =@AreaID and t.IsExit = 0 and t.DataStatus != @DataStatus ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AreaID", areaID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if(reader.Read())
                        { 
                            return reader[0].ToString().ToInt();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return 0;
        }

        public int GetAreaCarNum(string areaID, BaseCarType carType, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  COUNT(*) from ParkTimeseries t LEFT JOIN parkiorecord i on t.IORecordID=i.RecordID left JOIN parkcartype c on i.CarTypeID=c.CarTypeID 
                                        LEFT JOIN parkgate g on t.EnterGateID = g.GateID LEFT JOIN parkbox b on b.BoxID = g.BoxID where c.BaseTypeID =@BaseTypeID
                and b.AreaID =@AreaID and t.IsExit = 0 and t.DataStatus != @DataStatus ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("BaseTypeID", (int)carType);
                    dbOperator.AddParameter("AreaID", areaID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if(reader.Read())
                        {
                            return reader[0].ToString().ToInt();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return 0;
        }

        public int GetIsEditCarNum(string areaID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  COUNT(*) from ParkTimeseries t LEFT JOIN parkiorecord i on t.IORecordID=i.RecordID left JOIN parkcartype c on i.CarTypeID=c.CarTypeID 
                                        LEFT JOIN parkgate g on t.EnterGateID = g.GateID LEFT JOIN parkbox b on b.BoxID = g.BoxID where c.InOutEditCar =1
                and b.AreaID =@AreaID and t.IsExit = 0 and t.DataStatus != @DataStatus ");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                  
                    dbOperator.AddParameter("AreaID", areaID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader[0].ToString().ToInt();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return 0;
        }
        public DateTime GetLastRecordEnterTime(string parkingID, string iorecordid, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select top 1 EnterTime from ParkTimeseries where IORecordID=@IORecordID and ParkingID=@ParkingID  and DataStatus!=@DataStatus order by EnterTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("IORecordID", iorecordid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if(reader.Read())
                        {
                            return reader[0].ToString().ToDateTime(); 
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return DateTime.MinValue;
        }

        public DateTime GetLastRecordExitDate(string parkingID, string iorecordid, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select top 1 ExitTime from ParkTimeseries where IORecordID=@IORecordID and ParkingID=@ParkingID  and DataStatus!=@DataStatus order by ExitTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("IORecordID", iorecordid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        return reader[0].ToString().ToDateTime();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return DateTime.MinValue;
        }
         
        public ParkTimeseries GetTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkTimeseries where ParkingID=@ParkingID and IORecordID=@IORecordID and IsExit=0 and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkid);
                    dbOperator.AddParameter("IORecordID", iorecordID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        { 
                            return DataReaderToModel<ParkTimeseries>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }
         
        public bool ModifyTimeseries(ParkTimeseries model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update ParkTimeseries set IORecordID=@IORecordID,EnterGateID=@EnterGateID,EnterImage=@EnterImage,EnterTime=@EnterTime,ExitGateID=@ExitGateID,
                        ExitImage=@ExitImage,ExitTime=@ExitTime,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,IsExit=@IsExit,
                        ParkingID=@ParkingID,ReleaseType=@ReleaseType");
                    strSql.Append(" where TimeseriesID=@TimeseriesID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("EnterGateID", model.EnterGateID);
                    dbOperator.AddParameter("EnterImage", model.EnterImage);
                    dbOperator.AddParameter("EnterTime", model.EnterTime);
                    dbOperator.AddParameter("ExitGateID", model.ExitGateID);
                    dbOperator.AddParameter("ExitImage", model.ExitImage);
                    dbOperator.AddParameter("ExitTime", model.ExitTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("IsExit", model.IsExit);
                    dbOperator.AddParameter("ParkingID", model.ParkingID);
                    dbOperator.AddParameter("ReleaseType", model.ReleaseType);
                    dbOperator.AddParameter("TimeseriesID", model.TimeseriesID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public bool RemoveTimeseries(string timeseriesid)
        {
            return CommonDelete("ParkTimeseries", "TimeseriesID", timeseriesid);
        }

        public List<ParkTimeseries> GetTimeseriesIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> carTypeIDs, string carModelID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1)
        {
            string strWhere = string.Format(" t.ParkingID='{0}' and t.DataStatus!=2 ", parkingID);

            if (isExit == 0 || isExit == 1)
            {
                strWhere = strWhere + " and t.Isexit=" + isExit;
            }
            if (!likePlateNumber.IsEmpty())
            {
                strWhere = string.Format("{0} and i.PlateNumber like '%{1}%'", strWhere, likePlateNumber);
            }
            if (!carModelID.IsEmpty())
            {
                strWhere = strWhere + " and i.CarModelID='" + carModelID + "'";
            }
            if (carTypeIDs != null && carTypeIDs.Count > 0)
            {
                string cardids = "";
                foreach (var item in carTypeIDs)
                {
                    if (item.IsEmpty())
                    {
                        continue;
                    }
                    cardids += item + ",";
                }
                if (!cardids.IsEmpty())
                {
                    cardids = cardids.Substring(0, cardids.Length - 1);
                    strWhere = strWhere + " and i.CarTypeID in(" + cardids + ")";
                }
            }
            if (!ingateid.IsEmpty())
            {
                strWhere = strWhere + " and t.EnterGateID=" + ingateid;
            }
            if (!outgateid.IsEmpty())
            {
                strWhere = strWhere + " and t.ExitGateID='" + outgateid + "'";
            }
            if (stayDay >= 0)
            {
                strWhere = string.Format("{0} and DateDiff(D,t.Entertime,'{1}')={2}", strWhere, DateTime.Now, stayDay);
            }

            if (isExit == 0)
            {
                strWhere = strWhere + " and t.Entertime>='" + startTime + "'";
                strWhere = strWhere + " and t.Entertime<='" + endTime + "'";
            }
            else if (isExit == 1)
            {
                strWhere = strWhere + " and t.ExitTime>='" + startTime + "'";
                strWhere = strWhere + " and t.ExitTime<='" + endTime + "'";
            }
            else
            {
                strWhere = strWhere + " and t.Entertime>='" + startTime + "'";
                strWhere = strWhere + " and t.Entertime<='" + endTime + "'";
            }

            ErrorMessage = "";
            if (pageIndex < 1)
            {
                ErrorMessage = "pageIndex 从1开始";
            }
            pageCount = 0;
            List<ParkTimeseries> modes = new List<ParkTimeseries>();
            try
            {
                string countSql = "";
                string sql = "";
                if (strWhere.IsEmpty())
                {
                    strWhere = " 1=1 ";
                }
                countSql = string.Format(@"Select count(t.ID) Count From ParkTimeseries t  left join ParkIORecord i on t.IORecordID=i.RecordID WHERE {0}  ", strWhere);
                sql = string.Format(@"Select top {1} * From ParkTimeseries t left join ParkIORecord i on t.IORecordID=i.RecordID Where t.ID>=( 
                                    Select Max(ID) From (Select top {0} t.ID From ParkTimeseries t left join ParkIORecord i on t.IORecordID=i.RecordID WHERE {2} Order By t.ID ) As tmp ) AND {2} Order by t.Entertime desc; ", (pageIndex - 1) * pageSize + 1, pageSize, strWhere);


                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    using (DbDataReader reader = dbOperator.ExecuteReader(countSql))
                    {
                        if (reader.Read())
                        {
                            pageCount = reader.GetInt32(0); 
                        }
                    } 
                }

                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {  
                        while (reader.Read())
                        {  
                            var timeseries = DataReaderToModel<ParkTimeseries>.ToModel(reader);
                            var iorecord = DataReaderToModel<ParkIORecord>.ToModel(reader); 
                            timeseries.IORecord = iorecord;
                            modes.Add(timeseries); 
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                ErrorMessage = "服务异常!" + e.Message; 
            }
            return modes;
        }
        public ParkTimeseries GetTimeseries(string timeseriesID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            { 
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkTimeseries where TimeseriesID=@TimeseriesID and DataStatus!=@DataStatus");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("GateID", timeseriesID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkTimeseries>.ToModel(reader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public List<ParkTimeseries> GetTimeseriesIORecordByLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likeStr, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
             
            string strWhere = string.Format(" t.ParkingID='{0}' and t.DataStatus!=2 ", parkingID);

            if (isExit == 0 || isExit == 1)
            {
                strWhere = strWhere + " and t.Isexit=" + isExit;
            }
            if (!likeStr.IsEmpty())
            {
                strWhere = string.Format("{0} and {1}", strWhere, likeStr);
            }
             
            if (cardTypeIDs != null && cardTypeIDs.Count > 0)
            {
                string cardids = "";
                foreach (var item in cardTypeIDs)
                {
                    if (item.IsEmpty())
                    {
                        continue;
                    }
                    cardids += "'"+item + "',";
                }
                if (!cardids.IsEmpty())
                {
                    cardids = cardids.Substring(0, cardids.Length - 1);
                    strWhere = strWhere + " and i.CarTypeID in(" + cardids + ")";
                }
            }    

            ErrorMessage = "";
            if (pageIndex < 1)
            {
                ErrorMessage = "pageIndex 从1开始";
            }
            pageCount = 0;
            List<ParkTimeseries> modes = new List<ParkTimeseries>();
            try
            {

                string tWhere = " 1=1 ";
                if (strWhere.IsEmpty())
                {
                    strWhere = tWhere;
                }
                string countSql = string.Format(@"Select count(t.ID) Count From ParkTimeseries t  left join ParkIORecord i on t.IORecordID=i.RecordID  WHERE {0}  ", strWhere);

                string sql = string.Format(@"Select top {1} * From ParkTimeseries t left join ParkIORecord i on t.IORecordID=i.RecordID  Where t.ID>=( 
                                    Select Max(ID) From (Select top {0} t.ID From ParkTimeseries t left join ParkIORecord i on t.IORecordID=i.RecordID  WHERE {2} Order By t.ID) As tmp ) and {2}; ", (pageIndex - 1) * pageSize + 1, pageSize, strWhere);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    using (DbDataReader reader = dbOperator.ExecuteReader(countSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            pageCount = reader[0].ToInt();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        ParkTimeseries timeseries = null;
                        ParkIORecord iorecord = null;
                        while (reader.Read())
                        {
                            timeseries = new ParkTimeseries();
                            iorecord = new ParkIORecord();

                            timeseries = DataReaderToModel<ParkTimeseries>.ToModel(reader);
                            iorecord = DataReaderToModel<ParkIORecord>.ToModel(reader); 
                            timeseries.IORecord = iorecord;
                            modes.Add(timeseries);
                        } 
                    }
                }  
            }
            catch (Exception e)
            {
                ErrorMessage = "服务异常!" + e.Message; 
            }
            return modes;
        }
    }
}
