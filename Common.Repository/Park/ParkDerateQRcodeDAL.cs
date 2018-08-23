using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository.Park;
using Common.Entities.Enum;

namespace Common.SqlRepository
{
    public class ParkDerateQRcodeDAL : BaseDAL, IParkDerateQRcode
    {
        public bool Add(ParkDerateQRcode model)
        {
            model.CreateTime = DateTime.Now;
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = string.Format(@"insert ParkDerateQRcode(RecordID, DerateID, DerateValue,StartTime,EndTime,CreateTime,Remark,DataSource,OperatorId,CanUseTimes,AlreadyUseTimes,DerateQRcodeType,PKID,DataStatus,LastUpdateTime,HaveUpdate)
                                        values(@RecordID,@DerateID,@DerateValue,@StartTime,@EndTime,@CreateTime,@Remark,@DataSource,@OperatorId,@CanUseTimes,@AlreadyUseTimes,@DerateQRcodeType,@PKID,@DataStatus,@LastUpdateTime,@HaveUpdate);");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("DerateID", model.DerateID);
                dbOperator.AddParameter("DerateValue", model.DerateValue);
                dbOperator.AddParameter("StartTime", model.StartTime);
                dbOperator.AddParameter("EndTime", model.EndTime);
                dbOperator.AddParameter("CreateTime", model.CreateTime);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("OperatorId", model.OperatorId);
                dbOperator.AddParameter("DataSource", (int)model.DataSource);
                dbOperator.AddParameter("CanUseTimes", model.CanUseTimes);
                dbOperator.AddParameter("AlreadyUseTimes",0);
                dbOperator.AddParameter("DerateQRcodeType", model.DerateQRcodeType);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkDerateQRcode model)
        {
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = string.Format(@"update ParkDerateQRcode set DerateID=@DerateID,DerateValue=@DerateValue,CanUseTimes=@CanUseTimes,StartTime=@StartTime,EndTime=@EndTime,Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID;");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("DerateID", model.DerateID);
                dbOperator.AddParameter("DerateValue", model.DerateValue);
                dbOperator.AddParameter("StartTime", model.StartTime);
                dbOperator.AddParameter("EndTime", model.EndTime);
                dbOperator.AddParameter("CreateTime", model.CreateTime);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("CanUseTimes", model.CanUseTimes);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool Update(string recordId, string derateQRcodeZipFilePath, int totalNumbers, DbOperator dbOperator)
        {
            string strSql = string.Format(@"update ParkDerateQRcode set DerateQRcodeZipFilePath=@DerateQRcodeZipFilePath,CanUseTimes=@CanUseTimes,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID;");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", recordId);
            dbOperator.AddParameter("DerateQRcodeZipFilePath", derateQRcodeZipFilePath);
            dbOperator.AddParameter("CanUseTimes", totalNumbers);
            dbOperator.AddParameter("LastUpdateTime",DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool UpdateAlreadyUseTimes(string recordId, int alreadyUseTimes, DbOperator dbOperator)
        {

            string strSql = string.Format(@"update ParkDerateQRcode set AlreadyUseTimes=@AlreadyUseTimes,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID;");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", recordId);
            dbOperator.AddParameter("AlreadyUseTimes", alreadyUseTimes);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Delete(string recordId, DbOperator dbOperator)
        {
            return CommonDelete("ParkDerateQRcode", "RecordID", recordId, dbOperator);
        }

        public List<ParkDerateQRcode> QueryByDerateID(string derateId)
        {
            List<ParkDerateQRcode> models = new List<ParkDerateQRcode>();
            string sql = "select * from ParkDerateQRcode where DerateID=@DerateID and DataStatus!=@DataStatus";

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DerateID", derateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                     while(reader.Read()) {
                        models.Add(DataReaderToModel<ParkDerateQRcode>.ToModel(reader));
                    }
                }
            }
            return models;
        }
        public ParkDerateQRcode QueryByRecordId(string recordId)
        {

            string sql = "select * from ParkDerateQRcode where RecordID=@RecordID and DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkDerateQRcode>.ToModel(reader);
                    }
                }
            }
            return null;
        }
        public List<ParkDerateQRcode> QueryPage(string sellerId,string derateId,int derateQRcodeType,int? status,DerateQRCodeSource? source, int pagesize, int pageindex, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select q.*,d.Name DerateName,s.SellerName,s.VID,u.UserAccount,ps.SellerNo,d.DerateType from ParkDerateQRcode q inner join ParkDerate d on q.DerateID=d.DerateID");
            strSql.Append(" inner join parkseller s on d.SellerID=s.SellerID");
            strSql.Append("  left join sysuser u on u.RecordID=q.operatorId");
            strSql.Append("  left join ParkSeller ps on ps.SellerID=q.operatorId");
            strSql.Append(" where q.DerateQRcodeType=@DerateQRcodeType and  q.DataStatus!=@DataStatus and s.SellerID=@SellerID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SellerID", sellerId);
                dbOperator.AddParameter("DerateQRcodeType", derateQRcodeType);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                if (!string.IsNullOrWhiteSpace(derateId))
                {
                    strSql.Append(" and q.DerateID=@DerateID");
                    dbOperator.AddParameter("DerateID", derateId);
                }
                if (status.HasValue)
                {
                    if (status.Value == 0)
                    {
                        strSql.Append(" and (q.CanUseTimes=0 or q.CanUseTimes>q.AlreadyUseTimes)");
                        strSql.AppendFormat(" and q.StartTime<='{0}' and q.EndTime>='{0}'",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else {
                        strSql.AppendFormat(" and ((q.CanUseTimes!=0 and q.CanUseTimes<=q.AlreadyUseTimes) or q.StartTime>'{0}' or q.EndTime<'{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                  
                }

                if (source.HasValue)
                {
                    strSql.Append(" and q.DataSource=@DataSource");
                    dbOperator.AddParameter("DataSource",source.Value);
                }
               
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " ID DESC", pageindex, pagesize, out total))
                {
                    List<ParkDerateQRcode> models = new List<ParkDerateQRcode>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkDerateQRcode>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
