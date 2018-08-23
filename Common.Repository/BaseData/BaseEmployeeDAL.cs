using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BaseData;
using Common.DataAccess;
using Common.IRepository;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class BaseEmployeeDAL : BaseDAL, IBaseEmployee
    {
        public bool Add(BaseEmployee model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(BaseEmployee model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BaseEmployee(EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,HomePhone,Email,FamilyAddr,");
            strSql.Append("RegTime,EmployeeType,Remark,AreaName,DeptID,VID,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@EmployeeID,@EmployeeName,@Sex,@CertifType,@CertifNo,@MobilePhone,@HomePhone,@Email,@FamilyAddr,");
            strSql.Append("@RegTime,@EmployeeType,@Remark,@AreaName,@DeptID,@VID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("EmployeeName", model.EmployeeName);
            dbOperator.AddParameter("Sex", model.Sex);
            dbOperator.AddParameter("CertifType", (int)model.CertifType);
            dbOperator.AddParameter("CertifNo", model.CertifNo);
            dbOperator.AddParameter("MobilePhone", model.MobilePhone);
            dbOperator.AddParameter("HomePhone", model.HomePhone);
            dbOperator.AddParameter("Email", model.Email);
            dbOperator.AddParameter("FamilyAddr", model.FamilyAddr);
            dbOperator.AddParameter("RegTime", model.RegTime);
            dbOperator.AddParameter("EmployeeType", (int)model.EmployeeType);
            dbOperator.AddParameter("Remark", model.Remark);
            dbOperator.AddParameter("AreaName", model.AreaName);
            dbOperator.AddParameter("DeptID", model.DeptID);
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(BaseEmployee model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Update(model, dbOperator);
            }
        }

        public bool Update(BaseEmployee model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BaseEmployee set EmployeeName=@EmployeeName,Sex=@Sex,CertifType=@CertifType,CertifNo=@CertifNo,MobilePhone=@MobilePhone,HomePhone=@HomePhone,Email=@Email,FamilyAddr=@FamilyAddr,");
            strSql.Append("EmployeeType=@EmployeeType,Remark=@Remark,AreaName=@AreaName,DeptID=@DeptID,VID=@VID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where EmployeeID=@EmployeeID");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("EmployeeName", model.EmployeeName);
            dbOperator.AddParameter("Sex", model.Sex);
            dbOperator.AddParameter("CertifType", (int)model.CertifType);
            dbOperator.AddParameter("CertifNo", model.CertifNo);
            dbOperator.AddParameter("MobilePhone", model.MobilePhone);
            dbOperator.AddParameter("HomePhone", model.HomePhone);
            dbOperator.AddParameter("Email", model.Email);
            dbOperator.AddParameter("FamilyAddr", model.FamilyAddr);
            dbOperator.AddParameter("EmployeeType", (int)model.EmployeeType);
            dbOperator.AddParameter("Remark", model.Remark);
            dbOperator.AddParameter("AreaName", model.AreaName);
            dbOperator.AddParameter("DeptID", model.DeptID);
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public List<BaseEmployee> QueryEmployeeByVillageId(string villageId)
        {
            string sql = "SELECT ID,EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,HomePhone,Email,FamilyAddr,";
            sql += "RegTime,EmployeeType,Remark,LastUpdateTime,HaveUpdate,DataStatus,AreaName,DeptID,VID FROM BaseEmployee";
            sql += " where VID=@VID and DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseEmployee> models = new List<BaseEmployee>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseEmployee>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public BaseEmployee QueryBaseEmployeeByVillageIdAndMobile(string villageId, string mobile)
        {
            string sql = "SELECT ID,EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,HomePhone,Email,FamilyAddr,";
            sql += "RegTime,EmployeeType,Remark,LastUpdateTime,HaveUpdate,DataStatus,AreaName,DeptID,VID FROM BaseEmployee";
            sql += " where VID=@VID and (MobilePhone=@MobilePhone or HomePhone=@MobilePhone) and DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("MobilePhone", mobile);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseEmployee>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public BaseEmployee QueryByEmployeeId(string employeeId)
        {
            string sql = "SELECT ID,EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,HomePhone,Email,FamilyAddr,";
            sql += "RegTime,EmployeeType,Remark,LastUpdateTime,HaveUpdate,DataStatus,AreaName,DeptID,VID FROM BaseEmployee";
            sql += " where EmployeeID=@EmployeeID  and DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("EmployeeID", employeeId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseEmployee>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool LogoutEmployee(string employeeId, string logoutreason)
        {
            return false;
        }

        public bool Delete(string employeeId)
        {
            return CommonDelete("BaseEmployee", "EmployeeID", employeeId);
        }

        public bool Delete(string employeeId, DbOperator dbOperator)
        {
            return CommonDelete("BaseEmployee", "EmployeeID", employeeId, dbOperator);
        }

        public List<BaseEmployee> QueryEmployeeByHrdeptId(string hrdeptId)
        {
            string sql = "SELECT ID,EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,HomePhone,Email,FamilyAddr,";
            sql += "RegTime,EmployeeType,Remark,LastUpdateTime,HaveUpdate,DataStatus,AreaName,DeptID,VID FROM BaseEmployee";
            sql += " where DeptID=@DeptID  and DataStatus!=@DataStatus";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DeptID", hrdeptId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<BaseEmployee> models = new List<BaseEmployee>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseEmployee>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<BaseEmployee> QueryEmployeePage(EmployeeCondition condition, int pagesize, int pageindex, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM BaseEmployee WHERE  VID=@VID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("VID", condition.VillageId);
                if (!string.IsNullOrEmpty(condition.DeptId))
                {
                    sql.Append(" AND DeptID=@DeptID");
                    dbOperator.AddParameter("DeptID", condition.DeptId);
                }
                if (!string.IsNullOrEmpty(condition.Name))
                {
                    sql.Append(" AND EmployeeName like @EmployeeName");
                    dbOperator.AddParameter("UserAccount", "%" + condition.Name + "%");
                }
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    sql.Append(" AND (MobilePhone=@MobilePhone or HomePhone=@MobilePhone)");
                    dbOperator.AddParameter("MobilePhone", condition.Phone);
                }
                if (condition.EmployeeType.HasValue)
                {
                    sql.Append(" AND (EmployeeType=@EmployeeType or EmployeeType=@EmployeeType1)");
                    dbOperator.AddParameter("EmployeeType", (int)condition.EmployeeType);
                    dbOperator.AddParameter("EmployeeType1", (int)EmployeeType.OwnerAndStaff);

                }
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "LastUpdateTime DESC", pageindex, pagesize, out total))
                {
                    List<BaseEmployee> models = new List<BaseEmployee>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BaseEmployee>.ToModel(reader));
                    }
                    return models;

                }

            }
        }
    }
}