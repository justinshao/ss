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
    public class ParkCarlineInfoDAL : BaseDAL, IParkCarlineInfo
    {
        public bool Add(ParkCarlineInfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkCarlineInfo(Gateid,PKID,PlateNumber,TargetTime,Remark,DataStatus,HaveUpdate,LastUpdateTime)");
                strSql.Append(" values(@Gateid,@PKID,@PlateNumber,@TargetTime,@Remark,@DataStatus,@HaveUpdate,@LastUpdateTime)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Gateid", model.Gateid);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("TargetTime", model.TargetTime); 
                dbOperator.AddParameter("Remark", model.Remark); 
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
         

        public bool Delete(string gateId)
        {
            return CommonDelete("ParkCarlineInfo", "gateId", gateId);
        }

        public bool Update(ParkCarlineInfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkCarlineInfo set PKID=@PKID,PlateNumber=@PlateNumber,TargetTime=@TargetTime,Remark=@Remark");
                strSql.Append(",LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where Gateid=@Gateid");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("Gateid", model.Gateid);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                dbOperator.AddParameter("TargetTime", model.TargetTime);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public ParkCarlineInfo QueryByGate(string gateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkCarlineInfo  where GateId= @GateId");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateId", gateId); 
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        ParkCarlineInfo model = DataReaderToModel<ParkCarlineInfo>.ToModel(reader); 
                        return model;
                    }
                    return null;
                }
            }
        }

        public ParkCarlineInfo QueryMinTargetTimeInfo(string parkingID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from ParkCarlineInfo WHERE TargetTime = (SELECT min(TargetTime) FROM ParkCarlineInfo WHERE DataStatus!=@DataStatus and PKID=@PKID) and DataStatus!=@DataStatus and PKID=@PKID");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("PKID", parkingID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        ParkCarlineInfo model = DataReaderToModel<ParkCarlineInfo>.ToModel(reader);
                        return model;
                    }
                    return null;
                }
            }
        }
    }
}
