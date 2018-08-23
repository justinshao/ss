using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;

using Common.Core;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class ParkingDAL : BaseDAL, IParking
    {
        public bool Add(BaseParkinfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(BaseParkinfo model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BaseParkinfo(PKID,PKNo,PKName,CarBitNum,CarBitNumLeft,CarBitNumFixed,SpaceBitNum");
            strSql.Append(",CenterTime,AllowLoseDisplay,LinkMan,Mobile,Address,Coordinate,MobilePay");
            strSql.Append(",MobileLock,IsParkingSpace,IsReverseSeekingVehicle,FeeRemark,OnLine,Remark");
            strSql.Append(",LastUpdateTime,HaveUpdate,CityID,VID,DataStatus,DataSaveDays,PictureSaveDays,IsOnLineGathe,IsLine,NeedFee,ExpiredAdvanceRemindDay,DefaultPlate,PoliceFree,UnconfirmedCalculation,IsNoPlateConfirm,SupportAutoRefund,OuterringCharge,SupportNoSense)");
            strSql.Append(" values(@PKID,@PKNo,@PKName,@CarBitNum,@CarBitNumLeft,@CarBitNumFixed,@SpaceBitNum");
            strSql.Append(",@CenterTime,@AllowLoseDisplay,@LinkMan,@Mobile,@Address,@Coordinate,@MobilePay");
            strSql.Append(",@MobileLock,@IsParkingSpace,@IsReverseSeekingVehicle,@FeeRemark,@OnLine,@Remark");
            strSql.Append(",@LastUpdateTime,@HaveUpdate,@CityID,@VID,@DataStatus,@DataSaveDays,@PictureSaveDays");
            strSql.Append(",@IsOnLineGathe,@IsLine,@NeedFee,@ExpiredAdvanceRemindDay,@DefaultPlate,@PoliceFree,@UnconfirmedCalculation,@IsNoPlateConfirm,@SupportAutoRefund,@OuterringCharge,@SupportNoSense)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("PKNo", model.PKNo);
            dbOperator.AddParameter("PKName", model.PKName);
            dbOperator.AddParameter("CarBitNum", model.CarBitNum);
            dbOperator.AddParameter("CarBitNumLeft", model.CarBitNumLeft);
            dbOperator.AddParameter("CarBitNumFixed", model.CarBitNumFixed);
            dbOperator.AddParameter("SpaceBitNum", model.SpaceBitNum);
            dbOperator.AddParameter("CenterTime", model.CenterTime);
            dbOperator.AddParameter("AllowLoseDisplay", (int)model.AllowLoseDisplay);
            dbOperator.AddParameter("LinkMan", model.LinkMan);
            dbOperator.AddParameter("Mobile", model.Mobile);
            dbOperator.AddParameter("Address", model.Address);
            dbOperator.AddParameter("Coordinate", model.Coordinate);
            dbOperator.AddParameter("MobilePay", (int)model.MobilePay);
            dbOperator.AddParameter("MobileLock", (int)model.MobileLock);
            dbOperator.AddParameter("IsParkingSpace", (int)model.IsParkingSpace);
            dbOperator.AddParameter("IsReverseSeekingVehicle", (int)model.IsReverseSeekingVehicle);
            dbOperator.AddParameter("FeeRemark", model.FeeRemark);
            dbOperator.AddParameter("OnLine", (int)model.OnLine);
            dbOperator.AddParameter("Remark", model.Remark);
            dbOperator.AddParameter("CityID", model.CityID);
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("DataSaveDays", model.DataSaveDays);
            dbOperator.AddParameter("PictureSaveDays", model.PictureSaveDays);
            dbOperator.AddParameter("IsOnLineGathe", (int)model.IsOnLineGathe);
            dbOperator.AddParameter("IsLine", (int)model.IsLine);
            dbOperator.AddParameter("NeedFee", (int)model.NeedFee);
            dbOperator.AddParameter("ExpiredAdvanceRemindDay",model.ExpiredAdvanceRemindDay);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("DefaultPlate", model.DefaultPlate);
            dbOperator.AddParameter("PoliceFree", model.PoliceFree);
            dbOperator.AddParameter("UnconfirmedCalculation", model.UnconfirmedCalculation);
            dbOperator.AddParameter("IsNoPlateConfirm", model.IsNoPlateConfirm);
            dbOperator.AddParameter("SupportAutoRefund", model.SupportAutoRefund);
            dbOperator.AddParameter("OuterringCharge", model.OuterringCharge);
            dbOperator.AddParameter("SupportNoSense", model.SupportNoSense);
            
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
       
        public bool Update(BaseParkinfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BaseParkinfo set PKNo=@PKNo,PKName=@PKName");
                strSql.Append(",CenterTime=@CenterTime,AllowLoseDisplay=@AllowLoseDisplay,LinkMan=@LinkMan,Mobile=@Mobile,Address=@Address,Coordinate=@Coordinate,MobilePay=@MobilePay");
                strSql.Append(",MobileLock=@MobileLock,IsParkingSpace=@IsParkingSpace,IsReverseSeekingVehicle=@IsReverseSeekingVehicle,FeeRemark=@FeeRemark,OnLine=@OnLine,Remark=@Remark");
                strSql.Append(",LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,CityID=@CityID,VID=@VID,DataSaveDays=@DataSaveDays,PictureSaveDays=@PictureSaveDays,PoliceFree=@PoliceFree");
                strSql.Append(",IsOnLineGathe=@IsOnLineGathe,IsLine=@IsLine,NeedFee=@NeedFee,ExpiredAdvanceRemindDay=@ExpiredAdvanceRemindDay,DefaultPlate=@DefaultPlate,SpaceBitNum=@SpaceBitNum");
                strSql.Append(",CarBitNumFixed=@CarBitNumFixed,CarBitNumLeft=@CarBitNumLeft,OnlineDiscount=@OnlineDiscount,IsOnlineDiscount=@IsOnlineDiscount,UnconfirmedCalculation=@UnconfirmedCalculation,IsNoPlateConfirm=@IsNoPlateConfirm,SupportAutoRefund=@SupportAutoRefund,OuterringCharge=@OuterringCharge,SupportNoSense=@SupportNoSense where  PKID=@PKID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PKNo", model.PKNo);
                dbOperator.AddParameter("PKName", model.PKName);
                dbOperator.AddParameter("CenterTime", model.CenterTime);
                dbOperator.AddParameter("AllowLoseDisplay", (int)model.AllowLoseDisplay);
                dbOperator.AddParameter("LinkMan", model.LinkMan);
                dbOperator.AddParameter("Mobile", model.Mobile);
                dbOperator.AddParameter("Address", model.Address);
                dbOperator.AddParameter("Coordinate", model.Coordinate);
                dbOperator.AddParameter("MobilePay", (int)model.MobilePay);
                dbOperator.AddParameter("MobileLock", (int)model.MobileLock);
                dbOperator.AddParameter("IsParkingSpace", (int)model.IsParkingSpace);
                dbOperator.AddParameter("IsReverseSeekingVehicle", (int)model.IsReverseSeekingVehicle);
                dbOperator.AddParameter("FeeRemark", model.FeeRemark);
                dbOperator.AddParameter("OnLine", (int)model.OnLine);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("CityID", model.CityID);
                dbOperator.AddParameter("VID", model.VID);
                dbOperator.AddParameter("DataSaveDays", model.DataSaveDays);
                dbOperator.AddParameter("PictureSaveDays", model.PictureSaveDays);
                dbOperator.AddParameter("IsOnLineGathe", (int)model.IsOnLineGathe);
                dbOperator.AddParameter("IsLine", (int)model.IsLine);
                dbOperator.AddParameter("NeedFee", (int)model.NeedFee);
                dbOperator.AddParameter("ExpiredAdvanceRemindDay", model.ExpiredAdvanceRemindDay);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DefaultPlate", model.DefaultPlate);
                dbOperator.AddParameter("SpaceBitNum", model.SpaceBitNum);
                dbOperator.AddParameter("CarBitNumFixed", model.CarBitNumFixed);
                dbOperator.AddParameter("CarBitNumLeft", model.CarBitNumLeft);
                dbOperator.AddParameter("PoliceFree", model.PoliceFree);
                dbOperator.AddParameter("OnlineDiscount", model.OnlineDiscount);
                dbOperator.AddParameter("IsOnlineDiscount", model.IsOnlineDiscount);
                dbOperator.AddParameter("UnconfirmedCalculation", model.UnconfirmedCalculation);
                dbOperator.AddParameter("IsNoPlateConfirm", model.IsNoPlateConfirm);
                dbOperator.AddParameter("SupportAutoRefund", model.SupportAutoRefund);
                dbOperator.AddParameter("OuterringCharge", model.OuterringCharge);
                dbOperator.AddParameter("SupportNoSense", model.SupportNoSense);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("BaseParkinfo", "PKID", recordId);
        }

        public int UpdateCarBit(string PKID)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    int CarBit = 0;
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select * from ParkArea where PKID=@PKID and DataStatus!=2");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkArea model = DataReaderToModel<ParkArea>.ToModel(reader);
                            CarBit += model.CarbitNum;
                        }
                    }

                    strSql = new StringBuilder();
                    strSql.Append("update BaseParkinfo set CarBitNum=@CarBitNum,LastUpdateTime=getdate(),HaveUpdate=3  where PKID=@PKID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    dbOperator.AddParameter("CarBitNum", CarBit);
                    if (dbOperator.ExecuteNonQuery(strSql.ToString()) > 0)
                    {
                        return CarBit;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }

        public BaseParkinfo QueryParkingByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BaseParkinfo where PKID=@PKID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseParkinfo(reader).FirstOrDefault();
                }
            }
        }
        public List<BaseParkinfo> QueryAllParking()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BaseParkinfo where DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<BaseParkinfo> models = new List<BaseParkinfo>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseParkinfo>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        private List<BaseParkinfo> GetBaseParkinfo(DbDataReader reader)
        {
            List<BaseParkinfo> models = new List<BaseParkinfo>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<BaseParkinfo>.ToModel(reader)); 
            }
            return models;
        }
        public List<BaseParkinfo> QueryParkingByRecordIds(List<string> recordIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from BaseParkinfo where DataStatus!=@DataStatus and PKID in('{0}')", string.Join("','", recordIds));
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseParkinfo(reader);
                }
            }
        }

        public List<BaseParkinfo> QueryParkingByVillageId(string villageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BaseParkinfo where DataStatus!=@DataStatus and VID =@VID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseParkinfo(reader);
                }
            }
        }
        public List<BaseParkinfo> QueryParkingByCompanyIds(List<string> companyIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p.* from BaseParkinfo p inner join BaseVillage v on p.VID=v.VID");
            strSql.Append("  inner join BaseCompany c on v.CPID=c.CPID ");
            strSql.Append(" AND p.DataStatus!=@DataStatus AND v.DataStatus!=@DataStatus AND c.DataStatus!=@DataStatus");
            strSql.AppendFormat("  and c.CPID in('{0}')",string.Join("','",companyIds));

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseParkinfo(reader);
                }
            }
        }
        public List<BaseParkinfo> QueryParkingByVillageIds(List<string> villageIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from BaseParkinfo where DataStatus!=@DataStatus and VID in('{0}')", string.Join("','", villageIds));
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseParkinfo(reader);
                }
            }
        }
        /// <summary>
        /// 取得所有车场
        /// </summary>
        /// <returns></returns>
        public List<BaseParkinfo> QueryParkingAll()
        {
            List<BaseParkinfo> baseparkinfolist = new List<BaseParkinfo>();
            string strSql = "select * from BaseParkinfo where DataStatus!=2";
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        baseparkinfolist.Add(DataReaderToModel<BaseParkinfo>.ToModel(reader));
                    }
                }
            }
            return baseparkinfolist;
        }
        public BaseParkinfo QueryParkingByParkingID(string ParkingID)
        {
            BaseParkinfo baseparkinfo = null;
            string strSql = string.Format("select * from BaseParkinfo where pkid='{0}' and DataStatus!=2", ParkingID);
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        baseparkinfo = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                    }
                }
            }
            return baseparkinfo;
        }
        public BaseParkinfo QueryParkingByParkingNo(string parkingNo)
        {
            BaseParkinfo baseparkinfo = null;
            string strSql = "select * from BaseParkinfo where PKNo=@PKNo and  DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKNo", parkingNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        baseparkinfo = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                    }
                }
            }
            return baseparkinfo;
        }
        public List<BaseParkinfo> QueryPage(string villageId, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BaseParkinfo where DataStatus!=@DataStatus ");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(villageId)) {
                    strSql.Append("and VID=@VID ");
                    dbOperator.AddParameter("VID", villageId);
                }
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                string sequence = " id desc";
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), sequence, pageIndex, pageSize, out totalCount))
                {
                    return GetBaseParkinfo(reader);
                }
            }
        }


        public DateTime GetServerTime(out string error)
        {
            DateTime dateTime = DateTime.Now;
            error = "";
            try
            {
                string strSql = "select getdate() as DateTime";
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                    {
                        if (reader.Read())
                        {
                            dateTime = reader["DateTime"].ToDateTime();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                error = e.StackTrace; 
            }
            return dateTime;
        }
        public List<BaseParkinfo> GetParkingBySupportAutoRefund()
        {
            List<BaseParkinfo> models = new List<BaseParkinfo>();

            string sql = "select * from BaseParkinfo where DataStatus!=@DataStatus and SupportAutoRefund=1 ";
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseParkinfo>.ToModel(reader));
                    }
                }
            }

            return models;
        }
        public bool UpdateParkSettleConfig(BaseParkinfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BaseParkinfo set HandlingFee=@HandlingFee,MaxAmountOfCash=@MaxAmountOfCash,MinAmountOfCash=@MinAmountOfCash,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where  PKID=@PKID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("HandlingFee", model.HandlingFee);
                dbOperator.AddParameter("MaxAmountOfCash", model.MaxAmountOfCash);
                dbOperator.AddParameter("MinAmountOfCash", model.MinAmountOfCash);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}
