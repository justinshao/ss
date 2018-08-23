using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using Common.Utilities;
using Common.Core;
using System.Data.Common;
using Common.Entities.Statistics;

namespace Common.SqlRepository
{
    public class ParkChangeshiftrecordDAL : BaseDAL, IParkChangeshiftrecord
    {
        public ParkChangeshiftrecord AddChangeshiftrecord(ParkChangeshiftrecord model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
                    model.RecordID = GuidGenerator.GetGuidString();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ParkChangeshiftrecord(RecordID,UserID,StartWorkTime,EndWorkTime,BoxID,PKID,DataStatus,LastUpdateTime,HaveUpdate)");
                    strSql.Append(" values(@RecordID,@UserID,@StartWorkTime,@EndWorkTime,@BoxID,@PKID,@DataStatus,@LastUpdateTime,@HaveUpdate);");
                    strSql.Append(" select * from ParkChangeshiftrecord where RecordID=@RecordID;");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("UserID", model.UserID);
                    dbOperator.AddParameter("StartWorkTime", model.StartWorkTime);
                    if (model.EndWorkTime!=null)
                    {
                        dbOperator.AddParameter("EndWorkTime", model.EndWorkTime);
                    }
                    else
                    { 
                        dbOperator.AddParameter("EndWorkTime", DBNull.Value);
                    }
                    dbOperator.AddParameter("BoxID", model.BoxID);
                    dbOperator.AddParameter("PKID", model.PKID);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if(reader.Read())
                        { 
                            return DataReaderToModel<ParkChangeshiftrecord>.ToModel(reader);
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

        public ParkChangeshiftrecord GetUnChangeshiftrecord(string boxID, out string ErrorMessage)
        { 
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkChangeshiftrecord where BoxID=@BoxID and DataStatus!=@DataStatus and  EndWorkTime  IS NULL");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("BoxID", boxID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkChangeshiftrecord>.ToModel(reader);
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

        /// <summary>
        /// 获取所有在班信息
        /// </summary>
        /// <returns></returns>
        public List<ParkChangeshiftrecord> GetUnChangeshiftrecordALL()
        {
            try
            {
                 List<ParkChangeshiftrecord> parkchangeshiftrecordlist = new List<ParkChangeshiftrecord>();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select a.RecordID,a.StartWorkTime,BoxName,UserName from ParkChangeshiftrecord a left join ParkBox b on a.BoxID=b.BoxID ");
                strSql.Append("left join SysUser c on a.UserID=c.RecordID ");
                strSql.Append(" where  a.DataStatus!=2 and  EndWorkTime  IS NULL");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                           parkchangeshiftrecordlist.Add(DataReaderToModel<ParkChangeshiftrecord>.ToModel(reader));
                        }
                    }
                }
                return parkchangeshiftrecordlist;
            }
            catch (Exception e)
            {
               
            }
            return null;
        }

