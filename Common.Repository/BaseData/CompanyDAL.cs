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
    public class CompanyDAL : BaseDAL, ICompany
    {
        public bool Add(BaseCompany model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model,dbOperator);
            }
        }

        public bool Add(BaseCompany model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BaseCompany(CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@CPID,@CPName,@CityID,@Address,@LinkMan,@Mobile,@MasterID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("CPName", model.CPName);
            dbOperator.AddParameter("CityID", model.CityID);
            dbOperator.AddParameter("Address", model.Address);
            dbOperator.AddParameter("LinkMan", model.LinkMan);
            dbOperator.AddParameter("Mobile", model.Mobile);
            if (string.IsNullOrWhiteSpace(model.MasterID))
            {
                dbOperator.AddParameter("MasterID", DBNull.Value);
            }
            else {
                dbOperator.AddParameter("MasterID", model.MasterID);
            }
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(BaseCompany model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Update(model,dbOperator);
            }
        }

        public bool Update(BaseCompany model, DbOperator dbOperator)
        {
           
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BaseCompany set CPName=@CPName,CityID=@CityID,Address=@Address,LinkMan=@LinkMan,Mobile=@Mobile,MasterID=@MasterID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where CPID=@CPID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CPID", model.CPID);
            dbOperator.AddParameter("CPName", model.CPName);
            dbOperator.AddParameter("CityID", model.CityID);
            dbOperator.AddParameter("Address", model.Address);
            dbOperator.AddParameter("LinkMan", model.LinkMan);
            dbOperator.AddParameter("Mobile", model.Mobile);
            if (string.IsNullOrWhiteSpace(model.MasterID))
            {
            
                dbOperator.AddParameter("MasterID", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("MasterID", model.MasterID);
            }
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string recordId, DbOperator dbOperator)
        {
            return CommonDelete("BaseCompany", "CPID", recordId, dbOperator);
        }
        public List<BaseCompany> QueryTopCompany()
        {
            List<BaseCompany> company = null;
            string sql = string.Format("select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where (masterid is null or masterid='' or masterid='0') and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    company = new List<BaseCompany>();
                    if (reader.Read())
                    {
                        company.Add(DataReaderToModel<BaseCompany>.ToModel(reader));
                    }
                }
            }
            return company;
        }
        public List<BaseCompany> QueryCompanyByRecordIds(List<string> recordIds)
        {
            string sql = string.Format("select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where CPID in('{0}') and DataStatus!=@DataStatus;", string.Join("','", recordIds));
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader);
                }
            }
        }
        private List<BaseCompany> GetBaseCompany(DbDataReader reader)
        {
            List<BaseCompany> models = new List<BaseCompany>();
            while (reader.Read())
            {
                models.Add(DataReaderToModel<BaseCompany>.ToModel(reader)); 
            }
            return models;
        }
        public List<BaseCompany> QueryCompanysByMasterID(string masterId)
        {
            string sql = "select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where MasterID =@MasterID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MasterID", masterId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader);
                }
            }
        }

        public BaseCompany QueryCompanyByRecordId(string recordId)
        {
            string sql = "select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where CPID =@CPID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader).FirstOrDefault();
                }
            }
        }
        public BaseCompany QueryCompanyByCompanyName(string companyName)
        {
            string sql = "select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where CPName =@CPName and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CPName", companyName);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader).FirstOrDefault();
                }
            }
        }
        /// <summary>
        /// 根据单位名称模糊查询所有单位
        /// </summary> 
        /// <param name="companyName"></param>
        /// <returns></returns>
        public List<BaseCompany> QueryAllCompanyByName(string companyName)
        {
            List<BaseCompany> models = new List<BaseCompany>();

            string sql = "select ID,CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus from BaseCompany where DataStatus!=@DataStatus";
            if (!string.IsNullOrEmpty(companyName))
            {
                // sql += " and CPName like @CPName  ";
                // sql += " and CPName like '%@CPName%'  ";
                sql += " and CPName like '%" + companyName + "%'";

            }
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                // dbOperator.AddParameter("CPName", companyName);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader);
                }
            }
        }
        /// <summary>
        /// 查询所有父单位
        /// </summary> 
        /// <param name="models">根据单位名称模糊查询出来的所有单位</param>
        /// <returns></returns>
        public List<BaseCompany> QuerymasterCompanyBymodels(List<BaseCompany> models)
        {
            int cnt = models.Count;
            for (int i = 0; i < cnt; i++)
            {
                QueryAllmasterCompany(models[i], models);
            }
            return models;
        }
        /// <summary>
        /// 查询父节点递归调用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="models"></param>
        private void QueryAllmasterCompany(BaseCompany model, List<BaseCompany> result)
        {
            BaseCompany mastermodel = QueryCompanyByRecordId(model.MasterID);
            if (mastermodel != null)
            {
                if (!result.Contains(mastermodel))
                    result.Add(mastermodel);
                QueryAllmasterCompany(mastermodel, result);
            }

        }
        /// <summary>
        /// 查询所有子单位
        /// </summary>
        /// <param name="models">根据单位名称模糊查询出来的所有单位</param>
        ///  /// <param name="result">存放结果</param>
        /// <returns></returns>
        public List<BaseCompany> QuerySubordinateCompanyBymodels(List<BaseCompany> models)
        {
            int cnt = models.Count;
            for (int i = 0; i < cnt; i++)
            {
                QueryAllSubordinateCompany(models[i], models);

            }
            models = models.Distinct().ToList();
            return models;
        }
        /// <summary>
        /// 查询子节点递归调用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="models"></param>
        private void QueryAllSubordinateCompany(BaseCompany model, List<BaseCompany> result)
        {
            List<BaseCompany> subordinateCompanys = QueryCompanysByMasterID(model.CPID);
            if (subordinateCompanys != null && subordinateCompanys.Count > 0)
            {
                foreach (var item in subordinateCompanys)
                {

                    if (!result.Contains(item))
                        result.Add(item);
                    QueryAllSubordinateCompany(item, result);
                }
            }

        }
        public List<BaseCompany> QueryCompanyAndSubordinateCompany(string recordId)
        {
            List<BaseCompany> models = new List<BaseCompany>();
            BaseCompany model = QueryCompanyByRecordId(recordId);
            if (model != null)
            {
                models.Add(model);
                QueryAllSubordinateCompany(model.CPID, models);
            }
            return models;
        }
        private void QueryAllSubordinateCompany(string masterId, List<BaseCompany> models)
        {
            List<BaseCompany> subordinateCompanys = QueryCompanysByMasterID(masterId);
            if (subordinateCompanys != null && subordinateCompanys.Count > 0)
            {
                foreach (var item in subordinateCompanys)
                {
                    models.Add(item);
                    QueryAllSubordinateCompany(item.CPID, models);
                }
            }
        }

        public BaseCompany QueryTopCompanyByRecordId(string recordId)
        {
            BaseCompany model = QueryCompanyByRecordId(recordId);
            if (model == null) return null;

            if (string.IsNullOrWhiteSpace(model.MasterID)) return model;

            return QueryTopCompanyByRecordId(model.MasterID);
        }
        public bool SystemExistCompany() {
            string sql = "select ID from BaseCompany where DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if(reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        public BaseCompany QueryByParkingId(string parkingId)
        {
            string sql = "select c.ID,c.CPID,c.CPName,c.CityID,c.Address,c.LinkMan,c.Mobile,c.MasterID,c.LastUpdateTime,c.HaveUpdate,c.DataStatus from BaseParkinfo p inner join BaseVillage v on p.VID=v.VID inner join BaseCompany c on c.CPID=v.CPID ";
            sql += "  where p.PKID=@PKID and p.DataStatus!=@DataStatus and v.DataStatus!=@DataStatus and c.DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    return GetBaseCompany(reader).FirstOrDefault();
                }
            }
        }
        public BaseCompany QueryByBoxID(string boxId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.ID,c.CPID,c.CPName,c.CityID,c.Address,c.LinkMan,c.Mobile,c.MasterID,c.LastUpdateTime,c.HaveUpdate,c.DataStatus from BaseCompany  c inner join BaseVillage v on c.CPID=V.CPID ");
            strSql.Append("  INNER JOIN BaseParkinfo P ON P.VID=V.VID ");
            strSql.Append("  INNER JOIN ParkArea A ON A.PKID=P.PKID ");
            strSql.Append("  INNER JOIN ParkBox B ON B.AreaID=A.AreaID ");
            strSql.Append(" WHERE C.DataStatus!=@DataStatus AND V.DataStatus!=@DataStatus AND P.DataStatus!=@DataStatus");
            strSql.Append("   AND A.DataStatus!=@DataStatus AND B.DataStatus!=@DataStatus AND B.BoxID=@BoxID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BoxID", boxId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return GetBaseCompany(reader).FirstOrDefault();
                }
            }
        }
    }
}
