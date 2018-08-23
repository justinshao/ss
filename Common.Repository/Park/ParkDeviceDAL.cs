using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities.Parking;
using Common.IRepository;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
namespace Common.SqlRepository
{
    public class ParkDeviceDAL : BaseDAL, IParkDevice
    {
        public bool Add(ParkDevice model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkDevice(DeviceID,GateID,DeviceType,PortType,Baudrate,SerialPort,IpAddr,IpPort,UserName,UserPwd,NetID,LedNum,DeviceNo,OfflinePort,DataStatus,LastUpdateTime,HaveUpdate,IsCapture,IsSVoice,IsCarBit,IsContestDev,ControllerType,DisplayMode,IsMonitor)");
            strSql.Append(" values(@DeviceID,@GateID,@DeviceType,@PortType,@Baudrate,@SerialPort,@IpAddr,@IpPort,@UserName,@UserPwd,@NetID,@LedNum,@DeviceNo,@OfflinePort,@DataStatus,@LastUpdateTime,@HaveUpdate,@IsCapture,@IsSVoice,@IsCarBit,@IsContestDev,@ControllerType,@DisplayMode,@IsMonitor)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("DeviceID", model.DeviceID);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("DeviceType", model.DeviceType);
            dbOperator.AddParameter("PortType", model.PortType);
            dbOperator.AddParameter("Baudrate", model.Baudrate);
            dbOperator.AddParameter("SerialPort", model.SerialPort);
            dbOperator.AddParameter("IpAddr", model.IpAddr);
            dbOperator.AddParameter("IpPort", model.IpPort);
            dbOperator.AddParameter("UserName", model.UserName);
            dbOperator.AddParameter("UserPwd", model.UserPwd);
            dbOperator.AddParameter("NetID", model.NetID);
            dbOperator.AddParameter("LedNum", model.LedNum);
            dbOperator.AddParameter("DeviceNo", model.DeviceNo);
            dbOperator.AddParameter("OfflinePort", model.OfflinePort);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            dbOperator.AddParameter("IsCapture", model.IsCapture);
            dbOperator.AddParameter("IsSVoice", model.IsSVoice);
            dbOperator.AddParameter("IsCarBit", model.IsCarBit);
            dbOperator.AddParameter("IsContestDev", model.IsContestDev);
            dbOperator.AddParameter("ControllerType", model.ControllerType);
            dbOperator.AddParameter("DisplayMode", model.DisplayMode);
            dbOperator.AddParameter("IsMonitor", model.IsMonitor);
            
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(ParkDevice model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkDevice set DeviceID=@DeviceID,GateID=@GateID,DeviceType=@DeviceType,PortType=@PortType,Baudrate=@Baudrate,SerialPort=@SerialPort,IpAddr=@IpAddr,IpPort=@IpPort,UserName=@UserName,UserPwd=@UserPwd,NetID=@NetID,LedNum=@LedNum,IsMonitor=@IsMonitor");
            strSql.Append(",DeviceNo=@DeviceNo,OfflinePort=@OfflinePort,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsCapture=@IsCapture,IsSVoice=@IsSVoice,IsCarBit=@IsCarBit,IsContestDev=@IsContestDev,ControllerType=@ControllerType,DisplayMode=@DisplayMode where DeviceID=@DeviceID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("DeviceID", model.DeviceID);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("DeviceType", model.DeviceType);
            dbOperator.AddParameter("PortType", model.PortType);
            dbOperator.AddParameter("Baudrate", model.Baudrate);
            dbOperator.AddParameter("SerialPort", model.SerialPort);
            dbOperator.AddParameter("IpAddr", model.IpAddr);
            dbOperator.AddParameter("IpPort", model.IpPort);
            dbOperator.AddParameter("UserName", model.UserName);
            dbOperator.AddParameter("UserPwd", model.UserPwd);
            dbOperator.AddParameter("NetID", model.NetID);
            dbOperator.AddParameter("LedNum", model.LedNum);
            dbOperator.AddParameter("DeviceNo", model.DeviceNo);
            dbOperator.AddParameter("OfflinePort", model.OfflinePort);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("IsCapture", model.IsCapture);
            dbOperator.AddParameter("IsSVoice", model.IsSVoice);
            dbOperator.AddParameter("IsCarBit", model.IsCarBit);
            dbOperator.AddParameter("IsContestDev", model.IsContestDev);
            dbOperator.AddParameter("ControllerType", model.ControllerType);
            dbOperator.AddParameter("DisplayMode", model.DisplayMode);
            dbOperator.AddParameter("IsMonitor", model.IsMonitor);
            
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string recordId, DbOperator dbOperator)
        {
            return CommonDelete("ParkDevice", "DeviceID", recordId, dbOperator);
        }

        public List<ParkDevice> QueryParkDeviceByGateRecordId(string gateRecordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDevice where GateID=@GateID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", gateRecordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkDevice> models = new List<ParkDevice>();
                    while (reader.Read()) {
                        models.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public ParkDevice QueryParkDeviceByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDevice where DeviceID=@DeviceID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DeviceID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkDevice>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<ParkDevice> QueryParkDeviceByIPAddress(string ipaddress)
        {
            List<ParkDevice> parkdevicelist = new List<ParkDevice>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDevice where IpAddr=@IpAddr and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("IpAddr", ipaddress);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        parkdevicelist.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                    }
                }
            }
            return parkdevicelist;
        }
        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <returns></returns>
        public List<ParkDevice> QueryParkDeviceDetectionByParkingID(string villageid)
        {
            List<ParkDevice> parkdevicelist = new List<ParkDevice>();
            string strSql = string.Format(@"select p.PKID,d.ID,case d.connectionstate when 0 then '断开' when 1 then '连接' when 3 then '未知' else '' end ConnectionStateName,
                                            case device.devicetype when 0 then '大华_NVR' when 1 then '海康_NVR' when 2 then '火眼_NVR' when 3 then '华夏_NVR' 
                                            when 4 then '欧冠LED' when 5 then '车场控制器' when 6 then 'XM抓拍相机' else '' end DeviceTypeName,case device.porttype when 0 then 'TCP/IP' when 1 then '串口' else '' end PortTypeName,
                                            device.BaudRate,d.DisconnectTime,device.SerialPort,device.IpAddr,device.IpPort,device.LedNum,g.GateName,p.PKName ParkingName,b.BoxName
                                            from parkdevicedetection d left join parkdevice device on d.deviceid=device.deviceid 
                                            left join parkgate g on g.gateid=device.gateid 
                                            left join parkbox b on b.boxid=g.boxid
                                            left join parkarea a on a.areaid=b.areaid
                                            left join baseparkinfo p on p.pkid=a.pkid
                                            where p.vid=@villageid and d.datastatus!=2 and device.datastatus!=2");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("villageid", villageid);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        parkdevicelist.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                    }
                }
            }
            return parkdevicelist;
        }

        /// <summary>
        /// 获取所有设备
        /// </summary>
        /// <returns></returns>
        public List<ParkDevice> QueryParkDeviceAll()
        {
            List<ParkDevice> parkdevicelist = new List<ParkDevice>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDevice where  DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
            
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        parkdevicelist.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                    }
                    
                }
            }
            return parkdevicelist;
        }

        /// <summary>
        /// 根据设备ID获取设备参数信息
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public ParkDeviceParam QueryParkDeviceParamByDID(string DeviceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDeviceParam where DeviceID=@DeviceID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DeviceID", DeviceID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkDeviceParam>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public ParkDeviceParam QueryParkDeviceParamByDevID(int DevID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkDeviceParam where DevID=@DevID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DevID", DevID);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkDeviceParam>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 增加设备参数
        /// </summary>
        /// <param name="model"></param>
        public bool AddParam(ParkDeviceParam model, DbOperator dbOperator)
        {
            model.DataStatus = (int)DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkDeviceParam(RecordID,DeviceID,VipMode,TempMode,NetOffMode,VipDevMultIn,PloicFree,VipDutyDay,OverDutyYorN,OverDutyDay,SysID,DevID,SysInDev,SysOutDev,SysParkNumber,DevInorOut,SwipeInterval,UnKonwCardType,LEDNumber,DataStatus,LastUpdateTime,HaveUpdate)");
            strSql.Append(" values(@RecordID,@DeviceID,@VipMode,@TempMode,@NetOffMode,@VipDevMultIn,@PloicFree,@VipDutyDay,@OverDutyYorN,@OverDutyDay,@SysID,@DevID,@SysInDev,@SysOutDev,@SysParkNumber,@DevInorOut,@SwipeInterval,@UnKonwCardType,@LEDNumber,@DataStatus,@LastUpdateTime,@HaveUpdate)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("DeviceID", model.DeviceID);
            dbOperator.AddParameter("VipMode", model.VipMode);
            dbOperator.AddParameter("TempMode", model.TempMode);
            dbOperator.AddParameter("NetOffMode", model.NetOffMode);
            dbOperator.AddParameter("VipDevMultIn", model.VipDevMultIn);
            dbOperator.AddParameter("PloicFree", model.PloicFree);
            dbOperator.AddParameter("VipDutyDay", model.VipDutyDay);
            dbOperator.AddParameter("OverDutyYorN", model.OverDutyYorN);
            dbOperator.AddParameter("OverDutyDay", model.OverDutyDay);
            dbOperator.AddParameter("SysID", model.SysID);
            dbOperator.AddParameter("DevID", model.DevID);
            dbOperator.AddParameter("SysInDev", model.SysInDev);
            dbOperator.AddParameter("SysOutDev", model.SysOutDev);
            dbOperator.AddParameter("SysParkNumber", model.SysParkNumber);
            dbOperator.AddParameter("DevInorOut", model.DevInorOut);
            dbOperator.AddParameter("SwipeInterval", (int)model.SwipeInterval);
            dbOperator.AddParameter("UnKonwCardType", model.UnKonwCardType);
            dbOperator.AddParameter("LEDNumber", model.LEDNumber);

            dbOperator.AddParameter("DataStatus", model.DataStatus);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        /// <summary>
        /// 修改设备DEVID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="deviceNo"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public  bool UpdateParam(string deviceId, string deviceNo, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkDeviceParam set DevID=@DevID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.Append(" where DeviceID=@DeviceID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("DeviceID", deviceId);
            dbOperator.AddParameter("DevID",int.Parse( deviceNo));
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        /// <summary>
        /// 修改设备参数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public bool UpdateParam(ParkDeviceParam model, DbOperator dbOperator)
        {
            model.DataStatus = (int)DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkDeviceParam set DeviceID=@DeviceID,VipMode=@VipMode,TempMode=@TempMode,NetOffMode=@NetOffMode,VipDevMultIn=@VipDevMultIn,PloicFree=@PloicFree,VipDutyDay=@VipDutyDay,OverDutyYorN=@OverDutyYorN,OverDutyDay=@OverDutyDay,SysID=@SysID,");
            strSql.Append(" DevID=@DevID,SysInDev=@SysInDev,SysOutDev=@SysOutDev,SysParkNumber=@SysParkNumber,DevInorOut=@DevInorOut,SwipeInterval=@SwipeInterval,UnKonwCardType=@UnKonwCardType,LEDNumber=@LEDNumber,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.Append(" where RecordID=@RecordID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("DeviceID", model.DeviceID);
            dbOperator.AddParameter("VipMode", model.VipMode);
            dbOperator.AddParameter("TempMode", model.TempMode);
            dbOperator.AddParameter("NetOffMode", model.NetOffMode);
            dbOperator.AddParameter("VipDevMultIn", model.VipDevMultIn);
            dbOperator.AddParameter("PloicFree", model.PloicFree);
            dbOperator.AddParameter("VipDutyDay", model.VipDutyDay);
            dbOperator.AddParameter("OverDutyYorN", model.OverDutyYorN);
            dbOperator.AddParameter("OverDutyDay", model.OverDutyDay);
            dbOperator.AddParameter("SysID", model.SysID);
            dbOperator.AddParameter("DevID", model.DevID);
            dbOperator.AddParameter("SysInDev", model.SysInDev);
            dbOperator.AddParameter("SysOutDev", model.SysOutDev);
            dbOperator.AddParameter("SysParkNumber", model.SysParkNumber);
            dbOperator.AddParameter("DevInorOut", model.DevInorOut);
            dbOperator.AddParameter("SwipeInterval", (int)model.SwipeInterval);
            dbOperator.AddParameter("UnKonwCardType", model.UnKonwCardType);
            dbOperator.AddParameter("LEDNumber", model.LEDNumber);
            dbOperator.AddParameter("DataStatus", model.DataStatus);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        /// <summary>
        /// 删除设备参数
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public bool DeleParam(string deviceId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkDeviceParam set DataStatus=2,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate ");
            strSql.Append(" where DeviceID=@DeviceID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("DeviceID", deviceId);
         
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        
    }
}
