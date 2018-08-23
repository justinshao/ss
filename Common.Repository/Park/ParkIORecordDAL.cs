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
using Common.Core.Expands;
using Common.Entities.Statistics;

namespace Common.SqlRepository
{
    public class ParkIORecordDAL : BaseDAL, IParkIORecord
    {

        public ParkIORecord AddIORecord(ParkIORecord model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = (int)DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
                    model.RecordID = GuidGenerator.GetGuidString();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ParkIORecord(RecordID,CardID,PlateNumber,CardNo,CardNumb,EntranceTime,EntranceImage,EntranceGateID,EntranceOperatorID,ExitTime,ExitImage,ExitGateID,ExitOperatorID,CarTypeID,CarModelID,IsExit,AreaID,ParkingID,ReleaseType,EnterType,DataStatus,MHOut,LastUpdateTime,HaveUpdate,Remark,EntranceCertificateNo,ExitCertificateNo,EntranceCertificateImage,ExitcertificateImage,EntranceIDCardPhoto,ExitIDCardPhoto,IsScanCodeIn,IsScanCodeOut,ResFeature,LogName,EnterDistinguish,ExitDistinguish,OfflineID,IsOffline,ReserveBitID,EntranceCertName,EntranceNation,EntranceSex,EntranceBirthDate,EntranceAddress,NetWeight,Tare,Goods,Shipper,Shippingspace,DocumentsNo,VisitorID)");
                    strSql.Append(" values(@RecordID,@CardID,@PlateNumber,@CardNo,@CardNumb,@EntranceTime,@EntranceImage,@EntranceGateID,@EntranceOperatorID,@ExitTime,@ExitImage,@ExitGateID,@ExitOperatorID,@CarTypeID,@CarModelID,@IsExit,@AreaID,@ParkingID,@ReleaseType,@EnterType,@DataStatus,@MHOut,@LastUpdateTime,@HaveUpdate,@Remark,@EntranceCertificateNo,@ExitCertificateNo,@EntranceCertificateImage,@ExitcertificateImage,@EntranceIDCardPhoto,@ExitIDCardPhoto,@IsScanCodeIn,@IsScanCodeOut,@ResFeature,@LogName,@EnterDistinguish,@ExitDistinguish,@OfflineID,@IsOffline,@ReserveBitID,@EntranceCertName,@EntranceNation,@EntranceSex,@EntranceBirthDate,@EntranceAddress,@NetWeight,@Tare,@Goods,@Shipper,@Shippingspace,@DocumentsNo,@VisitorID);");
                    strSql.Append(" select * from ParkIORecord where RecordID=@RecordID ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("CardID", model.CardID);
                    dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                    dbOperator.AddParameter("CardNo", model.CardNo);
                    dbOperator.AddParameter("CardNumb", model.CardNumb);
                    if (model.EntranceTime != DateTime.MinValue)
                    {
                        dbOperator.AddParameter("EntranceTime", model.EntranceTime);
                    }
                    else
                    {
                        dbOperator.AddParameter("EntranceTime", DBNull.Value);
                    }
                    dbOperator.AddParameter("EntranceImage", model.EntranceImage);
                    dbOperator.AddParameter("EntranceGateID", model.EntranceGateID);
                    dbOperator.AddParameter("EntranceOperatorID", model.EntranceOperatorID);
                    if (model.ExitTime != DateTime.MinValue)
                    {
                        dbOperator.AddParameter("ExitTime", model.ExitTime);
                    }
                    else
                    {
                        dbOperator.AddParameter("ExitTime", DBNull.Value);
                    }
                    dbOperator.AddParameter("ExitImage", model.ExitImage);
                    dbOperator.AddParameter("ExitGateID", model.ExitGateID);
                    dbOperator.AddParameter("ExitOperatorID", model.ExitOperatorID);
                    dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                    dbOperator.AddParameter("CarModelID", model.CarModelID);
                    dbOperator.AddParameter("IsExit", model.IsExit);
                    dbOperator.AddParameter("AreaID", model.AreaID);
                    dbOperator.AddParameter("ParkingID", model.ParkingID);
                    dbOperator.AddParameter("ReleaseType", model.ReleaseType);
                    dbOperator.AddParameter("EnterType", model.EnterType);
                    dbOperator.AddParameter("DataStatus", model.DataStatus);
                    dbOperator.AddParameter("MHOut", model.MHOut);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("Remark", model.Remark);
                    dbOperator.AddParameter("EntranceCertificateNo", model.EntranceCertificateNo);
                    dbOperator.AddParameter("ExitCertificateNo", model.ExitCertificateNo);
                    dbOperator.AddParameter("EntranceCertificateImage", model.EntranceCertificateImage);
                    dbOperator.AddParameter("ExitcertificateImage", model.ExitcertificateImage);
                    dbOperator.AddParameter("EntranceIDCardPhoto", model.EntranceIDCardPhoto);
                    dbOperator.AddParameter("ExitIDCardPhoto", model.ExitIDCardPhoto);
                    dbOperator.AddParameter("IsScanCodeIn", model.IsScanCodeIn);
                    dbOperator.AddParameter("IsScanCodeOut", model.IsScanCodeOut);
                    dbOperator.AddParameter("OfflineID", model.OfflineID);
                    dbOperator.AddParameter("IsOffline", model.IsOffline);
                    dbOperator.AddParameter("ReserveBitID", model.ReserveBitID);
                    if (model.ResFeature == null)
                    {
                        dbOperator.AddParameter("ResFeature", new byte[] { });
                    }
                    else
                    {
                        dbOperator.AddParameter("ResFeature", model.ResFeature);
                    }
                    dbOperator.AddParameter("LogName", model.LogName);
                    dbOperator.AddParameter("EnterDistinguish", model.EnterDistinguish);
                    dbOperator.AddParameter("ExitDistinguish", model.ExitDistinguish);

                    dbOperator.AddParameter("EntranceCertName", model.EntranceCertName);
                    dbOperator.AddParameter("EntranceNation", model.EntranceNation);
                    dbOperator.AddParameter("EntranceSex", model.EntranceSex);
                    dbOperator.AddParameter("EntranceBirthDate", model.EntranceBirthDate);
                    dbOperator.AddParameter("EntranceAddress", model.EntranceAddress);
                    dbOperator.AddParameter("NetWeight", model.NetWeight);
                    dbOperator.AddParameter("Tare", model.Tare);
                    dbOperator.AddParameter("Goods", model.Goods);
                    dbOperator.AddParameter("Shipper", model.Shipper);
                    dbOperator.AddParameter("Shippingspace", model.Shippingspace);
                    dbOperator.AddParameter("DocumentsNo", model.DocumentsNo);
                    dbOperator.AddParameter("VisitorID", model.VisitorID);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
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

        public bool ModifyIORecord(ParkIORecord model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = (int)DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update ParkIORecord set CardID=@CardID,PlateNumber=@PlateNumber,CardNo=@CardNo,CardNumb=@CardNumb,EntranceTime=@EntranceTime,EntranceImage=@EntranceImage,EntranceGateID=@EntranceGateID,EntranceOperatorID=@EntranceOperatorID,ExitTime=@ExitTime,ExitImage=@ExitImage,ExitGateID=@ExitGateID,ExitOperatorID=@ExitOperatorID,CarTypeID=@CarTypeID,CarModelID=@CarModelID,IsExit=@IsExit,
                                            AreaID=@AreaID,ParkingID=@ParkingID,ReleaseType=@ReleaseType,EnterType=@EnterType,DataStatus=@DataStatus,MHOut=@MHOut,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,Remark=@Remark,EntranceCertificateNo=@EntranceCertificateNo,ExitCertificateNo=@ExitCertificateNo,EntranceCertificateImage=@EntranceCertificateImage,ExitcertificateImage=@ExitcertificateImage,EntranceIDCardPhoto=@EntranceIDCardPhoto,ExitIDCardPhoto=@ExitIDCardPhoto,IsScanCodeIn=@IsScanCodeIn,IsScanCodeOut=@IsScanCodeOut,EnterDistinguish=@EnterDistinguish,
                                            ExitDistinguish=@ExitDistinguish,IsOffline=@IsOffline,OfflineID=@OfflineID,ExitCertName=@ExitCertName,ExitNation=@ExitNation,ExitSex=@ExitSex,ExitBirthDate=@ExitBirthDate,ExitAddress=@ExitAddress");
                    strSql.Append(" where RecordID=@RecordID");

                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("CardID", model.CardID);
                    dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                    dbOperator.AddParameter("CardNo", model.CardNo);
                    dbOperator.AddParameter("CardNumb", model.CardNumb);
                    if (model.EntranceTime == DateTime.MinValue)
                    {
                        dbOperator.AddParameter("EntranceTime", DBNull.Value);
                    }
                    else
                    {
                        dbOperator.AddParameter("EntranceTime", model.EntranceTime);
                    }
                    dbOperator.AddParameter("EntranceImage", model.EntranceImage);
                    dbOperator.AddParameter("EntranceGateID", model.EntranceGateID);
                    dbOperator.AddParameter("EntranceOperatorID", model.EntranceOperatorID);
                    if (model.ExitTime == DateTime.MinValue)
                    {
                        dbOperator.AddParameter("ExitTime", DBNull.Value);
                    }
                    else
                    {
                        dbOperator.AddParameter("ExitTime", model.ExitTime);
                    }
                    dbOperator.AddParameter("ExitImage", model.ExitImage);
                    dbOperator.AddParameter("ExitGateID", model.ExitGateID);
                    dbOperator.AddParameter("ExitOperatorID", model.ExitOperatorID);
                    dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                    dbOperator.AddParameter("CarModelID", model.CarModelID);
                    dbOperator.AddParameter("IsExit", model.IsExit);
                    dbOperator.AddParameter("AreaID", model.AreaID);
                    dbOperator.AddParameter("ParkingID", model.ParkingID);
                    dbOperator.AddParameter("ReleaseType", model.ReleaseType);
                    dbOperator.AddParameter("EnterType", model.EnterType);
                    dbOperator.AddParameter("DataStatus", model.DataStatus);
                    dbOperator.AddParameter("MHOut", model.MHOut);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("Remark", model.Remark);
                    dbOperator.AddParameter("EntranceCertificateNo", model.EntranceCertificateNo);
                    dbOperator.AddParameter("ExitCertificateNo", model.ExitCertificateNo);
                    dbOperator.AddParameter("EntranceCertificateImage", model.EntranceCertificateImage);
                    dbOperator.AddParameter("ExitcertificateImage", model.ExitcertificateImage);
                    dbOperator.AddParameter("EntranceIDCardPhoto", model.EntranceIDCardPhoto);
                    dbOperator.AddParameter("ExitIDCardPhoto", model.ExitIDCardPhoto);
                    dbOperator.AddParameter("IsScanCodeIn", model.IsScanCodeIn);
                    dbOperator.AddParameter("IsScanCodeOut", model.IsScanCodeOut);
                    dbOperator.AddParameter("EnterDistinguish", model.EnterDistinguish);
                    dbOperator.AddParameter("ExitDistinguish", model.ExitDistinguish);
                    dbOperator.AddParameter("IsOffline", model.IsOffline);
                    dbOperator.AddParameter("OfflineID", model.OfflineID);


                    dbOperator.AddParameter("ExitCertName", model.ExitCertName);
                    dbOperator.AddParameter("ExitNation", model.ExitNation);
                    dbOperator.AddParameter("ExitSex", model.ExitSex);
                    dbOperator.AddParameter("ExitBirthDate", model.ExitBirthDate);
                    dbOperator.AddParameter("ExitAddress", model.ExitAddress);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public ParkIORecord GetNoExitIORecordByCardNo(string parkid, string cardNo, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 * from ParkIORecord where ParkingID=@ParkingID and CardNo=@CardNo and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkid);
                    dbOperator.AddParameter("CardNo", cardNo);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public ParkIORecord GetNoExitIORecordByPlateNumber(string parkid, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 * from ParkIORecord where ParkingID=@ParkingID and PlateNumber=@PlateNumber and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkid);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public bool RemoveRepeatInIORecordByPlateNumber(string parkingID, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update  ParkIORecord set DataStatus=@DataStatus,HaveUpdate=@HaveUpdate where  ParkingID=@ParkingID and PlateNumber=@PlateNumber and IsExit = 0 and DataStatus!=@DataStatus;");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("plateNumber", plateNumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public bool RemoveRepeatInIORecordByCardNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update  ParkIORecord set DataStatus=@DataStatus where  ParkingID=@ParkingID and CardNo=@CardNo and IsExit = 0 and DataStatus!=@DataStatus;");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("CardNo", cardNo);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public int GetAreaCarNum(string areaID, Entities.BaseCarType cartype, out string ErrorMessage)
        {
            ErrorMessage = "";
            string sql = "";
            int count = 0;
            try
            {
                sql = string.Format(@"SELECT count(*) FROM parkiorecord I LEFT JOIN parkcartype  C ON I.CarTypeID=C.CarTypeID  
                                      WHERE C.BaseTypeID=@BaseTypeID and I.AreaID=@AreaID and I.IsExit=0 and I.DataStatus!=@DataStatus and c.InOutEditCar =1 
                                      AND I.RecordID not in (select IORecordID from parktimeseries WHERE IsExit=0 )");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AreaID", areaID);
                    dbOperator.AddParameter("BaseTypeID", (int)cartype);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "GetAreaCarNum异常:" + e.Message;
            }
            return count;
        }


        public int GetNestAreaIsEditCarNum(string areaID, out string ErrorMessage)
        {
            ErrorMessage = "";
            string sql = "";
            int count = 0;
            try
            {
                sql = string.Format(@"SELECT count(*) FROM parkiorecord I LEFT JOIN parkcartype  C ON I.CarTypeID=C.CarTypeID  
                                      WHERE C.InOutEditCar=1 and I.AreaID=@AreaID and I.IsExit=0 and I.DataStatus!=@DataStatus
                                      AND I.RecordID not in (select IORecordID from parktimeseries WHERE IsExit=0 )");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AreaID", areaID);

                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "GetAreaCarNum异常:" + e.Message;
            }
            return count;
        }


        public int GetIsEditCarNum(string areaID, out string ErrorMessage)
        {
            ErrorMessage = "";
            string sql = "";
            int count = 0;
            try
            {
                sql = string.Format(@"SELECT  COUNT(*) from  parkiorecord i   left JOIN parkcartype c on i.CarTypeID=c.CarTypeID 
                                        LEFT JOIN parkgate g on i.entrancegateid= g.GateID LEFT JOIN parkbox b on b.BoxID = g.BoxID where c.InOutEditCar =1 
                                        and b.AreaID =@AreaID and i.IsExit != 1 and i.DataStatus !=@DataStatus AND I.RecordID not in (select IORecordID from parktimeseries WHERE IsExit=0 )");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AreaID", areaID);

                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "GetAreaCarNum异常:" + e.Message;
            }
            return count;
        }
        public int GetAreaCarNumWhenTimeseriesUnExit(string areaID, out string ErrorMessage)
        {
            ErrorMessage = "";
            string sql = "";
            int count = 0;
            try
            {
                sql = string.Format(@"SELECT count(*)  FROM parkiorecord  WHERE  AreaID=@AreaID and DataStatus!=@DataStatus  and IsExit=0 AND RecordID not in (select IORecordID from parktimeseries WHERE IsExit=0 )");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AreaID", areaID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "GetAreaCarNumWhenTimeseriesUnExit异常:" + e.Message;
            }
            return count;
        }

        public DateTime? GetLastRecordExitDateByPlateNumber(string parkingID, string platenumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 ExitTime from ParkIORecord  where Platenumber=@Platenumber and ParkingID=@ParkingID and DataStatus!=@DataStatus order by ExitTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("Platenumber", platenumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTimeDefaultMin(0);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public DateTime? GetLastRecordExitDateByCarNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 ExitTime from ParkIORecord  where ParkingID=@ParkingID and CardNo=@CardNo and DataStatus!=@DataStatus order by ExitTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("CardNo", cardNo);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTimeDefaultMin(0);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public DateTime? GetLastRecordEnterTimeByPlateNumber(string parkingID, string platenumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 EntranceTime from ParkIORecord  where Platenumber=@Platenumber and ParkingID=@ParkingID and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("PlateNumber", platenumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTimeDefaultMin(0);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public DateTime? GetLastRecordEnterTimeByCarNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 EntranceTime from ParkIORecord  where ParkingID=@ParkingID and CardNo=@CardNo and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("CardNo", cardNo);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTimeDefaultMin(0);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public ParkIORecord QueryCarLastNotExitIORecord(string parkingId, string plateNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from ParkIORecord  where ParkingID=@ParkingID and PlateNumber=@PlateNumber and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("PlateNumber", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkIORecord>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public ParkIORecord QueryLastExitIORecordByPlateNumber(string plateNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from ParkIORecord  where  PlateNumber=@PlateNumber and IsExit = 0 and DataStatus!=@DataStatus order by EntranceTime desc");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateNumber", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkIORecord>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public Dictionary<string, DateTime> QueryLastNotExitIORecord(string parkingId, List<string> plateNumbers)
        {
            Dictionary<string, DateTime> dicRecords = new Dictionary<string, DateTime>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PlateNumber,max(EntranceTime) EntranceTime from ParkIORecord");
            strSql.AppendFormat(" where ParkingID=@ParkingID and PlateNumber in('{0}') and IsExit = 0 and DataStatus!=@DataStatus", string.Join("','", plateNumbers));
            strSql.Append(" group by PlateNumber");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        string plateNumber = reader.GetStringDefaultEmpty(0);
                        if (!dicRecords.ContainsKey(plateNumber))
                        {
                            dicRecords.Add(plateNumber, reader.GetDateTimeDefaultMin(1));
                        }
                    }
                }
            }
            return dicRecords;
        }

        public List<string> QueryMonthExpiredNotPayAmountIORecordIds(DateTime start, DateTime end, string parkingId, List<string> plateNumbers)
        {
            List<string> IORecordIds = new List<string>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select p.RecordID from  ParkIORecord p inner join ParkOrder o on p.RecordID=o.TagID where p.IsExit=1 and p.EnterType=1 and o.Status=4");
            strSql.Append(" and o.OrderTime>=@StartDate and o.OrderTime<=@EndDate");
            strSql.AppendFormat("  and p.PlateNumber in('{0}') and p.ParkingID=@ParkingID and p.DataStatus!=@DataStatus", string.Join("','", plateNumbers));


            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("StartDate", start.ToString("yyyy-MM-dd"));
                dbOperator.AddParameter("EndDate", end.ToString("yyyy-MM-dd"));
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        IORecordIds.Add(reader.GetStringDefaultEmpty(0));
                    }
                }
            }
            return IORecordIds;
        }

        public List<ParkIORecord> QueryMonthExpiredIORecordIds(DateTime start, DateTime end, string parkingId, bool IsExit)
        {
            List<ParkIORecord> IORecordIds = new List<ParkIORecord>();
            StringBuilder strSql = new StringBuilder();
            if (IsExit)
            {
                strSql.Append(" select a.*,b.BaseTypeID from  ParkIORecord a left join ParkCarType b on a.CarTypeID=b.CarTypeID  where ExitTime>=@StartDate and ExitTime<=@EndDate and a.ParkingID=@ParkingID and a.DataStatus!=@DataStatus ");
            }
            else
            {
                strSql.Append(" select a.*,b.BaseTypeID from  ParkIORecord a left join ParkCarType b on a.CarTypeID=b.CarTypeID  where EntranceTime>=@StartDate and EntranceTime<=@EndDate and a.ParkingID=@ParkingID and a.DataStatus!=@DataStatus ");
            }
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("StartDate", start);
                dbOperator.AddParameter("EndDate", end);
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkIORecord model = DataReaderToModel<ParkIORecord>.ToModel(reader);
                        IORecordIds.Add(model);
                    }
                }
            }
            return IORecordIds;
        }

        public List<ParkIORecord> QueryInIORecordIds(string parkingId, bool IsExit)
        {
            List<ParkIORecord> IORecordIds = new List<ParkIORecord>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,b.BaseTypeID from  ParkIORecord a left join ParkCarType b on a.CarTypeID=b.CarTypeID  where a.ParkingID=@ParkingID and a.DataStatus!=@DataStatus and IsExit=@IsExit ");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("IsExit", IsExit);
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkIORecord model = DataReaderToModel<ParkIORecord>.ToModel(reader);
                        IORecordIds.Add(model);
                    }
                }
            }
            return IORecordIds;
        }

        public bool UpdateIORecordEnterType(List<string> recordIds, int enterType, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkIORecord set EnterType=@EnterType,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.AppendFormat(" where RecordID in('{0}')", string.Join("','", recordIds));
            dbOperator.ClearParameters();
            dbOperator.AddParameter("EnterType", enterType);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }


        public ParkIORecord GetIORecord(string recordID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkIORecord  where RecordID=@RecordID and DataStatus!=@DataStatus");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public ParkIORecord GetIORecordContainsDelete(string recordID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkIORecord  where RecordID=@RecordID ");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", recordID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }


        public int GetMonthIORecordCountByPlateNumber(string parkingID, string plateNumber, DateTime datetime, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select count(*) Count from ParkIORecord where PlateNumber = @PlateNumber and ParkingID = @ParkingID and DataStatus != @DataStatus
                                        and EntranceTime >= @StartTime and EntranceTime <= @EndTime");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ParkingID", parkingID);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("StartTime", datetime.AddDays(1 - datetime.Day));
                    dbOperator.AddParameter("EndTime", datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1));
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return 0;
        }

        public List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumber, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
            string strWhere = string.Format(" ParkingID='{0}' and DataStatus!=2 ", parkingID);

            if (isExit == 0 || isExit == 1)
            {
                strWhere = strWhere + " and Isexit=" + isExit;
            }
            if (!likePlateNumber.IsEmpty())
            {
                strWhere = string.Format("{0} and PlateNumber like '%{1}%'", strWhere, likePlateNumber);
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
                    cardids += "'" + item + "',";
                }
                if (!cardids.IsEmpty())
                {
                    cardids = cardids.Substring(0, cardids.Length - 1);
                    strWhere = strWhere + " and CarTypeID in(" + cardids + ")";
                }
            }
            return GetTabelWithPageTab<ParkIORecord>("ParkIORecord", pageSize, pageIndex, strWhere, out pageCount, out ErrorMessage, "EntranceTime");

        }

        public List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string carModelID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1)
        {
            string strWhere = string.Format(" ParkingID='{0}' and DataStatus!=2 ", parkingID);

            if (isExit == 0 || isExit == 1)
            {
                strWhere = strWhere + " and Isexit=" + isExit;
            }
            if (!likePlateNumber.IsEmpty())
            {
                strWhere = string.Format("{0} and PlateNumber like '%{1}%'", strWhere, likePlateNumber);
            }
            if (!carModelID.IsEmpty())
            {
                strWhere = strWhere + " and CarModelID='" + carModelID + "'";
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
                    cardids += "'" + item + "',";
                }
                if (!cardids.IsEmpty())
                {
                    cardids = cardids.Substring(0, cardids.Length - 1);
                    strWhere = strWhere + " and CarTypeID in(" + cardids + ")";
                }
            }
            if (!ingateid.IsEmpty())
            {
                strWhere = strWhere + " and EntranceGateID='" + ingateid + "'";
            }
            if (!outgateid.IsEmpty())
            {
                strWhere = strWhere + " and ExitGateID='" + outgateid + "'";
            }
            if (stayDay >= 0)
            {
                strWhere = string.Format("{0} and DateDiff(D,EntranceTime,'{1}')={2}", strWhere, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stayDay);
            }

            string orderStr = "EntranceTime";
            if (isExit == 0)
            {
                strWhere = strWhere + " and EntranceTime>='" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strWhere = strWhere + " and EntranceTime<='" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else if (isExit == 1)
            {
                strWhere = strWhere + " and ExitTime>='" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strWhere = strWhere + " and ExitTime<='" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else
            {
                strWhere = strWhere + " and EntranceTime>='" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strWhere = strWhere + " and EntranceTime<='" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            return GetTabelWithPageTab<ParkIORecord>("ParkIORecord", pageSize, pageIndex, strWhere, out pageCount, out ErrorMessage, orderStr);

        }

        public List<ParkIORecord> GetInParkingIORecords(string pkid)
        {
            List<ParkIORecord> ParkInterims = new List<ParkIORecord>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkIORecord where ParkingID=@ParkingID  and DataStatus!=@DataStatus and IsExit=0");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", pkid);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkInterims.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }
                }
            }
            return ParkInterims;
        }

        #region 进场数
        /// <summary>
        ///  获取通道进场数
        /// </summary>
        /// <param name="gateid">通道编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int EntranceCountByGate(string gateid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where entrancegateid=@gateid 
                                         and DataStatus!=2 and EntranceTime>=@starttime and EntranceTime<=@endtime");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("gateid", gateid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;
        }
        /// <summary>
        /// 获取岗停进场记录
        /// </summary>
        /// <param name="boxid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public int EntranceCountByBox(string boxid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where DataStatus!=2 and EntranceTime>=@StartTime and EntranceTime<=@EndTime 
                                         and EntranceGateID in (select GateID from parkgate where IoState=1 and boxid=@boxid)");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("boxid", boxid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;

        }
        /// <summary>
        /// 获取车场进场记录
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int EntranceCountByParkingID(string parkingid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where DataStatus!=2 
                                     and EntranceTime>=@starttime and EntranceTime<=@endtime and ParkingID=@parkingid");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("parkingid", parkingid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;
        }
        #endregion

        #region 出场数
        /// <summary>
        ///  获取通道进场数
        /// </summary>
        /// <param name="gateid">通道编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int ExitCountByGate(string gateid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where exitgateid=@gateid 
                                         and DataStatus!=2 and ExitTime>=@starttime and ExitTime<=@endtime");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("gateid", gateid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;
        }
        /// <summary>
        /// 获取岗停进场记录
        /// </summary>
        /// <param name="boxid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public int ExitCountByBox(string boxid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where DataStatus!=2 and ExitTime>=@StartTime and ExitTime<=@EndTime 
                                         and exitgateid in (select GateID from parkgate where IoState=2 and boxid=@boxid)");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("boxid", boxid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;

        }
        /// <summary>
        /// 获取车场进场记录
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int ExitCountByParkingID(string parkingid, DateTime starttime, DateTime endtime)
        {
            int _count = 0;
            string strSql = string.Format(@"select count(1) Count from ParkIORecord where DataStatus!=2 
                                     and ExitTime>=@starttime and ExitTime<=@endtime and ParkingID=@parkingid");
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("parkingid", parkingid);
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _count = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _count;
        }
        #endregion

        #region 进场卡片类型
        /// <summary>
        /// 通过车场获取进场卡片类型
        /// </summary>
        /// <param name="parkingid">车场编号/param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetInCardTypeByParkingID(string parkingid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> incardtypelist = new List<KeyValue>();
            string strSql = string.Format(@"select c.basetypeid KeyName,count(1) Key_Value from parkiorecord p left join 
                                         parkcartype c on p.cartypeid=c.cartypeid 
                                         where p.ParkingID=@parkingid and p.DataStatus!=2 and p.EntranceTime>=@starttime and p.EntranceTime<=@endtime group by c.basetypeid");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        incardtypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return incardtypelist;
        }
        /// <summary>
        /// 通过车场获取进场卡片类型
        /// </summary>
        /// <param name="parkingid">车场编号/param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetInCardTypeByBoxID(string boxid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> incardtypelist = new List<KeyValue>();
            string strSql = string.Format(@"select c.basetypeid KeyName,count(1) Key_Value from parkiorecord p left join 
                                         parkcartype c on p.cartypeid=c.cartypeid 
                                         where EntranceGateID in (select GateID from parkgate where IoState=1 and boxid=@boxid) and p.DataStatus!=2 and p.EntranceTime>=@starttime and p.EntranceTime<=@endtime group by c.basetypeid");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("boxid", boxid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        incardtypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return incardtypelist;
        }
        /// <summary>
        /// 获得通道进场卡片类型
        /// </summary>
        /// <param name="gateid">进场通道</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetInCardTypeByGateID(string gateid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> incardtypelist = new List<KeyValue>();
            string strSql = string.Format(@"select c.basetypeid KeyName,count(1) Key_Value from parkiorecord p left join 
                                         parkcartype c on p.cartypeid=c.cartypeid 
                                         where p.entrancegateid=@gateid and p.DataStatus!=2 and p.EntranceTime>=@StartTime and p.EntranceTime<=@EndTime group by c.basetypeid");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("gateid", gateid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        incardtypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return incardtypelist;
        }
        #endregion

        #region 放行类型
        /// <summary>
        /// 放行类型
        /// </summary>
        /// <param name="ParkingID">通道编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetReleaseTypeByParkingID(string parkingid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> releasetypelist = new List<KeyValue>();
            string strSql = string.Format(@"select count(releasetype) Key_Value,releasetype KeyName from parkiorecord 
                                         where ParkingID=@parkingid and DataStatus!=2 and ExitTime>=@starttime and ExitTime<=@endtime and isexit=1 group by releasetype");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        releasetypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return releasetypelist;
        }

        /// <summary>
        /// 放行类型
        /// </summary>
        /// <param name="ParkingID">通道编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetReleaseTypeByGate(string gateid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> releasetypelist = new List<KeyValue>();
            string strSql = string.Format(@"select count(releasetype) Key_Value,releasetype KeyName from parkiorecord 
                                         where exitgateid=@gateid and DataStatus!=2 and ExitTime>=@starttime 
                                         and ExitTime<=@endtime and isexit=1 
                                         group by releasetype");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("gateid", gateid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        releasetypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return releasetypelist;
        }
        /// <summary>
        /// 放行类型
        /// </summary>
        /// <param name="ParkingID">岗亭编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<KeyValue> GetReleaseTypeByBox(string boxid, DateTime starttime, DateTime endtime)
        {
            List<KeyValue> releasetypelist = new List<KeyValue>();
            string strSql = string.Format(@"select count(releasetype) Key_Value,releasetype KeyName from parkiorecord 
                                         where DataStatus!=2 and ExitTime>=@starttime 
                                         and ExitTime<=@endtime and isexit=1 and ExitGateID in (select GateID from parkgate where IoState=2 and boxid=@boxid)
                                         group by releasetype");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("boxid", boxid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        releasetypelist.Add(DataReaderToModel<KeyValue>.ToModel(reader));
                    }
                }
            }
            return releasetypelist;
        }
        #endregion
        /// <summary>
        /// 获取车辆的停车时长
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<ParkIORecord> GetCarEntranceTimeAndExitTime(string parkingid, DateTime starttime, DateTime endtime)
        {
            List<ParkIORecord> inhourlist = new List<ParkIORecord>();
            string strSql = string.Format(@"select EntranceTime,ExitTime  
                                         from parkiorecord where entrancetime>=@starttime 
                                         and entrancetime<=@endtime 
                                         and DataStatus!=2 
                                         and parkingid=@parkingid");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                dbOperator.AddParameter("starttime", starttime);
                dbOperator.AddParameter("endtime", endtime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        inhourlist.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }
                }
            }
            return inhourlist;
        }
        public List<ParkIORecord> QueryPageNotExit(string parkingId, string plateNumber, int pageSize, int pageIndex, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select a.PlateNumber,max(a.ID) ID from parkiorecord a left join ParkCarType b on a.CarTypeID=b.CarTypeID ");
            sql.Append(" where a.DataStatus!=@DataStatus  and b.DataStatus!=@DataStatus and a.IsExit=0");
            sql.Append("  and (b.BaseTypeID=@BaseTypeID or b.BaseTypeID is null)");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("BaseTypeID", (int)BaseCarType.TempCar);
                if (!string.IsNullOrWhiteSpace(parkingId))
                {
                    sql.Append(" and a.ParkingID=@ParkingID");
                    dbOperator.AddParameter("ParkingID", parkingId);
                }

                if (!string.IsNullOrWhiteSpace(plateNumber))
                {
                    sql.Append(" and a.PlateNumber like @PlateNumber");
                    dbOperator.AddParameter("PlateNumber", "%" + plateNumber + "%");
                }
                sql.Append(" group by a.PlateNumber ");
                List<int> pageDataIds = new List<int>();

                List<ParkIORecord> models = new List<ParkIORecord>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "ID DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        pageDataIds.Add(reader.GetInt32DefaultZero(1));
                    }

                }
                if (pageDataIds.Count > 0)
                {
                    models = QueryParkIORecordByIds(pageDataIds);
                }
                return models;

            }
        }
        private List<ParkIORecord> QueryParkIORecordByIds(List<int> ids)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from parkiorecord WHERE id in({0})", string.Join(",", ids));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                List<ParkIORecord> models = new List<ParkIORecord>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }

                }
                return models;

            }
        }
        public List<ParkIORecord> GetIORecordUseLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumberStr, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
            string strWhere = string.Format(" ParkingID='{0}' and DataStatus!=2 ", parkingID);

            if (isExit == 0 || isExit == 1)
            {
                strWhere = strWhere + " and Isexit=" + isExit;
            }
            if (!likePlateNumberStr.IsEmpty())
            {
                strWhere = string.Format("{0} and {1}", strWhere, likePlateNumberStr);
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
                    cardids += "'" + item + "',";
                }
                if (!cardids.IsEmpty())
                {
                    cardids = cardids.Substring(0, cardids.Length - 1);
                    strWhere = strWhere + " and CarTypeID in(" + cardids + ")";
                }
            }
            return GetTabelWithPageTab<ParkIORecord>("ParkIORecord", pageSize, pageIndex, strWhere, out pageCount, out ErrorMessage, "EntranceTime");

        }

        public List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid)
        {
            List<ParkIORecord> ParkInterims = new List<ParkIORecord>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkIORecord where ParkingID=@ParkingID and PlateNumber like '%无车牌%' and DataStatus!=@DataStatus and IsExit=0");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", pkid);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkInterims.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }
                }
            }
            return ParkInterims;
        }

        public List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid, DateTime datetime)
        {
            List<ParkIORecord> ParkInterims = new List<ParkIORecord>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkIORecord where ParkingID=@ParkingID and PlateNumber like '%无车牌%' and DataStatus!=@DataStatus and EntranceTime>@EntranceTimeStart and EntranceTime<@EntranceTimeEnd  and IsExit=0");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", pkid);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("EntranceTimeStart", datetime.Date);
                dbOperator.AddParameter("EntranceTimeEnd", datetime.Date.AddDays(1));
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkInterims.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }
                }
            }
            return ParkInterims;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="RecordID"></param>
        /// <returns></returns>
        public bool DelParkIORecord(string RecordID)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkIORecord set DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("DataStatus", DataStatus.Delete);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                    dbOperator.AddParameter("RecordID", RecordID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改车类型
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="CarType"></param>
        /// <returns></returns>
        public bool EditParkIORecord(string RecordID, string CarModelID)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkIORecord set CarModelID=@CarModelID,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("DataStatus", DataStatus.Normal);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                    dbOperator.AddParameter("RecordID", RecordID);
                    dbOperator.AddParameter("CarModelID", CarModelID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public ParkIORecord QueryInCarTempIORecordByLikePlateNumber(string parkid, string likeplateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                string strSql = "";
                strSql = @"SELECT TOP(1) * from ParkIORecord i LEFT JOIN ParkCarType c on c.CarTypeID=i.CarTypeID  
                        where i.ParkingID=@ParkingID  and i.PlateNumber like @PlateNumber 
                        and i.IsExit=0 and i.DataStatus!=@DataStatus and c.DataStatus!=@DataStatus and c.BaseTypeID=@BaseTypeID";
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNumber", "%" + likeplateNumber + "");
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("ParkingID", parkid);
                    dbOperator.AddParameter("BaseTypeID", (int)BaseCarType.TempCar);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public List<ParkIORecord> QueryPlatenumberIORecordByTime(DateTime start, DateTime end, string platenumber, string parkingId, bool IsExit)
        {
            List<ParkIORecord> IORecordIds = new List<ParkIORecord>();
            string strSql = "";
            if (!IsExit)
            {
                strSql = string.Format(" select * from ParkIORecord where EntranceTime<=@StartDate and ParkingID=@ParkingID and DataStatus!=@DataStatus and PlateNumber=@PlateNumber  and IsExit={0}", 0);
            }
            else
            {
                strSql = string.Format(" select * from ParkIORecord where ExitTime>=@StartDate and ExitTime<=@EndDate and ParkingID=@ParkingID and DataStatus!=@DataStatus and PlateNumber=@PlateNumber and IsExit={0}", 1);
            }
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("StartDate", start);
                dbOperator.AddParameter("EndDate", end);
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("PlateNumber", platenumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        IORecordIds.Add(DataReaderToModel<ParkIORecord>.ToModel(reader));
                    }
                }
            }
            return IORecordIds;
        }
    }
}
