using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Utilities;
using Common.Entities;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class ParkMonthlyCarApplyDAL : BaseDAL, IParkMonthlyCarApply
    {
        public bool Add(ParkMonthlyCarApply model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.ApplyDateTime = DateTime.Now;
                model.ApplyStatus = Entities.MonthlyCarApplyStatus.Applying;
                model.RecordID = GuidGenerator.GetGuid().ToString();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO ParkMonthlyCarApply(RecordID,AccountID,PKID,CarTypeID,CarModelID,ApplyName,ApplyMoblie,PlateNo,PKLot,FamilyAddress,ApplyRemark,ApplyStatus,ApplyDateTime)");
                strSql.Append(" values(@RecordID,@AccountID,@PKID,@CarTypeID,@CarModelID,@ApplyName,@ApplyMoblie,@PlateNo,@PKLot,@FamilyAddress,@ApplyRemark,@ApplyStatus,@ApplyDateTime)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("AccountID", model.AccountID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                dbOperator.AddParameter("CarModelID", model.CarModelID);
                dbOperator.AddParameter("ApplyName", model.ApplyName);
                dbOperator.AddParameter("ApplyMoblie", model.ApplyMoblie);
                dbOperator.AddParameter("PlateNo",model.PlateNo);
                dbOperator.AddParameter("PKLot", model.PKLot);
                dbOperator.AddParameter("FamilyAddress", model.FamilyAddress);
                dbOperator.AddParameter("ApplyRemark", model.ApplyRemark);
                dbOperator.AddParameter("ApplyStatus", (int)model.ApplyStatus);
                dbOperator.AddParameter("ApplyDateTime", model.ApplyDateTime);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool AgainApply(ParkMonthlyCarApply model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.ApplyDateTime = DateTime.Now;
                model.ApplyStatus = Entities.MonthlyCarApplyStatus.Applying;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkMonthlyCarApply set PKID=@PKID,CarTypeID=@CarTypeID,CarModelID=@CarModelID,ApplyName=@ApplyName,ApplyMoblie=@ApplyMoblie,PlateNo=@PlateNo,PKLot=@PKLot,FamilyAddress=@FamilyAddress,ApplyRemark=@ApplyRemark,ApplyStatus=@ApplyStatus,ApplyDateTime=@ApplyDateTime ");
                strSql.Append(" where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                dbOperator.AddParameter("CarModelID", model.CarModelID);
                dbOperator.AddParameter("ApplyName", model.ApplyName);
                dbOperator.AddParameter("ApplyMoblie", model.ApplyMoblie);
                dbOperator.AddParameter("PlateNo", model.PlateNo);
                dbOperator.AddParameter("PKLot", model.PKLot);
                dbOperator.AddParameter("FamilyAddress", model.FamilyAddress);
                dbOperator.AddParameter("ApplyRemark", model.ApplyRemark);
                dbOperator.AddParameter("ApplyStatus", (int)model.ApplyStatus);
                dbOperator.AddParameter("ApplyDateTime", model.ApplyDateTime);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Cancel(string recordId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkMonthlyCarApply set ApplyStatus=@ApplyStatus  where RecordID=@RecordID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("ApplyStatus", (int)MonthlyCarApplyStatus.Cancel);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public bool Passed(ParkMonthlyCarApply monthlyCarApply, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkMonthlyCarApply set ApplyStatus=@ApplyStatus,AuditRemark=@AuditRemark,CarTypeID=@CarTypeID,CarModelID=@CarModelID  where RecordID=@RecordID ");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", monthlyCarApply.RecordID);
            dbOperator.AddParameter("CarTypeID", monthlyCarApply.CarTypeID);
            dbOperator.AddParameter("CarModelID", monthlyCarApply.CarModelID);
            dbOperator.AddParameter("ApplyStatus", (int)MonthlyCarApplyStatus.Passed);
            dbOperator.AddParameter("AuditRemark", monthlyCarApply.AuditRemark);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Refused(string recordId, string remark)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ParkMonthlyCarApply set ApplyStatus=@ApplyStatus,AuditRemark=@AuditRemark  where RecordID=@RecordID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("ApplyStatus", (int)MonthlyCarApplyStatus.Refused);
                dbOperator.AddParameter("AuditRemark", remark);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public ParkMonthlyCarApply QueryByRecordID(string recordId) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,p.PKName,c.CarTypeName,m.CarModelName from ParkMonthlyCarApply a ");
            strSql.Append("  inner join BaseParkinfo p on a.PKID=p.PKID");
            strSql.Append("  inner join ParkCarType c on c.CarTypeID=a.CarTypeID");
            strSql.Append("  inner join ParkCarModel m on m.CarModelID=a.CarModelID");
            strSql.Append("  where a.RecordID=@RecordID");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<ParkMonthlyCarApply>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public List<ParkMonthlyCarApply> QueryByAccountID(string accountId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,p.PKName,c.CarTypeName,m.CarModelName from ParkMonthlyCarApply a ");
            strSql.Append("  inner join BaseParkinfo p on a.PKID=p.PKID");
            strSql.Append("  inner join ParkCarType c on c.CarTypeID=a.CarTypeID");
            strSql.Append("  inner join ParkCarModel m on m.CarModelID=a.CarModelID");
            strSql.Append("  where a.AccountID=@AccountID order by a.ApplyDateTime desc");
            List<ParkMonthlyCarApply> models = new List<ParkMonthlyCarApply>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AccountID", accountId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkMonthlyCarApply>.ToModel(reader));
                    }
                }
            }
            return models;
        }
        public List<ParkMonthlyCarApply> QueryPage(List<string> parkingIds, string applyInfo, MonthlyCarApplyStatus? Status, DateTime start, DateTime end, int pagesize, int pageindex, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.*,p.PKName,c.CarTypeName,m.CarModelName from ParkMonthlyCarApply a ");
            strSql.Append("  inner join BaseParkinfo p on a.PKID=p.PKID");
            strSql.Append("  inner join ParkCarType c on c.CarTypeID=a.CarTypeID");
            strSql.Append("  inner join ParkCarModel m on m.CarModelID=a.CarModelID");
            strSql.AppendFormat(" where a.PKID in('{0}')",string.Join("','",parkingIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                if (!string.IsNullOrWhiteSpace(applyInfo))
                {
                    strSql.Append(" and (a.PlateNo like @ApplyInfo or a.ApplyName like @ApplyInfo or a.ApplyMoblie like @ApplyInfo)");
                    dbOperator.AddParameter("ApplyInfo", "%" + applyInfo + "%");
                }
                if (Status.HasValue) {
                    strSql.Append(" and a.ApplyStatus=@ApplyStatus");
                    dbOperator.AddParameter("ApplyStatus", (int)Status.Value);
                }
                strSql.AppendFormat(" and a.ApplyDateTime>='{0}' and a.ApplyDateTime <='{1}'", start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " ApplyDateTime DESC", pageindex, pagesize, out total))
                {
                    List<ParkMonthlyCarApply> models = new List<ParkMonthlyCarApply>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkMonthlyCarApply>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
