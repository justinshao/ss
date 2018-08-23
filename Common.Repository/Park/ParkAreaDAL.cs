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
    public class ParkAreaDAL : BaseDAL, IParkArea
    {
        public bool Add(ParkArea model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkArea(AreaID,AreaName,MasterID,PKID,CarbitNum,NeedToll,CameraWaitTime,TwoCameraWait,Remark,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@AreaID,@AreaName,@MasterID,@PKID,@CarbitNum,@NeedToll,@CameraWaitTime,@TwoCameraWait,@Remark,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("AreaName", model.AreaName);
                dbOperator.AddParameter("MasterID", model.MasterID);
                dbOperator.AddParameter("PKID",model.PKID);
                dbOperator.AddParameter("CarbitNum", model.CarbitNum);
                dbOperator.AddParameter("NeedToll", (int)model.NeedToll);
                dbOperator.AddParameter("CameraWaitTime", (int)model.CameraWaitTime);
                dbOperator.AddParameter("TwoCameraWait", (int)model.TwoCameraWait);
                dbOperator.AddParameter("Remark",model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkArea model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkArea set AreaName=@AreaName,MasterID=@MasterID,CarbitNum=@CarbitNum,NeedToll=@NeedToll,CameraWaitTime=@CameraWaitTime,TwoCameraWait=@TwoCameraWait");
                strSql.Append(",Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where AreaID=@AreaID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("AreaName", model.AreaName);
                dbOperator.AddParameter("MasterID", model.MasterID);
                dbOperator.AddParameter("CarbitNum", model.CarbitNum);
                dbOperator.AddParameter("NeedToll", (int)model.NeedToll);
                dbOperator.AddParameter("CameraWaitTime", (int)model.CameraWaitTime);
                dbOperator.AddParameter("TwoCameraWait", (int)model.TwoCameraWait);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool UpdateCarbitNum(string recordId, int carNum)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkArea set CarbitNum=@CarbitNum,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where AreaID=@AreaID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", recordId);
                dbOperator.AddParameter("CarbitNum", carNum);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("ParkArea", "AreaID", recordId);
        }

        public ParkArea QueryByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from ParkArea where AreaID=@AreaID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader).FirstOrDefault();
                }
            }
        }
        private List<ParkArea> GetParkAreas(DbDataReader reader)
        {
            List<ParkArea> models = new List<ParkArea>();
            while (reader.Read())
            { 
                models.Add(DataReaderToModel<ParkArea>.ToModel(reader)); 
            }
            return models;
        }
        public ParkArea GetParkAreaByParkBoxRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.* ");
            strSql.Append("  from  ParkBox b left join ParkArea a on a.AreaID=b.AreaID ");
            strSql.Append(" where b.BoxID=@BoxID and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader).FirstOrDefault();
                }
            }
        }

        public ParkArea GetParkAreaByParkGateRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append("  from ParkGate p left join ParkBox b on p.BoxID= b.BoxID");
            strSql.Append("  left join ParkArea a on a.AreaID=b.AreaID ");
            strSql.Append(" where p.GateID=@GateID and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus and p.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader).FirstOrDefault();
                }
            }
        }

        public List<ParkArea> GetParkAreaByParkBoxIps(List<string> ips)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append("  from  ParkBox b left join ParkArea a on a.AreaID=b.AreaID ");
            strSql.AppendFormat(" where b.ComputerIP in('{0}') and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus", string.Join("','", ips));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader);
                }
            }
        }

        public List<ParkArea> GetParkAreaByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from ParkArea where PKID=@PKID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader);
                }
            }
        }

        public List<ParkArea> GetParkAreaByMasterId(string masterId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from ParkArea where MasterID=@MasterID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MasterID", masterId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader);
                }
            }
        }

        public List<ParkArea> GetParkAreaByParkingIds(List<string> parkingIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.AppendFormat(" from ParkArea where PKID in('{0}') and DataStatus!=@DataStatus",string.Join("','",parkingIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader);
                }
            }
        }

        public List<ParkArea> GetTopParkAreaByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from ParkArea where (MasterID is null or MasterID='') and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkAreas(reader);
                }
            }
        }
        public int GetCarBitNumByParkingId(string parkingId) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(CarbitNum) CarbitNum from ParkArea where PKID=@PKID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read()) {
                       return reader.GetInt32DefaultZero(0);
                    }
                    return 0;
                }
            }
        }
    }
}
