using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Parking;
namespace Common.SqlRepository
{
    public class ParkGateDAL : BaseDAL, IParkGate
    {
        public bool Add(ParkGate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ParkGate(GateID,GateNo,GateName,BoxID,IoState,IsTempInOut,IsEnterConfirm,OpenPlateBlurryMatch,Remark,DataStatus,LastUpdateTime,HaveUpdate,IsNeedCapturePaper,PlateNumberAndCard,IsWeight)");
                strSql.Append(" values(@GateID,@GateNo,@GateName,@BoxID,@IoState,@IsTempInOut,@IsEnterConfirm,@OpenPlateBlurryMatch,@Remark,@DataStatus,@LastUpdateTime,@HaveUpdate,@IsNeedCapturePaper,@PlateNumberAndCard,@IsWeight)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", model.GateID);
                dbOperator.AddParameter("GateNo", model.GateNo);
                dbOperator.AddParameter("GateName", model.GateName);
                dbOperator.AddParameter("BoxID", model.BoxID);
                dbOperator.AddParameter("IoState", (int)model.IoState);
                dbOperator.AddParameter("IsTempInOut", (int)model.IsTempInOut);
                dbOperator.AddParameter("IsEnterConfirm", (int)model.IsEnterConfirm);
                dbOperator.AddParameter("OpenPlateBlurryMatch", (int)model.OpenPlateBlurryMatch);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("IsNeedCapturePaper", model.IsNeedCapturePaper);
                dbOperator.AddParameter("PlateNumberAndCard", model.PlateNumberAndCard);
                dbOperator.AddParameter("IsWeight", model.IsWeight);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(ParkGate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkGate set GateNo=@GateNo,GateName=@GateName,BoxID=@BoxID,IoState=@IoState,IsTempInOut=@IsTempInOut,IsEnterConfirm=@IsEnterConfirm,OpenPlateBlurryMatch=@OpenPlateBlurryMatch");
                strSql.Append(",Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsNeedCapturePaper=@IsNeedCapturePaper,PlateNumberAndCard=@PlateNumberAndCard,IsWeight=@IsWeight where GateID=@GateID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", model.GateID);
                dbOperator.AddParameter("GateNo", model.GateNo);
                dbOperator.AddParameter("GateName", model.GateName);
                dbOperator.AddParameter("BoxID", model.BoxID);
                dbOperator.AddParameter("IoState", (int)model.IoState);
                dbOperator.AddParameter("IsTempInOut", (int)model.IsTempInOut);
                dbOperator.AddParameter("IsEnterConfirm", (int)model.IsEnterConfirm);
                dbOperator.AddParameter("OpenPlateBlurryMatch", (int)model.OpenPlateBlurryMatch);
                dbOperator.AddParameter("Remark", model.Remark);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                dbOperator.AddParameter("IsNeedCapturePaper", model.IsNeedCapturePaper);
                dbOperator.AddParameter("PlateNumberAndCard", model.PlateNumberAndCard);
                dbOperator.AddParameter("IsWeight", model.IsWeight);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("ParkGate", "GateID", recordId);
        }

        public ParkGate QueryByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" from ParkGate where GateID=@GateID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkGates(reader).FirstOrDefault();
                }
            }
        }
        private List<ParkGate> GetParkGates(DbDataReader reader)
        {
            List<ParkGate> models = new List<ParkGate>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<ParkGate>.ToModel(reader));
            }
            return models;
        }
        public List<ParkGate> QueryByParkBoxRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" from ParkGate where BoxID=@BoxID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkGates(reader);
                }
            }
        }

        public List<ParkGate> QueryByParkingId(string parkingId)
        {
            List<ParkGate> gatelist = new List<ParkGate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select g.* from ParkGate g");
            strSql.Append(" left join ParkBox b on g.BoxID=b.BoxID");
            strSql.Append(" left join ParkArea a on a.AreaID=b.AreaID");
            strSql.Append(" where a.PKID=@ParkingID and g.DataStatus!=@DataStatus and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        gatelist.Add(DataReaderToModel<ParkGate>.ToModel(reader));
                    }
                }
            }
            return gatelist;
        }

        public List<ParkGate> QueryByParkAreaRecordIds(List<string> areaIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select g.* from ParkGate g");
            strSql.Append(" left join ParkBox b on g.BoxID=b.BoxID");
            strSql.Append(" left join ParkArea a on a.AreaID=b.AreaID");
            strSql.AppendFormat(" where a.AreaID in ('{0}') and g.DataStatus!=@DataStatus and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus", string.Join("','", areaIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkGates(reader);
                }
            }
        }

        public List<ParkGate> QueryByParkingIdAndIoState(string parkingId, IoState ioState)
        {
            List<ParkGate> gatelist = new List<ParkGate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select g.* from ParkGate g");
            strSql.Append(" left join ParkBox b on g.BoxID=b.BoxID");
            strSql.Append(" left join ParkArea a on a.AreaID=b.AreaID");
            strSql.Append(" where a.PKID=@ParkingID and g.IoState=@IoState and g.DataStatus!=@DataStatus and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("IoState", (int)ioState);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetParkGates(reader);
                }
            }
        }
        public string QueryParkingIdByGateId(string gateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.PKID from ParkGate g inner join ParkBox b on g.BoxID=b.BoxID");
            strSql.Append(" inner join ParkArea a on b.AreaID=b.AreaID where g.GateID=@GateID");
            strSql.Append(" and g.DataStatus!=@DataStatus and a.DataStatus!=@DataStatus and b.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", gateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader.GetStringDefaultEmpty(0);
                    }
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <param name="villageId"></param>
        /// <param name="parkingId"></param>
        /// <param name="areaId"></param>
        /// <param name="boxId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordTotalCount"></param>
        /// <returns></returns>
        public List<RemotelyOpenGateView> QueryRemotelyOpenGate(List<string> parkingIds, string areaId, string boxId, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select g.ID,v.VName,p.PKName,p.PKID,a.AreaName,b.BoxName,g.GateName,g.GateID,g.IoState from ParkGate g ");
            sql.Append(" inner join ParkBox b on g.BoxID=b.BoxID ");
            sql.Append(" inner join ParkArea a on a.AreaID=b.AreaID ");
            sql.Append(" inner join BaseParkinfo p on a.PKID=P.PKID ");
            sql.Append(" inner join BaseVillage v on v.VID=p.VID ");
            sql.Append(" where g.DataStatus!=2 and b.DataStatus!=2 and a.DataStatus!=2 and p.DataStatus!=2 and v.DataStatus!=2");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                if (parkingIds.Count > 0)
                {
                    sql.AppendFormat(" and p.PKID in('{0}')", string.Join("','", parkingIds));
                }
                if (!string.IsNullOrEmpty(areaId))
                {
                    sql.Append(" and a.AreaID=@AreaID ");
                    dbOperator.AddParameter("AreaID", areaId);
                }

                if (!string.IsNullOrEmpty(boxId))
                {
                    sql.Append(" and b.BoxID=@BoxID");
                    dbOperator.AddParameter("BoxID", boxId);
                }

                List<RemotelyOpenGateView> models = new List<RemotelyOpenGateView>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "ID DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(new RemotelyOpenGateView()
                        {
                            VillageName = reader.GetStringDefaultEmpty(1),
                            ParkName = reader.GetStringDefaultEmpty(2),
                            ParkingID = reader.GetStringDefaultEmpty(3),
                            AreaName = reader.GetStringDefaultEmpty(4),
                            BoxName = reader.GetStringDefaultEmpty(5),
                            GateName = reader.GetStringDefaultEmpty(6),
                            GateID = reader.GetStringDefaultEmpty(7),
                            IoState = reader.GetInt32DefaultOne(8) == 1 ? "入口" : "出口"
                        });
                    }

                }
                return models;

            }
        }

        /// <summary>
        /// 根据通道获取进出规则
        /// </summary>
        /// <param name="gateId"></param>
        /// <returns></returns>
        public List<ParkGateIOTime> QueryGateIOTime(string gateId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from ParkGateIOTime where GateID=@GateID and DataStatus!=2");
            List<ParkGateIOTime> list = new List<ParkGateIOTime>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("GateID", gateId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderToModel<ParkGateIOTime>.ToModel(reader));
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 增加通道规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddIOTime(ParkGateIOTime model, DbOperator dbOperator)
        {

            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkGateIOTime(RecordID,GateID,RuleType,WeekIndex,RuleDate,StartTime,EndTime,InOutState,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@RecordID,@GateID,@RuleType,@WeekIndex,@RuleDate,@StartTime,@EndTime,@InOutState,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("RuleType", model.RuleType);
            dbOperator.AddParameter("WeekIndex", model.WeekIndex);
            dbOperator.AddParameter("RuleDate", model.RuleDate);
            dbOperator.AddParameter("StartTime", model.StartTime);
            dbOperator.AddParameter("EndTime", model.EndTime);
            dbOperator.AddParameter("InOutState", model.InOutState);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;

        }

        /// <summary>
        /// 修改通道规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIOTime(ParkGateIOTime model, DbOperator dbOperator)
        {

            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  ParkGateIOTime set GateID=@GateID,RuleType=@RuleType,WeekIndex=@WeekIndex,RuleDate=@RuleDate,StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime,InOutState=@InOutState,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where RecordID=@RecordID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("RuleType", model.RuleType);
            dbOperator.AddParameter("WeekIndex", model.WeekIndex);
            dbOperator.AddParameter("RuleDate", model.RuleDate);
            dbOperator.AddParameter("StartTime", model.StartTime);
            dbOperator.AddParameter("EndTime", model.EndTime);
            dbOperator.AddParameter("InOutState", model.InOutState);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;

        }

        /// <summary>
        /// 删除特殊规则
        /// </summary>
        /// <param name="ruleid"></param>
        /// <returns></returns>
        public bool DelIOTime(string ruleid)
        {
            return CommonDelete("ParkGateIOTime", "RecordID", ruleid);
        }
    }
}