        public List<ParkChangeshiftrecord> GetChangeShiftRecord(string boxid)
        {
            List<ParkChangeshiftrecord> parkchangeshiftrecordlist = new List<ParkChangeshiftrecord>();
            string strSql = string.Format(@"select top 3 * from ParkChangeshiftrecord where boxid=@boxid and DataStatus!=2 order by startworktime desc");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("boxid", boxid);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        parkchangeshiftrecordlist.Add(DataReaderToModel<ParkChangeshiftrecord>.ToModel(reader));
                    }
                }
            }
            return parkchangeshiftrecordlist;
        }

        public bool ModifyChangeshiftrecord(ParkChangeshiftrecord model, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkChangeshiftrecord set BoxID=@BoxID,DataStatus=@DataStatus,EndWorkTime=@EndWorkTime,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,PKID=@PKID,StartWorkTime=@StartWorkTime,UserID=@UserID");
                    strSql.Append(" where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("BoxID", model.BoxID);
                    dbOperator.AddParameter("DataStatus", model.DataStatus);
                    dbOperator.AddParameter("EndWorkTime", model.EndWorkTime);
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("PKID", model.PKID);
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("StartWorkTime", model.StartWorkTime);
                    dbOperator.AddParameter("UserID", model.UserID); 
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch(Exception e)
            {
                errorMsg = e.Message;
            }
            return false;
        }

        public bool EditChangeshiftrecord(string RecordID, DateTime EndWorkTime, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                 
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkChangeshiftrecord set EndWorkTime=@EndWorkTime,HaveUpdate=3,LastUpdateTime=getdate()");
                    strSql.Append(" where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("EndWorkTime", EndWorkTime);
                    dbOperator.AddParameter("RecordID", RecordID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return false;
        }


        public List<ParkChangeshiftrecord> GetChangeShiftByUserID(string UserID, DateTime begin, DateTime end)
        {
            try
            {
                List<ParkChangeshiftrecord> parkchangeshiftrecordlist = new List<ParkChangeshiftrecord>();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select a.*,b.UserName from ParkChangeshiftrecord a left join SysUser b on a.UserID=b.RecordID ");
                strSql.Append("where a.DataStatus!=2  and  a.EndWorkTime IS not NULL and a.EndWorkTime>=@begin and a.EndWorkTime<=@end ");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    if (UserID != "-1")
                    {
                        strSql.Append(" and a.UserID=@UserID");
                    }
                    strSql.Append(" order by a.lastupdatetime desc");
                    dbOperator.AddParameter("begin", begin);
                    dbOperator.AddParameter("end", end);
                    if (UserID != "-1")
                    {
                        dbOperator.AddParameter("UserID", UserID);
                    }

                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            parkchangeshiftrecordlist.Add(DataReaderToModel<ParkChangeshiftrecord>.ToModel(reader));
                        }
                    }
                }
                return parkchangeshiftrecordlist;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public List<OnDutyFL> GetOnDutyByUserID(string UserID, DateTime begin, DateTime end)
        {
            try
            {
                List<OnDutyFL> parkchangeshiftrecordlist = new List<OnDutyFL>();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select sum(Amount) as YSJE,sum(PayAmount) as SSJE,sum(isnull(DiscountAmount,0)) as XFJM,CarModelName as CardType,count(b.RecordID) as CarNum ");
                strSql.Append("from ParkOrder a left join ParkIORecord b ");
                strSql.Append("on a.TagID=b.RecordID left join ParkCarModel c on b.CarModelID=c.CarModelID ");
                strSql.Append("where  a.UserID=@UserID and PayTime>=@begin and PayTime<=@end and a.Status=1 and b.CarModelID is not null and (CarDerateID ='' or CarDerateID is null)  group by c.CarModelID,c.CarModelName ");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("begin", begin);
                    dbOperator.AddParameter("end", end);
                    dbOperator.AddParameter("UserID", UserID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            parkchangeshiftrecordlist.Add( DataReaderToModel<OnDutyFL>.ToModel(reader));

                        }
                    }
                }

                strSql = new StringBuilder();
                strSql.Append("select sum(Amount) as YSJE,sum(PayAmount) as SSJE,sum(isnull(DiscountAmount,0)) as XFJM,count(b.RecordID) as CarNum ");
                strSql.Append("from ParkOrder a left join ParkIORecord b ");
                strSql.Append("on a.TagID=b.RecordID left join ParkCarModel c on b.CarModelID=c.CarModelID ");
                strSql.Append("where  a.UserID=@UserID and PayTime>=@begin and PayTime<=@end and a.Status=1  and (CarDerateID !='' and CarDerateID is not null)   ");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("begin", begin);
                    dbOperator.AddParameter("end", end);
                    dbOperator.AddParameter("UserID", UserID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            OnDutyFL model = DataReaderToModel<OnDutyFL>.ToModel(reader);
                            model.CardType = "接待车";
                            parkchangeshiftrecordlist.Add(model);

                        }
                    }
                }

                return parkchangeshiftrecordlist;
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
