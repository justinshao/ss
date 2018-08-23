using Common.DataAccess;
using Common.Entities;
using Common.IRepository.Park;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Common.Entities.Parking;

namespace Common.SqlRepository
{
    public class ParkDeviceDetectionDAL : BaseDAL, IParkDeviceDetection
    {
        public bool Add(ParkDeviceDetection model,DbOperator dbOperator) {
               model.DataStatus = (int)DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkDeviceDetection(RecordID,DeviceID,PKID,ConnectionState,DisconnectTime,DataStatus,LastUpdateTime,HaveUpdate)");
                strSql.Append(" values(@RecordID,@DeviceID,@PKID,@ConnectionState,@DisconnectTime,@DataStatus,@LastUpdateTime,@HaveUpdate)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("DeviceID", model.DeviceID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("ConnectionState", model.ConnectionState);
                dbOperator.AddParameter("DisconnectTime", model.DisconnectTime);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Update(string deviceID,string parkingID, bool connectionState)
        {
            bool isRecordExits = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDeviceDetection");
            strSql.Append(" WHERE deviceID=@deviceID AND DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("deviceID", deviceID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete); 
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    isRecordExits = reader.Read(); 
                }
            }
            if(isRecordExits)//存在的话修改数据 
            { 
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    strSql = new StringBuilder();
                    strSql.Append("update ParkDeviceDetection set HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,ConnectionState=@ConnectionState,DisconnectTime=@DisconnectTime,PKID=@PKID");
                    strSql.Append(" where DeviceID=@DeviceID and DataStatus!=@DataStatus");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("DeviceID", deviceID);
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("DisconnectTime", DateTime.Now);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    dbOperator.AddParameter("ConnectionState", connectionState);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                } 
            }
            else//不存在的话新增数据
            { 
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                { 
                    strSql = new StringBuilder();
                    strSql.Append("insert into ParkDeviceDetection(RecordID,DeviceID,ConnectionState,DisconnectTime,LastUpdateTime,HaveUpdate,DataStatus,PKID)");
                    strSql.Append(" values(@RecordID,@DeviceID,@ConnectionState,@DisconnectTime,@LastUpdateTime,@HaveUpdate,@DataStatus,@PKID);");
                  
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", GuidGenerator.GetGuidString());
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("DeviceID", deviceID);
                    dbOperator.AddParameter("ConnectionState", connectionState); 
                    dbOperator.AddParameter("DisconnectTime", DateTime.Now); 
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                    dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                    dbOperator.AddParameter("DataStatus",(int) DataStatus.Normal); 
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
        }

        public bool Delete(string deviceId,DbOperator dbOperator)
        {
           return CommonDelete("ParkDeviceDetection", "DeviceID", deviceId, dbOperator);
        }
        public ParkDeviceDetection QueryByDeviceID(string deviceId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDeviceDetection where DeviceID=@DeviceID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DeviceID", deviceId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                   if(reader.Read()){
                        return DataReaderToModel<ParkDeviceDetection>.ToModel(reader);
                   }
                  return null;
                }
            }
        }
    }
}
