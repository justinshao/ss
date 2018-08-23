using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkSellerDAL : BaseDAL, IParkSeller
    {
        public List<ParkCarDerate> GetCanUseCarderatesByPlatenumber(string Platenumber, out string errorMsg)
        {
            List<ParkCarDerate> parkCarDerates = new List<ParkCarDerate>();
            errorMsg = "";
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkCarDerate where Platenumber=@Platenumber and DataStatus!=@DataStatus and Status=1");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("Platenumber", Platenumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            parkCarDerates.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return parkCarDerates;
        }

        public List<ParkCarDerate> GetCanUseCarderatesByIORecordid(string iorecordid, out string errorMsg)
        {
            List<ParkCarDerate> parkCarDerates = new List<ParkCarDerate>();
            errorMsg = "";
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkCarDerate where IORecordID=@IORecordID and DataStatus!=@DataStatus and Status=1");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("IORecordID", iorecordid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            parkCarDerates.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return parkCarDerates;
        }

        public ParkDerate GetDerate(string derateid, out string errorMsg)
        { 
            errorMsg = "";
            try
            { 
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkDerate where DerateID=@DerateID and DataStatus!=@DataStatus");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("DerateID", derateid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkDerate>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return null;
        }

        public ParkSeller GetSeller(string sellerid, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkSeller where Sellerid=@Sellerid and DataStatus!=@DataStatus");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("Sellerid", sellerid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkSeller>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return null;
        }

        public bool ModifyCarderate(ParkCarDerate model, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update ParkCarDerate set CarDerateNo=@CarDerateNo,DerateID=@DerateID,FreeTime=@FreeTime,FreeMoney=@FreeMoney,CardNo=@CardNo,
                        IORecordID=@IORecordID, Status=@Status, CreateTime=@CreateTime, ExpiryTime=@ExpiryTime, PKID=@PKID,AreaID=@AreaID,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime");
                    strSql.Append(" where CarDerateID=@CarDerateID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("CarDerateNo", model.CarDerateNo);
                    dbOperator.AddParameter("CarDerateID", model.CarDerateID);
                    dbOperator.AddParameter("DerateID", model.DerateID);
                    dbOperator.AddParameter("FreeTime", model.FreeTime);
                    dbOperator.AddParameter("FreeMoney", model.FreeMoney);
                    dbOperator.AddParameter("CardNo", model.CardNo);
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("Status", (int)model.Status);
                    dbOperator.AddParameter("CreateTime", model.CreateTime);
                    dbOperator.AddParameter("ExpiryTime", model.ExpiryTime);
                    dbOperator.AddParameter("PKID", model.PKID);
                    dbOperator.AddParameter("AreaID", model.AreaID);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return false;
        }

        public bool AddCarderate(ParkCarDerate model)
        { 
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkCarDerate(AreaID,CarDerateID,CarDerateNo,CardNo,CreateTime,DerateID,ExpiryTime,FreeMoney,HaveUpdate,IORecordID,LastUpdateTime,PKID,PlateNumber,Status,FreeTime,DataStatus)");
                strSql.Append(" values(@AreaID,@CarDerateID,@CarDerateNo,@CardNo,@CreateTime,@DerateID,@ExpiryTime,@FreeMoney,@HaveUpdate,@IORecordID,@LastUpdateTime,@PKID,@PlateNumber,@Status,@FreeTime,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("CarDerateID", model.CarDerateID);
                dbOperator.AddParameter("CarDerateNo", model.CarDerateNo);
                dbOperator.AddParameter("CardNo", model.CardNo);
                dbOperator.AddParameter("CreateTime", model.CreateTime); 
                dbOperator.AddParameter("DerateID", model.DerateID);
                dbOperator.AddParameter("ExpiryTime", model.ExpiryTime);
                dbOperator.AddParameter("FreeMoney", model.FreeMoney);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("IORecordID", model.IORecordID);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime); 
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("Status", (int)model.Status); 
                dbOperator.AddParameter("FreeTime", model.FreeTime);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }  
        }
        public bool ModifySeller(ParkSeller model, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update ParkSeller set PPSellerID=@PPSellerID ,Addr=@Addr,Balance=@Balance,Creditline=@Creditline,HaveUpdate=@HaveUpdate, LastUpdateTime=@LastUpdateTime,SellerName=@SellerName,VID=@VID");
                    strSql.Append(" where SellerID=@SellerID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("Addr", model.Addr);
                    dbOperator.AddParameter("Balance", model.Balance);
                    dbOperator.AddParameter("Creditline", model.Creditline);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("SellerID", model.SellerID);
                    dbOperator.AddParameter("SellerName", model.SellerName);
                    dbOperator.AddParameter("VID", model.VID);
                    dbOperator.AddParameter("PPSellerID", model.PPSellerID); 
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return false;
        }


        public bool Add(ParkSeller model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkSeller(SellerID,SellerNo,SellerName,PWD,VID,Addr,Creditline,Balance,LastUpdateTime,HaveUpdate,DataStatus,PPSellerID)");
                strSql.Append(" values(@SellerID,@SellerNo,@SellerName,@PWD,@VID,@Addr,@Creditline,@Balance,@LastUpdateTime,@HaveUpdate,@DataStatus,@PPSellerID)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", model.SellerID);
                dbOperator.AddParameter("SellerNo", model.SellerNo);
                dbOperator.AddParameter("SellerName", model.SellerName);
                dbOperator.AddParameter("PWD", model.PWD);
                dbOperator.AddParameter("VID", model.VID);
                dbOperator.AddParameter("Addr", model.Addr);
                dbOperator.AddParameter("Creditline", model.Creditline);
                dbOperator.AddParameter("Balance", model.Balance);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("PPSellerID", model.PPSellerID);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdatePassword(string sellerId, string password)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkSeller set PWD=@PWD,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where SellerID=@SellerID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("PWD", password);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool SellerRecharge(string sellerId, decimal balance, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkSeller set Balance=Balance+@Balance,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where SellerID=@SellerID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("SellerID", sellerId);
            dbOperator.AddParameter("Balance", balance);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool SellerDebit(string sellerId, decimal balance, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkSeller set Balance=Balance-@Balance,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where SellerID=@SellerID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("SellerID", sellerId);
            dbOperator.AddParameter("Balance", balance);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Delete(string sellerId)
        {
            return CommonDelete("ParkSeller", "SellerID", sellerId);
        }

        public List<ParkSeller> QueryByVillageId(string villageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkSeller where VID=@VID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkSeller> models = new List<ParkSeller>();
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkSeller>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkSeller> QueryByVillageIds(List<string> villageIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkSeller where VID in('{0}') and DataStatus!=@DataStatus", string.Join("','", villageIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkSeller> models = new List<ParkSeller>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkSeller>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkSeller> QueryPage(string villageId, string sellerName, int pagesize, int pageindex, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkSeller where VID=@VID  and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(sellerName))
                {
                    strSql.Append(" and SellerName like @SellerName");
                    dbOperator.AddParameter("SellerName", "%" + sellerName + "%");
                }
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), "ID DESC", pageindex, pagesize, out total))
                {
                    List<ParkSeller> models = new List<ParkSeller>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkSeller>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public ParkSeller QueryBySellerId(string sellerId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkSeller where SellerID=@SellerID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkSeller> models = new List<ParkSeller>();
                    if(reader.Read())
                    {
                        return DataReaderToModel<ParkSeller>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public ParkSeller QueryBySellerNo(string sellerNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkSeller where SellerNo=@SellerNo and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerNo", sellerNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkSeller> models = new List<ParkSeller>();
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkSeller>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public bool UpdateParkCarDerateStatus(List<string> derateIds,int status, DbOperator dbOperator) {
        
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ParkCarDerate set  Status=@Status,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.AppendFormat(" where CarDerateID in('{0}')",string.Join("','",derateIds));

            dbOperator.ClearParameters();
            dbOperator.AddParameter("Status", status);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

    }
}
