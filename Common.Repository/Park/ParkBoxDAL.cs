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
    public class ParkBoxDAL : BaseDAL,IParkBox
    {
        public bool Add(ParkBox model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkBox(BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,DataStatus,HaveUpdate,LastUpdateTime,IsCenterPayment)");
                strSql.Append(" values(@BoxID,@BoxNo,@BoxName,@ComputerIP,@AreaID,@Remark,@DataStatus,@HaveUpdate,@LastUpdateTime,@IsCenterPayment)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", model.BoxID);
                dbOperator.AddParameter("BoxNo", model.BoxNo);
                dbOperator.AddParameter("BoxName", model.BoxName);
                dbOperator.AddParameter("ComputerIP", model.ComputerIP);
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("IsCenterPayment", (int)model.IsCenterPayment);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkBox model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkBox set BoxNo=@BoxNo,BoxName=@BoxName,ComputerIP=@ComputerIP,AreaID=@AreaID,Remark=@Remark");
                strSql.Append(",IsCenterPayment=@IsCenterPayment,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where BoxID=@BoxID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", model.BoxID);
                dbOperator.AddParameter("BoxNo", model.BoxNo);
                dbOperator.AddParameter("BoxName", model.BoxName);
                dbOperator.AddParameter("ComputerIP", model.ComputerIP);
                dbOperator.AddParameter("AreaID",model.AreaID);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("IsCenterPayment", (int)model.IsCenterPayment);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("ParkBox", "BoxID", recordId);
        }

        public ParkBox QueryByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,IsCenterPayment,DataStatus,HaveUpdate,LastUpdateTime");
            strSql.Append(" from ParkBox where BoxID=@BoxID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkBoxs(reader).FirstOrDefault();
                }
            }
        }
        private List<ParkBox> GetParkBoxs(DbDataReader reader)
        {
            List<ParkBox> models = new List<ParkBox>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<ParkBox>.ToModel(reader)); 
            }
            return models;
        }
        public List<ParkBox> QueryByComputerIps(string ip)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,IsCenterPayment,DataStatus,HaveUpdate,LastUpdateTime");
            strSql.Append(" from ParkBox where ComputerIP=@ComputerIP and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ComputerIP", ip);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkBoxs(reader);
                }
            }
        }
        /// <summary>
        /// 通过车场编号取得岗亭信息
        /// </summary>
        /// <param name="parkingid"></param>
        /// <returns></returns>
        public List<ParkBox> QueryByParkingID(string parkingid)
        {
            List<ParkBox> boxlist = new List<ParkBox>();
            string strSql = "select * from parkbox where areaid in (select areaid from parkarea where pkid=@parkingid)";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("parkingid", parkingid);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        boxlist.Add(DataReaderToModel<ParkBox>.ToModel(reader));
                    }
                }
            }
            return boxlist;
        }

        public List<ParkBox> QueryByParkAreaId(string areaId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,IsCenterPayment,DataStatus,HaveUpdate,LastUpdateTime");
            strSql.Append(" from ParkBox where AreaID=@AreaID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", areaId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkBoxs(reader);
                }
            }
        }

        public List<ParkBox> QueryByParkAreaIds(List<string> areaIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,IsCenterPayment,DataStatus,HaveUpdate,LastUpdateTime");
            strSql.AppendFormat(" from ParkBox where AreaID in ('{0}') and DataStatus!=@DataStatus",string.Join("','",areaIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkBoxs(reader);
                }
            }
        }
    }
}
