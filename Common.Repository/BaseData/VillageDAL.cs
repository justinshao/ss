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
    public class VillageDAL : BaseDAL, IVillage
    {
        public bool Add(BaseVillage model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }
        public bool Add(BaseVillage model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BaseVillage(VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@VID,@VNo,@VName,@CPID,@LinkMan,@Mobile,@Address,@Coordinate,@ProxyNo,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("VNo", model.VNo);
            dbOperator.AddParameter("VName", model.VName);
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("LinkMan", model.LinkMan);
            dbOperator.AddParameter("Mobile", model.Mobile);
            dbOperator.AddParameter("Address", model.Address);
            dbOperator.AddParameter("Coordinate", model.Coordinate);
            dbOperator.AddParameter("ProxyNo", model.ProxyNo);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Update(BaseVillage model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BaseVillage set VNo=@VNo,VName=@VName,CPID=@CPID,LinkMan=@LinkMan,Mobile=@Mobile,Address=@Address,Coordinate=@Coordinate,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
                strSql.Append(" where VID=@VID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", model.VID);
                dbOperator.AddParameter("VNo", model.VNo);
                dbOperator.AddParameter("VName", model.VName);
                dbOperator.AddParameter("CPID", model.CPID);
                dbOperator.AddParameter("LinkMan", model.LinkMan);
                dbOperator.AddParameter("Mobile", model.Mobile);
                dbOperator.AddParameter("Address", model.Address);
                dbOperator.AddParameter("Coordinate", model.Coordinate);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("BaseVillage", "VID", recordId);
        }

        public BaseVillage QueryVillageByProxyNo(string proxyNo)
        {
            string sql = "select * from BaseVillage where ProxyNo=@ProxyNo and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ProxyNo", proxyNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read()) {
                        return DataReaderToModel<BaseVillage>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public BaseVillage QueryVillageByVillageNo(string villageNo,string companyId)
        {
            string sql = "select * from BaseVillage where VNo=@VNo and CPID=@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VNo", villageNo);
                dbOperator.AddParameter("CPID", companyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseVillage>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<BaseVillage> QueryVillageByUserId(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                List<BaseVillage> models = new List<BaseVillage>();
                strSql.Append("select distinct v.* from SysUserScopeMapping um ");
                strSql.Append(" inner join SysScopeAuthorize sa on um.ASID=sa.ASID");
                strSql.Append(" inner join BaseVillage v on sa.TagID =v.VID");
                strSql.Append(" where v.DataStatus!=@DataStatus and sa.DataStatus!=@DataStatus and um.DataStatus!=@DataStatus and sa.ASType=@ASType and um.UserRecordID=@UserRecordID");
                dbOperator.ClearParameters();

                dbOperator.AddParameter("ASType", (int)ASType.Village);
                dbOperator.AddParameter("UserRecordID", userId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<BaseVillage> QueryPage(string companyId, int pageIndex, int pageSize, out int totalRecord)
        {
            string sql = "select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where CPID=@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPID", companyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.Paging(sql, " ID DESC", pageIndex, pageSize, out totalRecord))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<BaseVillage> QueryPage(List<string> villageIds,string companyId, int pageIndex, int pageSize, out int totalRecord)
        {
            string sql = string.Format("select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where VID in('{0}') and DataStatus!=@DataStatus and  CPID=@CPID;", string.Join("','", villageIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("CPID", companyId);
                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.Paging(sql, " ID DESC", pageIndex, pageSize, out totalRecord))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<BaseVillage> QueryVillageByCompanyId(string companyId)
        {
            string sql = "select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where CPID=@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPID", companyId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<BaseVillage> QueryVillageByCompanyIds(List<string> companyIds)
        {
            string sql = string.Format("select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where CPID in('{0}') and DataStatus!=@DataStatus;", string.Join("','", companyIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public BaseVillage QueryVillageByRecordId(string recordId)
        {
            string sql = "select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where VID=@VID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseVillage>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public List<BaseVillage> QueryVillageByRecordIds(List<string> recordIds)
        {
            string sql = string.Format("select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where VID in('{0}') and DataStatus!=@DataStatus;",string.Join("','",recordIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseVillage> models = new List<BaseVillage>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<BaseVillage> QueryVillageAll()
        {
            string sql = "select ID,VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsOnLine from BaseVillage where  DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public List<BaseVillage> QueryVillageByEmployeeMobilePhone(string mobilePhone)
        {
            string sql = "select v.* from BaseVillage v left join BaseEmployee e on e.VID=v.VID where e.MobilePhone=@MobilePhone and e.DataStatus!=@DataStatus and v.DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("MobilePhone", mobilePhone);
                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
    }
}
